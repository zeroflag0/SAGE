#region LGPL License
//********************************************************************
//	author:         Thomas "zeroflag" Kraemer
//	author email:   zeroflag@zeroflag.de
//	
//	Copyright (C) 2006-2008  Thomas "zeroflag" Kraemer
//	
//	license:	(LGPL)
//	
//		This library is free software; you can redistribute it and/or
//		modify it under the terms of the GNU Lesser General Public
//		License as published by the Free Software Foundation; either
//		version 2.1 of the License, or (at your option) any later version.
//
//		This library is distributed in the hope that it will be useful,
//		but WITHOUT ANY WARRANTY; without even the implied warranty of
//		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//		Lesser General Public License for more details.
//
//		You should have received a copy of the GNU Lesser General Public
//		License along with this library; if not, write to the Free Software
//		Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//
//		http://www.gnu.org/licenses/lgpl.html#TOC1
//
//*********************************************************************
#endregion LGPL License
// <file><id value="$Id$"/></file>

using System;
using System.Collections.Generic;
using Sage.Modules;
using Sage.Modules.Logging;
using Sage.Threading;
using Sage.World;

namespace Sage
{
	public class Core : IDisposable
	{
		#region Logging

		//DONE: implement logging... (Sage.Modules.Logging)

		private Log _Log;

		public Log Log
		{
			get { return _Log; }
			protected set { _Log = value; }
		}

		LogModule _LogModule = new LogModule();

		public LogModule LogModule
		{
			get { return _LogModule; }
			set { _LogModule = value; }
		}

		#endregion Logging

		#region Modules

		private ModuleList _Modules = new ModuleList();

		/// <summary>
		/// All modules loaded into the engine.
		/// </summary>
		public ModuleList Modules
		{
			get { return _Modules; }
		}

		private Manager _World = new Manager();

		/// <summary>
		/// The engine's world(/scene) module.
		/// </summary>
		public Manager World
		{
			get { return _World; }
			set { _World = value; }
		}

		#endregion Modules

		#region Threading

		private Pool _Threads = new Pool();

		/// <summary>
		/// The engines threadpool.
		/// </summary>
		public Pool Threads
		{
			get { return this._Threads; }
		}

		#endregion Threading

		#region Initialization

		public Core()
		{
			this.Modules.Core = this;
			this.Modules.Add(this.LogModule);
			this.LogModule.Core = this;
			this.Log = new Log(this.LogModule, "CORE");
		}

		private bool _Initialized = false;

		public bool Initialized
		{
			get { return _Initialized; }
			protected set { _Initialized = value; }
		}


		public void Initialize()
		{
			if (this.Initialized)
			{
				return;
			}
			this.Log.Message("Initializing...");

			this.Log.Verbose("Initializing log...");
			this.LogModule.Writers.Add(new Sage.Modules.Logging.ConsoleWriter());
			this.LogModule.Writers.Add(new Sage.Modules.Logging.FileWriter());

			// initialize threading...
			this.Log.Verbose("Initializing threads...");
			this.Threads.Initialize();

			// register the core with the world...
			this.Log.Verbose("Registering the core with the world...");
			this.World.Core = this;

			// load all modules...
			this.Log.Message("Loading modules...");
			this.LoadModules();

			// initialize modules...
			this.Log.Message("Initializing modules...");
			this.InitializeModules();

			// build dependencies...
			this.Log.Verbose("Providing dependencies...");
			this.LoadDependencies();

			// start modules...
			this.Log.Verbose("Starting modules...");
			this.StartModules();

			// add world last so it initializes last...
			this.Modules.Add(this.World);

			this.Log.Message("Initialized.");
			this.Initialized = true;
		}

		protected virtual void LoadDependencies()
		{
			foreach (Module module in this.Modules)
			{
				this.Log.Verbose("Building dependencies for '" + module.Name + "'...");
				module.Dependencies.Build(module);
			}

			foreach (Module module in this.Modules)
			{
				this.Log.Verbose("Tracing dependencies for '" + module.Name + "'...");
				module.Dependencies.Break(module);
			}
		}

		protected virtual void InitializeModules()
		{
			foreach (Module module in this.Modules)
			{
				if (module.Log == null)
					module.Log = new Log(this.LogModule, module.Name);
				else
					module.Log.Module = this.LogModule;

				module.Core = this;

				this.Log.Verbose("Assigning module '" + module.Name + "' to thread '" + module.ThreadType + "'...");
				module.Scheduler.ThreadType = module.ThreadType;

				this.Log.Verbose("Initialized module '" + module.Name + "'.");
			}
		}

		protected virtual void StartModules()
		{
			foreach (Module module in this.Modules)
			{
				this.Log.Verbose("Putting '" + module.Name + "' in startup state...");
				module.Scheduler.Start();
			}
		}

		protected virtual void LoadModules()
		{
			//TODO: generic module loading...
			//this.Modules.Add(new Graphics.Module());
		}

		#endregion Initialization

		public virtual void Run()
		{
			lock (this)
			{
				this.PrintInfo();

				this.Initialize();

				this.Log.Verbose("Running threadpool.");
				this.Threads.Run();
				this.Log.Verbose("Threadpool returned!");
			}
		}

		public void PrintInfo()
		{
			this.Log.Message("<SageInfo>");
			this.Log.MessageIndent++;
			{
				System.Reflection.Assembly ass = System.Reflection.Assembly.GetAssembly(typeof(Sage.Core));

				this.Log.Message(ass.FullName);

				System.Text.StringBuilder system = new System.Text.StringBuilder("<System ");
				system.Append("OperatingSystem=\"").Append(System.Environment.OSVersion).Append("\" ");

				system.Append("Runtime=\"").Append(System.Environment.Version).Append("\" ");
				system.Append("ProcessorCount=\"").Append(System.Environment.ProcessorCount).Append("\" ");
				system.Append("TicksPerMillisecond=\"").Append(System.TimeSpan.TicksPerMillisecond).Append("\" ");

				long start, end;

				start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
				sw.Start();
				long delta = 0, now, last = Thread.Now, count = 0, max = 500;
				for (long i = 0; i < max; )
				{
					now = Thread.Now;
					delta = now - last;
					last = now;
					i += delta;
					if (delta != 0)
						count++;
				}
				sw.Stop();
				end = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
				system.Append("TimingPrecision").Append("=\"").Append((double)(sw.ElapsedMilliseconds) / (double)count).Append("ms, ").Append(count).Append(" steps in ").Append(sw.ElapsedMilliseconds).Append("ms\" ");


				start = Thread.Now;
				Thread.SleepShort();
				end = Thread.Now;
				system.Append("Sleep1msx1=\"").Append(end - start).Append("ms\" ");

				start = Thread.Now;
				for (int i = 0; i < 10; i++)
					System.Threading.Thread.Sleep(1);
				end = Thread.Now;
				system.Append("Sleep1msx10=\"").Append(((end - start) / 10.0)).Append("ms each (overall=").Append(end - start).Append(")\" ");

				system.Append("/>");
				this.Log.Message(system.ToString());

			}
			this.Log.MessageIndent--;
			this.Log.Message("</SageInfo>");
		}

		#region IDisposable Members

		public void Shutdown()
		{
			this.Threads.QueuePool(this.DoShutdown);
		}
		protected void DoShutdown()
		{
			this.Log.Verbose("Shutting down modules...");
			this.Modules.Dispose();
			this.Log.Verbose("Shutting down threads...");
			this.Threads.Dispose();
			this.Log.Message("Shutdown complete.");
			this.LogModule.ExplicitShutdown();
		}

		public void Dispose()
		{
			this.Shutdown();
		}

		#endregion
	}
}
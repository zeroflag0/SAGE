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
using Sage.Modules;
using real = System.Single;
using org.ogre;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sage.World;

namespace Sage.Graphics.Ogre
{
	public class Module : Sage.Graphics.Module, Modules.IGraphicsModule
	{
		public override string Name
		{
			get { return "Graphics.Ogre"; }
		}

		static ApplicationContextBase _OgreApp;
		static ApplicationContextBase CreateOgreApp()
		{
			var app = new ApplicationContextBase("SAGE2");
			app.initApp();
			return app;
		}
		public ApplicationContextBase OgreApp
		// lazy static init...
		{
			get
			{
				if (_OgreApp == null)
					lock (typeof(ApplicationContextBase))
						if (_OgreApp == null)
							_OgreApp = CreateOgreApp();
				return _OgreApp;
			}
		}

		public org.ogre.Root Root
		{
			get { return this.OgreApp.getRoot(); }
		}

		public SceneManager SceneManager
		{
			get;
			protected set;
		}

		public RenderWindow Window
		{
			get { return this.OgreApp.getRenderWindow(); }
		}

		public Camera Camera
		{
			get;
			protected set;
		}

		public Viewport Viewport
		{
			get;
			set;
		}

		protected virtual bool RestoreConfig
		{
			//TODO: configure from files...
			get { return false; }
		}

		protected override zeroflag.Collections.List<IFeatureFactory> FeaturesCreate
		{
			get
			{
				var features = base.FeaturesCreate;
				features.Add(new MeshFeatureFactory());

				return features;
			}
		}

		public Module() : base()
		{ }

		protected virtual void InitializeGraphics()
		{
			this.Configure();
			this.OgreApp.getRenderWindow().setActive(false);
			try
			{
				this.SetupResources();

				// Create any resource listeners (for loading screens)
				this.CreateResourceListener();
				// Load resources
				this.LoadResources();

				this.ChooseSceneManager();
				this.CreateCamera();
				this.CreateViewports();

				// Set default mipmap level (NB some APIs ignore this)
				TextureManager.getSingleton().setDefaultNumMipmaps(5);

				// Create the scene
				this.CreateScene();
			}
			finally
			{
				this.OgreApp.getRenderWindow().setActive(true);
			}
		}

		RenderSystem SelectRenderer()
		{
			RenderSystemList renderersRaw = this.Root.getAvailableRenderers();
			Dictionary<string, RenderSystem> renderers = new Dictionary<string, RenderSystem>();
			foreach (RenderSystem renderer in renderersRaw)
			{
				this.Log.Message("Available renderer: " + renderer.getName());
				renderers.Add(renderer.getName(), renderer);
			}
			string[] preferences =
			{
				"Vulkan Rendering Subsystem",
				"Direct3D11 Rendering Subsystem",
				"OpenGL 3+ Rendering Subsystem",
			};
			// select by preference...
			foreach (var name in preferences)
				if (renderers.ContainsKey(name))
					return renderers[name];

			foreach (RenderSystem renderer in renderers.Values)
				if (renderer != null)
					return renderer;

			return null;
		}
		protected virtual void Configure()
		{
			return;
			const string title = "SAGE";
			var renderer = this.SelectRenderer();
			this.Log.Message("Selected renderer: " + renderer.getName());
			this.Root.setRenderSystem(renderer);

			if (this.RenderingTarget == IntPtr.Zero)
			{
				if (this.RestoreConfig && this.Root.restoreConfig() || this.Root.showConfigDialog(null))
				{
					// If returned true, user clicked OK so initialise
					this.Root.initialise(true, title);
				}
				else
				{
					throw new Exception("Failed to configure OGRE.");
				}
			}
			else
			{
				this.Root.initialise(false, title);
				NameValueMap config = new NameValueMap();
				config["externalWindowHandle"] = this.RenderingTarget.ToInt64().ToString();
				this.Root.createRenderWindow(title, 800, 600, false, config);
			}
		}

		/// <summary>
		/// Method which will define the source of resources (other than current folder)
		/// </summary>
		public virtual void SetupResources()
		{
			return;
			// Load resource paths from config file
			//ConfigFile cf = new ConfigFile();
			//cf.loadDirect("resources.cfg", "\t:=", true);
			// in ogre C# bindings ConfigFile only generates SWIGTYPE_p_std__multimapT_std__string_std__string_t which has no bindings to get values...
			// manual implementation for now...
			var resFile = "resources.cfg";
			var lines = File.ReadAllLines(resFile);
			foreach (var line in lines)
			{
				if (line.Trim().Length == 0) continue;
				if (line.Trim().StartsWith("#")) continue;

				string section = "General";
				if (line.Trim().StartsWith("[") && line.Contains("]"))
				{
					var start = line.IndexOf('[');
					var end = line.IndexOf(']', start + 1);
					section = line.Substring(start + 1, end - start - 1);
					continue;
				}
				if (!line.Contains("="))
				{
					this.Log.Warning("Line doesn't seem to be res/ini-format: (" + resFile + ")\n" + line);
					continue;
				}
				string withoutComment = line.Split(new char[] { '#' }, 2)[0];
				var parts = withoutComment.Split(new char[] { '=' }, 2);

				ResourceGroupManager.getSingleton().addResourceLocation(parts[0], parts[1], section);
			}
		}

		public virtual void ChooseSceneManager()
		{
			// Get the SceneManager, in this case a generic one
			this.SceneManager = this.Root.createSceneManager();

		}

		public virtual void CreateCamera()
		{
			// Create the this.Camera
			this.Camera = this.SceneManager.createCamera("PlayerCam");

		}

		public virtual void CreateFrameListener()
		{
		}

		public virtual void CreateViewports()
		{
			// Create one this.Viewport, entire this.Window
			this.Viewport = this.Window.addViewport(this.Camera);
			this.Viewport.setBackgroundColour(new ColourValue(0, 0, 0));

			// Alter the this.Camera aspect ratio to match the this.Viewport
			this.Camera.setAspectRatio(((float)this.Viewport.getActualWidth()) / ((float)this.Viewport.getActualHeight()));
		}

		/// <summary>
		/// Optional override method where you can create resource listeners (e.g. for loading screens)
		/// </summary>
		public virtual void CreateResourceListener()
		{

		}

		/// <summary>
		/// Optional override method where you can perform resource group loading
		/// Must at least do ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
		/// </summary>
		public virtual void LoadResources()
		{
			// Initialise, parse scripts etc
			ResourceGroupManager.getSingleton().initialiseAllResourceGroups();
		}


		public override void TakeScreenshot(string fileName)
		{
			this.Window.writeContentsToFile(fileName);
		}


#if TESTSCENE
		Entity ogre;
		SceneNode node;
#endif

		protected override void CreateScene()
		{
			//TODO: create the scene from scene data...
#if TESTSCENE
			// sky box.
			this.SceneManager.SetSkyBox(true, "Examples/MorningSkyBox");

			// some light...
			this.SceneManager.AmbientLight = new ColourValue(0.2f, 0.2f, 0.2f, 1f);

			Light light1 = this.SceneManager.CreateLight("light1");
			light1.Type = Light.LightTypes.LT_POINT;
			light1.Position = new Vector3(4, 5, 10);
			light1.DiffuseColour = new ColourValue(0.7f, 0.7f, 0.1f);
			Light light2 = this.SceneManager.CreateLight("light2");
			light2.Type = Light.LightTypes.LT_POINT;
			light2.Position = new Vector3(-4, 2, 20);
			light2.DiffuseColour = new ColourValue(0.1f, 0.5f, 0.1f);

			light2 = this.SceneManager.CreateLight("light3");
			light2.Type = Light.LightTypes.LT_POINT;
			light2.Position = new Vector3(0, 10, 5);
			light2.DiffuseColour = new ColourValue(0.5f, 0.5f, 0.6f);

			ogre = this.SceneManager.CreateEntity("ogre", "ogrehead.mesh");
			node = this.SceneManager.RootSceneNode.CreateChildSceneNode();
			node.AttachObject(ogre);
			node.Translate(new Ogre.Vector3(0.0f, 10.0f, -50.0f));
#endif
		}

		protected override void DoInitialize()
		{
			this.Log.Message("Initializing...");
			try
			{
				//this.Interval = 10; // 50fps
				this.Scheduler.ThreadID = Threading.Pool.CurrentThreadID;

				this.InitializeGraphics();

				this.Log.Message("Initialized.");
			}
			catch (Exception exc)
			{
				this.Log.Error(exc);
				throw;
			}
		}

		protected override void DoUpdate(long timeSinceLastUpdate)
		{
			try
			{
				if (this.Window == null || this.Window.isClosed())
				{
					this.Log.Message("Window closed, requesting shutdown...");
					this.Core.Shutdown();
					return;
				}
				if (!this.Window.isActive())
					// keep rendering when deselected...
					this.Window.setActive(true);

				//RenderTarget.FrameStats stats = this.Window.GetStatistics();
				//if (1000f / stats.LastFPS < (float)this.Interval * 1.20f)
				//{
				//    this.Logging.Warning("R
				//}
				System.Diagnostics.Stopwatch stop = new System.Diagnostics.Stopwatch();
				stop.Start();
				this.Root.renderOneFrame();
				stop.Stop();
				if ((double)stop.ElapsedMilliseconds > (double)this.Interval * 1.1)
				{
					this.Log.Warning("Rendering was slower than interval time: " + stop.ElapsedMilliseconds + "ms, expected " + this.Interval + "ms");
				}
			}
			catch (Exception exc)
			{
				this.Log.Error(exc);
			}
		}

		public override void OnResize()
		{
			base.OnResize();

			if (this.Window != null)
				this.Window.windowMovedOrResized();

			if (this.Camera != null && this.Viewport != null)
			{
				this.Camera.setAspectRatio((real)this.Viewport.getActualWidth() / (real)this.Viewport.getActualHeight());
			}
		}
		public override void TestRotate(float d)
		{
#if TESTSCENE
			node.Rotate(Vector.UNIT_Y.ConvertTo<Ogre.Vector3>(), new Radian(d, true).ConvertTo<Radian>());
#endif
		}

		protected override void DoShutdown()
		{
			try
			{
				this.Log.Message("Shutting down...");
				//this.Window.destroy();
				//this.Root.Dispose();
				this.OgreApp.closeApp();
				this.Log.Message("Shutdown complete.");
			}
			catch (Exception exc)
			{
				this.Log.Error(exc);
			}
		}


	}
}
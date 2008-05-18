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

using Sage;
using Sage.Threading;

namespace SageApplication
{
	class Program
	{
		[System.STAThread]
		private static void Main(string[] args)
		{
			using (Core core = new Core())
			{
				core.Modules.Add(new Sage.Graphics.Ogre.Module());
				//core.Modules.Add(new CpuLoadModule());
				//core.Modules.Add(new CpuLoadModule());
				//core.Modules.Add(new CpuLoadModule());
				//core.Modules.Add(new CpuLoadModule());
				//core.Modules.Add(new CpuLoadModule());
				core.Modules.Add(new GraphicsTest());

				core.Initialize();

				//core.Threads.Queue(new Task(new Message(core.Shutdown), Pool.Now + 10000));

				core.Run();
			}

			#region #if TEST_LOG

#if TEST_LOG
			Sage.Modules.Logging.FileLogger logger = new Sage.Modules.Logging.FileLogger("test");
			Sage.Modules.Logging.Logger foo = logger.Create();

			logger.Message("test");
			logger.MessageIndent++;
			logger.Message(logger.MessageIndent);
			logger.MessageIndent++;
			logger.Message(logger.MessageIndent);

			logger.Error("error!");

			logger.Message("test");

			logger.MessageIndent = 0;

			logger.Message("test?");

			foo.Owner = "foo";

			foo.Message("foo");
			foo.MessageIndent++;
			foo.Message("bar");

			logger.Message("testing");
#endif //#if TEST_LOG

			#endregion #if TEST_LOG
		}
	}
}
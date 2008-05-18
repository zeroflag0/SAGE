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
using Mogre;
using real = System.Single;

namespace Sage.Graphics.Ogre
{
	public class Module : Sage.Graphics.Module, Modules.IGraphicsModule
	{
		public override string Name
		{
			get { return "Graphics.Ogre"; }
		}

		#region Root

		private Root _Root = default(Root);

		public Root Root
		{
			get { return _Root; }
			protected set
			{
				if (_Root != value)
				{
					_Root = value;
				}
			}
		}
		#endregion Root

		#region SceneManager

		private SceneManager _SceneManager = default(SceneManager);

		public SceneManager SceneManager
		{
			get { return _SceneManager; }
			protected set
			{
				if (_SceneManager != value)
				{
					_SceneManager = value;
				}
			}
		}
		#endregion SceneManager

		#region Window

		private RenderWindow _Window = default(RenderWindow);

		public RenderWindow Window
		{
			get { return _Window; }
			protected set
			{
				if (_Window != value)
				{
					_Window = value;
				}
			}
		}
		#endregion Window

		#region Camera

		private Camera _Camera = default(Camera);

		public Camera Camera
		{
			get { return _Camera; }
			protected set
			{
				if (_Camera != value)
				{
					_Camera = value;
				}
			}
		}
		#endregion Camera

		#region Viewport

		private Viewport _Viewport = default(Viewport);

		public Viewport Viewport
		{
			get { return _Viewport; }
			set
			{
				if (_Viewport != value)
				{
					_Viewport = value;
				}
			}
		}
		#endregion Viewport

		protected virtual bool RestoreConfig
		{
			//TODO: configure from files...
			get { return true; }
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

		protected virtual void InitializeGraphics()
		{
			this.Root = new Root();

			this.Configure();
			this.Window.IsActive = false;

			this.SetupResources();

			// Create any resource listeners (for loading screens)
			this.CreateResourceListener();
			// Load resources
			this.LoadResources();

			this.ChooseSceneManager();
			this.CreateCamera();
			this.CreateViewports();

			// Set default mipmap level (NB some APIs ignore this)
			TextureManager.Singleton.DefaultNumMipmaps = 5;

			// Create the scene
			this.CreateScene();
			this.Window.IsActive = true;
		}

		protected virtual void Configure()
		{
			if (this.RenderingTarget == IntPtr.Zero)
			{
				if (this.RestoreConfig && this.Root.RestoreConfig() || this.Root.ShowConfigDialog())
				{
					// If returned true, user clicked OK so initialise
					// Here we choose to let the system create a default rendering this.Window by passing 'true'
					this.Window = this.Root.Initialise(true);
				}
				else
				{
					throw new Exception("Failed to configure OGRE.");
				}
			}
			else
			{
				RenderSystemList renderers = this.Root.GetAvailableRenderers();
				foreach (RenderSystem renderer in renderers)
				{
					if (renderer != null)
					{
						this.Log.Message("Selected renderer: " + renderer.Name);
						this.Root.RenderSystem = renderer;
						break;
					}
				}
				RenderSystem renderSystem = this.Root.RenderSystem;
				renderSystem.SetConfigOption("Full Screen", "No");
				renderSystem.SetConfigOption("VSync", "No");
				renderSystem.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");

				NameValuePairList config = new NameValuePairList();
				config["externalWindowHandle"] = this.RenderingTarget.ToInt64().ToString();
				this.Root.Initialise(false);
				this.Window = this.Root.CreateRenderWindow("SAGE", 800, 600, false, config);
			}
		}

		/// <summary>
		/// Method which will define the source of resources (other than current folder)
		/// </summary>
		public virtual void SetupResources()
		{
			// Load resource paths from config file
			ConfigFile cf = new ConfigFile();
			cf.Load("resources.cfg", "\t:=", true);

			// Go through all sections & settings in the file
			ConfigFile.SectionIterator seci = cf.GetSectionIterator();

			String secName, typeName, archName;

			// Normally we would use the foreach syntax, which enumerates the values, but in this case we need CurrentKey too;
			while (seci.MoveNext())
			{
				secName = seci.CurrentKey;
				ConfigFile.SettingsMultiMap settings = seci.Current;
				foreach (System.Collections.Generic.KeyValuePair<string, string> pair in settings)
				{
					typeName = pair.Key;
					archName = pair.Value;
					ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
				}
			}
		}
		public virtual void ChooseSceneManager()
		{
			// Get the SceneManager, in this case a generic one
			this.SceneManager = this.Root.CreateSceneManager(SceneType.ST_GENERIC, "SceneMgr");

		}

		public virtual void CreateCamera()
		{
			// Create the this.Camera
			this.Camera = this.SceneManager.CreateCamera("PlayerCam");

			// Position it at 500 in Z direction
			this.Camera.Position = new Vector3(0, 20, 20);
			// Look back along -Z
			this.Camera.LookAt(new Vector3(0, 15, -1));
			this.Camera.NearClipDistance = 0.5f;
		}

		public virtual void CreateFrameListener()
		{
		}

		public virtual void CreateViewports()
		{
			// Create one this.Viewport, entire this.Window
			this.Viewport = this.Window.AddViewport(this.Camera);
			this.Viewport.BackgroundColour = new ColourValue(0, 0, 0);

			// Alter the this.Camera aspect ratio to match the this.Viewport
			this.Camera.AspectRatio = ((float)this.Viewport.ActualWidth) / ((float)this.Viewport.ActualHeight);
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
			ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
		}


		public override void TakeScreenshot(string fileName)
		{
			this.Window.WriteContentsToFile(fileName);
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
			node.Translate(new Mogre.Vector3(0.0f, 10.0f, -50.0f));
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
				if (this.Window == null || this.Window.IsClosed)
				{
					this.Log.Message("Window closed, requesting shutdown...");
					this.Core.Shutdown();
					return;
				}
				if (!this.Window.IsActive)
					// keep rendering when deselected...
					this.Window.IsActive = true;

				//RenderTarget.FrameStats stats = this.Window.GetStatistics();
				//if (1000f / stats.LastFPS < (float)this.Interval * 1.20f)
				//{
				//    this.Logging.Warning("R
				//}

				Mogre.WindowEventUtilities.MessagePump();
				System.Diagnostics.Stopwatch stop = new System.Diagnostics.Stopwatch();
				stop.Start();
				this.Root.RenderOneFrame();
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
				this.Window.WindowMovedOrResized();

			if (this.Camera != null && this.Viewport != null)
			{
				this.Camera.AspectRatio = (real)this.Viewport.ActualWidth / (real)this.Viewport.ActualHeight;
			}
		}
		public override void TestRotate(float d)
		{
#if TESTSCENE
			node.Rotate(Vector.UNIT_Y.ConvertTo<Mogre.Vector3>(), new Radian(d, true).ConvertTo<Mogre.Radian>());
#endif
		}

		protected override void DoShutdown()
		{
			try
			{
				this.Log.Message("Shutting down...");
				this.Window.Destroy();
				this.Root.Dispose();
				this.Log.Message("Shutdown complete.");
			}
			catch (Exception exc)
			{
				this.Log.Error(exc);
			}
		}


	}
}
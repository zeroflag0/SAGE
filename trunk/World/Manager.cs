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

using System.Collections.Generic;
using Sage.Basics;
using Sage.Modules;

namespace Sage.World
{
	public class Manager : Module
	{
		public override string Name
		{
			get { return "World"; }
		}

		private Root _Root;

		public Root Root
		{
			get { return _Root; }
			set { _Root = value; }
		}

		public Manager()
		{
			this.Root = new Root();
		}

		#region Nodes
		public virtual Node CreateNode(Node parent, string name)
		{
			return this.CreateNode(parent, name, new Vector(), new Quaternion());
		}

		public virtual Node CreateNode(Node parent, string name, Vector location)
		{
			return this.CreateNode(parent, name, location, new Quaternion());
		}

		/// <summary>
		/// Create a new node in the world graph.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="name"></param>
		/// <param name="location"></param>
		/// <param name="orientation"></param>
		/// <param name="parameters"></param>
		/**
		<remarks>
		begin create node
				get parameter map (e.g.)
					"graphcis_model"=>"some.mesh"
					"physics_weight"=>"100"
				map parameters to module from RequestMaps
					"graphics_model"=>Sage.Graphics.Module
					"physics_weight"=>Sage.Physics.Module
				generate features from registered modules
					=> Sage.Graphics.FeatureFactory.BeginCreateFeature(node)
					pass parameters
						=> Sage.Graphics.FeatureFactory.RequestMap[requestKey](node, requestKey, requestValue);
					=> Sage.Graphics.FeatureFactory.FinishCreateFeature()
					add features to node
						=> node.Features.Add(feature)
		finish create node
		add node to world
		</remarks>
		*/
		public virtual Node CreateNode(Node parent, string name, Vector location, Quaternion orientation)
		{
			//TODO: clean up messy CreateNode...

			Node node = new Node();
			// configure the node itself...
			node.Name = name;

			if (parent != null)
			{
				// if we have a configured parent, attach the node...
				node.Parent = parent;
			}
			else if (this.Root != null)
			{
				// no configured parent, attach child to root...
				node.Parent = this.Root;
			}
			return node;
		}

		#endregion Nodes

		#region Module
		protected override void DoInitialize()
		{
		}

		protected override void DoUpdate(long timeSinceLastUpdate)
		{
		}

		protected override void DoShutdown()
		{
		}
		#endregion Module
	}
}

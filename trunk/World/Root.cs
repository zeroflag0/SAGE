using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.World
{
	public class Root : Node
	{
		public Root()
		{
			this.Name = "World";
		}

		protected override Type[] AllowedChildrenCreate
		{
			get
			{
				return new Type[] { typeof(Tile) };
			}
		}

		public override Tile TileNode
		{
			get
			{
				return null;
			}
		}
	}
}

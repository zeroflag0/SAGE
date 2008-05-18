using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Graphics.Ogre
{
	public abstract class FeatureFactory<Feature> : Sage.Modules.FeatureFactory<Sage.Graphics.Ogre.Module, Feature>
		where Feature : Sage.Graphics.Ogre.Feature
	{
	}
}

using System;
namespace Sage.Modules
{
	public interface IFeature<T> : IFeature
	 where T : Module
	{
		T Module { get; set; }
	}
}

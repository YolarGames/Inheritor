using System;

namespace SimpleInjector
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class InjectSceneAttribute : Attribute
	{
		public InjectSceneAttribute() { }
	}
}
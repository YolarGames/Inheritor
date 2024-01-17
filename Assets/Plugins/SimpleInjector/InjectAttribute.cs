using System;

namespace SimpleInjector
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class InjectAttribute : Attribute
	{
		public InjectAttribute() { }
	}
}
using System.Reflection;

namespace SimpleInjector
{
	public static class Injector
	{
		public static void AddDependency<T0, T1>(T0 dependency, T1 depended) { }

		private const BindingFlags BINDING_FLAGS =
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
	}
}
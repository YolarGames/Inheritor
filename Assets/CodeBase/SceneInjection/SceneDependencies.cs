using System;
using System.Collections.Generic;
using System.Reflection;

namespace SceneInjection
{
	public struct SceneDependencies
	{
		public Type Root { get; }
		public HashSet<FieldInfo> Dependencies { get; }

		private SceneDependencies(ASceneRoot root, HashSet<FieldInfo> dependencies)
		{
			Root = root.GetType();
			Dependencies = dependencies;
		}

		public bool HasDependencies() =>
			Dependencies.Count > 0;

		public static SceneDependencies Create(ASceneRoot root) =>
			new(root, GetDependencies(root));

		private static HashSet<FieldInfo> GetDependencies(ASceneRoot root)
		{
			const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			var types = new HashSet<FieldInfo>();

			foreach (FieldInfo field in root.GetType().GetFields(BINDING_FLAGS))
			{
				if (field.HasAttribute(typeof(InjectSceneAttribute)) && typeof(ASceneRoot).IsAssignableFrom(field.FieldType))
					types.Add(field);
			}

			return types;
		}
	}
}
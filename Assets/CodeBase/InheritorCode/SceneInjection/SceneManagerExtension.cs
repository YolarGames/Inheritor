using System;
using System.Reflection;
using System.Text;

namespace SceneInjection
{
	public static class SceneManagerExtension
	{
		public static bool HasAttribute(this FieldInfo fieldInfo, Type attribute) =>
			Attribute.IsDefined(fieldInfo, attribute);

		public static string AsSceneRootName(this string str) =>
			str.Replace("Root", "").ToSnakeCase();

		private static string ToSnakeCase(this string str)
		{
			var sb = new StringBuilder();
			sb.Append(char.ToLowerInvariant(str[0]));

			for (int i = 1; i < str.Length; i++)
			{
				char c = str[i];

				if (char.IsLower(c))
				{
					sb.Append(c);
					continue;
				}

				sb.Append("_");
				sb.Append(char.ToLowerInvariant(c));
			}

			return sb.ToString();
		}
	}
}
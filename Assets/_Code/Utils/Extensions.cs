using System.Text;

namespace Utils
{
	public static class CustomExtensions
	{
		public static string AsSceneRootName(this string str)
		{
			str = str.Replace("Root", "");
			return str.ToSnakeCase();
		}

		public static string ToSnakeCase(this string str)
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
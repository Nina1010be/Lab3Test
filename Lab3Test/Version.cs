using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab3Test
{
    class Version
    {
		private readonly string preRelease;

		private readonly List<int> mainVersionParts;

		public Version(string version)
		{
			if (!IsCorrect(version))
			{
				throw new ArgumentException("Значение не корректно!");
			}

			mainVersionParts = new List<int>();

			var splitPreRelease = version.Split("-");

			mainVersionParts = splitPreRelease[0]
				.Split('.')
				.ToList()
				.ConvertAll(value => int.Parse(value));

			if (splitPreRelease.Length > 1)
			{
				preRelease = splitPreRelease[1];
			}
			else
			{
				preRelease = null;
			}
		}

		public static bool operator >(Version version1, Version version2)
		{
			return IsMore(version1, version2);
		}
		public static bool operator <(Version version1, Version version2)
		{
			return !IsMore(version1, version2);
		}

		private static bool IsCorrect(string version)
		{
			return Regex.IsMatch(version, @"\d+\.\d+\.\d+-?[\w+\.\w+]*");
		}
		private static void CompensationList(Version v1, Version v2)
		{
			while (v1.mainVersionParts.Count > v2.mainVersionParts.Count)
			{
				v2.mainVersionParts.Add(0);
			}
			while (v1.mainVersionParts.Count < v2.mainVersionParts.Count)
			{
				v1.mainVersionParts.Add(0);
			}
		}

		private static bool IsMore(Version v1, Version v2)
		{
			CompensationList(v1, v2);

			for (int i = 0; i < v1.mainVersionParts.Count; i++)
			{
				var version1 = v1.mainVersionParts[i];
				var version2 = v2.mainVersionParts[i];

				if (version1 > version2) return true;

				if (version1 == version2) continue;

				return false;
			}

			return ComparePreRelease(v1.preRelease, v2.preRelease) > 0;
		}

		private static int ComparePreRelease(string preRelease1, string preRelease2)
		{
			if (preRelease1 == null && preRelease2 != null) return 1;

			if (preRelease1 == null && preRelease2 == null) return 0;

			if (preRelease1 != null && preRelease2 == null) return -1;

			var splitPreRelease1 = preRelease1.Split(".");
			var splitPreRelease2 = preRelease2.Split(".");

			if (splitPreRelease1.Length > splitPreRelease2.Length) return 1;
			if (splitPreRelease1.Length < splitPreRelease2.Length) return -1;

			for (int i = 0; i < splitPreRelease1.Length; i++)
			{
				var compareResult = string.Compare(splitPreRelease1[i], splitPreRelease2[i]);

				if (compareResult == 0) continue;

				return compareResult;
			}

			return 0;
		}

		public static bool operator >=(Version version1, Version version2)
		{
			return IsMoreOrEqual(version1, version2);
		}

		public static bool operator <=(Version version1, Version version2)
		{
			return IsLessOrEqual(version1, version2);
		}

		private static bool IsMoreOrEqual(Version v1, Version v2)
		{
			CompensationList(v1, v2);

			for (int i = 0; i < v1.mainVersionParts.Count; i++)
			{
				var version1 = v1.mainVersionParts[i];
				var version2 = v2.mainVersionParts[i];

				if (version1 > version2) return true;

				if (version1 == version2) continue;

				return false;
			}

			return ComparePreRelease(v1.preRelease, v2.preRelease) >= 0;
		}

		private static bool IsLessOrEqual(Version v1, Version v2)
		{
			CompensationList(v1, v2);

			for (int i = 0; i < v1.mainVersionParts.Count; i++)
			{
				var version1 = v1.mainVersionParts[i];
				var version2 = v2.mainVersionParts[i];

				if (version1 < version2) return true;

				if (version1 == version2) continue;

				return false;
			}

			return ComparePreRelease(v1.preRelease, v2.preRelease) <= 0;
		}

		public static bool operator ==(Version version1, Version version2)
		{
			return IsEqual(version1, version2);
		}

		public static bool operator !=(Version version1, Version version2)
		{
			return !IsEqual(version1, version2);
		}

		private static bool IsEqual(Version v1, Version v2)
		{
			return v1.ToString() == v2.ToString();
		}

		public override string ToString()
		{
			if (preRelease != null)
			{
				return $"{mainVersionParts[0]}.{mainVersionParts[1]}.{mainVersionParts[2]}-{preRelease}";
			}

			return $"{mainVersionParts[0]}.{mainVersionParts[1]}.{mainVersionParts[2]}";
		}
	
}
}

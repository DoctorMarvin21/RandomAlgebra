using System.Globalization;
using System.Reflection;
using System.Resources;

namespace RandomAlgebra
{
    internal static class Resources
    {
        private static readonly ResourceManager ResourceManager
            = new ResourceManager(
                "RandomAlgebra.Resources.RandomAlgebra",
                Assembly.GetAssembly(typeof(Resources)));

        public static string GetMessage(string resourceKey)
        {
            return ResourceManager.GetString(resourceKey, CultureInfo.CurrentCulture);
        }
    }
}

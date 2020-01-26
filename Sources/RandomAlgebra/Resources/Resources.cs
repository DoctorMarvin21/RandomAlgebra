using System.Reflection;
using System.Resources;

namespace RandomAlgebra
{
    internal static class Resources
    {
        private static ResourceManager resourceManager
            = new ResourceManager("RandomAlgebra.Resources.RandomAlgebra",
                Assembly.GetAssembly(typeof(Resources)));

        public static string GetMessage(string resourceKey)
        {
            return resourceManager.GetString(resourceKey);
        }
    }
}

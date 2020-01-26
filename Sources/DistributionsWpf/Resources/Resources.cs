using System.Reflection;
using System.Resources;

namespace DistributionsWpf
{
    internal static class Resources
    {
        private static ResourceManager resourceManager
            = new ResourceManager("DistributionsWpf.Resources.Distributions",
                Assembly.GetAssembly(typeof(Resources)));

        public static string GetMessage(string resourceKey)
        {
            return resourceManager.GetString(resourceKey) ?? resourceKey;
        }
    }
}

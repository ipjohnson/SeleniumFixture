using System;
using System.Linq;
using System.Reflection;

namespace SeleniumFixture.xUnit.Impl
{
    static class SerializationHelper
    {
        /// <summary>
        /// Converts an assembly name + type name into a <see cref="Type"/> object.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="typeName">The type name.</param>
        /// <returns>The instance of the <see cref="Type"/>, if available; <c>null</c>, otherwise.</returns>
        public static Type GetType(string assemblyName, string typeName)
        {
#if XUNIT_FRAMEWORK    // This behavior is only for v2, and only done on the remote app domain side
            if (assemblyName.EndsWith(ExecutionHelper.SubstitutionToken, StringComparison.OrdinalIgnoreCase))
                assemblyName = assemblyName.Substring(0, assemblyName.Length - ExecutionHelper.SubstitutionToken.Length + 1) + ExecutionHelper.PlatformSuffix;
#endif

#if PLATFORM_DOTNET
            Assembly assembly = null;
            try
            {
                // Make sure we only use the short form
                var an = new AssemblyName(assemblyName);
                assembly = Assembly.Load(new AssemblyName { Name = an.Name, Version = an.Version });

            }
            catch { }
#else
            // Support both long name ("assembly, version=x.x.x.x, etc.") and short name ("assembly")
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyName || a.GetName().Name == assemblyName);
            if (assembly == null)
            {
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch { }
            }
#endif

            if (assembly == null)
                return null;

            return assembly.GetType(typeName);
        }
    }
}

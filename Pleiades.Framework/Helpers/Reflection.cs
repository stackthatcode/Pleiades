using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pleiades.Helpers
{
    public class Reflection
    {
        /// <summary>
        /// Get list of types in the calling assembly that inherits
        /// from a certain base type.
        /// </summary>
        /// <param name="baseType">The base type to check for.</param>
        /// <returns>A list of types that inherits from baseType.</returns>
        public static List<Type> GetClasses(Type baseType, Assembly callingAssembly)
        {
            return callingAssembly.GetTypes().Where(type => type.IsSubclassOf(baseType)).ToList();
        }

        public static object Construct(Type type, object[] args)
        {
            ConstructorInfo ctor = type.GetConstructor(new[] { type });
            return ctor.Invoke(args);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SLK.Web.Helpers
{
    public static class ReflectionHelper
    {
        public static IEnumerable<PropertyInfo> GetAllPropertiesWithAttribute(object instance, Type attributeType)
        {
            var result = new List<PropertyInfo>();
            ScanPropertiesForAttribute(instance, attributeType, result);

            return result;
        }
        private static void ScanPropertiesForAttribute(object instance, Type attributeType, List<PropertyInfo> result)
        {
            var properties = instance.GetType().GetProperties();
            result.AddRange(properties.Where(
                    prop => Attribute.IsDefined(prop, attributeType)));

            foreach (var property in properties)
            {
                var propType = property.PropertyType;
                //skip system types
                if (!propType.IsPrimitive && !propType.Namespace.StartsWith("System"))
                {
                    var value = property.GetValue(instance);
                    if (value != null)
                    {
                        ScanPropertiesForAttribute(value, attributeType, result);
                    }
                }
            }
        }

        public static bool HasPropertyWithAttribute(object instance, Type attributeType)
        {
            var properties = instance.GetType().GetProperties();
            var result = properties.Any(
                    prop => Attribute.IsDefined(prop, attributeType));
            
            if (result)
                return true;
            else
            {
                foreach (var property in properties)
                {
                    var propType = property.PropertyType;
                    //skip system types
                    if (!propType.IsPrimitive && !propType.Namespace.StartsWith("System"))
                    {
                        var value = property.GetValue(instance);
                        if (value != null)
                        {
                            result = HasPropertyWithAttribute(value, attributeType);
                            if (result)
                                return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}
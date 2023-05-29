using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Tsi.Core.Utilities.EnumUtilities
{
    public static class EnumOperations
    {
        public static List<EnumObject> GetObjects<T>() where T : struct
        {
            List<EnumObject> items = new List<EnumObject>();

            Type t = typeof(T);
            FieldInfo[] properties = t.GetFields(System.Reflection.BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo member in properties)
            {
                DisplayAttribute[] attributes = member.GetCustomAttributes(typeof(DisplayAttribute), true)
                    as DisplayAttribute[];

                int value = Convert.ToInt32(member.GetValue(null));

                EnumObject obj = new EnumObject();
                obj.Id = value;

                if (attributes.Length > 0)
                {
                    obj.Name = attributes[0].Name;
                }
                else
                {
                    obj.Name = Enum.GetName(typeof(T), value);
                }

                items.Add(obj);
            }

            return items;
        }
    }
}

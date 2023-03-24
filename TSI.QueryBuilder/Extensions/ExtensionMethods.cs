using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace TSI.QueryBuilder.Extensions
{
    public static class ExtensionMethods
    {
        public static List<T> DataReaderMapToList<T>(this IDataReader @this)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (@this.Read())
            {
                obj = Activator.CreateInstance<T>();

                int counter = 0;

                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(@this[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, @this[prop.Name], null);
                    }
                }
                list.Add(obj);
                counter++;
            }
            return list;
        }
    }
}

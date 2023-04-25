using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TSI.QueryBuilder.BaseClasses;

namespace TSI.QueryBuilder.Extensions
{
    public static class ExtensionMethods
    {
        public static List<T> DataReaderMapToList<T>(this IDataReader @this)
        {
            List<T> list = new List<T>();
            T obj = default(T);

            var columns = Enumerable.Range(0, @this.FieldCount).Select(@this.GetName).ToList();

            while (@this.Read())
            {
                obj = Activator.CreateInstance<T>();

                int counter = 0;

                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (columns.Contains(prop.Name))
                    {
                        if (@this.GetValue(@this.GetOrdinal(prop.Name)) != DBNull.Value)
                        {
                            if (!object.Equals(@this[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, @this[prop.Name], null);
                            }
                        }
                    }
                }
                list.Add(obj);
                counter++;
            }
            return list;
        }

        public static T DataReaderMapToGet<T>(this IDataReader @this)
        {
            T obj = Activator.CreateInstance<T>();

            var columns = Enumerable.Range(0, @this.FieldCount).Select(@this.GetName).ToList();

            while (@this.Read())
            {
                int counter = 0;

                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {

                    if (columns.Contains(prop.Name))
                    {
                        if (!object.Equals(@this[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, @this[prop.Name], null);
                        }
                    }
                }
                counter++;
            }
            return obj;
        }

        public static T[] DataReaderMapToArray<T>(this IDataReader @this)
        {
            return DataReaderMapToList<T>(@this).ToArray();
        }
    }
}

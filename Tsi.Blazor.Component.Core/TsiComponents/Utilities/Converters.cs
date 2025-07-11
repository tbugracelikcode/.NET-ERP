﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Tsi.Blazor.Component.Core.TsiComponents.Extensions;

namespace Tsi.Blazor.Component.Core.TsiComponents.Utilities
{
    public static class Converters
    {
        #region Constants

        private static readonly Type[] SimpleTypes =
        {
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(DateOnly),
            typeof(TimeSpan),
            typeof(TimeOnly),
            typeof(Guid)
        };

        #endregion

        #region Methods

        /// <summary>
        /// Converts an object to a dictionary object without the properties which have a null value or a [DataMember( EmitDefaultValue = false )] applied.
        /// This can be used as a workaround for System.Text.Json which will always serialize null values which breaks ChartJS functionality.
        /// </summary>
        /// <param name="source">The source object, can be null.</param>
        /// <param name="addEmptyObjects">Objects which do not have any properties are also added to the dictionary. Default value is true.</param>
        /// <param name="forceCamelCase">Force to use CamelCase, even if a DataMember has another casing defined. Default value is true.</param>
        /// <returns>Dictionary</returns>
        public static IDictionary<string, object> ToDictionary(object source, bool addEmptyObjects = true, bool forceCamelCase = true)
        {
            if (source == null)
            {
                return null;
            }

            var dictionary = new Dictionary<string, object>();

            object ProcessValue(object value, bool emitDefaultValue)
            {
                if (value != null && (emitDefaultValue || !IsEqualToDefaultValue(value)))
                {
                    var type = value.GetType();

                    if (IsSimpleType(type))
                    {
                        return value;
                    }

                    if (typeof(IEnumerable).IsAssignableFrom(type))
                    {
                        var list = new List<object>();
                        foreach (var item in (IEnumerable)value)
                        {
                            list.Add(ProcessValue(item, emitDefaultValue));
                        }

                        return type.IsArray ? (object)list.ToArray() : list;
                    }

                    if (value is LambdaExpression lambdaExpression)
                    {
                        return ExpressionConverter.ToTemplatedStringLiteral(lambdaExpression);
                    }

                    var dict = ToDictionary(value, addEmptyObjects);
                    if (addEmptyObjects || dict.Count > 0)
                    {
                        return dict;
                    }
                }

                return null;
            }

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                var dataMemberAttribute = property.Attributes.OfType<DataMemberAttribute>().FirstOrDefault();
                var emitDefaultValue = dataMemberAttribute?.EmitDefaultValue ?? true;

                var value = property.GetValue(source);
                var propertyName = dataMemberAttribute?.Name ?? property.Name;
                if (forceCamelCase)
                {
                    propertyName = propertyName.ToCamelcase();
                }

                if (value != null && (emitDefaultValue || !IsEqualToDefaultValue(value)))
                {
                    dictionary.Add(propertyName, ProcessValue(value, emitDefaultValue));
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="TValue">The type of object to return.</typeparam>
        /// <param name="value">An object that implements the <see cref="IConvertible"/> interface.</param>
        /// <returns>An object whose type is conversionType and whose value is equivalent to value.</returns>
        /// <remarks>
        /// https://stackoverflow.com/a/1107090/833106
        /// </remarks>
        public static TValue ChangeType<TValue>(object value)
        {
            Type conversionType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

            if (conversionType.IsEnum && EnumTryParse(value?.ToString(), conversionType, out TValue parsedValue))
                return parsedValue;

            try
            {
                return (TValue)Convert.ChangeType(value, conversionType);
            }
            catch
            {
                // If source value or TResult does not implement IConvertible the Convert.ChangeType will fail.
                // (One example is converting [Guid] to [object] type).
                //
                // So, as a fall-back mechanism we can just try casting it. It already failed so we can try this
                // additional step anyways.
                return (TValue)value;
            }
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <typeparam name="TValue">The type of object to return.</typeparam>
        /// <param name="value">An object that implements the <see cref="IConvertible"/> interface.</param>
        /// <param name="result">New instance of object whose value is equivalent to the specified object.</param>
        /// <param name="cultureInfo">Culture info to use for conversion.</param>
        /// <returns>True if conversion was successful.</returns>
        public static bool TryChangeType<TValue>(object value, out TValue result, CultureInfo cultureInfo = null)
        {
            try
            {
                Type conversionType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

                if (conversionType.IsEnum && EnumTryParse(value?.ToString(), conversionType, out TValue theEnum))
                    result = theEnum;
                else if (conversionType == typeof(Guid))
                    result = (TValue)Convert.ChangeType(Guid.Parse(value.ToString()), conversionType);
                else if (conversionType == typeof(DateOnly))
                    result = (TValue)Convert.ChangeType(DateOnly.Parse(value.ToString()), conversionType);
                else if (conversionType == typeof(DateTimeOffset))
                    result = (TValue)Convert.ChangeType(DateTimeOffset.Parse(value.ToString()), conversionType);
                else
                    result = (TValue)Convert.ChangeType(value, conversionType, cultureInfo ?? CultureInfo.InvariantCulture);

                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        // modified version of https://stackoverflow.com/a/11521834/833106
        public static bool EnumTryParse<TValue>(string input, Type conversionType, out TValue theEnum)
        {
            if (input != null)
            {
                foreach (string en in Enum.GetNames(conversionType))
                {
                    if (en.Equals(input, StringComparison.InvariantCultureIgnoreCase))
                    {
                        theEnum = (TValue)Enum.Parse(conversionType, input, true);
                        return true;
                    }
                }
            }

            theEnum = default;
            return false;
        }

        public static string FormatValue(byte value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(byte? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(short value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(short? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(int value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(int? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(long value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(long? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(float value, CultureInfo culture = null, int? decimals = null)
        {
            if (decimals != null)
                value = (float)Math.Round((double)value, decimals.Value, MidpointRounding.AwayFromZero);

            return value.ToString(culture ?? CultureInfo.CurrentCulture);
        }

        public static string FormatValue(float? value, CultureInfo culture = null, int? decimals = null)
        {
            if (value != null && decimals != null)
                value = (float)Math.Round((double)value.Value, decimals.Value, MidpointRounding.AwayFromZero);

            return value?.ToString(culture ?? CultureInfo.CurrentCulture);
        }

        public static string FormatValue(double value, CultureInfo culture = null, int? decimals = null)
        {
            if (decimals != null)
                value = Math.Round(value, decimals.Value, MidpointRounding.AwayFromZero);

            return value.ToString(culture ?? CultureInfo.CurrentCulture);
        }

        public static string FormatValue(double? value, CultureInfo culture = null, int? decimals = null)
        {
            if (value != null && decimals != null)
                value = Math.Round(value.Value, decimals.Value, MidpointRounding.AwayFromZero);

            return value?.ToString(culture ?? CultureInfo.CurrentCulture);
        }

        public static string FormatValue(decimal value, CultureInfo culture = null, int? decimals = null)
        {
            if (decimals != null)
                value = Math.Round(value, decimals.Value, MidpointRounding.AwayFromZero);

            return value.ToString(culture ?? CultureInfo.CurrentCulture);
        }

        public static string FormatValue(decimal? value, CultureInfo culture = null, int? decimals = null)
        {
            if (value != null && decimals != null)
                value = Math.Round(value.Value, decimals.Value, MidpointRounding.AwayFromZero);

            return value?.ToString(culture ?? CultureInfo.CurrentCulture);
        }

        public static string FormatValue(decimal? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(sbyte value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(sbyte? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(ushort value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(ushort? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(uint value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(uint? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(ulong value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(ulong? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(DateTime value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(DateTime? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(DateTimeOffset value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(DateTimeOffset? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(DateOnly value, CultureInfo culture = null) => value.ToString(culture ?? CultureInfo.CurrentCulture);

        public static string FormatValue(DateOnly? value, CultureInfo culture = null) => value?.ToString(culture ?? CultureInfo.CurrentCulture);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Gets the min and max possible value based on the supplied value type
        /// </summary>
        /// <typeparam name="TValue">Value data type.</typeparam>
        /// <returns>Returns the min and max value of supplied value type.</returns>
        /// <exception cref="System.InvalidOperationException">Throws when value type is unknown.</exception>
        public static (object, object) GetMinMaxValueOfType<TValue>()
        {
            var type = typeof(TValue);

            return type switch
            {
                Type byteType when byteType == typeof(byte) || byteType == typeof(byte?) => (byte.MinValue, byte.MaxValue),
                Type shortType when shortType == typeof(short) || shortType == typeof(short?) => (short.MinValue, short.MaxValue),
                Type intType when intType == typeof(int) || intType == typeof(int?) => (int.MinValue, int.MaxValue),
                Type longType when longType == typeof(long) || longType == typeof(long?) => (long.MinValue, long.MaxValue),
                Type floatType when floatType == typeof(float) || floatType == typeof(float?) => (float.MinValue, float.MaxValue),
                Type doubleType when doubleType == typeof(double) || doubleType == typeof(double?) => (double.MinValue, double.MaxValue),
                Type decimalType when decimalType == typeof(decimal) || decimalType == typeof(decimal?) => (decimal.MinValue, decimal.MaxValue),
                Type sbyteType when sbyteType == typeof(sbyte) || sbyteType == typeof(sbyte?) => (sbyte.MinValue, sbyte.MaxValue),
                Type ushortType when ushortType == typeof(ushort) || ushortType == typeof(ushort?) => (ushort.MinValue, ushort.MaxValue),
                Type uintType when uintType == typeof(uint) || uintType == typeof(uint?) => (uint.MinValue, uint.MaxValue),
                Type ulongType when ulongType == typeof(ulong) || ulongType == typeof(ulong?) => (ulong.MinValue, ulong.MaxValue),
                _ => throw new InvalidOperationException($"Unsupported type {type}"),
            };
        }

        private static bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                type.IsEnum ||
                SimpleTypes.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]));
        }

        private static bool IsEqualToDefaultValue<T>(T argument)
        {
            // deal with non-null nullables
            Type methodType = typeof(T);
            if (Nullable.GetUnderlyingType(methodType) != null)
            {
                return false;
            }

            // deal with boxed value types
            Type argumentType = argument.GetType();
            if (argumentType.IsValueType && argumentType != methodType)
            {
                object obj = Activator.CreateInstance(argument.GetType());
                return obj.Equals(argument);
            }

            return false;
        }

        #endregion
    }
}

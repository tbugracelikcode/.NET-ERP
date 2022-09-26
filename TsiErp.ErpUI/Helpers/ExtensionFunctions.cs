using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TsiErp.ErpUI.Helpers
{
    public static class ExtensionFunctions
    {
        public static Guid GetEntityId<TEntity>(this TEntity entity)
        {
            var property = typeof(TEntity).GetProperty("Id");
            return (Guid)property.GetValue(entity);
        }

        public static TItem SetSelectedItem<TItem>(this IList<TItem> listDataSource,
            int index)
        {
            int nextIndex = index;

            if (index == listDataSource.Count)
                nextIndex = index == 0 ? 0 : index - 1;

            if (listDataSource.Count > 0)
                return listDataSource[nextIndex];

            return default;
        }

        public static TEntity GetEntityById<TEntity>(this IList<TEntity> entities, Guid id)
        {
            var propertyInfo = typeof(TEntity).GetProperty("Id");

            foreach (var entity in entities)
                if (propertyInfo.GetValue(entity).Equals(id))
                    return entity;

            return default;
        }

        public static int FindIndex<T>(this IList<T> source, Predicate<T> selector)
        {
            for (var i = 0; i < source.Count; ++i)
            {
                if (selector(source[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static string[] RowHeights(params string[] rowHeights)
        {
            return rowHeights;
        }

        public static string[] ColumnWidths(params string[] columnWidths)
        {
            return columnWidths;
        }

        public static List<ComboBoxEnumItem<TEnum>> FillEnumToComboBox<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).OfType<TEnum>().Select(t => new ComboBoxEnumItem<TEnum>
            {
                Value = t,
                DisplayName = GetDisplayName(t)
            }).ToList();
        }

        public static string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }

    public class ComboBoxEnumItem<TEnum> where TEnum : Enum
    {
        public TEnum Value { get; set; }
        public string DisplayName { get; set; }
    }
}
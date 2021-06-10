using System;
using System.Collections.Generic;
using System.Linq;
using Skyline.DataMiner.Scripting;

namespace DataMiner.Generics
{
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedScripting.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("Interop.SLDms.dll")]
    public static class Extensions
    {
        /// <summary>
        /// A template extension method to the List&lt;object[]&gt; class to convert the provided list of object arrays into TQActionTableRow(s)
        /// </summary>
        /// <typeparam name="TQActionTableRow"></typeparam>
        /// <param name="rowsObjects"></param>
        /// <returns></returns>
        public static List<TQActionTableRow> ToQActionTableRowList<TQActionTableRow>(this IReadOnlyCollection<object[]> rowsObjects)
            where TQActionTableRow : QActionTableRow, new()
        {
            if (rowsObjects == null) throw new ArgumentNullException("rowsObjects");

            return rowsObjects.Select(aRow =>
            {
                return new TQActionTableRow
                {
                    Columns = aRow.Select((value, index) => new { value, index })
                        .ToDictionary(pair => pair.index, pair => pair.value)
                };
            }).ToList();
        }

        /// <summary>
        /// A template extension method to convert a list of TQActionTableRows to a list of object array.
        /// </summary>
        /// <typeparam name="TQActionTableRow"></typeparam>
        /// <param name="qActionTableRows"></param>
        /// <param name="sortColumns"></param>
        /// <returns></returns>
        public static List<object[]> ToObjectsList<TQActionTableRow>(this List<TQActionTableRow> qActionTableRows, bool sortColumns = true) where TQActionTableRow : QActionTableRow, new()
        {
            return sortColumns
                ? qActionTableRows.SortQActionTableRowColumnsByKey().Select(aRow => aRow.Columns.Values.ToArray()).ToList()
                : qActionTableRows.Select(aRow => aRow.Columns.Values.ToArray()).ToList();
        }

        /// <summary>
        /// A template extension method to convert a list of TQActionTableRows to columns array. It can be used when filling the table with columns instead of rows.
        /// </summary>
        /// <typeparam name="TQActionTableRow"></typeparam>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static object[] ToColumns<TQActionTableRow>(this List<TQActionTableRow> rows) where TQActionTableRow : QActionTableRow, new()
        {
            if (rows.Any())
            {
                return rows.SelectMany(aRow => aRow.Columns).OrderBy(aCol => aCol.Key)
                    .GroupBy(aCol => aCol.Key) /*Group the rows by their columns indexColumn (Key)*/
                    .Select(group => group.Select(pair => pair.Value).ToArray()) /* Select the value of each pair in each group */
                    .Select(value => (object)value).ToArray(); /*Convert each element from object[] to object  (the same structure as DM expects)*/
            }
            throw new ArgumentNullException("rows", "Unable to convert empty QAationTableRow list to columns ");
        }

        private static List<TQActionTableRow> SortQActionTableRowColumnsByKey<TQActionTableRow>(this List<TQActionTableRow> qActionTableRows) where TQActionTableRow : QActionTableRow, new()
        {
            foreach (var tableRow in qActionTableRows)
            {
                var keyValuePairs = tableRow.Columns.OrderBy(col => col.Key);
                tableRow.Columns = keyValuePairs.ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            return qActionTableRows;
        }
    }
}

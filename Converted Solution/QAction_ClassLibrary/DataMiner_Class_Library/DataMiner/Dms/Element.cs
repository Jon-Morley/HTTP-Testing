// ReSharper disable InlineOutVariableDeclaration

using DataMiner.Generics;

namespace DataMiner.Dms
{
    using Interop.SLDms;
    using Skyline.DataMiner.Library.Common.Attributes;
    using Skyline.DataMiner.Net.Messages;
    using Skyline.DataMiner.Scripting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [DllImport("SLManagedScripting.dll")]
    [DllImport("SLNetTypes.dll")]
    [DllImport("QActionHelperBaseClasses.dll")]
    public class Element : ObjectBase
    {
        /// <summary>
        /// The Protocol Name of the Element
        /// </summary>
        public string ProtocolName { get; set; }
        /// <summary>
        /// The Protocol Version of the Element
        /// </summary>
        public string ProtocolVersion { get; set; }

        public Element(uint dmaId, uint elementId) : base(dmaId, elementId)
        {
        }

        /// <summary>
        /// Set a custom name of a parameter
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="parameterName">The name to change the parameter to</param>
        /// <param name="pid">The parameter id</param>
        public static void SetParameterName(SLProtocol protocol, string parameterName, int pid)
        {
            var elementDetails = new[] { (uint)protocol.DataMinerID, (uint)protocol.ElementID };
            var update1 = new[]
            {
                "1",
                parameterName,
                Convert.ToString(pid)
            };
            var updates = new object[] { update1 };

            SetParameterName(protocol, elementDetails, updates);
        }

        /// <summary>
        /// Set custom names to multiple parameters
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="parameterName">The names to change the parameter to</param>
        /// <param name="pid">The parameter ids</param>
        public static void SetParameterName(SLProtocol protocol, string[] parameterName, int[] pid)
        {
            var pnCount = parameterName.Length;
            var pidCount = pid.Length;

            if (pnCount == pidCount)
            {
                var elementDetails = new[] { (uint)protocol.DataMinerID, (uint)protocol.ElementID };

                var updates = new object[pnCount];

                for (var i = 0; i < pnCount; i++)
                {
                    var update = new[]
                    {
                        "1",
                        parameterName[i],
                        Convert.ToString(pid[i])
                    };
                    updates[i] = update;
                }

                SetParameterName(protocol, elementDetails, updates);
            }
            else
            {
                protocol.Log("QA" + protocol.QActionID +
                             "|SetParameterName|parameterName length " + pnCount + " didn't match pid length " + pidCount,
                    LogType.Error, LogLevel.NoLogging);
            }
        }

        private static void SetParameterName(SLProtocol protocol, uint[] elementDetails, object[] updates)
        {
            var response = protocol.NotifyDataMinerQueued(127 /*NT_UPDATE_DESCRIPTION_XML */, elementDetails, updates);

            if (response != 0)
            {
                protocol.Log("QA" + protocol.QActionID + "|SetParameterName|Failed with " + response,
                    LogType.Error, LogLevel.NoLogging);
            }
        }

        /// <summary>
        /// Get the custom properties of an element
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <returns>Array of the custom properties of the Element</returns>
        public PropertyInfo[] GetCustomProperties(SLProtocol protocol)
        {
            var getElementByIdMessage = new GetElementByIDMessage { DataMinerID = protocol.DataMinerID, ElementID = protocol.ElementID };
            var dmsMessage = protocol.SLNet.SendMessage(getElementByIdMessage);
            return ((ElementInfoEventMessage)dmsMessage[0]).Properties;
        }
        /// <summary>
        /// Get the elements using the protocol name and protocol version
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <returns>List of the Elements that use the same Protocol Name and Version as this Element</returns>
        public List<string> GetElementsIdsByProtocol(SLProtocol protocol)
        {
            var protocolDetails = new[] { ProtocolName, ProtocolVersion };
            var dms = new DMS();
            object result;
            dms.Notify(102 /* DMS_GET_ELEMENTS_USING_PROTOCOL */, 0, protocolDetails, null, out result);
            return ((object[])result).Select(Convert.ToString).ToList();
        }

        /// <summary>
        /// Gets the current operational state of the element (Active, Paused, ..etc)
        /// </summary>
        /// <returns>ElementState value</returns>
        public ElementState GetState()
        {
            object result;
            Dms.Notify(91 /*DMS_GET_ELEMENT_Status*/, 0, dmaId, elementId, out result);
            return (ElementState)Enum.Parse(typeof(ElementState), result.ToString());
        }

        /// <summary>
        /// Deletes all rows of the table
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="tablePid">The table id</param>
        public static void ClearTable(SLProtocol protocol, int tablePid)
        {
            protocol.DeleteRow(tablePid, protocol.GetKeys(tablePid));
        }

        /// <summary>
        /// A Template method to get the table rows from any QActionTable and cast them to the provided type.
        /// </summary>
        /// <typeparam name="TQActionTableRow">The QActionTableRow static class of the table</typeparam>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="tablePid">The table Id, for best practice use the following form: Parameter.[TableName].tablePid</param>
        /// <returns>List of the table rows</returns>
        // todo table id as local class field and add constructor and non static version of these
        // methods that doesn't take the table ID
        public static List<TQActionTableRow> GetRows<TQActionTableRow>(SLProtocol protocol, int tablePid)
            where TQActionTableRow : QActionTableRow, new()
        {
            var testRow = new TQActionTableRow();
            var allTableRows = GetRows(protocol, tablePid, (uint)testRow.ColumnCount);
            return allTableRows.ToQActionTableRowList<TQActionTableRow>();
        }

        /// <summary>
        /// Get the rows of a table as objects
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="tablePid">The table Pid</param>
        /// <param name="columnCount">The columns count of the table</param>
        /// <returns>List of all table rows as objects</returns>
        public static List<object[]> GetRows(SLProtocol protocol, int tablePid, uint columnCount)
        {
            var indexes = new uint[columnCount];
            for (uint iIndex = 0; iIndex < indexes.Length; iIndex++)
            {
                indexes[iIndex] = iIndex;
            }

            return GetRows(protocol, tablePid, indexes, columnCount);
        }

        /// <summary>
        /// Get the rows of a table as objects
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="tablePid">The table Pid</param>
        /// <param name="indexes">The indices of the columns to be returned</param>
        /// <param name="columnCount">The columns count of the table (optional)</param>
        /// <returns></returns>
        public static List<object[]> GetRows(SLProtocol protocol, int tablePid, uint[] indexes, uint? columnCount = null)
        {
            var allColumnData = GetColumns(protocol, tablePid, indexes);
            var firstColumn = allColumnData[0] as object[];
            var allRows = new List<object[]>();

            if (firstColumn == null) return allRows;

            allRows = new List<object[]>(firstColumn.Length);

            var currentColumnCount = columnCount.HasValue ? columnCount.Value : indexes.Max() + 1;
            for (int iRow = firstColumn.GetLowerBound(0); iRow <= firstColumn.GetUpperBound(0); iRow++)
            {
                var row = new object[currentColumnCount];
                for (var iIndex = indexes.GetLowerBound(0); iIndex <= indexes.GetUpperBound(0); iIndex++)
                {
                    var column = indexes[iIndex];
                    var columnData = allColumnData[iIndex] as object[];
                    if (columnData != null)
                    {
                        row[column] = columnData[iRow];
                    }
                }

                allRows.Add(row);
            }
            return allRows;
        }

        /// <summary>
        /// Get the columns of the table
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="iTablePid">The table Pid</param>
        /// <param name="columnIdx">The indices of the columns to be returned</param>
        /// <returns></returns>
        public static object[] GetColumns(SLProtocol protocol, int iTablePid, uint[] columnIdx)
        {
            return (object[])protocol.NotifyProtocol(321, iTablePid, columnIdx);
        }

        /// <summary>
        /// Convert the provided Columns array to a QActionTableRow
        /// </summary>
        /// <typeparam name="TQActionTableRow">The type of the returned rows</typeparam>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="columns">Array of columns to be converted</param>
        /// <returns>List of rows of the type specified in the &lt;TypeParam&gt;</returns>
        public static List<TQActionTableRow> ColumnsToRows<TQActionTableRow>(SLProtocol protocol, object[] columns) where TQActionTableRow : QActionTableRow, new()
        {
            return ColumnsToRows(protocol, columns).Select(aRow =>
            {
                return new TQActionTableRow()
                {
                    Columns = aRow.Select((value, index) => new { value, index }).ToDictionary(pair => pair.index, pair => pair.value)
                };
            }).ToList();
        }

        /// <summary>
        /// Convert the provided Columns array to list of objects Rows
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="columns">Array of columns to be converted</param>
        /// <returns>Rows as List of Objects</returns>
        public static List<object[]> ColumnsToRows(SLProtocol protocol, object[] columns)
        {
            if (columns == null) return new List<object[]>();
            var rowCount = ((object[])columns[0]).Length;
            var rows = new List<object[]>();

            for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                var row = new List<object>();
                for (var columnIndex = 0; columnIndex < columns.Length; columnIndex++)
                {
                    row.Add(((object[])columns[columnIndex])[rowIndex] != null
                                ? ((object[])((object[])columns[columnIndex])[rowIndex])[0]
                                : string.Empty);
                }
                rows.Add(row.ToArray());
            }
            return rows;
        }


    }
}
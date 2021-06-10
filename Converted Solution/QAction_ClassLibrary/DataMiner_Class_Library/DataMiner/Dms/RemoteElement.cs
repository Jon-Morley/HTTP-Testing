// ReSharper disable InlineOutVariableDeclaration
// ReSharper disable UseNullPropagation

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
    [DllImport("Interop.SLDms.dll")]
    public class RemoteElement : ObjectBase
    {

        public RemoteElement(uint dmaId, uint elementId) : base(dmaId, elementId)
        {
        }

        /// <summary>
        /// Gets the custom properties of a Remote Element
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaId">DMA Id</param>
        /// <param name="elementId">Element Id</param>
        /// <returns>An array of the Skyline.DataMiner.Net.Messages.PropertyInfo</returns>
        public static PropertyInfo[] GetCustomProperties(SLProtocol protocol, int dmaId, int elementId)
        {
            var getElementByIdMessage = new GetElementByIDMessage { DataMinerID = dmaId, ElementID = elementId };
            var dmsMessage = protocol.SLNet.SendMessage(getElementByIdMessage);
            return ((ServiceInfoEventMessage)dmsMessage[0]).Properties;
        }

        /// <summary>
        /// Gets the element Id by element Name
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns>string: "DMA/Element" id</returns>
        public static string GetElementIdByElementName(string elementName)
        {
            var dms = new DMS();
            object elementId;
            dms.Notify(72 /* DMS_GET_ELEMENT_ID */, 0, elementName, null, out elementId);
            return (string)elementId;
        }

        /// <summary>
        /// Gets the element Id by IP Address (optional: BusAddress).
        /// It can be used to confirm if there is an element with the same IP and bus address
        /// </summary>
        /// <param name="ipAddress">The IP address of the element</param>
        /// <param name="busAddress">The Bus address of the element</param>
        /// <returns>string: "DMA/Element" id. Empty string if not found!</returns>
        public static string GetElementIdByIpAddress(string ipAddress, string busAddress = null)
        {
            object elementId;
            var connectionSettings = new[] { ipAddress, busAddress };
            Dms.Notify(76, 0, connectionSettings, null, out elementId);
            return elementId == null ? string.Empty : (string)elementId;
        }

        /// <summary>
        /// Get the elements using the protocol name and protocol version
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="protocolName">The Protocol Name</param>
        /// <param name="protocolVersion">The Protocol Version. Default value is: "Production"</param>
        /// <returns>List of the Elements that use the provided Protocol Name and Version</returns>
        public static List<string> GetElementsIdsByProtocol(SLProtocol protocol, string protocolName, string protocolVersion = "Production")
        {
            var protocolDetails = new[] { protocolName, protocolVersion };
            var dms = new DMS();
            object result;
            dms.Notify(102 /* DMS_GET_ELEMENTS_USING_PROTOCOL */, 0, protocolDetails, null, out result);
            return ((object[])result).Select(Convert.ToString).ToList();
        }

        /// <summary>
        /// Gets Information about the element including element name, description, alarm template, ...etc.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaId">DMA Id</param>
        /// <param name="elementId">Element Id</param>
        /// <returns>LiteElementInfoEvent object</returns>
        public static LiteElementInfoEvent GetInfo(SLProtocol protocol, int dmaId, int elementId)
        {
            var getLiteElementInfoMsg = new GetLiteElementInfo { DataMinerID = dmaId, ElementID = elementId };
            return protocol.SLNet.SendSingleResponseMessage(getLiteElementInfoMsg) as LiteElementInfoEvent;
        }

        /// <summary>
        /// Gets the remote element Name by its Id
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaId">DMA Id</param>
        /// <param name="elementId">Element Id</param>
        /// <returns>result (object): Holds the returned element name (empty string if there is no element with the
        /// specified element ID on the specified DataMiner Agent).</returns>
        public static string GetNameById(SLProtocol protocol, uint dmaId, uint elementId)
        {
            var dms = new DMS();
            object result;

            // Get the element name from DMS
            dms.Notify(67, 0, dmaId, elementId, out result);
            return (string)result;
        }

        /// <summary>
        /// Gets the current operational (Active, Paused, ..etc) by Element Name.
        /// </summary>
        /// <param name="elementName">The Element Name</param>
        /// <returns>ElementState value</returns>
        public static ElementState GetState(string elementName)
        {
            var elementId = GetElementIdByElementName(elementName);
            if (string.IsNullOrEmpty(elementId)) return ElementState.Undefined;
            var elementIdParts = elementId.Split('/').Select(x => Convert.ToUInt32(x)).ToArray();
            object result;
            Dms.Notify(91 /*DMS_GET_ELEMENT_Status*/, 0, elementIdParts[0], elementIdParts[1], out result);
            return (ElementState)Enum.Parse(typeof(ElementState), result.ToString());
        }

        /// <summary>
        /// Gets the current operational (Active, Paused, ..etc) by Element Id.
        /// </summary>
        /// <param name="dmaId">The DMA id</param>
        /// <param name="elementId">The Element id</param>
        /// <returns>ElementState value</returns>
        public static ElementState GetState(uint dmaId, uint elementId)
        {
            object result;
            Dms.Notify(91 /*DMS_GET_ELEMENT_Status*/, 0, dmaId, elementId, out result);
            return (ElementState)Enum.Parse(typeof(ElementState), result.ToString());
        }

        /// <summary>
        /// Checks if an element is exist by Element Name
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static bool Exists(SLProtocol protocol, string elementName)
        {
            var result = protocol.NotifyDataMiner(323 /*NT_DOES_ELEMENT_EXIST*/, elementName, null);
            return result != null;
        }


        /// <summary>
        /// Uses the GetParameterAsObject Method to get the table information
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaElementId"></param>
        /// <param name="tableId"></param>
        /// <param name="isService"></param>
        /// <returns></returns>
        public static object[] GetTableInfo(SLProtocol protocol, uint[] dmaElementId, int tableId, bool isService = false)
        {
            return (object[])GetParameterAsObject(protocol, dmaElementId, tableId, isService);
        }

        /// <summary>
        /// A Template method to get the table rows from any QActionTable and cast them to the provided type.
        /// </summary>
        /// <typeparam name="TQActionTableRow">The QActionTableRow static class of the table</typeparam>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="tableId">The table Id, for best practice use the following form: Parameter.[TableName].tablePid</param>
        /// <param name="dmaElementId">An array of the parts of the DMA/Element Id. index-0 is the DMA id. index-1 is the Element id</param>
        /// <param name="isService">Indicates if the parameter is a parameter of a service</param>
        /// <returns>List of the table rows</returns>
        public static List<TQActionTableRow> GetRows<TQActionTableRow>(SLProtocol protocol, int tableId, uint[] dmaElementId, bool isService = false) where TQActionTableRow : QActionTableRow, new()
        {
            return GetRows(protocol, tableId, dmaElementId, isService).Select(aRow =>
            {
                return new TQActionTableRow
                {
                    Columns = aRow.Select((value, index) => new { value, index }).ToDictionary(pair => pair.index, pair => pair.value)
                };
            }).ToList();
        }

        /// <summary>
        /// Get the rows of a table as objects
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="tableId">The table Id, for best practice use the following form: Parameter.[TableName].tablePid</param>
        /// <param name="dmaElementId">An array of the parts of the DMA/Element Id. index-0 is the DMA id. index-1 is the Element id</param>
        /// <param name="isService">Indicates if the parameter is a parameter of a service</param>
        /// <returns>List of all table rows as objects</returns>
        public static List<object[]> GetRows(SLProtocol protocol, int tableId, uint[] dmaElementId, bool isService = false)
        {
            var table = GetTableInfo(protocol, dmaElementId, tableId, isService);
            var columns = (object[])table[4];
            if (columns == null) return new List<object[]>();

            var rowsCount = ((object[])columns[0]).Length;
            var rows = new List<object[]>();

            for (var rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                rows.Add(columns.Select(column => ((object[])column)[rowIndex] == null ? "-1" : ((object[])((object[])column)[rowIndex])[0]).ToArray());
            }
            return rows;
        }

        /// <summary>
        /// Get the columns of the table
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="tableId">The table Pid</param>
        /// <param name="columnIndexes">The indices of the columns to be returned</param>
        /// <param name="dmaElementId">An array of the parts of the DMA/Element Id. index-0 is the DMA id. index-1 is the Element id</param>
        /// <param name="isService">Indicates if the parameter is a parameter of a service</param>
        /// <returns>an object array of the columns of the table</returns>
        public static object[] GetColumns(SLProtocol protocol, int tableId, uint[] columnIndexes, uint[] dmaElementId, bool isService = false)
        {

            var table = GetTableInfo(protocol, dmaElementId, tableId, isService);

            var tableColumns = (object[])table.ElementAtOrDefault(4);
            if (tableColumns == null) return null;

            var columns = new object[columnIndexes.Length];
            for (var i = 0; i < columnIndexes.Length; i++)
            {
                var tableColumn = (object[])tableColumns.ElementAtOrDefault((int)columnIndexes[i]);
                if (tableColumn == null)
                {
                    columns[i] = null;
                    continue;
                }

                var rowCount = tableColumn.Length;
                var column = new object[rowCount];

                for (var j = 0; j < rowCount; j++) column[j] = ((object[])tableColumn[j])[0];

                columns[i] = column;
            }

            return columns;
        }

        /// <summary>
        /// Gets the Row by row index
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="tableId">The table Pid</param>
        /// <param name="dmaElementId">An array of the parts of the DMA/Element Id. index-0 is the DMA id. index-1 is the Element id</param>
        /// <param name="rowIndex">The index of the row in the table</param>
        /// <param name="isService">Indicates if the parameter is a parameter of a service</param>
        /// <returns>Object array of the row</returns>
        public static object[] GetRow(SLProtocol protocol, int tableId, uint[] dmaElementId, int rowIndex, bool isService = false)
        {
            var table = GetTableInfo(protocol, dmaElementId, tableId, isService);
            var tableColumns = (object[])table.ElementAtOrDefault(4);
            if (tableColumns == null) return null;
            var columnCount = tableColumns.Length;
            var row = new object[columnCount];

            for (var columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                row[columnIndex] = ((object[])((object[])tableColumns[columnIndex])[rowIndex])[0];
            }
            return row;
        }

        /// <summary>
        /// Gets the Row by row Key
        /// </summary>
        /// <param name="protocol">The current instance of the SLProtocol</param>
        /// <param name="tableId">The table Pid</param>
        /// <param name="dmaElementId">An array of the parts of the DMA/Element Id. index-0 is the DMA id. index-1 is the Element id</param>
        /// <param name="rowKey">The Key of the row in the table</param>
        /// <param name="isService">Indicates if the parameter is a parameter of a service</param>
        /// <returns>Object array of the row</returns>
        public static object[] GetRow(SLProtocol protocol, int tableId, uint[] dmaElementId, string rowKey, bool isService = false)
        {
            var rowIdentification = new object[]
            {
                (int) dmaElementId[0], isService ? ((int) dmaElementId[1] + 1) : (int) dmaElementId[1], tableId, rowKey
            };
            var rowData = (object[])protocol.NotifyDataMiner(215, rowIdentification, null);
            return rowData;
        }

        public static object GetTableCell(SLProtocol protocol, int tableId, uint[] dmaElementId, int columnIndex, int rowIndex, bool isService = false)
        {
            var table = GetTableInfo(protocol, dmaElementId, tableId, isService);
            var tableColumns = (object[])table.ElementAtOrDefault(4);
            if (tableColumns == null) return null;
            var column = (object[])tableColumns.ElementAtOrDefault(columnIndex);
            if (column == null) return null;
            var row = (object[])column.ElementAtOrDefault(rowIndex);
            if (row == null) return null;
            return row[0];
        }

        /// <summary>
        /// Adds a row to a remote table of an Element/Service
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="tableId"></param>
        /// <param name="dmaElementId"></param>
        /// <param name="rowData"></param>
        /// <param name="isService"></param>
        public static void AddRow(SLProtocol protocol, int tableId, uint[] dmaElementId, object[] rowData, bool isService = false)
        {
            uint[] sids = { dmaElementId[0], isService ? (dmaElementId[1] + 1) : dmaElementId[1], (uint)tableId };
            protocol.NotifyDataMiner(149 /*NT_ADD_ROW*/, sids, rowData);
        }

        /// <summary>
        /// Deletes a row from a table of a remote elamen
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="tableId"></param>
        /// <param name="dmaElementId"></param>
        /// <param name="rowKey"></param>
        /// <param name="isService">Indicates if the parameter is a parameter of a service</param>
        public static void DeleteRow(SLProtocol protocol, int tableId, uint[] dmaElementId, string rowKey, bool isService = false)
        {
            uint[] sids = { dmaElementId[0], isService ? (dmaElementId[1] + 1) : dmaElementId[1], (uint)tableId };
            protocol.NotifyDataMiner(156 /*NT_DELETE_ROW*/, sids, rowKey);
        }

        /// <summary>
        /// Delete a list of rows from a remote element
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="tableId"></param>
        /// <param name="dmaElementId"></param>
        /// <param name="rows"> rows ro be deleted</param>
        /// <param name="isService">Indicates if the parameter is a parameter of a service</param>
        public static void DeleteRow(SLProtocol protocol, int tableId, uint[] dmaElementId, string[] rows, bool isService = false)
        {
            uint[] sids = { dmaElementId[0], isService ? (dmaElementId[1] + 1) : dmaElementId[1], (uint)tableId };
            protocol.NotifyDataMiner(156 /*NT_DELETE_ROW*/, sids, rows);
        }

        /// <summary>
        /// Clears a remote element/service Table
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="tableId"></param>
        /// <param name="dmaElementId"></param>
        /// <param name="indexColumn">The index of the Keys column</param>
        /// <param name="isService">Indicates if the parameter is a parameter of a service</param>
        public static void ClearTable(SLProtocol protocol, int tableId, uint[] dmaElementId, int indexColumn = 0, bool isService = false)
        {
            var table = GetTableInfo(protocol, dmaElementId, tableId, isService);

            var tableColumns = (object[])table.ElementAtOrDefault(4);
            if (tableColumns == null) return;

            var column = (object[])tableColumns.ElementAtOrDefault(indexColumn);
            if (column == null) return;

            var rowCount = column.Length;
            var keys = new string[rowCount];

            for (var i = 0; i < rowCount; i++) keys[i] = ((object[])column[i])[0].ToString();

            DeleteRow(protocol, tableId, dmaElementId, keys, isService);
        }

    }
}
// ReSharper disable InlineOutVariableDeclaration

namespace DataMiner.Dms
{
    using Interop.SLDms;

    using Skyline.DataMiner.Library.Common.Attributes;
    using Skyline.DataMiner.Scripting;

    [DllImport("SLManagedScripting.dll")]
    [DllImport("SLNetTypes.dll")]
    [DllImport("Interop.SLDms.dll")]
    public class ObjectBase
    {
        //MIGHT NEED TO CHANGE BACK TO DMSCLASS SEE PAGE 211 OF BIBLE
        public static readonly DMS Dms = new DMS();

        public readonly uint dmaId;

        public readonly uint elementId;

        public ObjectBase(uint dmaId, uint elementId)
        {
            this.dmaId = dmaId;
            this.elementId = elementId;
        }

        /// <summary>
        /// Get the value of the parameter
        /// </summary>
        /// <param name="paramId">The id of the parameter</param>
        /// <returns>The parameter value as an object</returns>
        public object GetParameter(int paramId)
        {
            object retValue;
            Dms.Notify(87 /*DMS_GET_VALUE*/, 0, new[] { dmaId, elementId }, paramId, out retValue);
            return (retValue as object[] ?? new object[5])[4];
        }

        /// <summary>
        /// Retrieves details about a parameter (including the value)
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaElementId">The "DMA/Element" id</param>
        /// <param name="parameterId">The parameter id</param>
        /// <param name="isService">Indicates if the parameter is a parameter of a service</param>
        /// <returns>
        ///  result (object)
        /// - In case a standalone parameter was retrieved, the returned object is an object array containing the following information:
        ///    result[0] (int): parameter ID
        ///    result[1] (string): parameter description
        ///    result[2] (string):
        ///    result[3] (string): unit
        ///    result[4]: Parameter value, type depends on parameter type.
        ///    result[5] (int):
        ///    result[6] (string):
        ///    result[7] (DateTime):
        ///    result[8] (DateTime):
        ///    result[9] (string): Name of the user who last changed the parameter.
        ///    result[10] (int): Alarm status (1: Normal, 2: Warning, 3: Minor, 4: Major, 5: Critical)
        ///    result[11] (int):
        ///    result[12] (int): 
        /// - In case a table or matrix was retrieved, the returned object is an object array. result[4] will
        /// hold the table data and be of type object[]. Each element from this array represents a column.
        /// </returns>
        public static object GetParameterAsObject(SLProtocol protocol,uint[] dmaElementId, int parameterId, bool isService = false)
        {
            var ids = new[] {dmaElementId[0], isService ? (dmaElementId[1] + 1) : dmaElementId[1]};
            var dms = new DMS();
            object result;
            dms.Notify(87 /*DMS_GET_VALUE*/, 0, ids, parameterId, out result);
            return result;
        }
        /// <summary>
        /// Gets the value of the parameter
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaElementId"></param>
        /// <param name="parameterId"></param>
        /// <param name="isService"></param>
        /// <returns></returns>
        public static object GetParameter(SLProtocol protocol,uint[] dmaElementId, int parameterId, bool isService = false)
        {
            object result = GetParameterAsObject(protocol, dmaElementId, parameterId, isService);
            return (result as object[] ?? new object[5])[4];
        }

        /// <summary>
        /// Sets a parameter.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="paramId">The parameter Id</param>
        /// <param name="value">The value to be assigned to the parameter</param>
        public void SetParameter(SLProtocol protocol, uint paramId, object value)
        {
            uint[] sids = { dmaId, elementId, paramId };
            protocol.NotifyDataMiner(50 /*NT_SET_PARAMETER*/, sids, value);
        }

        /// <summary>
        /// Set a parameter in a table
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="primaryKey">The key of the row in thetable</param>
        /// <param name="paramId"> The parameter id</param>
        /// <param name="value">The value to be assigned to the parameter</param>
        public void SetParameter(SLProtocol protocol, string primaryKey, uint paramId, object value)
        {
            uint[] sids = { dmaId, elementId, paramId };
            object[] parameterDetails = { primaryKey, value };
            protocol.NotifyDataMiner(50 /*NT_SET_PARAMETER*/, sids, parameterDetails);
        }

        /// <summary>
        /// Set Parameter value.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaId">The DMA id</param>
        /// <param name="elementId">The Element id</param>
        /// <param name="paramId"> The parameter id</param>
        /// <param name="value">The value</param>
        public static void SetParameter(SLProtocol protocol, uint dmaId, uint elementId, uint paramId, object value)
        {
            uint[] sids = { dmaId, elementId, paramId };
            protocol.NotifyDataMiner(50 /*NT_SET_PARAMETER*/, sids, value);
        }

        /// <summary>
        /// Set a Parameter of a Row in a table
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaId">The DMA id</param>
        /// <param name="elementId">The Element id</param>
        /// <param name="primaryKey">The primary key of the row</param>
        /// <param name="paramId"> The parameter id</param>
        /// <param name="value">The value</param>
        public static void SetParameter(SLProtocol protocol, uint dmaId, uint elementId, string primaryKey, uint paramId, object value)
        {
            uint[] sids = { dmaId, elementId, paramId };
            object[] parameterDetails = { primaryKey, value };
            protocol.NotifyDataMiner(50 /*NT_SET_PARAMETER*/, sids, parameterDetails);
        }
    }
}
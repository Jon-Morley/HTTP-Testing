namespace DataMiner.Dms
{
    using Skyline.DataMiner.Net.Messages;
    using Skyline.DataMiner.Scripting;

    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedScripting.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("Interop.SLDms.dll")]
    public class RemoteService : ObjectBase
    {
        public RemoteService(uint dmaId, uint elementId) : base(dmaId, elementId)
        {
        }
        /// <summary>
        /// Gets the custom properties of a Remote Service
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaId">DMA Id</param>
        /// <param name="elementId">Element Id</param>
        /// <returns>An array of the Skyline.DataMiner.Net.Messages.PropertyInfo</returns>
        public static PropertyInfo[] GetCustomProperties(SLProtocol protocol, int dmaId, int elementId)
        {
            var getServiceByIdMessage = new GetServiceByIDMessage { DataMinerID = dmaId, ServiceID = elementId - 1 };
            var dmsMessage = protocol.SLNet.SendMessage(getServiceByIdMessage);
            return ((ServiceInfoEventMessage)dmsMessage[0]).Properties;
        }
    }
}
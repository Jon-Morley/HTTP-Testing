using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMiner.Dms
{
    using Skyline.DataMiner.Scripting;

    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedScripting.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("Interop.SLDms.dll")]
    public class View
    {
        /// <summary>
        /// Gets the id of the view by its name
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static int GetIdByName(SLProtocol protocol, string viewName)
        {
            var viewId = Convert.ToInt32(protocol.NotifyDataMiner(179 /*NT_GET_VIEW_ID */ , viewName, null));
            return viewId;
        }

        /// <summary>
        /// Gets the View Properties including the custom properties.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="iViewId"></param>
        /// <returns></returns>
        public static object[] GetProperties(SLProtocol protocol, int iViewId)
        {
            var osViewProperties = (object[])protocol.NotifyDataMiner(316/*NT_GET_VIEW_PROPERTIES*/ , iViewId, 0);
            return osViewProperties;
        }

        /// <summary>
        /// Gets the View Names that host an element
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaIdParts"></param>
        /// <returns></returns>
        public static List<string> GetNamesByElementId(SLProtocol protocol, uint[] dmaIdParts)
        {
            //Get views Ids where this element is listed
            var result = GetViewsIdsOfElement(protocol, dmaIdParts);
            List<string> names = new List<string>();
            if (result.Any())
            {
                foreach (var viewId in (uint[]) result)
                {
                    names.Add(GetName(protocol, viewId));
                }
            }
            return names;
        }

        /// <summary>
        /// Gets the Views Ids of an element
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="dmaElementId"></param>
        /// <returns></returns>
        public static uint[] GetViewsIdsOfElement(SLProtocol protocol, uint[] dmaElementId)
        {
            var result = protocol.NotifyDataMiner(176 /*NT_GET_VIEWS_OF_ELEMENT*/, (int)dmaElementId[0], (int)dmaElementId[1]);

            if (result == null)
                return new uint[0];
            return (uint[]) result;
        }

        /// <summary>
        /// Gets the name of the view by its Id
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static string GetName(SLProtocol protocol, uint viewId)
        {
            return (string)protocol.NotifyDataMiner(303 /*NT_GET_VIEW_NAME*/, viewId, null);
        }
    }
}
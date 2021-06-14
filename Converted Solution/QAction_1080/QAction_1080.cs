using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Skyline.DataMiner.Library.Common;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Scripting;
using Parameter = Skyline.DataMiner.Scripting.Parameter;

/// <summary>
/// 
/// DataMiner QAction Class: Poll endpoints.
/// </summary>
public static class QAction
{
	/// <summary>
	/// The QAction entry point.
	/// </summary>
	/// <param name="protocol">Link with SLProtocol process.</param>
	public static void Run(SLProtocolExt protocol)
	{
		try
		{
			List<EndpointsQActionRow> rowObjects = new List<EndpointsQActionRow>();

			EndpointsQActionTable table = protocol.endpoints;
			for(int i =0; i< table.RowCount; i++)
			{
				object[] rowData = table.GetRow(i);
				EndpointsQActionRow nextRow = (EndpointsQActionRow)rowData;
				rowObjects.Add(nextRow);
			}



			//debug
			protocol.SetParameter(Parameter.http_debug_1081, "QAction 1080 ran at " + DateTime.Now.ToString() + " with " + rowObjects.Count.ToString() + "entries in the table");			
		}
		catch (Exception ex)
		{
			protocol.Log("QA" + protocol.QActionID + "|" + protocol.GetTriggerParameter() + "|Run|Exception thrown:" + Environment.NewLine + ex, LogType.Error, LogLevel.NoLogging);
		}
	}
}
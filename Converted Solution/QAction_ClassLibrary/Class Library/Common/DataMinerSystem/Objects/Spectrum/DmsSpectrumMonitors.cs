namespace Skyline.DataMiner.Library.Common
{
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.Advanced;

	using System;

	/// <summary>
	/// Represents the spectrum analyzer monitors.
	/// </summary>
	internal class DmsSpectrumAnalyzerMonitors : IDmsSpectrumAnalyzerMonitors
	{
		private readonly IDmsElement element;

		/// <summary>
		/// Initializes a new instance of the <see cref="DmsSpectrumAnalyzerMonitors"/> class.
		/// </summary>
		/// <param name="element">The element to which this spectrum analyzer component is part of.</param>
		public DmsSpectrumAnalyzerMonitors(IDmsElement element)
		{
			this.element = element;
		}

		/// <summary>
		/// Deletes the monitor with the specified ID.
		/// Replace: sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_MONITOR (8), monitorMetaInfo, monitorDetails, out result);
		/// </summary>
		/// <param name="monitorId">The ID of the monitor to be deleted.</param>
		/// <returns></returns>
		public int DeleteMonitor(int monitorId)
		{
			string[] monitorMetaInfo = new string[]
			{
				Convert.ToString(monitorId),	// monitor id. SL_NO_ID = 2100000000 = new monitor
				"delete"						// add/delete (also use "add" for updates to existing scripts)
			};

			string[] monitorDetails = new string[] { };

			monitorMetaInfo[1] = "delete"; // Forcing metaInfo to delete
			SetSpectrumInfoMessage message = new SetSpectrumInfoMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.Monitor,
				Sa1 = new SA(monitorMetaInfo),
				Sa2 = new SA(monitorDetails)
			};

			SetSpectrumInfoResponseMessage result = (SetSpectrumInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

			return result.NewID;
		}

		/// <summary>
		/// Replaces: sa.NotifyElement(userID, elementID, SPA_NE_GETINFO (4), SPAI_MONITORS_ALL (7), null, null, out result);
		/// </summary>
		/// <returns>An object representing all monitors.</returns>
		public object GetMonitors()
		{
			GetSpectrumManagerInfoMessage message = new GetSpectrumManagerInfoMessage()
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.AllMonitors
			};

			GetSpectrumManagerInfoResponseMessage result = (GetSpectrumManagerInfoResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);

			return PSA.ToInteropArray(result.Psa);
		}

		/// <summary>
		/// Replaces:sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_MONITOR (8), monitorMetaInfo, monitorDetails, out result);
		/// </summary>
		/// <param name="monitorId">The ID of the monitor.</param>
		/// <param name="monitorDetails">Details describing the monitor.</param>
		/// <exception cref="ArgumentNullException"><paramref name="monitorDetails"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="monitorDetails"/> must be an array of at least size 6.</exception>
		public void UpdateMonitor(int monitorId, string[] monitorDetails)
		{
			if(monitorDetails == null)
			{
				throw new ArgumentNullException("monitorDetails");
			}

			if(monitorDetails.Length < 6)
			{
				throw new ArgumentException("monitorDetails must be an array of at least size 6.");
			}

			string monitorName = monitorDetails[4];
			string monitorDescription = monitorDetails[5];

			string[] scriptMetaInfo = new[]
			{
				Convert.ToString(monitorId),	// script id. SL_NO_ID = 2100000000 = new script
				"add", // add/delete
				element.Protocol.Name,
				monitorName,
				monitorDescription
			};

			SetSpectrumInfoMessage message = new SetSpectrumInfoMessage
			{
				DataMinerID = element.AgentId,
				ElId = element.Id,
				What = (int)SpectrumInfoType.Monitor,
				Sa1 = new SA(scriptMetaInfo),
				Sa2 = new SA(monitorDetails)
			};

			element.Host.Dms.Communication.SendSingleResponseMessage(message);
		}

		/// <summary>
		/// Replaces:sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_MONITOR (8), monitorMetaInfo, monitorDetails, out result);
		/// Where monitorId is set to 2100000000 for creation
		/// </summary>
		/// <param name="monitorDetails">Details of the monitor.</param>
		/// <exception cref="ArgumentNullException"><paramref name="monitorDetails"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="monitorDetails"/> must be an array of at least size 6.</exception>
		public void AddMonitor(string[] monitorDetails)
		{
			UpdateMonitor(2100000000, monitorDetails);
		}
	}
}
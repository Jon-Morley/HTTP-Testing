namespace Skyline.DataMiner.Library.Common
{
	using System;

	/// <summary>
	/// Represents the spectrum analyzer monitors.
	/// </summary>
	public interface IDmsSpectrumAnalyzerMonitors
	{
		/// <summary>
		/// Deletes the monitor with the specified ID.
		/// Replaces sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_MONITOR (8), monitorMetaInfo, monitorDetails, out result);
		/// </summary>
		/// <param name="monitorId">The ID of the monitor to be deleted.</param>
		/// <returns></returns>
		int DeleteMonitor(int monitorId);

		/// <summary>
		/// Retrieves all monitors.
		/// Replaces sa.NotifyElement(userID, elementID, SPA_NE_GETINFO (4), SPAI_MONITORS_ALL (7), null, null, out result);
		/// </summary>
		/// <returns>An object representing all monitors.</returns>
		object GetMonitors();

		/// <summary>
		/// Updates the monitor with the specified ID.
		/// Replaces sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_MONITOR (8), monitorMetaInfo, monitorDetails, out result);
		/// </summary>
		/// <param name="monitorId">The ID of the monitor.</param>
		/// <param name="monitorDetails">Details describing the monitor.</param>
		/// <exception cref="ArgumentNullException"><paramref name="monitorDetails"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="monitorDetails"/> must be an array of at least size 6.</exception>
		void UpdateMonitor(int monitorId, string[] monitorDetails);

		/// <summary>
		/// Adds a monitor with the specified settings.
		/// Replaces:sa.NotifyElement(userID, elementID, SPA_NE_SETINFO (5), SPAI_MONITOR (8), monitorMetaInfo, monitorDetails, out result);
		/// Where monitorId is set to 2100000000 for creation
		/// </summary>
		/// <param name="monitorDetails">Details of the monitor.</param>
		/// <exception cref="ArgumentNullException"><paramref name="monitorDetails"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="monitorDetails"/> must be an array of at least size 6.</exception>
		void AddMonitor(string[] monitorDetails);
	}
}
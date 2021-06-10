namespace DataMiner.Generics
{
    using Skyline.DataMiner.Scripting;

    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLManagedScripting.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("Interop.SLDms.dll")]
    public static class Generic
    {
        /// <summary>
        /// Writes a line of a log to the log file of the element
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="method"></param>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="logLevel"></param>
        public static void Log( SLProtocol protocol, string method, string message, LogType logType = LogType.Error, LogLevel logLevel = LogLevel.NoLogging)
        {
            protocol.Log("QA" + protocol.QActionID + "|" + protocol.GetTriggerParameter() + "|" + method + "|Line: " + LineNumber() + "|" + message, logType, logLevel);
        }

        /// <summary>
        /// Writes a line of a log to the log file of the element
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="logLevel"></param>
        public static void Log(SLProtocol protocol, string message, LogType logType = LogType.Error, LogLevel logLevel = LogLevel.NoLogging)
        {
            protocol.Log("QA" + protocol.QActionID + "|" + protocol.GetTriggerParameter() + "|Line: " + LineNumber() + "|" + message, logType, logLevel);
        }

        /// <summary>
        /// Gets the estimate line number where an error occured.
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public static int LineNumber([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber - 2;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Text;

namespace Assessment.Logging
{
    public class Logger : ILogger
    {
        public void Information(string message)
        {
            Trace.TraceInformation(message);
        }

        public void Information(string format, params object[] vars)
        {
            Trace.TraceInformation(format, vars);
        }

        public void Information(Exception exception, string format, params object[] vars)
        {
            Trace.TraceInformation(FormatExceptionMessage(exception, format, vars));
        }
        public void Warning(string message)
        {
            Trace.TraceWarning(message);
        }

        public void Warning(string format, params object[] vars)
        {
            Trace.TraceWarning(format, vars);
        }

        public void Warning(Exception exception, string format, params object[] vars)
        {
            Trace.TraceWarning(FormatExceptionMessage(exception, format, vars));
        }

        public void Error(string message)
        {
            Trace.TraceError(message);
        }

        public void Error(string format, params object[] vars)
        {
            Trace.TraceError(format, vars);
        }

        public void Error(Exception exception, string format, params object[] vars)
        {
            Trace.TraceError(FormatExceptionMessage(exception, format, vars));
        }



        public void TraceApi(string componentName, string method, TimeSpan timespan)
        {
            TraceApi(componentName, method, timespan, "");
        }

        public void TraceApi(string componentName, string method, TimeSpan timespan, string fmt, params object[] vars)
        {
            TraceApi(componentName, method, timespan, string.Format(fmt, vars));
        }
        public void TraceApi(string componentName, string method, TimeSpan timespan, string properties)
        {
            string message = String.Concat("Component:", componentName, ";Method:", method, ";Timespan:", timespan.ToString(), ";Properties:", properties);
            Trace.TraceInformation(message);
        }

        private static string FormatExceptionMessage(Exception exception, string format, object[] vars)
        {
            // Simple exception formatting borrowed from Windows Azure 
            var sb = new StringBuilder();
            sb.Append(string.Format(format, vars));
            sb.Append(" Exception: ");
            sb.Append(exception.ToString());
            return sb.ToString();
        }

    }
}
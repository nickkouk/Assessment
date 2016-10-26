using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assessment.Logging
{
    public interface ILogger
    {
        //Information Logging
        void Information(string message);
        void Information(string format, params object[] vars);
        void Information(Exception exception, string format, params object[] vars);

        //Waring Logging
        void Warning(string message);
        void Warning(string format, params object[] vars);
        void Warning(Exception exception, string format, params object[] vars);

        //Error Logging
        void Error(string message);
        void Error(string format, params object[] vars);
        void Error(Exception exception, string format, params object[] vars);

        //TraceApi Logging
        void TraceApi(string componentName, string method, TimeSpan timespan);
        void TraceApi(string componentName, string method, TimeSpan timespan, string properties);
        void TraceApi(string componentName, string method, TimeSpan timespan, string format, params object[] vars);
    }
}
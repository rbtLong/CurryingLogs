using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace PZHelpers.DbLog
{

    public static class Logs
    {
        public static LogEntry Entry(string type, string title, string data)
        {
            if(ReferenceEquals(null, title))
                title = "blank";

            var ent = new LogEntry
            {
                Type = type ?? "",
                Title = title ?? "",
                Data = data ?? "",
            };

            var uid = "No PortalUser Instance Available";
            if (!ent.Aux.ContainsKey("uid"))
                ent.Add("uid", uid);

            return ent;
        }

        public static LogEntry Entry(string type, MethodBase m, string data)
        {
            var title = string.Format("{0} in {1}", m.Name, m.ReflectedType.FullName);

            var ent = new LogEntry
            {
                Type = type ?? "",
                Title = title ?? "",
                Data = data ?? "",
            };

            var uid = "No PortalUser Instance Available";
            if (!ent.Aux.ContainsKey("uid"))
                ent.Add("uid", uid);

            return ent;
        }

        public static LogEntry Info(this object data, string title = null)
        {
            return Inf(title, data as string ?? data.Json());
        }

        public static LogEntry Info(this object data, MethodBase m)
        {
            return Inf(m, data as string ?? data.Json());
        }

        public static LogEntry Inf(MethodBase m, string data)
        {
            return Entry("info", m, data);
        }

        public static LogEntry Inf(string title, string data)
        {
            return Entry("info", title, data);
        }

        public static LogEntry Status(this object data, string title = null)
        {
            return Stat(title, data as string ?? data.Json());
        }

        public static LogEntry Status(this object data, MethodBase m)
        {
            return Stat(m, data as string ?? data.Json());
        }

        public static LogEntry Stat(MethodBase m, string data)
        {
            return Entry("info", m, data);
        }

        public static LogEntry Stat(string title, string data)
        {
            return Entry("status", title, data);
        }

        public static LogEntry Error(this object data, string title = null)
        {
            return Err(title, data as string ?? data.Json());
        }

        public static LogEntry Error(this object data, MethodBase m)
        {
            return Err(m, data as string ?? data.Json());
        }

        public static LogEntry Err(string title, string data)
        {
            return Entry("error", title, data);
        }

        public static LogEntry Err(MethodBase m, string data)
        {
            return Entry("error", m, data);
        }

        /// <summary>
        /// Commits the log to database.
        /// </summary>
        public static int Ok(this LogEntry log, string sec)
        {
            try
            {
                EventLog.WriteEntry("ICSNET", log.Json());
                return L.Evt(log.Type, sec, log.Title, log.Data, log.Aux.Json());
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return -1;
            }
        }

        public static int Ok(this LogEntry log)
        {
            return log.Ok(Assembly.GetCallingAssembly().FullName);
        }

        /// <summary>
        /// Adds an entry to AUX
        /// </summary>
        public static LogEntry Add(this LogEntry log, string key, string val)
        {
            try
            {
                log.Aux.Add(key, val ?? "null");
            }
            catch (Exception ex)
            {
                log.Aux.Add("[add failed] " + key, ex);
            }
            return log;
        }

        /// <summary>
        /// Creates a description field in AUX
        /// </summary>
        public static LogEntry Describe(this LogEntry log, string description)
        {
            log.Add("description", description);
            return log;
        }

        /// <summary>
        /// returns the object with this expression (o ?? "null").ToString()
        /// </summary>
        public static string nstr(this object o)
        {
            return (o ?? "null").ToString();
        }

        public static string nget(this IDictionary d, string key)
        {
            try
            {
                return d[key] as string;
            }
            catch (Exception ex)
            {
                ex.Error($"No dictionary value for {key} in {d}")
                    .Add("dictionary", d.Json())
                    .Add("key", key)
                    .Ok();
            }
            return null;
        }
    }

    public class LogEntry
    {
        private Dictionary<string, object> _aux = new Dictionary<string, object>();

        public string Type { get; set; }
        public string Title { get; set; }
        public string Data { get; set; }
        public Dictionary<string, object> Aux
        {
            get { return _aux; }
            set { _aux = value; }
        }
    }
}
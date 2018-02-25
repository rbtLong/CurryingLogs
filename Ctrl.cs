using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace PZHelpers
{
    public static class Ctrl
    {

        public static HttpResponseMessage RespCode(HttpStatusCode code = HttpStatusCode.OK)
        {
            return new HttpResponseMessage(code);
        }

        public static HttpResponseMessage Resp(this object content, string type = "application/json")
        {
            string c;

            if (content is string)
                c = (string) content;
            else
                c = content.Json();

            return new HttpResponseMessage
            {
                Content = new StringContent(c, Encoding.UTF8, type)
            };
        }

        public static Dictionary<string, object> ToResult(this object content,
            object success, params KeyValuePair<string, object>[] fields)
        {
            var result = Result(success, new KeyValuePair<string, object>("content", content));
            return result;
        }

        public static Dictionary<string, object> Result(object success, params KeyValuePair<string, object>[] fields)
        {
            var result = new Dictionary<string, object>();
            result["success"] = success;

            if (!ReferenceEquals(null, fields))
                foreach (var f in fields)
                    result.Add(f.Key, f.Value);

            return result;
        }


        /// <summary>
        /// success = null assumes success is 0
        /// </summary>
        public static Dictionary<string, object> Fail(object success, string reason,
            params KeyValuePair<string, object>[] fields)
        {
            var result = Result(success, fields);

            if (!ReferenceEquals(null, reason))
                result.Add("reason", reason);

            return result;
        }

        /// <summary>
        /// Generates random string of size length.
        /// </summary>
        private static readonly Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

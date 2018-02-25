using System;

namespace PZHelpers.DbLog
{
    public static class L
    {
        public static int Evt(string type, string sec, string title, string data, string aux)
        {
            return DbLogQueries.Insert(Environment.MachineName, type, "jics", sec, title, data, aux);
        }
        
    }
}

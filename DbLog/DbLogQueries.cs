using System.Collections.Generic;
using System.Data;
using PZHelpers.MSQ;

namespace PZHelpers.DbLog
{
    public static class DbLogQueries
    {
        public static int Insert(string env, string type, string app, string sec, string title, string data, string aux)
        {
            return Db.Logs
                .Proc("[dbo].spDblog_Insert")
                .Param("@env", SqlDbType.NVarChar, 4000, env)
                .Param("@type", SqlDbType.NVarChar, 4000, type)
                .Param("@app", SqlDbType.NVarChar, 4000, app)
                .Param("@sec", SqlDbType.NVarChar, 4000, sec)
                .Param("@title", SqlDbType.NVarChar, 4000, title)
                .Param("@data", SqlDbType.NVarChar, -1, data)
                .Param("@aux", SqlDbType.NVarChar, -1, aux)
                .NonQuery();
        }

        public static Dictionary<string, object>[] Get()
        {
            return Db.Logs
                .Proc("spDblog_Get")
                .Rows();
        }
    }
}

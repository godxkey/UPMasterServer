using FreeSql;

namespace UPMasterServer {

    public class SqlConnection {

        IFreeSql sql;
        public IFreeSql SQL => sql;

        public SqlConnection() { }

        public void Init(string ip, int port, string user, string pwd, string db) {

            string connstr = $"Data Source={ip};Port={port};User ID={user};Password={pwd}; Initial Catalog={db};Charset=utf8; SslMode=none;Min pool size=1";
            sql = new FreeSqlBuilder()
                    .UseAutoSyncStructure(true)
                    .UseConnectionString(DataType.MySql, connstr)
                    .Build();

        }

    }

}
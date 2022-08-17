using System;
using JackFrame.HttpNS;
using FreeSql;

namespace UPMasterServer {

    class Program {

        static void Main(string[] args) {
            
            Console.WriteLine("Help:");
            Console.WriteLine("args[0] - ip");
            Console.WriteLine("args[1] - mysql user");
            Console.WriteLine("args[2] - mysql pwd");

            string ip = args[0];
            string user = args[1];
            string pwd = args[2];

            int port = 3306;

            SqlConnection conn = new SqlConnection();
            conn.Init(ip, port, user, pwd, "upmaster");

            // For Auto Sync Structure
            conn.SQL.Select<DependencyTable>();
            
        }

    }

}

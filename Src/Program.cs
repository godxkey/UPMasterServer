using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JackFrame.HttpNS;
using UPMasterServer.Facades;
using UPMasterServer.SubscribeBusiness.Controller;

namespace UPMasterServer {

    class Program {

        static async Task Main(string[] args) {

            {
                Action<object> log = Console.WriteLine;
                log.Invoke("Help:");
                log.Invoke("args[0] - mysql host");
                log.Invoke("args[1] - mysql user");
                log.Invoke("args[2] - mysql pwd");
            }

            // ==== SQL ====
            string ip = args[0];
            string user = args[1];
            string pwd = args[2];
            int sqlPort = 3306;
            SqlConnection conn = new SqlConnection();
            conn.Init(ip, sqlPort, user, pwd, "upmaster");

            // ==== Http ====
            int httpPort = 5010;
            JackHttpServer server = new JackHttpServer(httpPort);

            // ==== Facades ====
            GlobalFacades globalFacades = new GlobalFacades();
            globalFacades.Inject(server, conn);

            // ==== Controller ====
            DependancyController dependancyController = new DependancyController();

            dependancyController.Inject(globalFacades);

            try {
                dependancyController.Init();
                PLog.Log($"http Listening: {httpPort}");
            } catch (Exception ex) {
                PLog.Error("Init Error: " + ex.ToString());
            }

            string path = Path.Combine(Environment.CurrentDirectory, "exit.signal");
            PLog.Log("Create A File To Exit Program: " + path);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            while (!cancellationTokenSource.IsCancellationRequested) {
                await Task.Delay(34);
                if (File.Exists(path)) {
                    break;
                }
            }

            dependancyController.TearDown();
            File.Delete(path);

        }

    }

}

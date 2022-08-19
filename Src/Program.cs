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

            Console.WriteLine("Help:");
            Console.WriteLine("args[0] - mysql host");
            Console.WriteLine("args[1] - mysql user");
            Console.WriteLine("args[2] - mysql pwd");

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
                System.Console.WriteLine($"http Listening: {httpPort}");
            } catch {
                System.Console.WriteLine("Init Error");
            }

            string path = Path.Combine(Environment.CurrentDirectory, "exit.signal");
            System.Console.WriteLine("Create A File To Exit Program: " + path);
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

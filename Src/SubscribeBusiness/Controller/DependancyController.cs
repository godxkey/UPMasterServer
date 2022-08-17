using System;
using JackFrame.HttpNS;
using UPMasterServer.Facades;
using UPMasterServer.SubscribeBusiness.Facades;
using Newtonsoft.Json;

namespace UPMasterServer.SubscribeBusiness.Controller {

    public class DependancyController {

        GlobalFacades globalFacades;
        SubscribeFacades subscribeFacades;

        public DependancyController() {
            this.subscribeFacades = new SubscribeFacades();
        }

        public void Inject(GlobalFacades globalFacades) {

            this.globalFacades = globalFacades;

            // For Auto Sync Structure
            var conn = globalFacades.SqlConnection;
            conn.SQL.Select<DependencyTable>();

            // Dao
            DependencyDao dependencyDao = new DependencyDao();
            dependencyDao.Inject(conn.SQL);

            // Facades
            subscribeFacades.Inject(dependencyDao);

        }

        public void Init() {

            var server = globalFacades.Server;

            server.GetListen("/get_packages", (req, res) => {
                Console.WriteLine("get");
            });

            server.PostListen("/add_package", (req, res) => {
                Console.WriteLine("add");
            });

            server.PutListen("/update_package", (req, res) => {
                Console.WriteLine("update");
            });

            server.DeleteListen("/delete_package", (req, res) => {
                Console.WriteLine("delete");
            });

            server.Start();

        }

        public void TearDown() {

            var server = globalFacades.Server;
            server.Stop();

        }

    }

}
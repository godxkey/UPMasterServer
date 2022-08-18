using System;
using System.Threading.Tasks;
using System.Text;
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
            DependencyRemoteDao dependencyRemoteDao = new DependencyRemoteDao();
            dependencyRemoteDao.Inject(conn.SQL);

            DependencyLocalDao dependencyLocalDao = new DependencyLocalDao();
            DependencyRepo dependencyRepo = new DependencyRepo();
            dependencyRepo.Inject(dependencyRemoteDao, dependencyLocalDao);

            AddPackageBo addPackageBo = new AddPackageBo();
            addPackageBo.Inject(subscribeFacades);

            AllBo allBo = new AllBo();
            allBo.Inject(addPackageBo);

            // Facades
            subscribeFacades.Inject(allBo, dependencyRepo);

        }

        public void Init() {

            var server = globalFacades.Server;

            server.GetListen("/get_test", async (req, res) => {
                await TaskHelper.AsyncEmptyAwait;
                res.StatusCode = 200;
                res.SendBuffer(new byte[1] { 127 });
            });

            server.GetListen("/get_packages", async (req, res) => {
                Console.WriteLine("get");
                var dao = subscribeFacades.DependencyDao;
                // PERF: 分页
                var all = await dao.GetAllAsync();
                try {
                    string jsonStr = JsonConvert.SerializeObject(all);
                    res.StatusCode = 200;
                    await res.SendUTF8StringAsync(jsonStr);
                } catch {
                    System.Console.WriteLine("Get Packages Error");
                    res.StatusCode = 400;
                    res.SendBuffer(new byte[0]);
                }
            });

            server.PostListen("/add_package", async (req, res) => {
                await TaskHelper.AsyncEmptyAwait;
                System.Console.WriteLine("add package");
                var bo = subscribeFacades.AllBo.AddPackageBo;
                bo.OnAddPackage(req, res);
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
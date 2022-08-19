using System;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using JackFrame.HttpNS;
using UPMasterServer.SubscribeBusiness.Facades;
using UPMasterServer.SubscribeBusiness.Protocol;

namespace UPMasterServer.SubscribeBusiness {

    public class AddPackageBo {

        SubscribeFacades subscribeFacades;

        object lockObj;
        byte[] buffer;

        public AddPackageBo() {
            this.lockObj = new object();
            this.buffer = new byte[512];
        }

        public void Inject(SubscribeFacades subscribeFacades) {
            this.subscribeFacades = subscribeFacades;
        }

        public void OnAddPackage(HttpListenerRequest req, HttpListenerResponse res) {
            lock (lockObj) {
                try {
                    var arr = req.ReadBuffer(buffer);
                    string dataStr = Encoding.UTF8.GetString(arr);
                    var msg = JsonConvert.DeserializeObject<SubscribeAddPackageReqMessage>(dataStr);
                    var table = new DependencyTable() {
                        packageName = msg.packageName,
                        gitUrl = msg.gitUrl,
                        branchOrTag = msg.branchOrTag
                    };

                    var dao = subscribeFacades.DependencyDao;
                    var old = dao.FindByPackageNameAsync(table.packageName).Result;
                    if (old == null) {
                        _ = dao.InsertAsync(table).Result;
                    } else {
                        _ = dao.UpdateAsync(table).Result;
                    }
                    res.StatusCode = 200;
                    res.SendBuffer(new byte[0]);
                } catch(Exception ex) {
                    System.Console.WriteLine("Add Package Error: " + ex.ToString());
                    res.StatusCode = 400;
                    res.SendBuffer(new byte[0]);
                }
            }
        }

    }

}
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
            this.buffer = new byte[4096];
        }

        public void Inject(SubscribeFacades subscribeFacades) {
            this.subscribeFacades = subscribeFacades;
        }

        public void OnAddPackage(HttpListenerRequest req, HttpListenerResponse res) {
            lock (lockObj) {
                try {
                    var arr = req.ReadBuffer(buffer);
                    string dataStr = Encoding.UTF8.GetString(arr);
                    var data = JsonConvert.DeserializeObject<SubscribeAddPackageReqMessage>(dataStr);
                    // data = (SubscribeAddPackageReqMessage)msg.body["data"];
                    var table = new DependencyTable() {
                        packageName = data.packageName,
                        gitUrl = data.gitUrl,
                        branchOrTag = data.branchOrTag
                    };

                    var dao = subscribeFacades.DependencyDao;
                    var old = dao.FindByPackageNameAsync(table.packageName).Result;
                    if (old == null) {
                        _ = dao.InsertAsync(table).Result;
                        PLog.Log($"[Add Package]recv: {dataStr}");
                    } else {
                        _ = dao.UpdateAsync(table).Result;
                        PLog.Log($"[Update Package]recv: {dataStr}");
                    }
                    res.StatusCode = 200;
                    res.SendBuffer(new byte[0]);
                } catch (Exception ex) {
                    PLog.Error("Add Package Error: " + ex.ToString());
                    res.StatusCode = 400;
                    res.SendBuffer(new byte[0]);
                }
            }
        }

    }

}
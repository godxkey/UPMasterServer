using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UPMasterServer.SubscribeBusiness {

    public class DependencyRepo {

        IDependencyDao remoteDao;
        IDependencyDao localDao;

        public DependencyRepo() { }

        public void Inject(IDependencyDao dao, IDependencyDao localDao) {
            this.remoteDao = dao;
            this.localDao = localDao;
        }

        public void Init() {
            this.localDao.SetAll(this.remoteDao.GetAllAsync().Result);
            this.localDao.Version = this.remoteDao.Version;
        }

        public async Task<List<DependencyTable>> GetAllAsync() {
            if (remoteDao.Version == localDao.Version) {
                return await localDao.GetAllAsync();
            } else {
                var list = await remoteDao.GetAllAsync();
                if (list != null) {
                    localDao.SetAll(list);
                    remoteDao.Version += 1;
                    localDao.Version = remoteDao.Version;
                }
                return list;
            }
        }

        public void SetAll(List<DependencyTable> all) {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(DependencyTable table) {
            int count = await remoteDao.InsertAsync(table);
            if (count > 0) {
                await localDao.InsertAsync(table);
                remoteDao.Version += 1;
                localDao.Version = remoteDao.Version;
            }
            return count;
        }

        public async Task<int> RemoveAsync(DependencyTable table) {
            int count = await remoteDao.RemoveAsync(table);
            if (count > 0) {
                await localDao.RemoveAsync(table);
                remoteDao.Version += 1;
                localDao.Version = remoteDao.Version;
            }
            return count;
        }

        public async Task<int> UpdateAsync(DependencyTable table) {
            int count = await remoteDao.UpdateAsync(table);
            if (count > 0) {
                await localDao.UpdateAsync(table);
                remoteDao.Version += 1;
                localDao.Version = remoteDao.Version;
                PLog.Log("UpdateAsync: " + table.packageName);
            }
            return count;
        }

        public async Task<DependencyTable> FindByPackageNameAsync(string packageName) {
            if (remoteDao.Version == localDao.Version) {
                return await localDao.FindByPackageNameAsync(packageName);
            } else {
                var table = await remoteDao.FindByPackageNameAsync(packageName);
                if (table != null) {
                    await localDao.InsertAsync(table);
                    remoteDao.Version += 1;
                    localDao.Version = remoteDao.Version;
                }
                return table;
            }
        }

    }

}
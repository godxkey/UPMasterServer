using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UPMasterServer.SubscribeBusiness {

    public class DependencyRepo : IDependencyDao {

        int IDependencyDao.Version { get; set; }

        IDependencyDao dao;
        IDependencyDao localDao;

        public DependencyRepo() { }

        public void Inject(IDependencyDao dao, IDependencyDao localDao) {
            this.dao = dao;
            this.localDao = localDao;
        }

        public void Init() {
            this.dao.SetAll(this.localDao.GetAllAsync().Result);
        }

        public async Task<List<DependencyTable>> GetAllAsync() {
            if (dao.Version == localDao.Version) {
                return await localDao.GetAllAsync();
            } else {
                var list = await dao.GetAllAsync();
                if (list != null) {
                    localDao.SetAll(list);
                    dao.Version += 1;
                    localDao.Version = dao.Version;
                }
                return list;
            }
        }

        public void SetAll(List<DependencyTable> all) {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(DependencyTable table) {
            int count = await dao.InsertAsync(table);
            if (count > 0) {
                await localDao.InsertAsync(table);
                dao.Version += 1;
                localDao.Version = dao.Version;
            }
            return count;
        }

        public async Task<int> RemoveAsync(DependencyTable table) {
            int count = await dao.RemoveAsync(table);
            if (count > 0) {
                await localDao.RemoveAsync(table);
                dao.Version += 1;
                localDao.Version = dao.Version;
            }
            return count;
        }

        public async Task<int> UpdateAsync(DependencyTable table) {
            int count = await dao.UpdateAsync(table);
            if (count > 0) {
                await localDao.UpdateAsync(table);
                dao.Version += 1;
                localDao.Version = dao.Version;
            }
            return count;
        }

        public async Task<DependencyTable> FindByPackageNameAsync(string packageName) {
            if (dao.Version == localDao.Version) {
                return await localDao.FindByPackageNameAsync(packageName);
            } else {
                var table = await dao.FindByPackageNameAsync(packageName);
                if (table != null) {
                    await localDao.InsertAsync(table);
                    dao.Version += 1;
                    localDao.Version = dao.Version;
                }
                return table;
            }
        }

    }

}
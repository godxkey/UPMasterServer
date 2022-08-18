using System.Collections.Generic;
using System.Threading.Tasks;

namespace UPMasterServer.SubscribeBusiness {

    public class DependencyLocalDao : IDependencyDao {

        int version;
        int IDependencyDao.Version { get => version; set => version = value; }

        List<DependencyTable> cacheList;

        public DependencyLocalDao() {
            this.cacheList = new List<DependencyTable>();
            this.version = -1;
        }

        public void SetAll(List<DependencyTable> all) {
            this.cacheList = all;
        }

        public async Task<List<DependencyTable>> GetAllAsync() {
            await TaskHelper.SyncEmptyAwait;
            return cacheList;
        }

        public async Task<int> InsertAsync(DependencyTable table) {
            await TaskHelper.SyncEmptyAwait;
            cacheList.Add(table);
            return 1;
        }

        public async Task<int> RemoveAsync(DependencyTable table) {
            await TaskHelper.SyncEmptyAwait;
            cacheList.Remove(table);
            return 1;
        }

        public async Task<int> UpdateAsync(DependencyTable table) {
            await TaskHelper.SyncEmptyAwait;
            int index = cacheList.FindIndex(value => value.id == table.id);
            if (index != -1) {
                cacheList[index] = table;
            }
            return 1;
        }

        public async Task<DependencyTable> FindByPackageNameAsync(string packageName) {
            await TaskHelper.SyncEmptyAwait;
            return cacheList.Find(value => value.packageName == packageName);
        }
    }

}
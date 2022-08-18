using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeSql;

namespace UPMasterServer.SubscribeBusiness {

    public class DependencyRemoteDao : IDependencyDao {

        int version;
        int IDependencyDao.Version { get => version; set => version = value; }

        IFreeSql sql;

        public DependencyRemoteDao() {
            this.version = 1;
        }

        public void Inject(IFreeSql conn) {
            this.sql = conn;
        }

        public async Task<int> InsertAsync(DependencyTable table) {
            return await sql.Insert(table).ExecuteAffrowsAsync();
        }

        public void SetAll(List<DependencyTable> all) {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(DependencyTable table) {
            return await sql.Update<DependencyTable>(table).Where(value => value.id == table.id).ExecuteAffrowsAsync();
        }

        public async Task<int> RemoveAsync(DependencyTable table) {
            return await sql.Delete<DependencyTable>().Where(value => value.id == table.id).ExecuteAffrowsAsync();
        }

        public async Task<List<DependencyTable>> GetAllAsync() {
            return await sql.Select<DependencyTable>().ToListAsync();
        }

        public async Task<DependencyTable> FindByPackageNameAsync(string packageName) {
            return await sql.Select<DependencyTable>().Where(value => value.packageName == packageName).FirstAsync();
        }
    }

}
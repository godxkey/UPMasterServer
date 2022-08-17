using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreeSql;

namespace UPMasterServer.SubscribeBusiness {

    public class DependencyDao {

        object lockObj;
        IFreeSql sql;

        public DependencyDao() {
            this.lockObj = new Object();
        }

        public void Inject(IFreeSql conn) {
            this.sql = conn;
        }

        public async Task<int> Insert(DependencyTable table) {
            return await sql.Insert(table).ExecuteAffrowsAsync();
        }

        public async Task<int> Update(DependencyTable table) {
            return await sql.Update<DependencyTable>(table).Where(value => value.id == table.id).ExecuteAffrowsAsync();
        }

        public async Task<int> Remove(DependencyTable table) {
            return await sql.Delete<DependencyTable>().Where(value => value.id == table.id).ExecuteAffrowsAsync();
        }

        public async Task<List<DependencyTable>> GetAll() {
            return await sql.Select<DependencyTable>().ToListAsync();
        }

    }

}
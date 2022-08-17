using FreeSql;
using FreeSql.DatabaseModel;
using FreeSql.DataAnnotations;

namespace UPMasterServer.SubscribeBusiness {

    [Table]
    public class DependencyTable {
        
        [Column(IsPrimary = true, IsIdentity = true)]
        public long id { get; set; }

        [Column(DbType = "varchar(128)")]
        public string packageName { get; set; }

        [Column(DbType = "varchar(128)")]
        public string gitUrl { get; set; }

        [Column(DbType = "varchar(32)")]
        public string branchOrTag { get; set; }

    }

}
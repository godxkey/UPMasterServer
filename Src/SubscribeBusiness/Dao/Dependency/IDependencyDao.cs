using System.Collections.Generic;
using System.Threading.Tasks;

namespace UPMasterServer.SubscribeBusiness {
    public interface IDependencyDao {
        int Version { get; set; }
        Task<List<DependencyTable>> GetAllAsync();
        void SetAll(List<DependencyTable> all);
        Task<DependencyTable> FindByPackageNameAsync(string packageName);
        Task<int> InsertAsync(DependencyTable table);
        Task<int> RemoveAsync(DependencyTable table);
        Task<int> UpdateAsync(DependencyTable table);
    }

}
namespace UPMasterServer.SubscribeBusiness.Facades {

    public class SubscribeFacades {

        object lockObj;

        AllBo allBo;
        public AllBo AllBo => allBo;

        IDependencyDao dependencyDao;
        public IDependencyDao DependencyDao {
            get { lock (lockObj) { return dependencyDao; } }
        }

        public SubscribeFacades() {
            this.lockObj = new object();
        }

        public void Inject(AllBo allBo, IDependencyDao dependencyDao) {
            this.allBo = allBo;
            this.dependencyDao = dependencyDao;
        }

    }
}
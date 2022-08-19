namespace UPMasterServer.SubscribeBusiness.Facades {

    public class SubscribeFacades {

        object lockObj;

        AllBo allBo;
        public AllBo AllBo => allBo;

        DependencyRepo dependencyDao;
        public DependencyRepo DependencyDao {
            get { lock (lockObj) { return dependencyDao; } }
        }

        public SubscribeFacades() {
            this.lockObj = new object();
        }

        public void Inject(AllBo allBo, DependencyRepo dependencyDao) {
            this.allBo = allBo;
            this.dependencyDao = dependencyDao;
        }

    }
}
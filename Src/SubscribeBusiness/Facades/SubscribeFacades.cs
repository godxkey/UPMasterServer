namespace UPMasterServer.SubscribeBusiness.Facades {

    public class SubscribeFacades {

        object lockObj;

        DependencyDao dependencyDao;
        public DependencyDao DependencyDao {
            get { lock (lockObj) { return dependencyDao; } }
        }

        public SubscribeFacades() {
            this.lockObj = new object();
        }

        public void Inject(DependencyDao dependencyDao) {
            this.dependencyDao = dependencyDao;
        }

    }
}
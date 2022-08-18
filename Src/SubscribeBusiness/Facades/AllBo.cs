namespace UPMasterServer.SubscribeBusiness.Facades {

    public class AllBo {

        public AddPackageBo AddPackageBo { get; private set; }

        public AllBo() {}

        public void Inject(AddPackageBo addPackageBo) {
            this.AddPackageBo = addPackageBo;
        }

    }
}
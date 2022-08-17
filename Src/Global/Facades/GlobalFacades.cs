using JackFrame.HttpNS;

namespace UPMasterServer.Facades {

    public class GlobalFacades {

        // Global State
        AppStateEntity appStateEntity;
        public AppStateEntity AppStateEntity => appStateEntity;

        // Server
        JackHttpServer server;
        public JackHttpServer Server => server;

        // Sql
        SqlConnection sqlConnection;
        public SqlConnection SqlConnection => sqlConnection;

        public GlobalFacades() {
            this.appStateEntity = new AppStateEntity();
        }

        public void Inject(JackHttpServer server, SqlConnection sqlConnection) {
            this.server = server;
            this.sqlConnection = sqlConnection;
        }

    }
}
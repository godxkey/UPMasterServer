using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace UPMasterServer {

    public static class TaskHelper {

        public static Task SyncEmptyAwait = Task.CompletedTask;
        public static YieldAwaitable AsyncEmptyAwait = Task.Yield();

        static void Nothing() {}

    }
}
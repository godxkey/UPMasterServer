namespace UPMasterServer {

    public static class PLog {
        
        public static void Log(string message) {
            System.Console.WriteLine("[log]" + message);
        }

        public static void Error(string message) {
            System.Console.WriteLine("[err]" + message);
        }

        public static void Warning(string message) {
            System.Console.WriteLine("[warn]" + message);
        }

    }
}
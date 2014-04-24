using System.Diagnostics;

namespace AcceptanceTests
{
    class ApplicationLifetime
    {
        private static TestedApplication application;

        public static void Start()
        {
            if (application == null)
            {
                application = new TestedApplication();
            }
        }

        public static void Stop()
        {
            if (application != null)
            {
                application.Exit();
                application = null;
            }
        }

        public static TestedApplication GetApplication()
        {
            Start();
            return application;
        }

        public static bool IsRunning()
        {
            return application.IsApplicationRunning();
        }
    }
}

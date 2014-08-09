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

        public static TestedApplication GetStartedApplication()
        {
            Start();
            return application;
        }

        public static TestedApplication GetApplication()
        {
            return application;
        }

        public static bool IsRunning()
        {
            return application.IsApplicationRunning();
        }
    }
}

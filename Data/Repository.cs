using System;
using System.IO;

namespace Data
{
    public class Repository
    {
        public class Troop
        {
        }

        public class IoFailure : Exception
        {
            public IoFailure(string reason)
            {
            }
        }

        public void Write(string path)
        {
            try
            {
                WriteWithoutErrorHandling(path);
            }
            catch (SystemException e)
            {
                throw new IoFailure("Failed to write troop file " +
                                    e.Message);
            }
        }

        private void WriteWithoutErrorHandling(string path)
        {
            File.WriteAllText(path, "nothing yet");
        }
    }
}

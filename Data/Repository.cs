using System;

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
            throw new IoFailure("Could not write troop file");
        }
    }
}

using System;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestData
{
    [TestClass]
    public class RepositoryTest
    {
        private Repository repository = new Repository();

        [TestMethod]
        [ExpectedException(typeof(Repository.IoFailure))]
        public void WritingFailureIsNoticed()
        {
            repository.Write(@"unexistingFolder\file.json");
        }
    }
}

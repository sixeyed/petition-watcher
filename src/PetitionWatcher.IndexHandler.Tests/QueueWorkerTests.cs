using NUnit.Framework;
using PetitionWatcher.MessageHandlers.IndexProspect.Workers;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var worker = new QueueWorker(null, null);
            worker.IndexPetitionState(241584);
        }
    }
}
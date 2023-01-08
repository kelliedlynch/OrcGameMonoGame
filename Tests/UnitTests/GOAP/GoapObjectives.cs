using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;
using OrcGame.GOAP.Core;

namespace Tests
{
    public class GoapAgentAndObjectives
    {
        private Creature _orc = new Creature();
        private Item _bone = new Item();
        private Item _stick = new Item();

        [SetUp]
        public void Setup()
        {
            _orc = new Orc();
            _bone = new Bone();
        }

        [Test]
        public void Simulate_Creature_With_Inventory()
        {
            _orc.AddToCarried(_bone);
            _orc.AddToOwned(_bone);

            var sim = new SimulatedCreature(_orc);
            Assert.That(sim, Is.Not.Null);
        }
    }
}


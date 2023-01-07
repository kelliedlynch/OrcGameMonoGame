using System.Numerics;
using OrcGame.GOAP;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Goal;
using OrcGame.Entity.Creature;
using OrcGame.Entity.Item;

namespace Tests
{
    public class GoapAgentAndObjectives
    {
        private BaseCreature _orc = new BaseCreature();
        private BaseItem _bone = new BaseItem();
        private BaseItem _stick = new BaseItem();

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


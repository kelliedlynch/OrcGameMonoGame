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
            _orc = new BaseCreature()
            {
                Location = new Vector2(1, 1),
                EntityName = "Orc",
                InstanceName = "Thog",
                CreatureType = CreatureType.Humanoid,
                CreatureSubtype = CreatureSubtype.Orc,
                IdleState = IdleState.Idle
            };

            _bone = new BaseItem()
            {
                Location = new Vector2(1, 1),
                EntityName = "Bone",
                InstanceName = "Big Bone",
                Weight = 1.2f,
                Material = Material.Bone
            };

            _stick = new BaseItem()
            {
                Location = new Vector2(1, 1),
                EntityName = "Stick",
                InstanceName = "A Stick",
                Weight = 3.4f,
                Material = Material.Wood
            };

        }

        [Test]
        public void Simulate_Creature_With_Inventory()
        {
            _orc.Carried.Add(_bone);
            _orc.Owned.Add(_bone);
            _orc.Owned.Add(_stick);

            var sim = GoapSimulator.SimulateEntity(_orc);
            Assert.That(sim, Is.Not.Null);
        }

        [Test]
        public void Test_Complete_Goal_Against_Simulated_Creature()
        {
            _orc.Carried.Add(_bone);
            _orc.Owned.Add(_bone);
            var sim = GoapSimulator.SimulateEntity(_orc);
            var agent = new Agent();
            var goal = new ClaimBone(_orc);
            
            Assert.That(agent.IsGoalReached(goal, sim), Is.True);
        }

        [Test]
        public void Test_Incomplete_Goal_Against_Simulated_Creature()
        {
            _orc.Carried.Add(_bone);
            var sim = GoapSimulator.SimulateEntity(_orc);
            var agent = new Agent();
            var goal = new ClaimBone(_orc);
            
            Assert.That(agent.IsGoalReached(goal, sim), Is.False);
        }
        
        
    }
}


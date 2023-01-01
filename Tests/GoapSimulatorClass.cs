using System.Diagnostics;
using System.Numerics;
using OrcGame.GOAP;
using OrcGame.Entity;


namespace Tests
{
    public class GoapSimulatorClass
    {
        private Creature _orc = new Creature();
        private Item _bone = new Item();
        private Item _stick = new Item();

        [SetUp]
        public void Setup()
        {
            _orc = new Creature()
            {
                Location = new Vector2(1, 1),
                EntityName = "Orc",
                InstanceName = "Thog",
                CreatureType = CreatureType.Humanoid,
                CreatureSubtype = CreatureSubtype.Orc,
                IdleState = IdleState.Idle
            };

            _bone = new Item()
            {
                Location = new Vector2(1, 1),
                EntityName = "Bone",
                InstanceName = "Big Bone",
                Weight = 1.2f,
                Material = Material.Bone
            };

            _stick = new Item()
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
            foreach (var pair in sim)
            {
                Debug.Write("WHAT IS GOING ON");
                Debug.Write(pair.Key);
                Debug.Write(pair.Value);
            }
            
            Assert.That(sim, Is.Not.Null);
        }

    }
}


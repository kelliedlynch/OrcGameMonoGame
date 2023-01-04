using System.Numerics;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Goal;
using OrcGame.Entity.Creature;
using OrcGame.Entity.Item;
using OrcGame.GOAP.Action;

namespace Tests
{
    public class GoapActions
    {
        private BaseCreature _orc = null!;
        private BaseItem _bone = null!;
        private BaseItem _stick = null!;
        private readonly ItemManager _itemManager = ItemManager.GetItemManager();
        
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
            _itemManager.AddItemToWorld(_bone);
            
            _stick = new BaseItem()
            {
                Location = new Vector2(1, 1),
                EntityName = "Stick",
                InstanceName = "A Stick",
                Weight = 3.4f,
                Material = Material.Wood
            };
            _itemManager.AddItemToWorld(_stick);

        }

        [Test]
        public void PickUpItem_IsValid()
        {
            var goal = new ClaimBone(_orc);
            _orc.Tagged.Add(_bone);
            var action = new PickUpItem();
            var sim = GoapSimulator.SimulateWorldStateFor(_orc);
            var objective = goal.GetObjective();
            var isValid = action.IsValid(objective, sim);
            Assert.That(isValid.Item1, Is.True);
            Assert.That(isValid.Item2["Creature"]["Carried"].Count, Is.Not.Zero);
            Assert.That(isValid.Item2["Creature"]["Tagged"].Count, Is.Zero);
        }

        [TearDown]
        public void TearDown()
        {
            _itemManager.RemoveItemFromWorld(_bone);
            _itemManager.RemoveItemFromWorld(_stick);
        }
    }
}


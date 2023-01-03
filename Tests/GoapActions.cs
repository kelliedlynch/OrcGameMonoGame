using System.Numerics;
using OrcGame.GOAP;
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
            var action = new PickUpItem();
            var sim = GoapSimulator.SimulateWorldStateFor(_orc);
            var desired = goal.DesiredState();
            var objective = goal.GetObjective(desired);
            // var isValid = action.IsValid(objective);
            // Assert.That(isValid, Is.True);
        }

        // [Test]
        // public void Test_Complete_Goal_Against_Simulated_Creature()
        // {
        //     _orc.Carried.Add(_bone);
        //     _orc.Owned.Add(_bone);
        //     var sim = GoapSimulator.SimulateEntity(_orc);
        //     var agent = new Agent();
        //     var goal = new ClaimBone(_orc);
        //     
        //     Assert.That(agent.IsGoalReached(goal, sim), Is.True);
        // }
        //
        // [Test]
        // public void Test_Incomplete_Goal_Against_Simulated_Creature()
        // {
        //     _orc.Carried.Add(_bone);
        //     var sim = GoapSimulator.SimulateEntity(_orc);
        //     var agent = new Agent();
        //     var goal = new ClaimBone(_orc);
        //     
        //     Assert.That(agent.IsGoalReached(goal, sim), Is.False);
        // }

        [TearDown]
        public void TearDown()
        {
            _itemManager.RemoveItemFromWorld(_bone);
            _itemManager.RemoveItemFromWorld(_stick);
        }
    }
}


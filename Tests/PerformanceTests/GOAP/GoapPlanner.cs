using System.Numerics;
using MonoGame.Extended.Collections;
using OrcGame.Entity;
using OrcGame.Entity.Creature;
using OrcGame.Entity.Item;
using OrcGame.GOAP;
using OrcGame.GOAP.Action;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Goal;

namespace Tests.PerformanceTests.GOAP

{
    public class TestPlannerWithSmallSets
    {
        private BaseCreature _orc = null!;
        private BaseItem _bone = null!;
        private BaseItem _stick = null!;
        private ClaimBone _claimBone = null!;
        private PickUpItem _pickUpItem = null!;
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

            _claimBone = new ClaimBone(_orc);
            _orc.Goals.Add(_claimBone);
            _pickUpItem = new PickUpItem();
            _orc.Actions.Add(_pickUpItem);
        }

        [Test]
        public void GetPlanForOneCreature()
        {
            var state = GoapState.SimulateWorldStateFor(_orc);
            var plan = Planner.FindPathToGoal(_orc, _claimBone.GetObjective(), state);
            Assert.That(plan, Is.Not.Null);
        }

    }
    
    public class TestPlannerWithLargeSets
    {
        private readonly Bag<BaseCreature> _orcs = new();
        // private Bag<BaseItem> _bones = new();
        private readonly ItemManager _itemManager = ItemManager.GetItemManager();

        [OneTimeSetUp]
        public void MakeTooManyOrcs()
        {
            var pickUpItem = new PickUpItem();
            for (var i = 0; i < 1000; i++)
            {
                var orc = new BaseCreature()
                {
                    Location = new Vector2(1, 1),
                    EntityName = "Orc",
                    InstanceName = "Thog",
                    CreatureType = CreatureType.Humanoid,
                    CreatureSubtype = CreatureSubtype.Orc,
                    IdleState = IdleState.Idle
                };

                var bone = new BaseItem()
                {
                    Location = new Vector2(1, 1),
                    EntityName = "Bone",
                    InstanceName = "Big Bone",
                    Weight = 1.2f,
                    Material = Material.Bone
                };
                _itemManager.AddItemToWorld(bone);

                for (var j = 0; j < 10; j++)
                {
                    var anotherBone = new BaseItem()
                    {
                        Location = new Vector2(1, 1),
                        EntityName = "Bone",
                        InstanceName = "Big Bone",
                        Weight = 1.2f,
                        Material = Material.Bone
                    };
                    _itemManager.AddItemToWorld(anotherBone);
                }

                orc.Owned.Add(bone);
                var goal = new ClaimBone(orc);
                orc.Goals.Add(goal);
                // var action = pickUpItem;
                orc.Actions.Add(pickUpItem);
                
                _orcs.Add(orc);
            }
        }

        [Test]
        public void GetPlanForHundredCreatures()
        {
            var hundredOrcs = _orcs.Take(100);
            foreach (var orc in hundredOrcs)
            {
                var state = GoapState.SimulateWorldStateFor(orc);
                var plan = Planner.FindPathToGoal(orc, orc.Goals.FirstOrDefault().GetObjective(), state);
            }
        }
    }
}
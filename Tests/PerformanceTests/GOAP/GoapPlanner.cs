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
        private readonly ItemManager _itemManager = ItemManager.GetItemManager();

        [SetUp]
        public void Setup()
        {
            _orc = new Orc();
            _bone = new Bone();
            _itemManager.AddItemToWorld(_bone);
        }

        [Test]
        public void GetPlanForOneCreature()
        {
            var state = new SimulatedState(_orc);
            var plan = Planner.FindPathToGoal(_orc, _orc.Goals.First().GetObjective(), state);
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
            for (var i = 0; i < 1000; i++)
            {
                var orc = new Orc();
                var bone = new Bone();
                _itemManager.AddItemToWorld(bone);

                for (var j = 0; j < 10; j++)
                {
                    var anotherBone = new Bone();
                    _itemManager.AddItemToWorld(anotherBone);
                }

                orc.AddToCarried(bone);
                
                _orcs.Add(orc);
            }
        }

        [Test]
        public void GetPlanForHundredCreatures()
        {
            var hundredOrcs = _orcs.Take(100);
            foreach (var orc in hundredOrcs)
            {
                var state = new SimulatedState(orc);
                var plan = Planner.FindPathToGoal(orc, orc.Goals.FirstOrDefault()!.GetObjective(), state);
            }
        }
    }
}
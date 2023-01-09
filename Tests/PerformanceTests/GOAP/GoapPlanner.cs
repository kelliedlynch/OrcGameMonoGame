using System.Numerics;
using MonoGame.Extended.Collections;
using OrcGame.OgEntity;
using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;
using OrcGame.GOAP;
using OrcGame.GOAP.Action;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Goal;

namespace Tests.PerformanceTests.GOAP

{
    public class TestPlannerWithSmallSets
    {
        private Creature _orc = null!;
        private Item _bone = null!;
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
        // private Orc[] _orcs;
        private HashSet<Orc> _orcs = new();
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

                for (var j = 0; j < 3; j++)
                {
                    var anotherBone = new Bone();
                    _itemManager.AddItemToWorld(anotherBone);
                    var stick1 = new Stick();
                    _itemManager.AddItemToWorld(stick1);
                    var stick2 = new Stick();
                    _itemManager.AddItemToWorld(stick2);
                }

                orc.AddToOwned(bone);
                
                _orcs.Add(orc);
            }
        }

        [Test]
        public void GetPlanForHundredCreatures()
        {
            // var hundredOrcs = _orcs.Take(100);
            // var plans = new HashSet<Planner.Branch>();
            foreach (var orc in _orcs)
            {
                // var state = new SimulatedState(orc);
                var state = Planner.StatePool.Request();
                state.InitState(orc);
                var plan = Planner.FindPathToGoal(orc, orc.Goals.FirstOrDefault()!.GetObjective(), state);
                Planner.StatePool.Dispose(state);
                Planner.BranchPool.Dispose(plan);
                // plans.Add(plan);
            }
            // Assert.That(plans, Is.Not.Empty);
        }
    }
}
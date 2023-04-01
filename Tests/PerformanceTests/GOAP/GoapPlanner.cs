using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;
using OrcGame.GOAP;
using OrcGame.GOAP.Core;

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
            var plan = Planner.FindPathToGoal(_orc, _orc.Goals.First().GetObjective());
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
        public void MakeABunchOfOrcs()
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
        public void GetPlanForCreatures()
        {
            // var hundredOrcs = _orcs.Take(100);
            // var plans = new HashSet<Planner.Branch>();
            foreach (var orc in _orcs)
            {

                var plan = Planner.FindPathToGoal(orc, orc.Goals.FirstOrDefault()!.GetObjective());
                Planner.BranchPool.Dispose(plan);
                // plans.Add(plan);
            }
            // Assert.That(plans, Is.Not.Empty);
        }
    }
}
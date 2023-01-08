using System.Numerics;
using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;
using OrcGame.GOAP.Action;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Goal;

namespace Tests.UnitTests.GOAP
{
    public class GoapActions
    {
        private Creature _orc = null!;
        private Item _bone = null!;
        private Item _stick = null!;
        private readonly ItemManager _itemManager = ItemManager.GetItemManager();
        
        [SetUp]
        public void Setup()
        {
            _orc = new Orc();
            _bone = new Bone();
            _itemManager.AddItemToWorld(_bone);
        }

        [Test]
        public void PickUpItem_IsValid()
        {
            _orc.AddToTagged(_bone);
            var sim = new SimulatedState(_orc);
            var objective = _orc.Goals.First().GetObjective();
            var isValid = _orc.Actions.First().IsValid(objective);
            Assert.That(isValid, Is.True);
            // Assert.That(isValid["Creature"]["Carried"].Count, Is.Not.Zero);
            // Assert.That(isValid["Creature"]["Tagged"].Count, Is.Zero);
        }

        [TearDown]
        public void TearDown()
        {
            _itemManager.RemoveItemFromWorld(_bone);
            _itemManager.RemoveItemFromWorld(_stick);
        }
    }
}


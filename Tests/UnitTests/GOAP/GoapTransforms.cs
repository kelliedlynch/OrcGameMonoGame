using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using MonoGame.Extended.Collections;
using NUnit.Framework;
using OrcGame.Entity.Item;
using OrcGame.Entity.Creature;
using OrcGame.GOAP;
using OrcGame.GOAP.Core;


namespace Tests
{
    public class GoapTransforms
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
        public void Apply_Simple_Transform()
        {
            var transform = new MathTransform
            {
                Target = "WorkSpeed",
                Operator = MathOperator.Plus,
                Value = 2.5f
            };
            var state = GoapState.SimulateEntity(_orc);
            var afterState = transform.Apply(state);
            
            Assert.That(afterState["WorkSpeed"], Is.EqualTo(3.5f));
        }

        [Test]
        public void Apply_Add_List_Item_Transform()
        {
            var transform = new AddListItemTransform()
            {
                Target = "Tagged",
                AddItem = new Dictionary<string, object>()
                {
                    { "Material", Material.Bone },
                    { "Weight", 2.3f }
                },
                Qty = 2
            };
            var state = GoapState.SimulateEntity(_orc);
            var afterState = transform.Apply(state);
            var tagged = afterState["Tagged"] as Bag<Dictionary<string, object>>;
            
            Assert.That(tagged.Count, Is.EqualTo(2));
        }
    }
}
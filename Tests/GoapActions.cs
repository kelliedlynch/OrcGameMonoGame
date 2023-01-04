﻿using System.Numerics;
using MonoGame.Extended.Collections;
using Moq;
using OrcGame.Entity;
using OrcGame.Entity.Creature;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Goal;
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
            
            // var fakeOrc = new Mock<BaseCreature>();
            // var fakeBone = new Mock<BaseItem>();
            // fakeOrc.SetupAllProperties();
            // fakeOrc.Setup(orc => new Bag<BaseItem>() { fakeBone });
            // fakeOrc.AddToTagged(fakeBone);
            // var fakeGoal = new Mock<ClaimBone>();
            var goal = new ClaimBone(_orc);
            _orc.Tagged.Add(_bone);
            var action = new PickUpItem();
            var sim = GoapState.SimulateWorldStateFor(_orc);
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


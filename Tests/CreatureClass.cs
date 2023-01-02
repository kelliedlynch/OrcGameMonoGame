using System.Numerics;
using OrcGame.Entity.Creature;

namespace Tests;

public class CreatureClass
{
    // Creature dummyCreature;

    [SetUp]
    public void Setup()
    {
        // dummyCreature = new Creature();
    }

    [Test]
    public void Create_New_Creature()
    {
        var creature = new BaseCreature()
        {
            Location = new Vector2(1, 1),
            EntityName = "Orc",
            InstanceName = "Thog",
            CreatureType = CreatureType.Humanoid,
            CreatureSubtype = CreatureSubtype.Orc,
            IdleState = IdleState.Idle
        };
        Assert.That(creature, Is.Not.Null);
    }
}


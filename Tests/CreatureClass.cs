using System.Numerics;
using OrcGame.OgEntity.OgCreature;

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
        var creature = new Orc();
        Assert.That(creature, Is.Not.Null);
    }
}


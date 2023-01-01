using System.Numerics;
using OrcGame.Entity;

namespace Tests;

public class ItemClass
{

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Create_New_Item()
    {
        var item = new Item()
        {
            Location = new Vector2(1, 1),
            EntityName = "Bone",
            InstanceName = "Big Bone",
            Weight = 1.2f,
            Material = Material.Bone
        };
        Assert.That(item, Is.Not.Null);
    }
}
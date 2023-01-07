using System.Numerics;
using OrcGame.Entity.Item;

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
        var item = new Bone();
        Assert.That(item, Is.Not.Null);
    }
}
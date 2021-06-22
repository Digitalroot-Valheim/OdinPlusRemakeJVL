using NUnit.Framework;
using OdinPlusRemakeJVL.Common;

namespace UnitTests
{
  [TestFixture]
  public class NamesTests
  {
    [Test]
    public void PetNamesTest()
    {
      Assert.That(PetNames.TrollPet, Is.Not.Null);
      Assert.That(PetNames.WolfPet, Is.Not.Null);
      Assert.That(PetNames.AllNames, Is.Not.Null);
      Assert.That(PetNames.AllNames, Is.Not.Empty);
      Assert.That(PetNames.AllNames, Is.Unique);
      Assert.That(PetNames.AllNames, Contains.Item(PetNames.TrollPet));
      Assert.That(PetNames.AllNames, Contains.Item(PetNames.WolfPet));
    }

    [Test]
    public void PetsStatusEffectsNamesTest()
    {
      Assert.That(PetsStatusEffectNames.Troll, Is.Not.Null);
      Assert.That(PetsStatusEffectNames.Wolf, Is.Not.Null);
      Assert.That(PetsStatusEffectNames.AllNames, Is.Not.Null);
      Assert.That(PetsStatusEffectNames.AllNames, Is.Not.Empty);
      Assert.That(PetsStatusEffectNames.AllNames, Is.Unique);
      Assert.That(PetsStatusEffectNames.AllNames, Contains.Item(PetsStatusEffectNames.Troll));
      Assert.That(PetsStatusEffectNames.AllNames, Contains.Item(PetsStatusEffectNames.Wolf));
    }
  }
}

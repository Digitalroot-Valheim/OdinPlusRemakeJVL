using NUnit.Framework;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Names;
using System;
using System.IO;
using System.Reflection;

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

    [Test]
    public void LoaderTest()
    {
      var path = Path.Combine(AssemblyDirectory.FullName, "MonoBehaviourRepository.dll");
      Assembly simpleasm = Assembly.LoadFile(path);
      Assert.That(simpleasm, Is.Not.Null);

      Type testBehaviour = simpleasm.GetType("UnRemoveableCustomMonoBehaviour");
      Assert.That(testBehaviour, Is.Not.Null);

      foreach (var type in simpleasm.GetTypes())
      {
        System.Console.WriteLine(type.FullName);
      }
    }

    private static DirectoryInfo AssemblyDirectory
    {
      get
      {
        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        var fi = new FileInfo(Uri.UnescapeDataString(uri.Path));
        return fi.Directory;
      }
    }
  }
}

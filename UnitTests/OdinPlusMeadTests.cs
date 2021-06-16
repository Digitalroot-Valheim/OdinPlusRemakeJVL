using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdinPlus.Common;
using OdinPlus.Items;

namespace UnitTests
{
  [TestFixture]
  public class OdinPlusMeadTests
  {
    [Test]
    public void OdinPlusMead()
    {
      var meadList = MeadFactory.GetMeadList().ToList();
      Assert.That(meadList, Is.Not.Empty);

      foreach (var meadInfo in meadList)
      {
        Console.WriteLine(JsonSerializationProvider.ToJson(meadInfo));
        Assert.That(meadInfo.FullName, Does.StartWith(meadInfo.Name));
        Assert.That(meadInfo.FullName, Does.EndWith(meadInfo.Size));
        Assert.That(meadInfo.Size, Is.EqualTo("S").Or.EqualTo("M").Or.EqualTo("L"));
        Assert.That(meadInfo.IconName, Does.StartWith(meadInfo.FullName));
        Assert.That(meadInfo.IconName, Does.EndWith(".png"));
        Assert.That(meadInfo.Cost, Is.Positive);
      }
    }
  }
}

using System;
using System.Collections.Generic;

namespace OdinPlus.Items
{
  public static class MeadFactory
  {
    public static IEnumerable<MeadInfo> GetMeadList()
    {
      var defaultCostSmall = 20;
      var defaultCostMMedium = 20;
      var defaultCostLarge = 20;

      foreach (var mead in (MeadEnum[])Enum.GetValues(typeof(MeadEnum)))
      {
        switch (mead)
        {
          case MeadEnum.Exp:
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Small, Cost = 5 };
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Medium, Cost = 10 };
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Large, Cost = 20 };
            break;

          case MeadEnum.Weight:
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Small, Cost = 20 };
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Medium, Cost = 30 };
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Large, Cost = 40 };
            break;

          case MeadEnum.Invisible:
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Small, Cost = 30 };
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Medium, Cost = 60 };
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Large, Cost = 90 };
            break;

          case MeadEnum.Speed:
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Large, Cost = 20 };
            break;

          case MeadEnum.Pickaxe:
          case MeadEnum.Bows:
          case MeadEnum.Swords:
          case MeadEnum.Axe:
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Small, Cost = defaultCostSmall };
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Medium, Cost = defaultCostMMedium };
            yield return new MeadInfo { Name = MeadsNames.GetMeadName(mead), Size = MeadSize.Large, Cost = defaultCostLarge };
            break;

          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
  }
}

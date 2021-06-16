using System;

namespace OdinPlus.Items
{
  public static class MeadsNames
  {
    private const string Mead = "Mead";
    public static readonly string Exp = $"Exp{Mead}";
    public static readonly string Weight = $"Weight{Mead}";
    public static readonly string Invisible = $"Invisible{Mead}";
    public static readonly string Pickaxe = $"Pickaxe{Mead}";
    public static readonly string Bows = $"Bows{Mead}";
    public static readonly string Swords = $"Swords{Mead}";
    public static readonly string Speed = $"Speed{Mead}";
    public static readonly string Axe = $"Axe{Mead}";

    public static string GetMeadName(MeadEnum meadEnum)
    {
      switch (meadEnum)
      {
        case MeadEnum.Exp:
          return Exp;
        case MeadEnum.Weight:
          return Weight;
        case MeadEnum.Invisible:
          return Invisible;
        case MeadEnum.Pickaxe:
          return Pickaxe;
        case MeadEnum.Bows:
          return Bows;
        case MeadEnum.Swords:
          return Swords;
        case MeadEnum.Speed:
          return Speed;
        case MeadEnum.Axe:
          return Axe;
        default:
          throw new ArgumentOutOfRangeException(nameof(meadEnum), meadEnum, null);
      }
    }
  }
}

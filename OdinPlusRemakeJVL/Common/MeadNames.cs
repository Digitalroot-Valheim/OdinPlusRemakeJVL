using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common
{
  public static class MeadNames
  {
    public static string ExpMeadS = nameof(ExpMeadS);
    public static string ExpMeadM = nameof(ExpMeadM);
    public static string ExpMeadL = nameof(ExpMeadL);
    public static string WeightMeadS = nameof(WeightMeadS);
    public static string WeightMeadM = nameof(WeightMeadM);
    public static string WeightMeadL = nameof(WeightMeadL);
    public static string InvisibleMeadS = nameof(InvisibleMeadS);
    public static string InvisibleMeadM = nameof(InvisibleMeadM);
    public static string InvisibleMeadL = nameof(InvisibleMeadL);
    public static string PickaxeMeadS = nameof(PickaxeMeadS);
    public static string PickaxeMeadM = nameof(PickaxeMeadM);
    public static string PickaxeMeadL = nameof(PickaxeMeadL);
    public static string BowsMeadS = nameof(BowsMeadS);
    public static string BowsMeadM = nameof(BowsMeadM);
    public static string BowsMeadL = nameof(BowsMeadL);
    public static string SwordsMeadS = nameof(SwordsMeadS);
    public static string SwordsMeadM = nameof(SwordsMeadM);
    public static string SwordsMeadL = nameof(SwordsMeadL);
    public static string SpeedMeadsL = nameof(SpeedMeadsL);
    public static string AxeMeadS = nameof(AxeMeadS);
    public static string AxeMeadM = nameof(AxeMeadM);
    public static string AxeMeadL = nameof(AxeMeadL);

    public static readonly IEnumerable<string> AllNames = Utils.AllNames(typeof(MeadNames));
  }
}

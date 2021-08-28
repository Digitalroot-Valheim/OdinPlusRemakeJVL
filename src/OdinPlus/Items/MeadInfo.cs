namespace OdinPlus.Items
{
  public class MeadInfo
  {
    public string Name { get; set; }
    public string Size { get; set; }
    public int Cost { get; set; }
    public string FullName => $"{Name}{Size}";
    public string IconName => $"{FullName}.png";
    public override string ToString() => FullName;
  }
}

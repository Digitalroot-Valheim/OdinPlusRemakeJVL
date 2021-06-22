namespace OdinPlusRemakeJVL.Managers
{
  public interface IAbstractManager
  {
    bool IsInitialized { get; }
    void Initialize();
    bool PostInitialize();
  }
}

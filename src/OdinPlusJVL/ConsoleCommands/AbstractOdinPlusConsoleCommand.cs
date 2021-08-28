using Jotunn.Entities;
using Jotunn.Managers;

namespace OdinPlusJVL.ConsoleCommands
{
  internal abstract class AbstractOdinPlusConsoleCommand : ConsoleCommand
  {
    public override string Help => LocalizationManager.Instance.TryTranslate($"$op_{Name}_command_help");
  }
}

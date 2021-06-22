using Jotunn.Entities;
using Jotunn.Managers;

namespace OdinPlusRemakeJVL.ConsoleCommands
{
  internal abstract class AbstractOdinPlusConsoleCommand : ConsoleCommand
  {
    public override string Help => LocalizationManager.Instance.TryTranslate($"$op_{Name}_command_help");
  }
}

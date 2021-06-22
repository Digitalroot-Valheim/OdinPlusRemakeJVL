using System.Collections.Generic;
using System.Linq;

namespace OdinPlusRemakeJVL.Common
{
  public static class Utils
  {
    internal static IEnumerable<string> AllNames(System.Type type)
    {
      var f = type.GetFields().Where(f1=> f1.FieldType == typeof(string));
      // var m = type.GetMembers();
      // var p = type.GetProperties();
      foreach (var fieldInfo in f)
      {
        yield return fieldInfo.GetValue(null).ToString();
      }
    }
  }
}

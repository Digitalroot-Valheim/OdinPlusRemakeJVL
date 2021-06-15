using UnityEngine;

namespace OdinPlus.Common
{
  public static class JsonSerializationProvider 
  {
    public static T FromJson<T>(string json)
    {
      return JsonUtility.FromJson<T>(json);
    }

    public static string ToJson(object obj)
    {
      return JsonUtility.ToJson(obj);
    }
  }
}

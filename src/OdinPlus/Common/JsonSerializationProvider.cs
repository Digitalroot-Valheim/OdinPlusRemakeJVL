using fastJSON;
using UnityEngine;

namespace OdinPlus.Common
{
  /// <summary>
  /// Can't use Unity's JsonUtility because of Error: "ECall methods must be packaged into a system module."
  /// Can't use litJson because it is unable to convert Vector3 objects.
  /// </summary>
  public static class JsonSerializationProvider
  {
    private static readonly JSONParameters JsonParameters = new JSONParameters {SerializeNullValues = false, SerializeToLowerCaseNames = false, BadListTypeChecking = true, ShowReadOnlyProperties = true, KVStyleStringDictionary = false, UseEscapedUnicode = true, InlineCircularReferences = true};
    private static bool _customTypeLoaded;

    private static void Init()
    {
      if (_customTypeLoaded) return;

      JSON.RegisterCustomType(typeof(Vector3),
        (x) =>
        {
          var v3 = (Vector3) x;
          var a = new[] { v3.x, v3.y, v3.z };
          return JSON.ToJSON(a);
        },
        (x) =>
        {
          var a = JSON.ToObject<float[]>(x);
          return new Vector3(a[0], a[1], a[2]);
        });

      JSON.RegisterCustomType(typeof(Quaternion),
        (x) =>
        {
          var q = (Quaternion)x;
          var a = new[] { q.x, q.y, q.z, q.w };
          return JSON.ToJSON(a);
        },
        (x) =>
        {
          var a = JSON.ToObject<float[]>(x);
          return new Quaternion(a[0], a[1], a[2], a[3]);
        });
      
      _customTypeLoaded = true;

    }

    public static T FromJson<T>(string json)
    {
      Init();
      return JSON.ToObject<T>(json, JsonParameters); // fastJson
      // return JsonMapper.ToObject<T>(json);  // litJson
      // return JsonUtility.FromJson<T>(json); // Unity
    }

    public static string ToJson(object obj, bool pretty = false)
    {
      Init();
      if (pretty)
      {
        return JSON.ToNiceJSON(obj, JsonParameters); // fastJson
      }
      return JSON.ToJSON(obj, JsonParameters); // fastJson
      // return JsonMapper.ToJson(obj);  // litJson
      // return JsonUtility.ToJson(obj); // Unity
    }
  }
}

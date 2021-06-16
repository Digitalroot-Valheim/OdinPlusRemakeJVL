namespace OdinPlus.Common
{
  /// <summary>
  /// Can't use Unity's JsonUtility because of Error: "ECall methods must be packaged into a system module."
  /// Can't use litJson because it is unable to convert Vector3 objects.
  /// </summary>
  public static class JsonSerializationProvider 
  {
    public static T FromJson<T>(string json)
    {
      return fastJSON.JSON.ToObject<T>(json);  // fastJson
      // return JsonMapper.ToObject<T>(json);  // litJson
      // return JsonUtility.FromJson<T>(json); // Unity
    }

    public static string ToJson(object obj)
    {
      return fastJSON.JSON.ToJSON(obj, new fastJSON.JSONParameters { SerializeNullValues = false, SerializeToLowerCaseNames = false });  // fastJson
      // return JsonMapper.ToJson(obj);  // litJson
      // return JsonUtility.ToJson(obj); // Unity
    } 
  }
}

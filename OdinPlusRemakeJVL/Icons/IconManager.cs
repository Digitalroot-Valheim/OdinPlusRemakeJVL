using System.Linq;
using System.Reflection;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace OdinPlusRemakeJVL.Icons
{
  internal static class IconManager
  {
    public static Sprite LoadResourceIcon(string name)
    {
      return LoadSpriteFromTexture(LoadTextureRaw(GetResource(Assembly.GetCallingAssembly(), "OdinPlusRemakeJVL.Resources." + name + ".png")));
    }

    private static Texture2D LoadTextureRaw(byte[] file)
    {
      if (file.Any())
      {
        Texture2D texture2D = new Texture2D(2, 2);
        bool flag2 = texture2D.LoadImage(file);
        if (flag2)
        {
          return texture2D;
        }
      }
      return null;
    }

    private static byte[] GetResource(Assembly asm, string resourceName)
    {
      var manifestResourceStream = asm.GetManifestResourceStream(resourceName);
      Debug.Assert(manifestResourceStream != null, nameof(manifestResourceStream) + " != null");
      var array = new byte[manifestResourceStream.Length];
      manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
      return array;
    }

    private static Sprite LoadSpriteFromTexture(Texture2D spriteTexture, float pixelsPerUnit = 100f)
    {
      return spriteTexture ? Sprite.Create(spriteTexture, new Rect(0f, 0f, spriteTexture.width, spriteTexture.height), new Vector2(0f, 0f), pixelsPerUnit) : null;
    }
  }
}

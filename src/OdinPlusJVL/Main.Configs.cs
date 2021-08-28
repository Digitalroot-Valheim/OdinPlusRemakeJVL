using BepInEx.Configuration;
using Digitalroot.Valheim.Common.Interfaces;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace OdinPlusJVL
{
  public partial class Main
  {
    internal const string Version = "1.0.0";
    internal const string Name = "OdinPlusJVL";
    // ReSharper disable once MemberCanBePrivate.Global
    public const string Guid = "digitalroot.valheim.mods.odinplusjvl";
    public const string Namespace = Name;
    public static Main Instance;
    private Harmony _harmony;
    private List<IInitializeable> _managersList;
    // ReSharper disable once MemberCanBePrivate.Global
    internal static GameObject RootObject;

    #region Config

    // ReSharper disable once MemberCanBePrivate.Global
    public static ConfigEntry<int> NexusId;
    internal static ConfigEntry<Vector3> ConfigEntryOdinPosition;

    // ReSharper disable once MemberCanBePrivate.Global
    internal static ConfigEntry<bool> ConfigEntryForceOdinPosition;


    private void InitializeConfigs()
    {
      Config.SaveOnConfigSet = true;
      NexusId = Config.Bind("Config", "NexusID", 000, new ConfigDescription("Nexus mod ID for updates", null, new ConfigurationManagerAttributes { Browsable = false }));
      ConfigEntryOdinPosition = Config.Bind("Config", "Odins Position", Vector3.zero, new ConfigDescription("Odin's Position", null, new ConfigurationManagerAttributes { Browsable = false, IsAdminOnly = true }));
      ConfigEntryForceOdinPosition = Config.Bind("Config", "Force Odins Position", false, new ConfigDescription("Force Odin's Position", null, new ConfigurationManagerAttributes { Browsable = false, IsAdminOnly = true }));
    }

    #endregion

    private void InitializeRootObject()
    {
      RootObject = new GameObject(Name)
      {
        transform =
        {
          position = Vector3.zero, rotation = Quaternion.identity
        }
      };
      DontDestroyOnLoad(RootObject);
    }
  }
}

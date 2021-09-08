using JetBrains.Annotations;
using OdinPlusJVL.OdinsCamp;
using UnityEngine;

namespace OdinPlusJVL.Behaviours
{
  /// <summary>
  /// Used for debugging the campsite. 
  /// </summary>
  [UsedImplicitly]
  public class OdinsCampBehaviour : AbstractCustomMonoBehaviour
  {
    public GameObject 
       _odinsCauldron
      , _odinsEmissary
      , _odinsFirePit
      , _odinsMunin
      , _odinsShaman
      ;

    public void Awake()
    {

    }

    public void SpawnCampMembers() => OdinsCampManager.Instance.SpawnCampMembers();
  }
}

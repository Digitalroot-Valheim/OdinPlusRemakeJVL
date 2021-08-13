using HarmonyLib;
using OdinPlus.Common;
using OdinPlus.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

//opt:Create base class for task target//notice Use Invoke
namespace OdinPlus.Quests
{
  public class HuntTarget : MonoBehaviour
	{
		#region var
		private ZNetView _zNetView;
		public string Id = "";
		public int Level=1;
		public int Key=1;
		public string OwnerName = "";
		public bool Placing = false;
		private Character _character;
		private Humanoid _humanoid;
		private CharacterDrop _characterDrop;
		private MonsterAI _monsterAi;

		#endregion var

		#region Mono +Death
		private void Awake()
		{
			_zNetView = gameObject.GetComponent<ZNetView>();
			_character = gameObject.GetComponent<Character>();
			_monsterAi = gameObject.GetComponent<MonsterAI>();
			_characterDrop = GetComponent<CharacterDrop>();
			_character.m_onDeath = (Action)Delegate.Combine(new Action(this.OnDeath), _character.m_onDeath);
			_humanoid = gameObject.GetComponent<Humanoid>();

		}
		private void Start()
		{
			var zdo = _zNetView.GetZDO();
			if (Id != "")
			{

				zdo.Set("TaskID", Id);
				zdo.Set("HuntLevel", Level);
				zdo.Set("HuntKey", Key);
				zdo.Set("OwnerName", OwnerName);
				Tweakers.ValSpawn("vfx_GodExplosion", transform.position);
			}
			else
			{
				Id = zdo.GetString("TaskID");
				Level = zdo.GetInt("HuntLevel");
				Key = zdo.GetInt("HuntKey");
				OwnerName = zdo.GetString("OwnerName", "");
			}
			_monsterAi.SetPatrolPoint();
      _humanoid.m_seman.AddStatusEffect(StatusEffectsManager.Instance.GetMonsterStatusEffectsListByIndex(Level).Key);
		}
		private void Update()
		{
			if (Id=="")
			{
				return;
			}
			var m_task = QuestManager.Instance.GetQuest(Id);
			if (m_task == null)
			{
				DBG.blogInfo("Cant find task,Destroy Hunt Target" + Id);
				Traverse.Create(_characterDrop).Field<bool>("m_dropsEnabled").Value = false;
				_zNetView.Destroy();
				return;
			}
		}
		public void OnDeath()
		{
			if (Player.GetClosestPlayer(transform.position, 100).GetHoverName() == OwnerName)
			{
				QuestManager.Instance.GetQuest(Id).Finish();
			}
			else
			{
				if (OwnerName == "")
				{

				}
				else
				{
					string n = string.Format("Hey you found the chest belong to <color=yellow><b>{0}</b></color>", OwnerName);//trans
					DBG.InfoCT(n);
				}
			}
			Tweakers.ValSpawn("vfx_GodExplosion", transform.position);
			var r = Instantiate(ZNetScene.instance.GetPrefab(OdinPlusItem.OdinLegacy), transform.localPosition, Quaternion.identity);
			r.GetComponent<ItemDrop>().m_itemData.m_quality = Key;
			r.GetComponent<ItemDrop>().m_itemData.m_stack = Level;

		}

		#endregion Mono

		#region Tool
		public void Setup(int key, int lvl)
		{
			Level = lvl;
			Key = key;
			_character.SetLevel(Mathf.Clamp(Level + 2, 2, 5));
			_character.m_health *= (0.5f * Level + 1);
			_humanoid.m_faction = Character.Faction.Boss;
		}
		public static GameObject CreateMonster(string name)
		{
			var go = Instantiate(ZNetScene.instance.GetPrefab(name), OdinPlus.PrefabParent.transform);
			name = Regex.Replace(name, @"[_]", "");
			go.name = name + "Hunt";
			go.AddComponent<HuntTarget>();
			go.GetComponent<Humanoid>().m_name += " $op_hunt_target";
			DestroyImmediate(go.GetComponent<CharacterDrop>());
			var fx = Instantiate(FxAssetManager.Instance.GetFxNN(OdinPlusFx.GreenSmoke), go.transform);
			fx.transform.position = go.FindObject("Spine2").transform.position;//opt Random smoke
			return go;
		}
		public void CreateDrop()
		{
			var d = new CharacterDrop.Drop();
			d.m_chance = 1;
			d.m_amountMax = Level + Key;
			d.m_amountMin = d.m_amountMax;
			d.m_levelMultiplier = false;
			d.m_prefab = ZNetScene.instance.GetPrefab(OdinPlusItem.OdinLegacy);
			_characterDrop.m_drops = new List<CharacterDrop.Drop>();
			Traverse.Create(_characterDrop).Field<bool>("m_dropsEnabled").Value = true;
			_characterDrop.m_drops.Add(d);
		}
		public static void Place(Vector3 pos, string monster, string id,string _owner, int p_key, int p_lvl)
		{
			float y = 0;
			ZoneSystem.instance.FindFloor(pos, out y);
			pos = new Vector3(pos.x, y + 2, pos.z + 5);
			var Reward = Instantiate(ZNetScene.instance.GetPrefab(monster + "Hunt"), pos, Quaternion.identity);
			Reward.GetComponent<HuntTarget>().Id = id;
			Reward.GetComponent<HuntTarget>().OwnerName=_owner;
			Reward.GetComponent<HuntTarget>().Setup(p_key, p_lvl);
			DBG.blogWarning("Placed Hunt " + monster + " at : " + Reward.transform.localPosition);
		}
		#endregion Tool

	}

}
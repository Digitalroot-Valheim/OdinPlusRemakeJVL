using System;
using System.Collections.Generic;
using HarmonyLib;
using OdinPlus.Common;
using OdinPlus.Managers;
using UnityEngine;

namespace OdinPlus.Pets
{
	public class PetWolf : MonoBehaviour, OdinInteractable
	{
		#region var
		private Container container;
		private Tameable tame;
		private Inventory m_inventory;
		private Humanoid m_hum;
		private float m_weight=500;
		#endregion var

		#region Mono
		private void Awake()
		{
			PetManager.WolfIns = this.gameObject;
			container = this.GetComponent<Container>();

			Character character = this.GetComponent<Character>();
			character.m_onDeath = (Action)Delegate.Combine(new Action(this.OnDeath),character.m_onDeath);

			tame = this.GetComponent<Tameable>();
			tame.Tame();
			tame.m_fedDuration = 1800;
			Traverse.Create(tame).Method("ResetFeedingTimer").GetValue();

			m_hum = this.GetComponent<Humanoid>();
		}
		private void Start()
		{
			m_inventory = Traverse.Create(container).Field<Inventory>("m_inventory").Value;
		}
		private void Update()
		{
			if (this.GetComponent<Tameable>().IsHungry())
			{
				ZNetScene.instance.Destroy(this.gameObject);
			}
			var weight = Traverse.Create(m_inventory).Field<float>("m_totalWeight").Value;
			if (weight > 0)
			{
				if (weight >= m_weight)
				{
					m_hum.ChangeSpeed(0.5f);
					return;
				}
				m_hum.ChangeSpeed((m_weight - weight) / m_weight * 1.5f + 0.5f);
				return;
			}
			m_hum.ChangeSpeed(2);
		}
		public void SecondaryInteract(Humanoid user)
		{
			container.Interact(user, false);
		}

		#endregion Mono
		#region Feature
		public void Teleport()
		{
			this.transform.position = Player.m_localPlayer.transform.forward * 2f + Vector3.up;
		}
		private void OnDeath()
		{
			if (m_inventory.SlotsUsedPercentage() == 0)
			{
				return;
			}
			List<ItemDrop.ItemData> allItems = m_inventory.GetAllItems();
			int num = 1;
			foreach (ItemDrop.ItemData item in allItems)
			{
				Vector3 position = base.transform.position + Vector3.up * 0.5f + UnityEngine.Random.insideUnitSphere * 0.3f;
				Quaternion rotation = Quaternion.Euler(0f, (float)UnityEngine.Random.Range(0, 360), 0f);
				ItemDrop.DropItem(item, 0, position, rotation);
				num++;
			}
			//m_inventory.RemoveAll();
			//Traverse.Create(container).Method("Save").GetValue();
		}
		#endregion Feature

	}
}
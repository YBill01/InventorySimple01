using GameName.Gameplay.Objects;
using System;
using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Game/ItemData", fileName = "Item", order = 20)]
	public class ItemData : ScriptableObject, IEquatable<ItemData>
	{
		public int ID;

		[Space]
		public string Name;

		[Space]
		public ItemType type;

		[Space]
		public InteractionCondition interaction;

		[Space]
		public ItemViewData view;

		[Space]
		public int stackCapacity = 1;

		public bool Equals(ItemData other)
		{
			return ID == other.ID;
		}
	}
}
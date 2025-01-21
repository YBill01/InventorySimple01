using System.Collections.Generic;
using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Core/SharedData", fileName = "SharedData", order = 6)]
	public class SharedData : ScriptableObject
	{
		public ItemData[] itemsData;

		private Dictionary<int, ItemData> _items;

		public void HashingData()
		{
			_items = new Dictionary<int, ItemData>();

			for (int i = 0; i < itemsData.Length; i++)
			{
				_items.Add(itemsData[i].ID, itemsData[i]);
			}
		}

		public ItemData GetItemData(int id)
		{
			if (_items.TryGetValue(id, out ItemData value))
			{
				return value;
			}

			return default;
		}
	}
}
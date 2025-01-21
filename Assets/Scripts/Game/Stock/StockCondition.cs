using GameName.Data;
using GameName.Gameplay.Objects;
using System;
using System.Linq;
using UnityEngine;

namespace GameName.Gameplay.Features.Stock
{
	[Serializable]
	public struct StockCondition : IStockPredicate<StockItem>
	{
		public ItemType[] types;

		public IncludeItemsFilter includeFilter;
		public ExcludeItemsFilter excludeFilter;

		/// <summary>
		/// automatic data binding in Editor
		/// TODO use db
		/// </summary>
		[HideInInspector]
		public SharedData sharedData;

		[Serializable]
		public struct IncludeItemsFilter
		{
			public ItemData[] items;

			public bool Has(ItemData item)
			{
				return items.Length == 0 || items.Contains(item);
			}
		}
		[Serializable]
		public struct ExcludeItemsFilter
		{
			public ItemData[] items;

			public bool Has(ItemData item)
			{
				return items.Contains(item);
			}
		}

		public bool Has(StockItem item)
		{
			ItemData itemData = sharedData.GetItemData(item.id);

			return types.Contains(itemData.type) && includeFilter.Has(itemData) && !excludeFilter.Has(itemData);
		}
	}
}
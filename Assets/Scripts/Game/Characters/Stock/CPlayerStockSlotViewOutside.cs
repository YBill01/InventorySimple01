using GameName.Data;
using GameName.Gameplay.Features.Stock;
using GameName.Gameplay.Objects;
using UnityEngine;

namespace GameName.Gameplay
{
	public class CPlayerStockSlotViewOutside : CPlayerStockSlotView
	{
		[Space]
		[SerializeField]
		private Transform[] m_itemsContainers;

		public override Transform GetTransform(int itemIndex)
		{
			return _items[itemIndex].transform;
		}

		public override StockedItem Add(StockItem item, int slotIndex, int itemIndex)
		{
			ItemData itemData = _sharedData.GetItemData(item.id);

			StockedItem stockedItem = _itemFactory.Instantiate(
				itemData.view.stockedPrefab,
				itemData,
				Vector3.zero,
				Quaternion.identity,
				Vector3.one);

			stockedItem.SetStockViewItemData(new StockViewItemData
			{
				item = item,
				stockView = _parent,
				slotIndex = slotIndex,
				itemIndex = itemIndex
			});

			stockedItem.gameObject.SetActive(!item.inactive);

			stockedItem.transform.SetParent(m_itemsContainers[itemIndex], false);
			stockedItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

			_items[itemIndex] = stockedItem;

			return stockedItem;
		}
		public override void Remove(StockItem item, int itemIndex)
		{
			StockedItem stockedItem = _items[itemIndex];
			_itemFactory.Dispose(_sharedData.GetItemData(item.id).view.stockedPrefab, stockedItem);
			stockedItem.SetStockViewItemData(default);

			_items[itemIndex] = null;

			_modelSlot.Sort(_quantityComparer);
		}
		public override void Clear()
		{
			for (int i = 0; i < _items.Length; i++)
			{
				if (_items[i] != null)
				{
					_itemFactory.Dispose(_sharedData.GetItemData(_items[i].StockViewItemData.item.id).view.stockedPrefab, _items[i]);
					_items[i] = null;
				}
			}
		}

		protected override void OnChanged(StockSlot<StockItem> modelSlot)
		{
			Clear();

			for (int i = 0; i < modelSlot.Capacity; i++)
			{
				if (modelSlot[i].Quantity > 0)
				{
					Add(modelSlot[i], _slotIndex, i);
				}
			}
		}
	}
}
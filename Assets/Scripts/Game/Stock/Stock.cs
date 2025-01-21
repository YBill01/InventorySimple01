using GameName.Data;
using System;

namespace GameName.Gameplay.Features.Stock
{
	public class Stock : StockBase<StockItem>
	{
		public event Action<StockItem, int, int> OnQuantityChanged = delegate { };
		public event Action<int, int, bool> OnInactivateItem = delegate { };

		private StockConfigData _config;
		public StockConfigData Config => _config;

		public Stock(StockConfigData config) : base()
		{
			_config = config;

			for (int i = 0; i < _config.slots.Length; i++)
			{
				_slots.Add(new StockSlot<StockItem>(_config.slots[i].capacity, _config.slots[i].condition));
			}
		}

		public bool Has(StockItem item, out int slotIndex, out int itemIndex, bool invert = false, bool excludeInactive = true)
		{
			slotIndex = -1;
			itemIndex = -1;

			if (excludeInactive)
			{
				if (item.inactive)
				{
					return false;
				}

				for (int i = 0; i < _slots.Count; i++)
				{
					if (_slots[i].Has(item, out itemIndex, invert))
					{
						if (!_slots[i][itemIndex].inactive)
						{
							slotIndex = i;

							return true;
						}
					}
				}
			}
			else
			{
				return base.Has(item, out slotIndex, out itemIndex, invert);
			}

			return false;
		}
		public bool HasEmpty(StockItem item, out int slotIndex, out int itemIndex, out bool isEmpty, bool hasQuantity = false)
		{
			slotIndex = -1;
			itemIndex = -1;
			isEmpty = false;

			for (int i = 0; i < _slots.Count; i++)
			{
				if (hasQuantity)
				{
					if (_slots[i].HasCondition(item))
					{
						for (int j = 0; j < _slots[i].Capacity; j++)
						{
							if (_slots[i][j].Quantity == 0)
							{
								slotIndex = i;
								itemIndex = j;

								isEmpty = true;

								return true;
							}

							if (_slots[i][j].Equals(item) && _slots[i][j].Quantity > 0)
							{
								ItemData itemData = _config.sharedData.GetItemData(_slots[i][j].id);
								if (itemData.interaction.stackable && item.Quantity + _slots[i][j].Quantity <= itemData.stackCapacity)
								{
									slotIndex = i;
									itemIndex = j;

									return true;
								}
							}
						}
					}
				}
				else if (_slots[i].HasEmpty(item, out itemIndex))
				{
					slotIndex = i;
					isEmpty = true;

					return true;
				}
			}

			return false;
		}

		public bool TryAdd(StockItem item, out int slotIndex, out int itemIndex, bool hasQuantity = false)
		{
			if (hasQuantity && HasEmpty(item, out slotIndex, out itemIndex, out bool isEmpty, true))
			{
				if (isEmpty)
				{
					return base.TryAdd(item, out slotIndex, out itemIndex);
				}

				_slots[slotIndex].ChangeQuantity(itemIndex, _slots[slotIndex][itemIndex].Quantity + item.Quantity);

				OnQuantityChanged(_slots[slotIndex][itemIndex], slotIndex, itemIndex);

				return true;
			}

			return base.TryAdd(item, out slotIndex, out itemIndex);
		}
		public bool TryTake(StockItem item, out int slotIndex, out int itemIndex, bool hasQuantity = false, bool excludeInactive = true)
		{
			if (hasQuantity && Has(item, out slotIndex, out itemIndex, hasQuantity, excludeInactive))
			{
				if (_slots[slotIndex][itemIndex].Quantity - item.Quantity > 0)
				{
					_slots[slotIndex].ChangeQuantity(itemIndex, _slots[slotIndex][itemIndex].Quantity - item.Quantity);

					OnQuantityChanged(_slots[slotIndex][itemIndex], slotIndex, itemIndex);

					return true; 
				}
			}
			else if (excludeInactive && Has(item, out slotIndex, out itemIndex, hasQuantity, excludeInactive))
			{
				return base.TryTake(slotIndex, itemIndex);
			}
			else
			{
				slotIndex = -1;
				itemIndex = -1;

				return false;
			}

			return base.TryTake(item, out slotIndex, out itemIndex);
		}
		public bool TryTake(int slotIndex, int itemIndex, int count)
		{
			if (_slots[slotIndex][itemIndex].Quantity - count > 0)
			{
				_slots[slotIndex].ChangeQuantity(itemIndex, _slots[slotIndex][itemIndex].Quantity - count);

				OnQuantityChanged(_slots[slotIndex][itemIndex], slotIndex, itemIndex);

				return true;
			}

			return base.TryTake(slotIndex, itemIndex);
		}

		public bool TryInactivateItem(int slotIndex, int itemIndex, bool value)
		{
			if (_slots[slotIndex][itemIndex].Quantity > 0)
			{
				StockItem item = _slots[slotIndex][itemIndex];
				item.inactive = value;
				_slots[slotIndex][itemIndex] = item;

				OnInactivateItem(slotIndex, itemIndex, value);

				return true;
			}

			return false;
		}
	}
}
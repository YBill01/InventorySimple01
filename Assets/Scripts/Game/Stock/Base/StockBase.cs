using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameName.Gameplay.Features.Stock
{
	public abstract class StockBase<T> : IEnumerable<StockSlot<T>> where T : struct, IStockItem<T>
	{
		public event Action<StockBase<T>> OnChanged = delegate { };
		public event Action<T, int, int> OnAdd = delegate { };
		public event Action<T, int, int> OnTake = delegate { };

		public int Capacity => _slots.Sum(x => x.Capacity);
		public int Count => _slots.Sum(x => x.Count);

		public bool IsEmpty => Count == 0;
		public bool IsFull => Count >= Capacity;

		public StockSlot<T> this[int index] => _slots[index];

		protected List<StockSlot<T>> _slots;

		public StockBase()
		{
			_slots = new List<StockSlot<T>>();
		}

		public virtual bool Has(T item, out int slotIndex, out int itemIndex, bool invert = false)
		{
			slotIndex = -1;
			itemIndex = -1;

			for (int i = 0; i < _slots.Count; i++)
			{
				if (_slots[i].Has(item, out itemIndex, invert))
				{
					slotIndex = i;

					return true;
				}
			}

			return false;
		}
		public virtual bool HasEmpty(T item)
		{
			for (int i = 0; i < _slots.Count; i++)
			{
				if (_slots[i].HasEmpty(item))
				{
					return true;
				}
			}

			return false;
		}
		public virtual bool HasEmpty(T item, out int slotIndex, out int itemIndex)
		{
			slotIndex = -1;
			itemIndex = -1;

			for (int i = 0; i < _slots.Count; i++)
			{
				if (_slots[i].HasEmpty(item, out itemIndex))
				{
					slotIndex = i;

					return true;
				}
			}

			return false;
		}

		public virtual void Add(T item, int slotIndex, int itemIndex)
		{
			_slots[slotIndex].Insert(item, itemIndex);

			OnAdd(item, slotIndex, itemIndex);
		}
		public virtual bool TryAdd(T item, out int slotIndex, out int itemIndex)
		{
			slotIndex = -1;
			itemIndex = -1;

			for (int i = 0; i < _slots.Count; i++)
			{
				if (_slots[i].TryInsert(item, out itemIndex))
				{
					slotIndex = i;

					OnAdd(item, i, itemIndex);

					return true;
				}
			}

			return false;
		}
		public virtual bool TryTake(T item, out int slotIndex, out int itemIndex, bool invert = true)
		{
			slotIndex = -1;
			itemIndex = -1;

			for (int i = 0; i < _slots.Count; i++)
			{
				if (_slots[i].TryExtract(item, out itemIndex, invert))
				{
					slotIndex = i;

					OnTake(item, i, itemIndex);

					return true;
				}
			}

			return false;
		}
		public virtual bool TryTake(int slotIndex, int itemIndex)
		{
			return TryTake(slotIndex, itemIndex, out T item);
		}
		public virtual bool TryTake(int slotIndex, int itemIndex, out T item)
		{
			item = default;

			if (_slots[slotIndex][itemIndex].Quantity > 0)
			{
				item = _slots[slotIndex].Extract(itemIndex);

				OnTake(item, slotIndex, itemIndex);

				return true;
			}

			return false;
		}

		public void Clear()
		{
			foreach (StockSlot<T> slot in _slots)
			{
				slot.Clear();
			}

			OnChanged(this);
		}

		public IEnumerator<StockSlot<T>> GetEnumerator() => _slots.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
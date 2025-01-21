using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameName.Gameplay.Features.Stock
{
	public class StockSlot<T> : IEnumerable<T> where T : struct, IStockItem<T>
	{
		public event Action<StockSlot<T>> OnChanged = delegate { };
		public event Action<T, int> OnQuantityChanged = delegate { };
		public event Action<T, int> OnAdd = delegate { };
		public event Action<T, int> OnRemove = delegate { };

		public T[] items;

		private int _capacity;
		public int Capacity
		{
			get => _capacity;
			set
			{
				_capacity = value;

				Array.Resize(ref items, _capacity);

				OnChanged(this);
			}
		}
		public int Count => items.Count(x => x.Quantity > 0);

		public bool IsEmpty => Count == 0;
		public bool IsFull => Count >= _capacity;

		public T this[int index]
		{
			get => items[index];
			set => items[index] = value;
		}
		//public T this[int index] => items[index];
		
		private IStockPredicate<T> _condition;

		public StockSlot(int capacity, IStockPredicate<T> condition, IList<T> initialList = null)
		{
			_capacity = capacity;
			_condition = condition;

			items = new T[capacity];

			if (initialList != null)
			{
				initialList.Take(capacity)
					.ToArray()
					.CopyTo(items, 0);

				OnChanged(this);
			}
		}

		public bool Has(T item, out int index, bool invert = false)
		{
			index = -1;

			if (invert)
			{
				for (int i = items.Length - 1; i >= 0; i--)
				{
					if (items[i].Equals(item) && items[i].Quantity > 0)
					{
						index = i;

						return true;
					}
				}
			}
			else
			{
				for (int i = 0; i < items.Length; i++)
				{
					if (items[i].Equals(item) && items[i].Quantity > 0)
					{
						index = i;

						return true;
					}
				}
			}

			return false;
		}
		public bool HasEmpty(T item)
		{
			return !IsFull && _condition.Has(item);
		}
		public bool HasEmpty(T item, out int index)
		{
			index = -1;

			if (HasEmpty(item))
			{
				for (var i = 0; i < items.Length; i++)
				{
					if (items[i].Quantity == 0)
					{
						index = i;

						return true;
					}
				}
			}

			return false;
		}
		public bool HasCondition(T item)
		{
			return _condition.Has(item);
		}

		public bool TryInsert(T item, out int index)
		{
			index = -1;

			if (!_condition.Has(item))
			{
				return false;
			}

			for (var i = 0; i < items.Length; i++)
			{
				if (items[i].Quantity == 0)
				{
					index = i;

					Insert(item, i);

					return true;
				}
			}

			return false;
		}
		public bool TryExtract(T item, out int index, bool invert = false)
		{
			if (Has(item, out index, invert))
			{
				Extract(index);

				return true;
			}

			return false;
		}

		public void Insert(T item, int index)
		{
			items[index] = item;

			OnAdd(item, index);
		}
		public T Extract(int index)
		{
			T item = items[index];
			items[index] = default;

			OnRemove(item, index);

			return item;
		}

		public void Swap(int index1, int index2)
		{
			(items[index1], items[index2]) = (items[index2], items[index1]);

			OnChanged(this);
		}

		public int Combine(int source, int target)
		{
			T item = items[target];
			if (item.Equals(items[source]))
			{
				item.Quantity += items[source].Quantity;

				Insert(item, source);
				Extract(source);
			}

			return item.Quantity;
		}

		public int ChangeQuantity(int index, int quantity)
		{
			T item = items[index];
			if (item.Quantity > 0 && quantity > 0)
			{
				item.Quantity = quantity;
				items[index] = item;

				OnQuantityChanged(item, index);
			}

			return item.Quantity;
		}

		public void Clear()
		{
			items = new T[_capacity];

			OnChanged(this);
		}

		public void Sort(IComparer<T> comparer)
		{
			Array.Sort(items, comparer);

			OnChanged(this);
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (T item in items)
			{
				if (item.Quantity != 0)
				{
					yield return item;
				}
			}
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
using GameName.Data;
using GameName.Gameplay.Features.Stock;
using GameName.Gameplay.Objects;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace GameName.Gameplay
{
	public class GroundStorageStockView : MonoBehaviour, IStockView, IPausable
	{
		private StockConfigData _config;
		public StockConfigData Config => _config;

		public Stock Stock => _model;

		[SerializeField]
		private StockTransferData m_transferData;
		
		[Space]
		[SerializeField]
		private Transform m_itemsContainer;

		[Space]
		[SerializeField]
		private float m_itemSpawnRadius = 1.0f;
		[SerializeField]
		private float m_itemSpawnHeight = 1.0f;
		[SerializeField]
		private float m_itemSpawnForce = 10.0f;

		[HideInInspector]
		public Transform itemSpawnTransformHelper;

		private Stock _model;

		private List<DroppedItem[]> _items;


		private DroppedItemFactory _itemFactory;

		private IObjectResolver _resolver;

		private SharedData _sharedData;

		[Inject]
		public void Construct(
			SharedData sharedData)
		{
			_sharedData = sharedData;
		}

		private void Awake()
		{
			_itemFactory = new DroppedItemFactory(m_itemsContainer);

			itemSpawnTransformHelper = transform;
		}

		public void InitView(Stock model)
		{
			_model = model;
			_config = _model.Config;

			_items = new List<DroppedItem[]>(_config.slots.Length);
			for (int i = 0; i < _config.slots.Length; i++)
			{
				_items.Add(new DroppedItem[_config.slots[i].capacity]);
			}

			_model.OnAdd += OnAdd;
			_model.OnTake += OnTake;
			_model.OnInactivateItem += OnInactivateItem;

			Filling();
		}

		public void Filling()
		{
			//TODO fill items...
		}

		public void SetPause(bool pause)
		{
			for (int i = 0; i < _items.Count; i++)
			{
				for (int j = 0; j < _items[i].Length; j++)
				{
					_items[i][j]?.SetPause(pause);
				}
			}
		}

		private void OnAdd(StockItem item, int slotIndex, int itemIndex)
		{
			if (_items[slotIndex][itemIndex] != null)
			{
				return;
			}

			ItemData itemData = _sharedData.GetItemData(item.id);

			Vector2 direction = UnityEngine.Random.insideUnitCircle.normalized;

			DroppedItem droppedItem = _itemFactory.Instantiate(
				itemData.view.dropedPrefab,
				itemData,
				itemSpawnTransformHelper.position + new Vector3(
					direction.x * m_itemSpawnRadius,
					m_itemSpawnHeight,
					direction.y * m_itemSpawnRadius),
				UnityEngine.Random.rotation,
				Vector3.one);

			droppedItem.GetComponent<Rigidbody>()
				.AddForce(new Vector3(direction.x, 0.0f, direction.y) * m_itemSpawnForce, ForceMode.Impulse);

			droppedItem.SetStockViewItemData(new StockViewItemData
			{
				item = item,
				stockView = this,
				slotIndex = slotIndex,
				itemIndex = itemIndex
			});

			droppedItem.gameObject.SetActive(!item.inactive);

			_items[slotIndex][itemIndex] = droppedItem;
		}

		private void OnTake(StockItem item, int slotIndex, int itemIndex)
		{
			DroppedItem droppedItem = _items[slotIndex][itemIndex];

			droppedItem.SetStockViewItemData(default);
			_itemFactory.Dispose(_sharedData.GetItemData(item.id).view.dropedPrefab, droppedItem);

			_items[slotIndex][itemIndex] = null;
		}

		private void OnInactivateItem(int slotIndex, int itemIndex, bool value)
		{
			_items[slotIndex][itemIndex].gameObject.SetActive(!value);
		}

		public bool TryGetItem(int slotIndex, int itemIndex, out StockViewItemData stockViewItemData)
		{
			stockViewItemData = default;

			if (_model[slotIndex][itemIndex].Quantity > 0)
			{
				stockViewItemData = ((IStockable)_items[slotIndex][itemIndex]).StockViewItemData;

				return true;
			}

			return false;
		}

		public StockTransferData TransferData(StockViewItemData stockViewItemData)
		{
			return m_transferData;
		}

		public bool TryTransferAdd(StockItem stockItem, out StockViewItemData stockViewItemData, out Transform transform)
		{
			stockViewItemData = default;
			transform = null;

			if (_model.TryAdd(stockItem, out int slotIndex, out int itemIndex, false))
			{
				stockViewItemData = ((IStockable)_items[slotIndex][itemIndex]).StockViewItemData;
				transform = _items[slotIndex][itemIndex].transform;

				return true;
			}

			return false;
		}

		public bool TryTransferTake(StockViewItemData stockViewItemData, out Transform transform)
		{
			transform = _items[stockViewItemData.slotIndex][stockViewItemData.itemIndex].transform;

			if (_model.TryTake(stockViewItemData.slotIndex, stockViewItemData.itemIndex, 1))
			{
				return true;
			}

			return false;
		}

		public void TransferDone(StockViewItemData stockViewItemData)
		{
			_model.TryInactivateItem(stockViewItemData.slotIndex, stockViewItemData.itemIndex, false);
		}

		public void TransferReject(StockViewItemData stockViewItemData)
		{
			_items[stockViewItemData.slotIndex][stockViewItemData.itemIndex].StockTarnsferReject();
		}
	}
}
using GameName.Data;
using GameName.Gameplay.Features.Stock;
using GameName.Helpers.Factory;
using System;
using UnityEngine;

namespace GameName.Gameplay.Objects
{
	[RequireComponent(typeof(Rigidbody))]
	public class DroppedItem : MonoBehaviour, IGameObjectFactory<DroppedItem, ItemData>, IItem, IStockable, IPausable
	{
		public event Action<DroppedItem> OnSleep;

		[SerializeField]
		private float m_itemRejectForce = 10.0f;

		private ItemData _config;
		public ItemData Config => _config;

		private bool _isPaused = false;
		public bool IsPaused => _isPaused;

		private StockViewItemData _stockViewItemData;
		public StockViewItemData StockViewItemData => _stockViewItemData;

		private Rigidbody _rb;
		private DroppedItemInteractionBehaviour _interactionBehaviour;

		private bool _isDropped;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody>();
			_interactionBehaviour = GetComponentInChildren<DroppedItemInteractionBehaviour>();
		}

		private void OnEnable()
		{
			_rb.isKinematic = false;
			_interactionBehaviour.InteractionEnable = false;
			_isDropped = true;
			_isPaused = false;
		}
		private void OnDisable()
		{
			_isDropped = false;
		}

		public void OnCreate(ItemData config)
		{
			_config = config;

			_interactionBehaviour.InteractionCondition = _config.interaction;
		}
		public void OnDispose()
		{
			
		}

		private void FixedUpdate()
		{
			if (_isPaused)
			{
				return;
			}

			if (_isDropped && !_rb.isKinematic && _rb.IsSleeping())
			{
				_rb.isKinematic = true;

				OnSleep?.Invoke(this);

				_interactionBehaviour.InteractionEnable = true;
				_isDropped = false;
			}
		}

		public void SetPause(bool pause)
		{
			_isPaused = pause;

			if (_isDropped)
			{
				//TODO pause/resume rb...
			}
		}

		public void SetStockViewItemData(StockViewItemData stockViewItemData)
		{
			_stockViewItemData = stockViewItemData;
		}

		public void StockTarnsferReject()
		{
			_rb.isKinematic = false;
			_interactionBehaviour.InteractionEnable = false;
			_isDropped = true;
			_rb.AddForce(new Vector3(0.0f, 1.0f, 0.0f) * m_itemRejectForce, ForceMode.Impulse);
		}
	}
}
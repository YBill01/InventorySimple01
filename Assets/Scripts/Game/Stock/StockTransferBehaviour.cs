using GameName.Data;
using GameName.Gameplay.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace GameName.Gameplay.Features.Stock
{
	public class StockTransferBehaviour : IUpdatable
	{
		private Transform _container;

		private List<TransferringHandle> _handles;
		private TransferredItemFactory _itemFactory;

		public StockTransferBehaviour(Transform container)
		{
			_container = container;

			_handles = new List<TransferringHandle>();
			_itemFactory = new TransferredItemFactory(container);
		}

		public void OnUpdate(float deltaTime)
		{
			for (int i = _handles.Count - 1; i >= 0; i--)
			{
				if (_handles[i].Process(deltaTime))
				{
					TransferObjectInfo transferObject = _handles[i].TransferObjectInfo;
					_itemFactory.Dispose(transferObject.prefab, transferObject.instanceObject);

					_handles[i].SendTargetEnd();

					_handles.RemoveAt(i);
				}
			}
		}

		public void Process(
			StockViewItemData sourceStockViewItem,
			StockViewItemData targetStockViewItem,
			TransferredItem prefab,
			ItemData itemData,
			Transform originTransform,
			Transform targetTransform,
			float duration)
		{
			StockTransferData sourceTransferData = sourceStockViewItem.stockView.TransferData(sourceStockViewItem);
			StockTransferData targetTransferData = targetStockViewItem.stockView.TransferData(targetStockViewItem);

			TransferInfo transferInfo = new TransferInfo
			{
				origin = new TransferOriginInfo
				{
					source = sourceStockViewItem,
					position = originTransform.position,
					rotation = originTransform.rotation,
					scale = sourceTransferData.scale
				},
				target = new TransferTargetInfo
				{
					target = targetStockViewItem,
					transform = targetTransform,
					scale = targetTransferData.scale
				},

				transferObject = new TransferObjectInfo
				{
					prefab = prefab,
					instanceObject = _itemFactory.Instantiate(prefab, itemData, originTransform.position, originTransform.rotation, sourceTransferData.scale),
					itemData = itemData
				},

				transferData = targetTransferData,

				duration = duration
			};

			_handles.Add(new TransferringHandle(transferInfo));
		}

		public void ClearAll()
		{
			for (int i = _handles.Count - 1; i >= 0; i--)
			{
				TransferObjectInfo transferObject = _handles[i].TransferObjectInfo;
				_itemFactory.Dispose(transferObject.prefab, transferObject.instanceObject);

				_handles.RemoveAt(i);
			}
		}

		public struct TransferInfo
		{
			public TransferObjectInfo transferObject;

			public TransferOriginInfo origin;
			public TransferTargetInfo target;

			public StockTransferData transferData;

			public float duration;
		}
		public struct TransferObjectInfo
		{
			public TransferredItem prefab;
			public TransferredItem instanceObject;
			public ItemData itemData;
		}
		public struct TransferOriginInfo
		{
			public StockViewItemData source;

			public Vector3 position;
			public Quaternion rotation;
			public Vector3 scale;
		}
		public struct TransferTargetInfo
		{
			public StockViewItemData target;

			public Transform transform;
			public Vector3 scale;
		}

		public class TransferringHandle
		{
			public TransferObjectInfo TransferObjectInfo => _info.transferObject;

			private TransferInfo _info;

			private float _time = 0.0f;

			private Transform _instanceObjectTransform;

			public TransferringHandle(TransferInfo info)
			{
				_info = info;

				_instanceObjectTransform = _info.transferObject.instanceObject.transform;
			}

			public bool Process(float deltaTime)
			{
				_time += deltaTime;

				float t = Mathf.Clamp01(_info.transferData.timeCurve.Evaluate(_time / _info.duration));

				Vector3 position = Vector3.Lerp(_info.origin.position, _info.target.transform.position, _info.transferData.positionCurve.Evaluate(t));
				position.y += _info.transferData.height * _info.transferData.heightCurve.Evaluate(t);

				Quaternion rotation = Quaternion.Lerp(_info.origin.rotation, _info.target.transform.rotation, _info.transferData.rotationCurve.Evaluate(t));
				Vector3 scale = Vector3.Lerp(_info.origin.scale, _info.target.scale, _info.transferData.scaleCurve.Evaluate(t));

				_instanceObjectTransform.SetPositionAndRotation(position, rotation);
				_instanceObjectTransform.localScale = scale;

				return _time >= _info.duration;
			}

			public void SendTargetEnd()
			{
				_info.target.target.stockView.TransferDone(_info.target.target);
			}
		}
	}
}
using GameName.Data;
using GameName.Helpers.Factory;
using UnityEngine;

namespace GameName.Gameplay.Objects
{
	public class TransferredItem : MonoBehaviour, IGameObjectFactory<TransferredItem, ItemData>
	{
		public void OnCreate(ItemData config)
		{
			
		}

		public void OnDispose()
		{
			
		}
	}
}
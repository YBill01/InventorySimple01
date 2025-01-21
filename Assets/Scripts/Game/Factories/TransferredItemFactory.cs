using GameName.Data;
using GameName.Gameplay.Objects;
using GameName.Helpers.Factory;
using UnityEngine;

namespace GameName.Gameplay
{
	public class TransferredItemFactory : GameObjectFactories<TransferredItem, ItemData>
	{
		public TransferredItemFactory(Transform container) : base(container)
		{
		}
	}
}
using GameName.Data;
using GameName.Gameplay.Objects;
using GameName.Helpers.Factory;
using UnityEngine;

namespace GameName.Gameplay
{
	public class DroppedItemFactory : GameObjectFactories<DroppedItem, ItemData>
	{
		public DroppedItemFactory(Transform container) : base(container)
		{
		}
	}
}
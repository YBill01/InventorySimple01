using GameName.Data;
using GameName.Gameplay.Objects;
using GameName.Helpers.Factory;
using UnityEngine;

namespace GameName.Gameplay
{
	public class StockedItemFactory : GameObjectFactories<StockedItem, ItemData>
	{
		public StockedItemFactory(Transform container) : base(container)
		{
		}
	}
}
using GameName.Gameplay.Objects;
using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Game/ItemViewData", fileName = "ItemView", order = 21)]
	public class ItemViewData : ScriptableObject
	{
		public string displayName;

		[Space]
		public Sprite icon;

		[Space]
		public DroppedItem dropedPrefab;
		public StockedItem stockedPrefab;
		public TransferredItem transferredPrefab;
	}
}
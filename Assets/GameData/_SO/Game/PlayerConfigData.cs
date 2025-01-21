using GameName.Gameplay;
using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Game/PlayerConfigData", fileName = "PlayerConfig", order = 0)]
	public class PlayerConfigData : ScriptableObject
	{
		public string displayName;

		[Space]
		public CPlayer prefab;

		public StockConfigData stock;

		//public float collectingDuration;

		//public float kickDelay;
	}
}
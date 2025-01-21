using GameName.Gameplay;
using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Game/StorageConfigData", fileName = "StorageConfig", order = 2)]
	public class StorageConfigData : ScriptableObject
	{
		public StockConfigData stock;

		[Space]
		public GameObject prefab;
	}
}
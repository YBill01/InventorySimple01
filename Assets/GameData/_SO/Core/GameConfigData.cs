using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Core/GameConfigData", fileName = "GameConfig", order = 5)]
	public class GameConfigData : ScriptableObject
	{
		[Space]
		public SharedData sharedData;

		//[Space]
		//public NetworkConfigData networkData;

		[Space]
		public LevelConfigData[] levels;





		//[Space]
		//public PlayerConfigData player;

		//[Space]
		//public BuildingConfigData[] buildings;

		//[Space]
		//public StorageConfigData[] storages;
	}
}
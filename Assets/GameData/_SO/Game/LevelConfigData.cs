using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Game/LevelConfigData", fileName = "LevelConfig", order = -100)]
	public class LevelConfigData : ScriptableObject
	{
		public PlayerConfigData playerConfigData;
		public StorageConfigData groundStorageConfigData;

		[Space]
		public Vector3 playerStartPosition;
		public float playerStartRotation;

		[Space]
		[Range(0.0f, 1.0f)]
		public float playerStartCameraZoom = 0.5f;
	}
}
using UnityEditor;
using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Game/StockConfigData", fileName = "StockConfig", order = 50)]
	public class StockConfigData : ScriptableObject
	{
		public StockSlotConfigData[] slots;

		[Space]
		public float transferDuration;

		/// <summary>
		/// automatic data binding in Editor
		/// TODO use db
		/// </summary>
		[HideInInspector]
		public SharedData sharedData;
#if UNITY_EDITOR
		private void Reset()
		{
			sharedData = AssetDatabase.LoadAssetAtPath<SharedData>("Assets/GameData/Core/SharedData.asset");
		}
#endif
	}
}
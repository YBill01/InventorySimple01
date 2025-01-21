using GameName.Gameplay.Features.Stock;
using UnityEditor;
using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Game/StockSlotConfigData", fileName = "StockSlotConfig", order = 51)]
	public class StockSlotConfigData : ScriptableObject
	{
		public int capacity;

		[Space]
		public bool stackable;

		[Space]
		public StockCondition condition;
#if UNITY_EDITOR
		private void Reset()
		{
			condition.sharedData = AssetDatabase.LoadAssetAtPath<SharedData>("Assets/GameData/Core/SharedData.asset");
		}
#endif
	}
}
using UnityEngine;

namespace GameName.Data
{
	[CreateAssetMenu(menuName = "GameName/Game/StockTransferData", fileName = "StockTransfer", order = 70)]
	public class StockTransferData : ScriptableObject
	{
		public AnimationCurve timeCurve;

		[Space]
		public AnimationCurve positionCurve;
		public AnimationCurve rotationCurve;

		[Space]
		public float height;
		public AnimationCurve heightCurve;

		[Space]
		public Vector3 scale;
		public AnimationCurve scaleCurve;
	}
}
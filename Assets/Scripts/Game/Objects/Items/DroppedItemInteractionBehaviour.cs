using UnityEngine;

namespace GameName.Gameplay.Objects
{
	public class DroppedItemInteractionBehaviour : InteractionBehaviour
	{
		[SerializeField]
		private DroppedItem m_droppedItem;

		protected override void Awake()
		{
			base.Awake();

			_sourceGameObject = m_droppedItem.gameObject;
		}
	}
}
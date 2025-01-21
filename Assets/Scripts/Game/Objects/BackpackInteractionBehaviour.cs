using UnityEngine;

namespace GameName.Gameplay.Objects
{
	public class BackpackInteractionBehaviour : InteractionBehaviour
	{
		[SerializeField]
		private CPlayer m_player;

		protected override void Awake()
		{
			base.Awake();

			InteractionEnable = true;

			_sourceGameObject = m_player.gameObject;
		}
	}
}
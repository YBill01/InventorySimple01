using GameName.Gameplay.Input;
using UnityEngine;

namespace GameName.Gameplay.Objects
{
	[RequireComponent(typeof(Outline))]
	public abstract class InteractionBehaviour : MonoBehaviour, IInteraction
	{
		[field: SerializeField]
		public bool InteractionEnable { get; set; }

		protected GameObject _sourceGameObject;
		public GameObject SourceGameObject => _sourceGameObject;

		[SerializeField]
		protected InteractionCondition m_interactionCondition;
		public InteractionCondition InteractionCondition
		{
			get => m_interactionCondition;
			set
			{
				m_interactionCondition = value;
			}
		}

		protected Outline _outline;

		protected virtual void Awake()
		{
			_outline = GetComponent<Outline>();
			_outline.enabled = false;

			_sourceGameObject = gameObject;
		}

		public virtual void OnTouch(bool enter)
		{
			_outline.enabled = enter;
		}
		public virtual void OnDown() { }
		public virtual void OnClick() { }
	}
}
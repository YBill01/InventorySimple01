using GameName.Gameplay.Objects;
using UnityEngine;

namespace GameName.Gameplay.Input
{
	public interface IInteraction
	{
		bool InteractionEnable { get; set; }

		GameObject SourceGameObject { get; }

		InteractionCondition InteractionCondition { get; set; }

		void OnTouch(bool enter);
		void OnDown();
		void OnClick();
	}
}
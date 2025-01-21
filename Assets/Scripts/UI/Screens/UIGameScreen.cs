using GameName.Core;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameName.UI
{
	public class UIGameScreen : UIScreen
	{
		[Space]
		[SerializeField]
		private Button m_homeButton;

		[Space]
		[SerializeField]
		private UIToggleButton m_pauseToggleButton;

		[Space]
		[SerializeField]
		private Button m_backpackButton;


		//private App _app;
		private CoreFlow _coreFlow;
		private Gameplay.Game _game;
		private UIGame _uiGame;

		[Inject]
		public void Construct(
			/*App app,*/
			CoreFlow coreFlow,
			Gameplay.Game game,/*,
			GameFlow gameFlow*/
			UIGame uiGame)
		{
			//_app = app;
			_coreFlow = coreFlow;
			_game = game;
			//_gameFlow = gameFlow;
			_uiGame = uiGame;
		}

		private void OnEnable()
		{
			m_homeButton.onClick.AddListener(HomeButtonOnClick);
			m_pauseToggleButton.onValueChanged += PauseToggleButtonOnValueChanged;
			m_backpackButton.onClick.AddListener(BackpackButtonButtonOnClick);


		}
		private void OnDisable()
		{
			m_homeButton.onClick.RemoveListener(HomeButtonOnClick);
			m_pauseToggleButton.onValueChanged -= PauseToggleButtonOnValueChanged;
			m_backpackButton.onClick.RemoveListener(BackpackButtonButtonOnClick);
		}

		private void HomeButtonOnClick()
		{
			_coreFlow.EndGame();
		}
		private void PauseToggleButtonOnValueChanged(bool value)
		{
			_game.SetPause(value);
			//_coreFlow.SetPauseValue(value);
		}

		private void BackpackButtonButtonOnClick()
		{
			_uiGame.PlayerInventoryShow();
			//Debug.Log("BackpackButtonButtonOnClick");
		}

	}
}
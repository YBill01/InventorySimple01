using GameName.Core;
using GameName.Gameplay;
using GameName.Gameplay.Input;
using GameName.UI.Input;
using System;
using UnityEngine;
using VContainer;

namespace GameName.UI
{
	public class UIGame : MonoBehaviour
	{
		[SerializeField]
		private UIScreenController m_uiController;

		private UIGameScreen _gameScreen;
		private UIPlayerInventoryScreen _playerInventoryScreen;

		private IObjectResolver _resolver;

		private CoreFlow _coreFlow;
		private InputUIControl _inputUIControl;
		private InputInteractionControl _inputInteractionControl;
		private GameWorld _gameWorld;

		[Inject]
		public void Construct(
			IObjectResolver resolver,
			CoreFlow coreFlow,
			InputUIControl inputUIControl,
			InputInteractionControl inputInteractionControl,
			GameWorld gameWorld)
		{
			_resolver = resolver;

			_coreFlow = coreFlow;
			_inputUIControl = inputUIControl;
			_inputInteractionControl = inputInteractionControl;
			_gameWorld = gameWorld;
		}

		private void OnEnable()
		{
			_inputUIControl.OnCancel += InputUIControlOnCancel;
			_inputInteractionControl.OnClick += InputInteractionControlOnClick;

			m_uiController.OnShow.AddListener<UIGameScreen>(UIGameScreenOnShow);
			m_uiController.OnHide.AddListener<UIGameScreen>(UIGameScreenOnHide);
			
			m_uiController.OnShow.AddListener<UIPlayerInventoryScreen>(UIPlayerInventoryScreenOnShow);
			m_uiController.OnHide.AddListener<UIPlayerInventoryScreen>(UIPlayerInventoryScreenOnHide);
		}
		private void OnDisable()
		{
			_inputUIControl.OnCancel -= InputUIControlOnCancel;
			_inputInteractionControl.OnClick -= InputInteractionControlOnClick;

			m_uiController.OnShow.RemoveListener<UIGameScreen>();
			m_uiController.OnHide.RemoveListener<UIGameScreen>();
			
			m_uiController.OnShow.RemoveListener<UIPlayerInventoryScreen>();
			m_uiController.OnHide.RemoveListener<UIPlayerInventoryScreen>();
		}


		public UIPlayerInventoryScreen PlayerInventoryShow()
		{
			return m_uiController.Show<UIPlayerInventoryScreen>();
		}
		public void PlayerInventoryHide()
		{
			m_uiController.Hide<UIPlayerInventoryScreen>();
		}

		private void InputUIControlOnCancel()
		{
			if (_playerInventoryScreen != null && _playerInventoryScreen.State == UIScreen.ScreenState.Show)
			{
				_playerInventoryScreen.SelfHide();
			}
		}
		private void InputInteractionControlOnClick(IInteraction interaction)
		{
			//TODO check if Player...
			//bad...
			if (interaction.SourceGameObject.tag == "Player")
			{
				PlayerInventoryShow();
			}
		}

		private void UIGameScreenOnShow(UIScreen screen)
		{
			UIGameScreen gameScreen = screen as UIGameScreen;

			_resolver.Inject(gameScreen);

			_gameScreen = gameScreen;
		}
		private void UIGameScreenOnHide(UIScreen screen)
		{
			UIGameScreen gameScreen = screen as UIGameScreen;

			_gameScreen = null;
		}

		private void UIPlayerInventoryScreenOnShow(UIScreen screen)
		{
			UIPlayerInventoryScreen playerInventoryScreen = screen as UIPlayerInventoryScreen;

			_resolver.Inject(playerInventoryScreen);

			playerInventoryScreen.Init(_gameWorld.Player, _gameWorld.GroundStorage);

			_playerInventoryScreen = playerInventoryScreen;
		}
		private void UIPlayerInventoryScreenOnHide(UIScreen screen)
		{
			UIPlayerInventoryScreen gameScreen = screen as UIPlayerInventoryScreen;

			_playerInventoryScreen = null;
		}

	}
}
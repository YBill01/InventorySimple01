using GameName.Data;
using GameName.Gameplay;
using GameName.Gameplay.Cmd;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameName.UI
{
	public class UIPlayerInventoryScreen : UIPopupScreen
	{
		private const string TITLE_REPLACE_STR = "@Name";

		[Space]
		[SerializeField]
		private TMP_Text m_titleText;

		[Space]
		[SerializeField]
		private Button m_closeButton;

		[Space]
		[SerializeField]
		private UIPlayerStockPanel m_stockPanel;

		private CPlayer _player;

		private string _titleSource;

		private UIPlayerStockPresenter _stockPresenter;

		private UIGame _uiGame;
		private GameCmd _gameCmd;
		private SharedData _sharedData;

		[Inject]
		public void Construct(
			UIGame uiGame,
			GameCmd gameCmd,
			SharedData sharedData)
		{
			_uiGame = uiGame;
			_gameCmd = gameCmd;
			_sharedData = sharedData;
		}

		public void Init(CPlayer player, GroundStorage groundStorage)
		{
			_player = player;
			
			_stockPresenter = new UIPlayerStockPresenter(
				player.Stock,
				m_stockPanel,
				player.StockView,
				groundStorage.StockView,
				_gameCmd,
				_sharedData);
		}

		protected override void Awake()
		{
			base.Awake();

			_titleSource = m_titleText.text;
		}

		protected override void OnPreShow()
		{
			m_titleText.text = _titleSource.Replace(TITLE_REPLACE_STR, _player.Config.displayName);
		}
		protected override void OnHide()
		{
			
		}

		private void OnEnable()
		{
			m_closeButton.onClick.AddListener(CloseButtonOnClick);
		}
		private void OnDisable()
		{
			m_closeButton.onClick.RemoveListener(CloseButtonOnClick);

			_stockPresenter?.Dispose();
			_stockPresenter = null;
		}

		private void CloseButtonOnClick()
		{
			_uiGame.PlayerInventoryHide();
		}
	}
}
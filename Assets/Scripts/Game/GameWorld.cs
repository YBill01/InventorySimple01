using GameName.Data;
using GameName.Gameplay.Cmd;
using GameName.Gameplay.Features.Stock;
using GameName.Gameplay.Input;
using System;
using UnityEngine;
using VContainer;

namespace GameName.Gameplay
{
	public class GameWorld : MonoBehaviour, IPausable, IUpdatable, IDisposable
	{
		[SerializeField]
		private Transform m_objectContainer;

		private CPlayer _player;
		public CPlayer Player => _player;

		private GroundStorage _groundStorage;
		public GroundStorage GroundStorage => _groundStorage;
		
		private IObjectResolver _resolver;

		private GameCmd _gameCmd;
		private VCamera _vCamera;
		private InputPlayerControl _inputPlayerControl;
		private InputInteractionControl _inputInteractionControl;
		private GameSpawner _spawner;
		private StockTransferBehaviour _stockTransfer;

		[Inject]
		public void Construct(
			IObjectResolver resolver,
			GameCmd gameCmd,
			VCamera vCamera,
			InputPlayerControl inputPlayerControl,
			InputInteractionControl inputInteractionControl,
			GameSpawner spawner,
			StockTransferBehaviour stockTransfer)
		{
			_resolver = resolver;

			_gameCmd = gameCmd;
			_vCamera = vCamera;
			_inputPlayerControl = inputPlayerControl;
			_inputInteractionControl = inputInteractionControl;
			_spawner = spawner;
			_stockTransfer = stockTransfer;
		}

		public void Dispose()
		{
			_inputInteractionControl.OnClick -= InputInteractionControlOnClick;
		}

		public void Init(LevelConfigData config)
		{
			_vCamera.distanceValue = config.playerStartCameraZoom;

			_player = _spawner.SpawnPlayer(config.playerConfigData, config.playerStartPosition, config.playerStartRotation);

			_vCamera.SetFollowTarget(_player.CameraTarget);
			_inputPlayerControl.SetControllers(_vCamera, _player.Controller);

			_groundStorage = _spawner.SpawnGroundStorage(config.groundStorageConfigData, m_objectContainer);

			_inputInteractionControl.OnClick += InputInteractionControlOnClick;
		}

		public void OnUpdate(float deltaTime)
		{
			_player.OnUpdate(deltaTime);

			_gameCmd.ExecuteAll();

			_stockTransfer.OnUpdate(deltaTime);
			//...
		}

		public void SetPause(bool pause)
		{
			_player.SetPause(pause);
			_groundStorage.SetPause(pause);
			//...
		}

		private void InputInteractionControlOnClick(IInteraction interaction)
		{
			//TODO check if Items in the ground stock...
			//bad...
			if (interaction.SourceGameObject.TryGetComponent(out IStockable stockItem))
			{
				_gameCmd.Process(new StockTransferCommand
				{
					source = stockItem.StockViewItemData,
					target = _player.StockView
				});
			}
		}
	}
}
using GameName.Data;
using GameName.Gameplay.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameName.Gameplay
{
	public class GameSpawner
	{
		private readonly IObjectResolver _resolver;

		private GameConfigData _gameConfig;
		private VCamera _vCamera;
		private PlayerFactory _playerFactory;
		private InputPlayerControl _inputPlayerControl;

		public GameSpawner(
			IObjectResolver resolver,
			GameConfigData gameConfig,
			VCamera vCamera,
			PlayerFactory playerFactory,
			InputPlayerControl inputPlayerControl)
		{
			_resolver = resolver;

			_gameConfig = gameConfig;
			_vCamera = vCamera;
			_playerFactory = playerFactory;
			_inputPlayerControl = inputPlayerControl;
		}

		public CPlayer SpawnPlayer(PlayerConfigData config, Vector3 position, float rotation)
		{
			CPlayer player = _playerFactory.Create(new PlayerFactory.Settings
			{
				data = config,
				position = position,
				rotation = rotation
			});

			return player;
		}

		public GroundStorage SpawnGroundStorage(StorageConfigData config, Transform container)
		{
			GroundStorage groundStorage = UnityEngine.Object.Instantiate(config.prefab, container)
				.GetComponent<GroundStorage>();
			groundStorage.OnCreate(config);

			_resolver.InjectGameObject(groundStorage.gameObject);

			return groundStorage;
		}
	}
}
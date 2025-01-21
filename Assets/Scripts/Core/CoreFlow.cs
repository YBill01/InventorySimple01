using GameName.Core.HFSM;
using GameName.Data;
using GameName.Services;
using GameName.UI;
using System;
using VContainer.Unity;

namespace GameName.Core
{
	public class CoreFlow : IInitializable, IStartable, ITickable
	{
		public event Action OnStartGame;
		public event Action OnEndGame;

		private GameConfigData _gameConfig;
		private SharedData _sharedData;
		private SceneLoader _sceneLoader;
		private UILoader _uiLoader;
		private UICore _uiCore;

		private CoreStateMachine _stateMachine;

		public CoreFlow(
			GameConfigData gameConfig,
			SharedData sharedData,
			SceneLoader sceneLoader,
			UILoader uiLoader,
			UICore uiCore)
		{
			_gameConfig = gameConfig;
			_sharedData = sharedData;
			_sceneLoader = sceneLoader;
			_uiLoader = uiLoader;
			_uiCore = uiCore;
		}

		public void Initialize()
		{
			_sharedData.HashingData();

			MetaState metaState = new MetaState(_sceneLoader, _uiLoader, _uiCore);
			GameState gameState = new GameState(_sceneLoader, _uiLoader, _uiCore);

			_stateMachine = new CoreStateMachine(metaState, gameState);

			OnStartGame += metaState.AddEventTransition(gameState);
			OnEndGame += gameState.AddEventTransition(metaState);
		}

		public void Start()
		{
			_stateMachine.Init();
		}

		public void Tick()
		{
			_stateMachine.Update();
		}

		public void StartGame()
		{
			OnStartGame?.Invoke();
		}
		public void EndGame()
		{
			OnEndGame?.Invoke();
		}
	}
}
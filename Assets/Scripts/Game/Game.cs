using Cysharp.Threading.Tasks;
using UnityEngine;
using GameName.Data;
using VContainer;

namespace GameName.Gameplay
{
	public class Game : MonoBehaviour, IPausable
	{
		private bool _isPlayGame = false;
		public bool IsPlayGame => _isPlayGame;

		private bool _isPaused = false;
		public bool IsPaused => _isPaused;

		private IObjectResolver _resolver;

		private GameWorld _gameWorld;

		[Inject]
		public void Construct(
			IObjectResolver resolver,
			GameWorld gameWorld)
		{
			_resolver = resolver;

			_gameWorld = gameWorld;
		}

		public async UniTask StartGame(LevelConfigData config)
		{
			await UniTask.NextFrame();

			//...
			_gameWorld.Init(config);

			await UniTask.NextFrame();

			_isPlayGame = true;
		}
		public void EndGame()
		{
			_isPlayGame = false;

			_gameWorld.Dispose();
		}

		public void Update()
		{
			if (!_isPlayGame || _isPaused)
			{
				return;
			}

			float deltaTime = Time.deltaTime;

			_gameWorld.OnUpdate(deltaTime);
		}

		public void SetPause(bool pause)
		{
			_isPaused = pause;

			_gameWorld.SetPause(pause);
		}
	}
}
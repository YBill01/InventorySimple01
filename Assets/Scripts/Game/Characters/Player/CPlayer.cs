using GameName.Data;
using GameName.Gameplay.Features.Stock;
using GameName.Helpers.Factory;
using UnityEngine;
using VContainer;

namespace GameName.Gameplay
{
	public class CPlayer : MonoBehaviour, IGameObjectFactory<CPlayer, PlayerConfigData>, IPausable, IUpdatable, IStock
	{
		private PlayerConfigData _config;
		public PlayerConfigData Config => _config;

		[field: SerializeField, Space]
		public VCameraTarget CameraTarget { get; private set; }

		[SerializeField]
		private CPlayerStockView m_stockView;
		public IStockView StockView => m_stockView;

		private Stock _stock;
		public Stock Stock => _stock;

		private CPlayerController _controller;
		public CPlayerController Controller => _controller;

		private IObjectResolver _resolver;

		[Inject]
		public void Construct(
			IObjectResolver resolver)
		{
			_resolver = resolver;
		}

		private void Awake()
		{
			_controller = GetComponent<CPlayerController>();
		}

		public void SetPause(bool pause)
		{
			_controller.SetPause(pause);
			m_stockView.SetPause(pause);
		}

		public void OnUpdate(float deltaTime)
		{
			_controller.OnUpdate(deltaTime);
		}

		public void SetOrientation(Vector3 position, Quaternion rotation)
		{
			_controller.SetOrientation(position, rotation);
		}

		public void OnCreate(PlayerConfigData config)
		{
			_config = config;

			_stock = new Stock(_config.stock);
			m_stockView.InitView(_stock);

			//...
			Debug.Log($"Player: OnCreate: {_config.displayName}");
		}
		public void OnDispose()
		{
			_config = null;

			_stock.Clear();
			_stock = null;
		}
	}
}
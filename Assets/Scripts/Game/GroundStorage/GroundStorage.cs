using GameName.Data;
using GameName.Gameplay.Features.Stock;
using UnityEngine;

namespace GameName.Gameplay
{
	public class GroundStorage : MonoBehaviour, IStock, IPausable
	{
		private StorageConfigData _config;
		public StorageConfigData Config => _config;

		[SerializeField]
		private GroundStorageStockView m_stockView;
		public IStockView StockView => m_stockView;

		private Stock _stock;
		public Stock Stock => _stock;

		public void OnCreate(StorageConfigData config)
		{
			_config = config;

			_stock = new Stock(_config.stock);
			m_stockView.InitView(_stock);
		}
		public void OnDispose()
		{
			_config = null;

			_stock.Clear();
			_stock = null;
		}

		public void SetPause(bool pause)
		{
			m_stockView.SetPause(pause);
		}
	}
}
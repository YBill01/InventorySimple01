using GameName.Data;
using GameName.Gameplay.Features.Stock;
using GameName.Helpers.Factory;
using UnityEngine;

namespace GameName.Gameplay.Objects
{
	public class StockedItem : MonoBehaviour, IGameObjectFactory<StockedItem, ItemData>, IItem, IStockable, IPausable
	{
		private ItemData _config;
		public ItemData Config => _config;

		private StockViewItemData _stockViewItemData;
		public StockViewItemData StockViewItemData => _stockViewItemData;

		public void OnCreate(ItemData config)
		{
			_config = config;
		}
		public void OnDispose()
		{

		}

		public void SetPause(bool pause)
		{
			// TODO some behaviour paused...
		}

		public void SetStockViewItemData(StockViewItemData stockViewItemData)
		{
			_stockViewItemData = stockViewItemData;
		}

		public void StockTarnsferReject()
		{
			Debug.Log("stock transfer reject");
		}
	}
}
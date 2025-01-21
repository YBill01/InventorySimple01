namespace GameName.Gameplay.Features.Stock
{
	public interface IStockable
	{
		StockViewItemData StockViewItemData { get; }

		void SetStockViewItemData(StockViewItemData stockViewItemData);
	}
}
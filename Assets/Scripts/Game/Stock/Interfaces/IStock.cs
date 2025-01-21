namespace GameName.Gameplay.Features.Stock
{
	public interface IStock
	{
		Stock Stock { get; }

		IStockView StockView { get; }
	}
}
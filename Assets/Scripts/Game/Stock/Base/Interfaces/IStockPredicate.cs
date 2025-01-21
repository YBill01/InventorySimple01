namespace GameName.Gameplay.Features.Stock
{
	public interface IStockPredicate<in T> where T : struct, IStockItem<T>
	{
		bool Has(T item);
	}
}
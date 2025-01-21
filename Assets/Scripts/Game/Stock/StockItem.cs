namespace GameName.Gameplay.Features.Stock
{
	public struct StockItem : IStockItem<StockItem>
	{
		public int id;

		public bool inactive;

		public int Quantity { get; set; }

		public bool Equals(StockItem other)
		{
			return id == other.id;
		}

		public override string ToString()
		{
			return $"id: {id}\ninactive: {inactive}\nQuantity: {Quantity}";
		}
	}
}
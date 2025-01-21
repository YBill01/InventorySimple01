using System;

namespace GameName.Gameplay.Features.Stock
{
	public interface IStockItem<T> : IEquatable<T> where T : struct
	{
		int Quantity { get; set; }
	}
}
using GameName.Gameplay.Features.Stock;
using GameName.Services.Cmd;

namespace GameName.Gameplay.Cmd
{
	public struct StockTransferCommand : ICommand
	{
		public StockViewItemData source;

		public IStockView target;
	}
}
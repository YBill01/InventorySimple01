using IngameDebugConsole;
using VContainer.Unity;
using VContainer;
using GameName.Core;
using GameName.Gameplay;
using GameName.Gameplay.Cmd;
using GameName.Gameplay.Features.Stock;

namespace GameName.Utilities
{
	public static class ConsoleCommands
	{
		[ConsoleMethod("quit", "Quits the player application.")]
		public static void Quit()
		{
			LifetimeScope.Find<BootstrapScope>()
				.Container
				.Resolve<App>()
				.Quit();
		}

		[ConsoleMethod("s.tr", "Test 01.")]
		public static void TestStockDropItemTransfer(int slotIndex, int itemIndex)
		{
			GameWorld world = LifetimeScope.Find<GameScope>()
				.Container
				.Resolve<GameWorld>();

			CPlayer player = world.Player;
			GroundStorage groundStorage = world.GroundStorage;
			GameCmd gameCmd = LifetimeScope.Find<GameScope>()
				.Container
				.Resolve<GameCmd>();

			if (player.StockView.TryGetItem(slotIndex, itemIndex, out StockViewItemData stockViewItemData))
			{
				for (int i = 0; i < stockViewItemData.item.Quantity; i++)
				{
					gameCmd.Process(new StockTransferCommand
					{
						source = stockViewItemData,
						target = groundStorage.StockView
					});
				}
			}
		}

		[ConsoleMethod("s.sp", "Item spawn")]
		public static void TestStockGroundSpawnItem(int id)
		{
			GameCmd gameCmd = LifetimeScope.Find<GameScope>()
				.Container
				.Resolve<GameCmd>();

			GameWorld world = LifetimeScope.Find<GameScope>()
				.Container
				.Resolve<GameWorld>();

			CPlayer player = world.Player;
			GroundStorage groundStorage = world.GroundStorage;

			gameCmd.Process(new StockGroundSpawnItemCommand
			{
				id = id,
				spawnTransformHelper = player.transform
			});
		}
	}
}
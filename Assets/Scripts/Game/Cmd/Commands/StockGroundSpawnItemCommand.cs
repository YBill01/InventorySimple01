using GameName.Services.Cmd;
using UnityEngine;

namespace GameName.Gameplay.Cmd
{
	public struct StockGroundSpawnItemCommand : ICommand
	{
		public int id;

		public Transform spawnTransformHelper;
	}
}
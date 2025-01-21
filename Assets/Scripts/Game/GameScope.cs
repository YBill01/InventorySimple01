using GameName.Gameplay.Cmd;
using GameName.Gameplay.Features.Stock;
using GameName.Gameplay.Input;
using GameName.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameName.Gameplay
{
	public class GameScope : LifetimeScope
	{
		[Space]
		[SerializeField]
		private Transform m_playerFactoryContainer;
		
		[SerializeField]
		private Transform m_stockTransferContainer;

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterComponentInHierarchy<UIGame>();

			builder.Register<GameSpawner>(Lifetime.Scoped);

			builder.RegisterComponentInHierarchy<VCamera>();

			builder.Register<PlayerFactory>(Lifetime.Scoped)
				.WithParameter(typeof(Transform), m_playerFactoryContainer);

			builder.Register<StockTransferBehaviour>(Lifetime.Scoped)
				.WithParameter(typeof(Transform), m_stockTransferContainer);

			builder.RegisterComponentInHierarchy<InputPlayerControl>();
			builder.RegisterComponentInHierarchy<InputInteractionControl>();

			builder.Register<GameCmd>(Lifetime.Scoped);

			builder.RegisterComponentInHierarchy<Game>();
			builder.RegisterComponentInHierarchy<GameWorld>();

			builder.RegisterEntryPoint<GameFlow>();
		}
	}
}
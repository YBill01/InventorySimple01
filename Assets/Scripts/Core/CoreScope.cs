using GameName.Data;
using GameName.UI.Input;
using GameName.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameName.Core
{
	public class CoreScope : LifetimeScope
	{
		[Space]
		[SerializeField]
		private GameConfigData m_gameConfig;

		protected override void Awake()
		{
			base.Awake();

			DontDestroyOnLoad(this);
		}

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterInstance(m_gameConfig);
			builder.RegisterInstance(m_gameConfig.sharedData);

			builder.RegisterComponentInHierarchy<UICore>();
			builder.RegisterComponentInHierarchy<InputUIControl>();

			builder.RegisterEntryPoint<CoreFlow>()
				.AsSelf();
		}
	}
}
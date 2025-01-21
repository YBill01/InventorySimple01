using GameName.Services;
using GameName.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameName.Core
{
	public class BootstrapScope : LifetimeScope
	{
		[Space]
		[SerializeField]
		private App m_app;

		protected override void Awake()
		{
			base.Awake();

			DontDestroyOnLoad(this);
		}

		protected override void Configure(IContainerBuilder builder)
		{
			builder.Register<SceneLoader>(Lifetime.Singleton);

			builder.RegisterComponent(m_app);
			
			builder.RegisterEntryPoint<BootstrapFlow>();
		}
	}
}
using GameName.Data;
using GameName.Helpers.Factory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameName.Gameplay
{
	public class PlayerFactory : GameObjectFactories<CPlayer, PlayerConfigData>
	{
		private Settings _settings;
		
		private readonly IObjectResolver _resolver;

		public PlayerFactory(
			IObjectResolver resolver,
			Transform container) : base(container)
		{
			_resolver = resolver;
		}

		public struct Settings
		{
			public PlayerConfigData data;

			public Vector3 position;
			public float rotation;
		}

		public CPlayer Create(Settings settings)
		{
			_settings = settings;
			
			CPlayer player = Instantiate(
				settings.data.prefab,
				settings.data,
				settings.position,
				Quaternion.AngleAxis(settings.rotation, Vector3.up),
				Vector3.one);

			_resolver.InjectGameObject(player.gameObject);

			return player;
		}
		public void Remove(CPlayer player)
		{
			Dispose(_settings.data.prefab, player);
		}
	}
}
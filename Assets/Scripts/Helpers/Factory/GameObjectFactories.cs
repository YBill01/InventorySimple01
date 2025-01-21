using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameName.Helpers.Factory
{
	public abstract class GameObjectFactories<T, K> where T : MonoBehaviour, IGameObjectFactory<T, K> where K : class
	{
		private Transform _container;

		private Dictionary<T, GameObjectFactory<T, K>> _factories;

		protected Func<T> _createFunc = null;

		public GameObjectFactories(Transform container)
		{
			_container = container;

			_factories = new Dictionary<T, GameObjectFactory<T, K>>();
		}

		public T Instantiate(T prefab, K config, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			return GetOrCreateFactory(prefab, config).Instantiate(position, rotation, scale);
		}
		public void Dispose(T prefab, T gameObject)
		{
			if (_factories.TryGetValue(prefab, out GameObjectFactory<T, K> factory))
			{
				factory.Dispose(gameObject);
			}
			else
			{
				Debug.LogError($"Factory {prefab} is not found.");
			}
		}

		private GameObjectFactory<T, K> GetOrCreateFactory(T prefab, K config)
		{
			if (_factories.TryGetValue(prefab, out GameObjectFactory<T, K> factory))
			{
				return factory;
			}
			else
			{
				_factories.Add(prefab, new GameObjectFactory<T, K>(prefab, config, _container, _createFunc));

				return _factories[prefab];
			}
		}
	}
}
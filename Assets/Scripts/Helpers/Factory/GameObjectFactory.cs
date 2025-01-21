using System;
using UnityEngine;
using UnityEngine.Pool;

namespace GameName.Helpers.Factory
{
	public class GameObjectFactory<T, K> where T : MonoBehaviour, IGameObjectFactory<T, K> where K : class
	{
		private T _prefab;
		private K _config;

		private Transform _container;

		private IObjectPool<T> _objectPool;

		public GameObjectFactory(T prefab, K config, Transform container, Func<T> createFunc = null)
		{
			_prefab = prefab;
			_config = config;

			_container = container;

			_objectPool = new ObjectPool<T>(createFunc ?? OnCreate, OnGet, OnRelease, OnDestroy, false, 10, int.MaxValue);
		}

		private T OnCreate()
		{
			return UnityEngine.Object.Instantiate(_prefab, _container);
		}

		private void OnGet(T gameObject)
		{
			gameObject.gameObject.SetActive(true);

			gameObject.OnCreate(_config);
		}

		private void OnRelease(T gameObject)
		{
			gameObject.gameObject.SetActive(false);

			gameObject.OnDispose();
		}

		private void OnDestroy(T gameObject)
		{
			UnityEngine.Object.Destroy(gameObject.gameObject);
		}

		public T Instantiate(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			T gameObject = _objectPool.Get();

			gameObject.transform.SetPositionAndRotation(position, rotation);
			gameObject.transform.localScale = scale;

			return gameObject;
		}
		public void Dispose(T gameObject)
		{
			_objectPool.Release(gameObject);
		}
	}
}
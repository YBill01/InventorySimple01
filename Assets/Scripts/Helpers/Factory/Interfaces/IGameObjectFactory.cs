using UnityEngine;

namespace GameName.Helpers.Factory
{
	public interface IGameObjectFactory<T, K> where T : MonoBehaviour where K : class
	{
		void OnCreate(K config);
		void OnDispose();
	}
}
using GameName.Services;
using GameName.Utilities;
using System;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace GameName.Core
{
	public class BootstrapFlow : IStartable
	{
		private SceneLoader _sceneLoader;

		public BootstrapFlow(
			SceneLoader sceneLoader)
		{
			_sceneLoader = sceneLoader;
			
		}

		public void Start()
		{
			

			LoadNext();
		}

		private void LoadNext()
		{
			(int, LoadSceneMode) scene = RuntimeConstants.Scenes.LOADING;

			_sceneLoader.Load(scene.Item1, scene.Item2);
		}
	}
}
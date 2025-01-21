using UnityEngine;
using VContainer;

namespace GameName.UI
{
	public class UIMeta : MonoBehaviour
	{
		[SerializeField]
		private UIScreenController m_uiController;

		private UIMainMenuScreen _mainMenuScreen;

		private IObjectResolver _resolver;

		[Inject]
		public void Construct(
			IObjectResolver resolver)
		{
			_resolver = resolver;
		}

		private void OnEnable()
		{
			m_uiController.OnShow.AddListener<UIMainMenuScreen>(UIMainMenuScreenOnShow);
			m_uiController.OnHide.AddListener<UIMainMenuScreen>(UIMainMenuScreenOnHide);
		}
		private void OnDisable()
		{
			m_uiController.OnShow.RemoveListener<UIMainMenuScreen>();
			m_uiController.OnHide.RemoveListener<UIMainMenuScreen>();
		}

		private void UIMainMenuScreenOnShow(UIScreen screen)
		{
			UIMainMenuScreen mainMenuScreen = screen as UIMainMenuScreen;

			_resolver.Inject(mainMenuScreen);

			_mainMenuScreen = mainMenuScreen;
		}
		private void UIMainMenuScreenOnHide(UIScreen screen)
		{
			UIMainMenuScreen mainMenuScreen = screen as UIMainMenuScreen;

			_mainMenuScreen = null;
		}
	}
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameName.UI.Input
{
	public class InputUIControl : MonoBehaviour
	{
		public event Action OnSubmit;
		public event Action OnCancel;

		private InputSystem_Actions _inputActions;

		private void Awake()
		{
			_inputActions = new InputSystem_Actions();
		}

		private void OnEnable()
		{
			_inputActions.Enable();

			_inputActions.UI.Submit.performed += OnSubmitButton;
			_inputActions.UI.Submit.canceled += OnSubmitButton;

			_inputActions.UI.Cancel.performed += OnCancelButton;
			_inputActions.UI.Cancel.canceled += OnCancelButton;

		}
		private void OnDisable()
		{
			_inputActions.UI.Submit.performed -= OnSubmitButton;
			_inputActions.UI.Submit.canceled -= OnSubmitButton;

			_inputActions.UI.Cancel.performed -= OnCancelButton;
			_inputActions.UI.Cancel.canceled -= OnCancelButton;

			_inputActions.Disable();
		}

		private void OnSubmitButton(InputAction.CallbackContext context)
		{
			bool value = context.ReadValueAsButton();

			if (value)
			{
				OnSubmit?.Invoke();
			}
		}
		private void OnCancelButton(InputAction.CallbackContext context)
		{
			bool value = context.ReadValueAsButton();

			if (!value)
			{
				OnCancel?.Invoke();
			}
		}
	}
}
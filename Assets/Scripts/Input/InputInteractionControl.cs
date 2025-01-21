using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GameName.Gameplay.Input
{
	public class InputInteractionControl : MonoBehaviour
	{
		public event Action<IInteraction> OnTouch;
		public event Action<IInteraction> OnDown;
		public event Action<IInteraction> OnClick;

		private IInteraction _selectedObject;

		private InputSystem_Actions _inputActions;

		private bool _isPointerOverGameObject;

		private bool _isClick;

		private void Awake()
		{
			_inputActions = new InputSystem_Actions();
		}

		private void OnEnable()
		{
			_inputActions.Enable();

			_inputActions.UI.Click.performed += OnClickButton;
			//_inputActions.UI.Click.canceled += OnClickButton;
		}
		private void OnDisable()
		{
			_inputActions.UI.Click.performed -= OnClickButton;
			//_inputActions.UI.Click.canceled -= OnClickButton;

			_inputActions.Disable();
		}

		private void Update()
		{
			_isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();

			if (_isClick)
			{
				return;
			}

			if (TryGetInteractionObject(out IInteraction interactionObject))
			{
				if (!ReferenceEquals(interactionObject, _selectedObject))
				{
					_selectedObject?.OnTouch(false);

					if (interactionObject.InteractionCondition.touchable)
					{
						_selectedObject = interactionObject;
						_selectedObject.OnTouch(true);

						OnTouch?.Invoke(_selectedObject);
					}
				}
			}
			else if (_selectedObject != null)
			{
				_selectedObject.OnTouch(false);
				_selectedObject = null;
			}
		}

		private bool TryGetInteractionObject(out IInteraction interactionObject)
		{
			interactionObject = null;

			Ray rayOrigin = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

			if (Physics.Raycast(rayOrigin, out RaycastHit hitInfo))
			{
				GameObject go = hitInfo.collider.gameObject;
				if (go.TryGetComponent(out interactionObject) && !_isPointerOverGameObject)
				{
					return interactionObject.InteractionEnable;
				}
			}

			return false;
		}

		private void OnClickButton(InputAction.CallbackContext context)
		{
			_isClick = context.ReadValueAsButton();

			if (_isPointerOverGameObject)
			{
				return;
			}

			if (_isClick)
			{
				_selectedObject?.OnDown();

				OnDown?.Invoke(_selectedObject);
			}
			else if (TryGetInteractionObject(out IInteraction interactionObject))
			{
				if (_selectedObject != null && ReferenceEquals(interactionObject, _selectedObject) && _selectedObject.InteractionCondition.clickable)
				{
					_selectedObject?.OnClick();

					OnClick?.Invoke(_selectedObject);
				}
			}
		}
	}
}
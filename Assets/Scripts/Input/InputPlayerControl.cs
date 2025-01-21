using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace GameName.Gameplay.Input
{
	public class InputPlayerControl : MonoBehaviour
	{
		//private CPlayer _player;
		private VCamera _vCamera;
		private CPlayerController _playerController;

		private InputSystem_Actions _inputActions;

		/*[Inject]
		public void Construct(
			//CPlayer player,
			CPlayerController playerController,
			VCamera vCamera)
		{
			//_player = player;
			_playerController = playerController;
			_vCamera = vCamera;
		}*/

		private void Awake()
		{
			_inputActions = new InputSystem_Actions();
		}

		private void OnEnable()
		{
			_inputActions.Enable();

			_inputActions.Player.Move.performed += OnMoveDelta;
			_inputActions.Player.Move.canceled += OnMoveDelta;

			_inputActions.Player.Jump.performed += OnJumpButton;
			_inputActions.Player.Jump.canceled += OnJumpButton;

			_inputActions.Player.Zoom.performed += OnZoomDelta;
		}
		private void OnDisable()
		{
			_inputActions.Player.Move.performed -= OnMoveDelta;
			_inputActions.Player.Move.canceled -= OnMoveDelta;

			_inputActions.Player.Jump.performed -= OnJumpButton;
			_inputActions.Player.Jump.canceled -= OnJumpButton;

			_inputActions.Player.Zoom.performed -= OnZoomDelta;

			_inputActions.Disable();
		}

		public void SetControllers(VCamera camera, CPlayerController playerController)
		{
			_vCamera = camera;
			_playerController = playerController;
		}

		private void OnMoveDelta(InputAction.CallbackContext context)
		{
			Vector2 value = context.ReadValue<Vector2>();

			Vector3 valueOnCamera = Quaternion.AngleAxis(_vCamera.transform.eulerAngles.y, Vector3.up) * new Vector3(value.x, 0.0f, value.y);

			_playerController.Move(valueOnCamera);
		}
		private void OnJumpButton(InputAction.CallbackContext context)
		{
			bool value = context.ReadValueAsButton();

			_playerController.Jump(value);
		}

		private void OnZoomDelta(InputAction.CallbackContext context)
		{
			Vector2 value = context.ReadValue<Vector2>();

			_vCamera.distanceValue = Math.Clamp(_vCamera.distanceValue - value.y * 0.005f, 0, 1);
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptureGem.Shell
{
    public class GamePlayerMovement : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private CharacterController _controller;
        [SerializeField] private float _movementSpeed = 6f;

        [SerializeField] private float _turnSmoothTime = 0.1f;
        [SerializeField] private float _turnSmoothVelocity;
        
        [SerializeField] private bool _isGrounded;
        [SerializeField] private Vector3 _playerVelocity;
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravity = -9f;

        private void Start()
        {
            if (_controller == null)
                _controller = GetComponent<CharacterController>();
            
            if(_camera == null)
                _camera = Camera.main;
        }

        private void Update()
        {
            _isGrounded = _controller.isGrounded;

            if (_isGrounded && _playerVelocity.y < 0.5f)
            {
                _playerVelocity.y = 0f;
            }
            
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"))
                .normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    _turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle,0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _controller.Move(moveDirection.normalized * _movementSpeed * Time.deltaTime);
            }

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
            }

            _playerVelocity.y += _gravity * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        public void CarryGem(bool bIsCarryGem)
        {
            _movementSpeed = bIsCarryGem ? 4f : 6f;
        }
    }
}

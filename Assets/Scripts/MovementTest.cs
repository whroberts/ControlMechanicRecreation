using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    public CharacterController _playerCharController;

    public Vector3 _playerMovementVector;
    public Vector3 _momentumVector;

    public float _playerMoveSpeed;
    public float _playerJumpVelocity;
    public float _playerJumpAcceleration;
    public float _playerJumpSpeed;

    public float _gravity = -9.81f;
    public float _jumpHeight = 2f;

    public float _clampXAxisSpeed;
    public float _clampZAxisSpeed;

    public bool _isPlayerGrounded;
    public bool _isPlayerJumping;
    public bool _isPlayerMoving;

    // Use this for initialization
    void Start()
    {
        _playerCharController = this.GetComponent<CharacterController>();

        _playerMoveSpeed = 15;
        _playerJumpAcceleration = 19.6f;
        _playerJumpSpeed = 15;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGroundingState();
        MovePlayerCharacter();
        ApplyJumpVelocity();
        ApplyMomentum();
        ApplyGravity();
    }

    public void FixedUpdate()
    {
        //ApplyMomentum();
        //ApplyGravity();
    }

    public void MovePlayerCharacter()
    {
        if (_playerCharController.isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
            {
                _playerCharController.SimpleMove(transform.forward * _playerMoveSpeed);// * Time.deltaTime);
                _momentumVector += _playerCharController.velocity;
            }

            if (Input.GetKey(KeyCode.A))
            {
                _playerCharController.SimpleMove(-transform.right * _playerMoveSpeed);// * Time.deltaTime);
                _momentumVector += _playerCharController.velocity;
            }

            if (Input.GetKey(KeyCode.S))
            {
                _playerCharController.SimpleMove(-transform.forward * _playerMoveSpeed);// * Time.deltaTime);
                _momentumVector += _playerCharController.velocity;
            }

            if (Input.GetKey(KeyCode.D))
            {
                _playerCharController.SimpleMove(transform.right * _playerMoveSpeed);// * Time.deltaTime);
                _momentumVector += _playerCharController.velocity;
            }
        }

        _clampXAxisSpeed = Mathf.Clamp(_momentumVector.x, -_playerMoveSpeed, _playerMoveSpeed);
        _clampZAxisSpeed = Mathf.Clamp(_momentumVector.z, -_playerMoveSpeed, _playerMoveSpeed);

        _momentumVector = new Vector3(_clampXAxisSpeed, _momentumVector.y, _clampZAxisSpeed);

        if (Input.GetButtonDown("Jump") && !_playerCharController.isGrounded)
        {
            _isPlayerJumping = true;
            Debug.Log("Jumped");
        }
    }

    public void ApplyJumpVelocity()
    {
        if (_isPlayerJumping)
        {
            _playerJumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            _isPlayerJumping = false;
        }
    }

    public void ApplyMomentum()
    {
        if (!_playerCharController.isGrounded)
        {
            _playerMovementVector = new Vector3(_momentumVector.x, _playerMovementVector.y, _momentumVector.z);
        }
    }

    public void ApplyGravity()
    {
        _playerJumpVelocity += _gravity * Time.deltaTime;
        _playerMovementVector = new Vector3(_playerMovementVector.x, _playerJumpVelocity, _playerMovementVector.z);
        _playerCharController.Move(_playerMovementVector * Time.deltaTime);
    }

    public void CheckGroundingState()
    {
        if (_playerCharController.isGrounded)
        {
            _playerJumpVelocity = 0f;
            _momentumVector = Vector3.zero;
            _playerMovementVector = Vector3.zero;
        }
    }
}

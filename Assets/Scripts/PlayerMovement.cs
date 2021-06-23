using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] CharacterController _controller = null;
    [SerializeField] float _speed = 12f;
    [SerializeField] float _sprintBoost = 5f;

    [Header("Gravity")]
    [SerializeField] Transform _groundCheck = null;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _groundDistance = 0.4f;
    [SerializeField] float _jumpHeight = 2f;
    [SerializeField] LayerMask _groundMask;

    Vector3 _velocity;
    Vector3 _momentum;
    public bool _isGrounded = true;
    bool _playerJumped = false;
    float _startSpeed;
    float _maxSpeed;

    private void Start()
    {
        _startSpeed = _speed;
        _maxSpeed = _startSpeed + _sprintBoost;
    }

    void Update()
    {
        GroundCheck();
        Movement();
        JumpVelocity();
    }

    private void FixedUpdate()
    {
        Gravity();
    }

    void Movement()
    {
        if (_isGrounded)
        {
            _playerJumped = false;
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            _controller.SimpleMove(move * _speed);// * Time.deltaTime);
            _momentum = _controller.velocity;

            if (Input.GetButtonDown("Jump"))
            {
                _playerJumped = true;
            }
        }
    }
    void JumpVelocity()
    {
        if (_playerJumped)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }

    void Gravity()
    {
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    void GroundCheck()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

    }
    void ApplyMomentum()
    {
        if (_isGrounded)
        {
            
        }
    }

}

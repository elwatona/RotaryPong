using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Watona.Utils.Variables;

namespace RotaryPong
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] FloatReference _playerSpeed;
        [SerializeField] FloatReference _rotationAmmount;
        [SerializeField] FloatReference _distanceFromCenter;
        [SerializeField] BooleanReference _hasInterpolatedRotation;
        [SerializeField] FloatReference _interpolatedRotationSpeed;

        private Rigidbody _rigidbody;
        private Vector3 _startingPoint;
        private bool _leftRotationInput;
        private bool _rightRotationInput;
        private float _timeOutside;

        private Vector2 _movementInput;

        public void OnMove(InputAction.CallbackContext ctx) => _movementInput = ctx.ReadValue<Vector2>();
        public void OnRotateLeft(InputAction.CallbackContext ctx) => _leftRotationInput = ctx.ReadValueAsButton();
        public void OnRotateRight(InputAction.CallbackContext ctx) => _rightRotationInput = ctx.ReadValueAsButton();

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _startingPoint = transform.position;
        }

        private void Update()
        {
            CheckDistanceFromCenter();
        }

        ///<summary> Comprueba la distancia del jugador respecto al centro del mapa para considerar su posible reinicio de posicion </summary>
        private void CheckDistanceFromCenter()
        {
            float distanceFromZero = Vector2.Distance(transform.position, Vector2.zero);
            float distanceFromCenter = _distanceFromCenter.Value;

            if (distanceFromZero < distanceFromCenter)
            {
                _timeOutside = 0;
                return;
            }

            _timeOutside += Time.deltaTime;
            if (_timeOutside >= 1)
            {
                transform.position = _startingPoint;
            }
        }

        private void FixedUpdate()
        {
            Movement();
            Rotation();
        }

        private void Rotation()
        {
            float rotationInZ = transform.eulerAngles.z;
            float rotationAmount = _rotationAmmount.Value;
            float interpolatedRotationSpeed = _interpolatedRotationSpeed.Value;
            bool hasInterpolatedRotation = _hasInterpolatedRotation.Value;

            if (_leftRotationInput)
            {
                rotationInZ -= rotationAmount;
            }
            else if (_rightRotationInput)
            {
                rotationInZ += rotationAmount;
            }

            if (!hasInterpolatedRotation)
            {
                if (_leftRotationInput)
                {
                    _leftRotationInput = false;
                }
                if (_rightRotationInput)
                {
                    _rightRotationInput = false;
                }

                _rigidbody.rotation = Quaternion.Euler(0, 0, rotationInZ);
            }
            else
            {
                _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.Euler(0, 0, rotationInZ), Time.deltaTime * interpolatedRotationSpeed);
            }
        }

        private void Movement()
        {
            float playerSpeed = _playerSpeed.Value;
            Vector3 toMove = _movementInput;
            float step = Time.deltaTime * playerSpeed;

            _rigidbody.velocity = Vector3.zero;
            toMove *= step;
            _rigidbody.position += toMove;
        }
    }
}
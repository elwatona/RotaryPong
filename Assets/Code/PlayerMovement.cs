using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RotaryPong
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float _playerSpeed;
        [SerializeField] float _rotationAmmount;
        [SerializeField] float _distanceFromCenter;
        [SerializeField] bool _hasInterpolatedRotation;
        [SerializeField] float _interpolatedRotationSpeed;

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
            GetPlayerPrefs();
            _rigidbody = GetComponent<Rigidbody>();
            _startingPoint = transform.position;
        }

        ///<summary> Toma los valores almacenados en PlayerPrefs de cada variable </summary>
        void GetPlayerPrefs()
        {
            if (PlayerPrefs.HasKey("playerSpeed"))
            {
                _playerSpeed = PlayerPrefs.GetFloat("playerSpeed");
            }
            if (PlayerPrefs.HasKey("playerSpeed"))
            {
                _rotationAmmount = PlayerPrefs.GetFloat("playerRotAmmount");
            }
            if (PlayerPrefs.HasKey("playerRotSmooth"))
            {
                int interpolatedRotBool = PlayerPrefs.GetInt("playerRotSmooth");
                if (interpolatedRotBool == 0)
                {
                    _hasInterpolatedRotation = false;
                }
                else if (interpolatedRotBool == 1)
                {
                    _hasInterpolatedRotation = true;
                }
            }
            if (PlayerPrefs.HasKey("playerRotSpeed"))
            {
                _interpolatedRotationSpeed = PlayerPrefs.GetFloat("playerRotSpeed");
            }
        }

        private void Update()
        {
            CheckDistanceFromCenter();
        }

        ///<summary> Comprueba la distancia del jugador respecto al centro del mapa para considerar su posible reinicio de posicion </summary>
        private void CheckDistanceFromCenter()
        {
            float distanceFromZero = Vector2.Distance(transform.position, Vector2.zero);

            if (distanceFromZero < _distanceFromCenter)
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

            if (_leftRotationInput)
            {
                rotationInZ -= _rotationAmmount;
            }
            else if (_rightRotationInput)
            {
                rotationInZ += _rotationAmmount;
            }

            if (!_hasInterpolatedRotation)
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
                _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.Euler(0, 0, rotationInZ), Time.deltaTime * _interpolatedRotationSpeed);
            }
        }

        private void Movement()
        {
            _rigidbody.velocity = Vector3.zero;
            Vector3 toMove = _movementInput;
            float step = Time.deltaTime * _playerSpeed;
            toMove *= step;
            _rigidbody.position += toMove;
        }
    }
}
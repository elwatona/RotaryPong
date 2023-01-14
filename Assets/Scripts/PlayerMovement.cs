using UnityEngine;
using UnityEngine.InputSystem;
using Watona.Variables;
using RotaryPong.Events;

namespace RotaryPong
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] Paint _team;
        [Header("Parameters")]
        [SerializeField] VariableReference<float> _playerSpeed;
        [SerializeField] VariableReference<float> _rotationAmmount;
        [SerializeField] VariableReference<float> _distanceFromCenter;
        [SerializeField] VariableReference<bool> _hasInterpolatedRotation;
        [SerializeField] VariableReference<float> _interpolatedRotationSpeed;
        private Vector3 _startingPoint;
        private float _timeOutside;

        [Header("Events")]
        [SerializeField] BallEffectEvent _ballEffect;
        [SerializeField] SpinMapInputEvent _spinMap;

        private Rigidbody _rigidbody;
        private Vector2 _movementInput;
        private bool _leftRotationInput;
        private bool _rightRotationInput;

        public void OnMove(InputAction.CallbackContext ctx) => _movementInput = ctx.ReadValue<Vector2>();
        public void OnRotateLeft(InputAction.CallbackContext ctx) => _leftRotationInput = ctx.ReadValueAsButton();
        public void OnRotateRight(InputAction.CallbackContext ctx) => _rightRotationInput = ctx.ReadValueAsButton();
        public void OnBallEfect(InputAction.CallbackContext ctx) => BallEffect(ctx.ReadValue<Vector2>());
        public void OnRotateMap(InputAction.CallbackContext ctx) => SpinMap(ctx.ReadValue<float>());

        private void BallEffect(Vector2 input)
        {
            BallEffectParameters parameters = new BallEffectParameters {SourceInput = input, SourceTeam = _team};
            
            _ballEffect.Raise(parameters);
        }
        private void SpinMap(float input)
        {
            SpinMapInputParameter parameters = new SpinMapInputParameter {SourceDirection = input, SourceTeam = _team};

            _spinMap.Raise(parameters);
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
        ///<summary> Contiene la logica que permite el movimiento </summary>
        private void Movement()
        {
            Vector3 direction = _movementInput;
            float timeSpeed = Time.deltaTime * _playerSpeed.Value;

            _rigidbody.velocity = Vector3.zero;
            direction *= timeSpeed;
            _rigidbody.position += direction;
        }
        ///<summary> Contiene la logica que permite la rotacion </summary>
        private void Rotation()
        {
            bool hasInterpolatedRotation = _hasInterpolatedRotation.Value;

            float currentRotation = transform.eulerAngles.z;
            float rotationAmount = _rotationAmmount.Value;
            float rotationTimeSpeed = Time.deltaTime * _interpolatedRotationSpeed.Value;
            float newRotation = ChangeRotationValue(currentRotation, rotationAmount);

            Quaternion fixedRotation = Quaternion.Euler(0, 0, newRotation);
            Quaternion lerpRotation = Quaternion.Lerp(_rigidbody.rotation, fixedRotation, rotationTimeSpeed);

            if (!hasInterpolatedRotation) ResetRotationInput();

            _rigidbody.rotation = hasInterpolatedRotation ? lerpRotation : fixedRotation;
        }
        ///<summary> Retorna la rotacion deseada dependiendo del input apretado </summary> 
        private float ChangeRotationValue(float value, float amount)
        {
            if (_leftRotationInput) return value -= amount;
            else if (_rightRotationInput) return value += amount;

            return value;
        }
        ///<summary> Reinicia el valor de los inputs que permiten la rotacion </summary>
        private void ResetRotationInput()
        {
            if (_leftRotationInput)
                _leftRotationInput = false;
            if (_rightRotationInput)
                _rightRotationInput = false;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            _startingPoint = transform.position;
        }
        private void Update()
        {
            CheckDistanceFromCenter();
        }
        private void FixedUpdate()
        {
            Movement();
            Rotation();
        }
    }
}
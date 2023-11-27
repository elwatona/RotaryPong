using UnityEngine;
using Watona.Variables;
using RotaryPong.Events;

namespace RotaryPong
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallPhysics : MonoBehaviour
    {
        [SerializeField, Header("Input")] BooleanVariable _canChangeDirection;
        [SerializeField] FloatVariable _effectTimerInSeconds;
        private float _effectTimer;
        [SerializeField] private bool _hasLimits;
        [SerializeField] VariableReference<float> _distanceFromCenter;
        [SerializeField] FloatVariable _ballSpeed;
        [SerializeField] FloatVariable _minVelocity;
        [SerializeField] GameObject _currentGrabber;
        private Vector3 _startingPoint;
        private float _timeOutside;
        private Vector3 lastFrameVelocity;
        private Rigidbody _rigidbody;
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
            _effectTimer -= Time.deltaTime;
        }
        private void FixedUpdate()
        {
            if(_hasLimits)
            {
                CheckDistanceFromCenter();
                return;
            }
            CheckPositionRelativeToScreen(transform.localScale.y);
        }
        private void LateUpdate()
        {
            lastFrameVelocity = _rigidbody.velocity;
        }
        ///<summary> Comprueba la distancia del objeto respecto al centro del mapa para considerar su posible reinicio de posicion </summary>
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
        void CheckPositionRelativeToScreen(float offset)
        {
            float maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - offset;
            float maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - offset;
            float minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + offset;
            float minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y + offset;

            Vector3 currentPosition = transform.position;

            if (currentPosition.x < minX || currentPosition.x > maxX || currentPosition.y < minY || currentPosition.y > maxY)
            {
                Vector3 collisionNormal = GetCollisionNormal(currentPosition, minX, maxX, minY, maxY);
                Bounce(collisionNormal);
            }
        }

        Vector3 GetCollisionNormal(Vector3 currentPosition, float minX, float maxX, float minY, float maxY)
        {
            Vector3 collisionNormal = Vector3.zero;
            if (currentPosition.x < minX) collisionNormal += Vector3.right;
            else if (currentPosition.x > maxX) collisionNormal -= Vector3.right;
            if (currentPosition.y < minY) collisionNormal += Vector3.up;
            else if (currentPosition.y > maxY) collisionNormal -= Vector3.up;
            return collisionNormal.normalized;
        }
        public void ResetPosition()
        {
            transform.position = _startingPoint;
            _rigidbody.velocity = Vector3.zero;
        }
        ///<summary> Permite el rebote del objeto calculando la velocidad y direccion del rigidbody en relacion a <paramref name="collisionNormal"/></summary>
        public void Bounce(Vector3 collisionNormal)
        {
            print(collisionNormal);
            float speed = lastFrameVelocity.magnitude;
            float ballSpeed = _ballSpeed.Value;
            float minVelocity = _minVelocity.Value;

            if (speed > minVelocity)
            {
                Vector3 ballVelocity = lastFrameVelocity.normalized;
                Vector3 reflectedDirection = Vector3.Reflect(ballVelocity, collisionNormal).normalized;
                Vector3 newDirection = (reflectedDirection + ballVelocity).normalized;
                float newSpeed = Mathf.Max(speed, ballSpeed);
                _rigidbody.velocity = newDirection * newSpeed;
                Debug.DrawRay(transform.position, newDirection * 3, Color.red, 5);
            }
            _rigidbody.velocity = collisionNormal * ballSpeed;
        }
        public void Dropped(DropBallParameters parameters)
        {
            if(parameters.SourceGrabber != _currentGrabber) return;
            float ballVelocity = lastFrameVelocity.magnitude;
            float ballSpeed = _ballSpeed.Value;
            float minVelocity = _minVelocity.Value;
            Vector3 direction = parameters.SourceDirection;

            if (ballVelocity > minVelocity)
            {
                if (ballVelocity <= ballSpeed) ballVelocity = ballSpeed;

                _rigidbody.velocity = direction * Mathf.Max(ballVelocity, minVelocity);
                Debug.DrawRay(transform.position, direction * 3, Color.red, 5);
                return;
            }
            _rigidbody.velocity = direction * ballSpeed;
            
            _rigidbody.velocity = direction * Mathf.Max(ballSpeed, minVelocity);
            _currentGrabber = null;
            _effectTimer = _effectTimerInSeconds.Value;
        }
        public void SetGrabber(BallGrabbedParameters parameters)
        {
            _currentGrabber = parameters.SourceGrabber;
        }
        ///<summary> Comprueba si el tag corresponde a player para luego comparar el nombre de <paramref name="collision"/> y asi sonar audio a la vez que cambiar colores </summary>
        public void CheckForPlayer(GameObject gameObject)
        {
            if (gameObject.tag != "Player")
            {
                _effectTimer = 0;
                return;
            }
            _effectTimer = _effectTimerInSeconds.Value;
        }
        public void EffectMovement(BallEffectParameters parameter, Paint paint)
        {   
            if(_canChangeDirection.Value && _effectTimer > 0 && parameter.SourceTeam == paint) _rigidbody.AddForce(parameter.SourceInput, ForceMode.Impulse);
        }
    }
}
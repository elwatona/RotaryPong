using System.Collections;
using UnityEngine;
using Watona.Variables;
using Watona.Events;
using RotaryPong.Events;

namespace RotaryPong
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    [RequireComponent(typeof(Renderer), typeof(TrailRenderer))]
    public class Ball : MonoBehaviour
    {
        IEnumerator OnScoreMade()
        {
            SetInvis();
            yield return new WaitForSeconds(2);
            TurnBackOn();
        }
        [SerializeField] CodedGameEventListener<Paint> _playerHit;
        [SerializeField] CodedEventListener _afterScoreListener;
        [SerializeField] CodedGameEventListener<BallEffectParameters> _inputListener;
        [SerializeField] CodedGameEventListener<BallGrabbedParameters> _ballGrabbedListener;
        [SerializeField] CodedGameEventListener<DropBallParameters> _dropBallListener;
        [SerializeField, Header("Parameters")] PaintVariable _paint;
        [SerializeField] BooleanVariable _canScore;
        [SerializeField] FloatVariable _ballSpeed;
        [SerializeField] FloatVariable _minVelocity;
        [SerializeField] VariableReference<float> _distanceFromCenter;
        [SerializeField, Header("Color")] BooleanVariable _canChangeWithBounces;
        [SerializeField] FloatVariable _colorDurationSeconds;
        [SerializeField] FloatVariable _colorDurationBounces;
        [SerializeField, Header("Input")] BooleanVariable _canChangeDirection;
        [SerializeField] FloatVariable _effectTimerInSeconds;
        [SerializeField, Header("Configurations")] Material[] PlayerMaterials;
        [SerializeField] AudioClip[] _playerAudioClips;
        [SerializeField] GameObject _currentGrabber;
        private Material _startingMaterial;
        private Color _startingColor;
        private Vector3 lastFrameVelocity;
        private Vector3 _startingPoint;
        private float _colorBounces;
        private float _colorTimer;
        private float _timeOutside;
        private float _effectTimer;

        [Header("Components")]
        private Rigidbody _rigidbody;
        private Collider _collider;
        private Renderer _renderer;
        private TrailRenderer _trailRenderer;

        private void OnEnable()
        {
            _afterScoreListener?.OnEnable(() => StartCoroutine(OnScoreMade()));
            _inputListener?.OnEnable(EffectMovement);
            _playerHit?.OnEnable(ColorChange);
            _ballGrabbedListener?.OnEnable(SetGrabber);
            _dropBallListener?.OnEnable(Dropped);
        }
        private void OnDisable()
        {
            _afterScoreListener?.OnDisable();
            _inputListener?.OnDisable();
            _playerHit?.OnDisable();
            _ballGrabbedListener?.OnDisable();
            _dropBallListener?.OnDisable();
        }
        private void Awake()
        {
            GetComponents();
        }
        private void Start()
        {
            _startingMaterial = _renderer.material;
            _startingColor = _trailRenderer.startColor;
            _startingPoint = transform.position;
            _canScore.SetValue(true);
        }
        private void Update()
        {
            lastFrameVelocity = _rigidbody.velocity;
            
            if (_paint.Value != Paint.White) CheckPaint();
            CheckDistanceFromCenter();
            _effectTimer -= Time.deltaTime;
        }
        private void OnCollisionEnter(Collision collision)
        {
            Bounce(collision.contacts[0].normal);
            CheckForPlayer(collision.gameObject);
        }

        private void SetGrabber(BallGrabbedParameters parameters)
        {
            _currentGrabber = parameters.SourceGrabber;
        }
        ///<summary> Toma los componentes para cada referencia </summary>
        private void GetComponents()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _renderer = GetComponent<Renderer>();
            _collider = GetComponent<Collider>();
            _trailRenderer = GetComponent<TrailRenderer>();
        }
        ///<summary> Permite el rebote del objeto calculando la velocidad y direccion del rigidbody en relacion a <paramref name="collisionNormal"/></summary>
        private void Bounce(Vector3 collisionNormal)
        {
            float speed = lastFrameVelocity.magnitude;
            float ballSpeed = _ballSpeed.Value;
            float minVelocity = _minVelocity.Value;
            _colorBounces --;

            if (speed > minVelocity)
            {
                Vector3 ballVelocity = lastFrameVelocity.normalized;
                Vector3 direction = Vector3.Reflect(ballVelocity, collisionNormal);
                if (speed <= ballSpeed)
                {
                    speed = ballSpeed;
                }
                _rigidbody.velocity = direction * Mathf.Max(speed, minVelocity);
                Debug.DrawRay(transform.position, direction * 3, Color.red, 5);
                return;
            }
            _rigidbody.velocity = collisionNormal * ballSpeed;
        }
        private void Dropped(DropBallParameters parameters)
        {
            if(parameters.SourceGrabber != _currentGrabber)
                return;

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
        ///<summary> Comprueba si el tag corresponde a player para luego comparar el nombre de <paramref name="collision"/> y asi sonar audio a la vez que cambiar colores </summary>
        private void CheckForPlayer(GameObject gameObject)
        {
            if (gameObject.tag != "Player")
            {
                _effectTimer = 0;
                return;
            }
            _effectTimer = _effectTimerInSeconds.Value;
        }
        ///<summary> Cambia el color del objeto referenciando el index <paramref name="who"/> </summary>
        private void ColorChange(Paint team)
        {
            int who = (int)team - 1;

            _colorTimer = _colorDurationSeconds.Value;
            _colorBounces = _colorDurationBounces.Value;

            _paint.SetValue(team);

            _renderer.material = PlayerMaterials[who];
            _trailRenderer.material = _renderer.material;
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
        ///<summary> Comprueba la pintura actual, en caso que no sea blanca cambiara su color cuando <paramref name="_colorTimer"/> llegue a 0 </summary>
        private void CheckPaint()
        {
            if(_canChangeWithBounces.Value)
            {
                ColorByBounces();
                return;
            }
            ColorByTime();
        }
        private void ColorByTime()
        {
            _colorTimer -= Time.deltaTime;
            if (_colorTimer <= 0) LosePaint();
        }
        private void ColorByBounces()
        {
            if(_colorBounces <= 0) LosePaint();
        }
        ///<summary> Configura <paramref name="Paint"/> a White, a la vez que vuelve las propiedades visuales del objeto a su estado inciail</summary>
        private void LosePaint()
        {
            Debug.Log("lose paint");
            _paint.SetValue(Paint.White);
            _renderer.material = _startingMaterial;
            _renderer.material.SetColor("_EmissionColor", _renderer.material.color);
            _trailRenderer.material = _renderer.material;
            _colorBounces = _colorDurationBounces.Value;
        }

        public void EffectMovement(BallEffectParameters parameter)
        {   
            if(_canChangeDirection.Value && _effectTimer > 0 && parameter.SourceTeam == _paint.Value) _rigidbody.AddForce(parameter.SourceInput, ForceMode.Impulse);
        }
        ///<summary> Reinicia las configuraciones del objeto, dejandole en el estado y posicion incial </summary>
        public void TurnBackOn()
        {
            transform.position = _startingPoint;
            _rigidbody.velocity = Vector3.zero;
            _renderer.enabled = true;
            _collider.enabled = true;
            _trailRenderer.enabled = true;
            _canScore.SetValue(true);
        }
        ///<summary> Desactiva el aspecto visual </summary>
        public void SetInvis()
        {
            LosePaint();
            _renderer.enabled = false;
            _collider.enabled = false;
            _trailRenderer.enabled = false;
            _canScore.SetValue(false);
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Watona.Variables;
using RotaryPong.Events;

namespace RotaryPong
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    [RequireComponent(typeof(Renderer), typeof(TrailRenderer))]
    public class Ball : MonoBehaviour
    {
        [Header("Parameters")]
        public PaintVariable BallPaint;
        public bool CanScore;
        [SerializeField] VariableReference<float> _ballSpeed;
        [SerializeField] VariableReference<float> _distanceFromCenter;
        [SerializeField] VariableReference<float> _minVelocity;
        [SerializeField, Space] VariableReference<bool> _canChangeWithBounces;
        [SerializeField] VariableReference<float> _colorDurationSeconds;
        [SerializeField] VariableReference<int> _colorDurationBounces;
        [SerializeField, Space] VariableReference<bool> _canChangeDirection;
        [SerializeField] VariableReference<float> _effectTimerInSeconds;

        [Header("Configurations")]
        [SerializeField] Material[] PlayerMaterials;
        [SerializeField] AudioClip[] _playerAudioClips;
        private Material _startingMaterial;
        private Color _startingColor;
        private Vector3 lastFrameVelocity;
        private Vector3 _startingPoint;
        [SerializeField] private Vector2 _effectInput;
        private int _colorBounces;
        private float _colorTimer;
        private float _timeOutside;
        [SerializeField] private float _effectTimer;

        [Header("Components")]
        private AudioSource _audioSource;
        private Rigidbody _rigidbody;
        private Collider _collider;
        private Renderer _renderer;
        private TrailRenderer _trailRenderer;

        ///<summary> Toma los componentes para cada referencia </summary>
        private void GetComponents()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
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
        ///<summary> Comprueba si el tag corresponde a player para luego comparar el nombre de <paramref name="collision"/> y asi sonar audio a la vez que cambiar colores </summary>
        private void CheckForPlayer(GameObject gameObject)
        {
            if (gameObject.tag != "Player")
            {
                _effectTimer = 0;
                return;
            }

            switch(gameObject.name)
            {
                case "p1":
                    PlayAudio(0);
                    ColorChange(0);
                break;
                case "p2":
                    PlayAudio(1);
                    ColorChange(1);
                break;
            }
            _effectTimer = _effectTimerInSeconds.Value;
        }
        ///<summary> Reproduce la pista de audio cuyo index es <paramref name="who"/> </summary>
        private void PlayAudio(int who)
        {
            _audioSource.clip = _playerAudioClips[who];
            _audioSource.Play();
        }
        ///<summary> Cambia el color del objeto referenciando el index <paramref name="who"/> </summary>
        private void ColorChange(int who)
        {
            _colorTimer = _colorDurationSeconds.Value;
            _colorBounces = _colorDurationBounces.Value;
            if (who == 0)
            {
                BallPaint.SetValue(Paint.Pink);
            }
            else if (who == 1)
            {
                BallPaint.SetValue(Paint.Blue);
            }
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
            BallPaint.SetValue(Paint.White);
            _renderer.material = _startingMaterial;
            _renderer.material.SetColor("_EmissionColor", _renderer.material.color);
            _trailRenderer.material = _renderer.material;
            _colorBounces = _colorDurationBounces.Value;
        }

        public void EffectMovement(BallEffectParameters parameter)
        {   
            if(_canChangeDirection.Value && _effectTimer > 0 && parameter.SourceTeam == BallPaint.Value) _rigidbody.AddForce(parameter.SourceInput, ForceMode.Impulse);
        }
        ///<summary> Reinicia las configuraciones del objeto, dejandole en el estado y posicion incial </summary>
        public void TurnBackOn()
        {
            transform.position = _startingPoint;
            _rigidbody.velocity = Vector3.zero;
            _renderer.enabled = true;
            _collider.enabled = true;
            _trailRenderer.enabled = true;
            CanScore = true;
        }
        ///<summary> Desactiva el aspecto visual </summary>
        public void SetInvis()
        {
            LosePaint();
            _renderer.enabled = false;
            _collider.enabled = false;
            _trailRenderer.enabled = false;
            CanScore = false;
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
            CanScore = true;
        }
        private void Update()
        {
            lastFrameVelocity = _rigidbody.velocity;
            
            if (BallPaint.Value != Paint.White) CheckPaint();
            CheckDistanceFromCenter();
            _effectTimer -= Time.deltaTime;
        }
        private void OnCollisionEnter(Collision collision)
        {
            Bounce(collision.contacts[0].normal);
            CheckForPlayer(collision.gameObject);
        }
        
    }
}
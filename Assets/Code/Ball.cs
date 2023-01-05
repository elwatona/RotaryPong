using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RotaryPong
{
    public enum BallPaint
    {
        White,
        Pink,
        Blue
    }
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        public BallPaint Paint;
        public Material[] PlayerMaterials;
        public bool CanScore;
        [SerializeField] float _ballSpeed;
        [SerializeField] float _distanceFromCenter;
        [SerializeField] float _minVelocity;
        [SerializeField] float _colorDuration;
        [SerializeField] AudioClip[] _playerAudioClips;

        [Header("Configurations")]
        private Rigidbody _rigidbody;
        private AudioSource _audioSource;
        private Renderer _renderer;
        private Collider _collider;
        private TrailRenderer _trailRenderer;
        private Material _startingMaterial;
        private Color _startingColor;
        private Vector3 lastFrameVelocity;
        private Vector3 _startingPoint;
        private float _colorTimer;
        private float _timeOutside;

        private void Start()
        {
            GetComponents();
            GetPlayerPrefs();

            _startingMaterial = _renderer.material;
            _startingColor = _trailRenderer.startColor;
            _startingPoint = transform.position;
            CanScore = true;
        }
        ///<summary> Toma los componentes para cada variable </summary>
        private void GetComponents()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
            _renderer = GetComponent<Renderer>();
            _collider = GetComponent<Collider>();
            _trailRenderer = GetComponent<TrailRenderer>();
        }
        ///<summary> Toma los valores almacenados en PlayerPrefs de cada variable </summary>
        private void GetPlayerPrefs()
        {
            if (PlayerPrefs.HasKey("ballSpeed"))
            {
                _ballSpeed = PlayerPrefs.GetFloat("ballSpeed");
            }
            if (PlayerPrefs.HasKey("ballDrag"))
            {
                _rigidbody.drag = PlayerPrefs.GetFloat("ballDrag");
            }
            if (PlayerPrefs.HasKey("ballMinSpeed"))
            {
                _minVelocity = PlayerPrefs.GetFloat("ballMinSpeed");
            }
            if (PlayerPrefs.HasKey("ballColorDuration"))
            {
                _colorDuration = PlayerPrefs.GetFloat("ballColorDuration");
            }
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
        ///<summary> Apaga el aspecto visual y desactiva CanScore </summary>
        public void SetInvis()
        {
            LosePaint();
            _renderer.enabled = false;
            _collider.enabled = false;
            _trailRenderer.enabled = false;
            CanScore = false;
        }
        ///<summary> Permite el rebote del objeto calculando la velocidad y direccion del rigidbody en relacion a <paramref name="collisionNormal"/></summary>
        private void Bounce(Vector3 collisionNormal)
        {
            float speed = lastFrameVelocity.magnitude;

            if (speed > _minVelocity)
            {
                Vector3 ballVelocity = lastFrameVelocity.normalized;
                Vector3 direction = Vector3.Reflect(ballVelocity, collisionNormal);
                if (speed <= _ballSpeed)
                {
                    speed = _ballSpeed;
                }
                _rigidbody.velocity = direction * Mathf.Max(speed, _minVelocity);
                Debug.DrawRay(transform.position, direction * 3, Color.red, 5);
                return;
            }
            _rigidbody.velocity = collisionNormal * _ballSpeed;
        }
        ///<summary> Comprueba si el tag corresponde a player para luego comparar el nombre de <paramref name="collObject"/> y asi sonar audio a la vez que cambiar colores </summary>
        private void CheckForPlayer(GameObject collObject)
        {
            if (collObject.tag == "Player")
            {
                if (collObject.name == "p1")
                {
                    PlayAudio(0);
                    ColorChange(0);
                }
                else if (collObject.name == "p2")
                {
                    PlayAudio(1);
                    ColorChange(1);
                }
            }
        }
        ///<summary> Reproduce la pista de audio cuyo index es <paramref name="who"/></summary>
        private void PlayAudio(int who)
        {
            _audioSource.clip = _playerAudioClips[who];
            _audioSource.Play();
        }
        ///<summary> Cambia el color del objeto referenciando el index <paramref name="who"/> </summary>
        private void ColorChange(int who)
        {
            _colorTimer = _colorDuration;
            if (who == 0)
            {
                Paint = BallPaint.Pink;
            }
            else if (who == 1)
            {
                Paint = BallPaint.Blue;
            }
            _renderer.material = PlayerMaterials[who];
            _trailRenderer.material = _renderer.material;
        }

        private void Update()
        {
            lastFrameVelocity = _rigidbody.velocity;
            CheckPaint();
            CheckDistanceFromCenter();
        }
        ///<summary> Comprueba la distancia del objeto respecto al centro del mapa para considerar su posible reinicio de posicion </summary>
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
        ///<summary> Comprueba la pintura actual, en caso que no sea blanca cambiara su color cuando <paramref name="_colorTimer"/> llegue a 0 </summary>
        private void CheckPaint()
        {
            if (Paint != BallPaint.White)
            {
                _colorTimer -= Time.deltaTime;
                if (_colorTimer <= 0)
                {
                    LosePaint();
                }
            }
        }
        ///<summary> Configura <paramref name="Paint"/> a White, a la vez que vuelve las propiedades visuales del objeto a su estado inciail</summary>
        private void LosePaint()
        {
            Debug.Log("lose paint");
            Paint = BallPaint.White;
            _renderer.material = _startingMaterial;
            //trailRend.startColor = startingColor;
            //trailRend.endColor = startingColor;
            _renderer.material.SetColor("_EmissionColor", _renderer.material.color);
            _trailRenderer.material = _renderer.material;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Bounce(collision.contacts[0].normal);
            CheckForPlayer(collision.gameObject);
        }
        
    }
}
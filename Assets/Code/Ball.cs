using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Utils.Variables;

namespace RotaryPong
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    [RequireComponent(typeof(Renderer), typeof(TrailRenderer))]
    public class Ball : MonoBehaviour
    {
        [Header("Parameters")]
        public Paint Paint;
        public bool CanScore;
        [SerializeField] FloatReference _ballSpeed;
        [SerializeField] FloatReference _distanceFromCenter;
        [SerializeField] FloatReference _minVelocity;
        [SerializeField] FloatReference _colorDuration;

        [Header("Configurations")]
        public Material[] PlayerMaterials;
        [SerializeField] AudioClip[] _playerAudioClips;
        private Material _startingMaterial;
        private Color _startingColor;
        private Vector3 lastFrameVelocity;
        private Vector3 _startingPoint;
        private float _colorTimer;
        private float _timeOutside;

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
            if (gameObject.tag == "Player")
            {
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
            _colorTimer = _colorDuration.Value;
            if (who == 0)
            {
                Paint = Paint.Pink;
            }
            else if (who == 1)
            {
                Paint = Paint.Blue;
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
            if (Paint != Paint.White)
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
            Paint = Paint.White;
            _renderer.material = _startingMaterial;
            //trailRend.startColor = startingColor;
            //trailRend.endColor = startingColor;
            _renderer.material.SetColor("_EmissionColor", _renderer.material.color);
            _trailRenderer.material = _renderer.material;
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

        private void Start()
        {
            GetComponents();

            _startingMaterial = _renderer.material;
            _startingColor = _trailRenderer.startColor;
            _startingPoint = transform.position;
            CanScore = true;
        }
        private void Update()
        {
            lastFrameVelocity = _rigidbody.velocity;
            CheckPaint();
            CheckDistanceFromCenter();
        }
        private void OnCollisionEnter(Collision collision)
        {
            Bounce(collision.contacts[0].normal);
            CheckForPlayer(collision.gameObject);
        }
        
    }
}
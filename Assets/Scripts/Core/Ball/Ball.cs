using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Watona.Variables;
using Watona.Events;
using RotaryPong.Events;

namespace RotaryPong
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(PaintController))]
    [RequireComponent(typeof(BallPhysics))]
    public class Ball : MonoBehaviour
    {
        IEnumerator OnScoreMade()
        {
            SetInvis();
            yield return new WaitForSeconds(2);
            if(!_gameEnded.Value) TurnBackOn();
        }
        [SerializeField] CodedGameEventListener<Paint> _playerHit;
        [SerializeField] CodedEventListener _afterScoreListener;
        [SerializeField] CodedGameEventListener<BallEffectParameters> _inputListener;
        [SerializeField] CodedGameEventListener<BallGrabbedParameters> _ballGrabbedListener;
        [SerializeField] CodedGameEventListener<DropBallParameters> _dropBallListener;
        [SerializeField] BooleanVariable _canScore;
        [SerializeField] BooleanVariable _gameEnded;

        [Header("Components")]
        private Collider _collider;
        private Renderer _renderer;
        private PaintController _paintController;
        private BallPhysics _ballPhysics;

        private void Awake()
        {
            GetComponents();
        }
        private void OnEnable()
        {
            _afterScoreListener?.OnEnable(() => StartCoroutine(OnScoreMade()));
            _inputListener?.OnEnable((x) => _ballPhysics.EffectMovement(x, _paintController.GetPaint()));
            _playerHit?.OnEnable(_paintController.ColorChange);
            _ballGrabbedListener?.OnEnable(_ballPhysics.SetGrabber);
            _dropBallListener?.OnEnable(_ballPhysics.Dropped);
        }
        private void OnDisable()
        {
            _afterScoreListener?.OnDisable();
            _inputListener?.OnDisable();
            _playerHit?.OnDisable();
            _ballGrabbedListener?.OnDisable();
            _dropBallListener?.OnDisable();
        }
        private void Start()
        {
            _canScore.SetValue(true);
        }
        private void OnCollisionEnter(Collision collision)
        {
            _ballPhysics.Bounce(collision.contacts[0].normal);
            _paintController.Bounce();
            _ballPhysics.CheckForPlayer(collision.gameObject);
        }

        ///<summary> Toma los componentes para cada referencia </summary>
        private void GetComponents()
        {
            _renderer = GetComponent<Renderer>();
            _collider = GetComponent<Collider>();
            _paintController = GetComponent<PaintController>();
            _ballPhysics = GetComponent<BallPhysics>();
        }

        ///<summary> Reinicia las configuraciones del objeto, dejandole en el estado y posicion incial </summary>
        private void TurnBackOn()
        {
            _ballPhysics.ResetPosition();
            _renderer.enabled = true;
            _collider.enabled = true;
            _paintController.SetTrailRendererActive(true);
            _canScore.SetValue(true);
        }
        ///<summary> Desactiva el aspecto visual </summary>
        private void SetInvis()
        {
            _paintController.LosePaint();
            _renderer.enabled = false;
            _collider.enabled = false;
            _paintController.SetTrailRendererActive(false);
            _canScore.SetValue(false);
        }
    }
}
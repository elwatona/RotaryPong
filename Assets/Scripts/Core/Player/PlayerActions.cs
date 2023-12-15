using UnityEngine;
using UnityEngine.InputSystem;
using Watona.Events;
using RotaryPong.Events;
using RotaryPong.UICursor;

namespace RotaryPong
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(GamepadCursor))]
    public class PlayerActions : MonoBehaviour
    {
        private PlayerController _player;
        private PlayerMovement _movement;
        private PlayerInput _input;
        private GamepadCursor _gamepadCursor;
        private GameObject _ballGrabber;
        [SerializeField, Header("Listeners")] CodedGameEventListener<BallGrabbedParameters> _ballGrabbedListener;
        [SerializeField] CodedEventListener _afterScoreListener;
        [SerializeField, Header("Events")] private BallEffectEvent _ballEffect;
        [SerializeField] private SpinMapInputEvent _spinMap;
        [SerializeField] private PaintEvent _ballHit;
        [SerializeField] private DropBallEvent _dropBall;
        [SerializeField] private PauseEvent _pause;
        public void OnBallEfect(InputAction.CallbackContext ctx) => BallEffect(ctx.ReadValue<Vector2>());
        public void OnRotateMap(InputAction.CallbackContext ctx) => SpinMap(ctx.ReadValue<float>());
        public void OnDropBal(InputAction.CallbackContext ctx) => DropBall();
        public void OnPause(InputAction.CallbackContext ctx) => Pause(ctx);
        private void OnEnable()
        {
            _ballGrabbedListener?.OnEnable(OnBallGrabbed);
            _afterScoreListener?.OnEnable(DropBall);
        }
        private void OnDisable()
        {
            _ballGrabbedListener?.OnDisable();
            _afterScoreListener?.OnDisable();
        }
        private void Awake()
        {
            _player = GetComponent<PlayerController>();
            _ballGrabber = GetComponentInChildren<BallGrabber>(true).gameObject;
            _movement = GetComponent<PlayerMovement>();
            _input = GetComponent<PlayerInput>();
            _gamepadCursor = GetComponent<GamepadCursor>();
        }
        private void Start()
        {
            _gamepadCursor.enabled = false;
        }
        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.name == "ball") _ballHit?.Raise(_player.Team);
        }
        private void BallEffect(Vector2 input)
        {
            BallEffectParameters parameters = new BallEffectParameters {SourceInput = input, SourceTeam = _player.Team};
            
            _ballEffect.Raise(parameters);
        }
        private void SpinMap(float input)
        {
            SpinMapInputParameter parameters = new SpinMapInputParameter {SourceDirection = input, SourceTeam = _player.Team};

            _spinMap.Raise(parameters);
            print(input);
        }
        private void OnBallGrabbed(BallGrabbedParameters parameters)
        {
            Rigidbody rigidbody = _movement.Rigidbody;
            GameObject grabber = parameters.SourceGrabber;
            
            if(grabber != _ballGrabber) return;

            _movement.SetMoveBool(false);
            rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

            _ballHit?.Raise(_player.Team);
        }
        private void DropBall()
        {
            if(_player.DesiredShape != Shape.C || _movement.CanMove) return;
            
            Rigidbody rigidbody = _movement.Rigidbody;
            DropBallParameters parameters = new DropBallParameters{SourceDirection = this.transform.right, SourceGrabber = _ballGrabber};
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            _dropBall?.Raise(parameters);
            _ballHit?.Raise(_player.Team);

            _movement.SetMoveBool(true);
        }
        private void Pause(InputAction.CallbackContext ctx)
        {
            if(ctx.phase != InputActionPhase.Performed) return;
            
            PauseParameters parameters = new PauseParameters 
            {
                sourcePlayerInput = _input,
                sourceGamepadCursor = _gamepadCursor
            };

            _pause.Raise(parameters);
        }
    }
}
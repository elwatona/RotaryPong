using UnityEngine;
using UnityEngine.InputSystem;
using Watona.Variables;
using Watona.Events;
using RotaryPong.Events;

namespace RotaryPong
{
    public interface CustomShape
    {
        CustomShape DoShape(PlayerController player);
        void Movement(PlayerController player);
        void Rotation(PlayerController player);
    }
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
    #region State Machine
        [SerializeField] CustomShape _currentShape;
        public ShapeVariable DesiredShape;

        public BarShape BarShape = new BarShape();
        public CShape CShape = new CShape();
    #endregion

        [SerializeField] CodedGameEventListener<BallGrabbedParameters> _ballGrabbedListener;
        [SerializeField] CodedEventListener _afterScoreListener;

        [SerializeField] Paint _team;
        [SerializeField] GameObject[] _shapes;
        [SerializeField] GameObject _ballGrabber;
        [SerializeField, Header("Parameters")] VariableReference<float> _distanceFromCenter;
        public FloatVariable PlayerSpeed;
        public FloatVariable RotationAmmount;
        public BooleanVariable HasInterpolatedRotation;
        public FloatVariable InterpolatedRotationSpeed;
        public FloatVariable BounceAmount;
        public BooleanVariable CanBounce;

        public bool CanMove;

        private Vector3 _startingPoint;
        private float _timeOutside;

        [SerializeField, Header("Events")] BallEffectEvent _ballEffect;
        [SerializeField] SpinMapInputEvent _spinMap;
        [SerializeField] PaintEvent _ballHit;
        [SerializeField] DropBallEvent _dropBall;

        [HideInInspector] public Rigidbody Rigidbody;
        [HideInInspector] public Vector2 MovementInput;
        [HideInInspector] public bool LeftRotationInput;
        [HideInInspector] public bool RightRotationInput;

    #region Input System
        public void OnMove(InputAction.CallbackContext ctx) => MovementInput = ctx.ReadValue<Vector2>();
        public void OnRotateLeft(InputAction.CallbackContext ctx) => LeftRotationInput = ctx.ReadValueAsButton();
        public void OnRotateRight(InputAction.CallbackContext ctx) => RightRotationInput = ctx.ReadValueAsButton();
        public void OnBallEfect(InputAction.CallbackContext ctx) => BallEffect(ctx.ReadValue<Vector2>());
        public void OnRotateMap(InputAction.CallbackContext ctx) => SpinMap(ctx.ReadValue<float>());
        public void OnDropBal(InputAction.CallbackContext ctx) => DropBall();
    #endregion

        private void OnEnable()
        {
            _currentShape = GetDesiredShape();
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
            Rigidbody = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            _startingPoint = transform.position;
        }
        private void Update()
        {
            _currentShape = _currentShape.DoShape(this);
            CheckDistanceFromCenter();
        }
        private void FixedUpdate()
        {
            _currentShape.Movement(this);
            _currentShape.Rotation(this);
        }
        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.name == "ball") _ballHit?.Raise(_team);
        }
        
        private void OnBallGrabbed(BallGrabbedParameters parameters)
        {
            GameObject grabber = parameters.SourceGrabber;
            
            if(grabber != _ballGrabber)
            return;

            CanMove = false;
            Rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            _ballHit?.Raise(_team);
        }
        private void DropBall()
        {
            DropBallParameters parameters = new DropBallParameters{SourceDirection = this.transform.right, SourceGrabber = _ballGrabber};
            _dropBall?.Raise(parameters);
            Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            CanMove = true;
        }
        private void EnableBody()
        {
            foreach(GameObject body in _shapes)
            {
                body.TryGetComponent(out Renderer renderer);
                renderer.material = GetMaterial(_team.ToString());
                body.SetActive(body.name == DesiredShape.Value.ToString());
            }
            Debug.LogWarning("Player cambio su forma");
        }
        private Material GetMaterial(string materialName)
        {
            return Resources.Load<Material>(string.Format("01_Materials/{0}", materialName));
        }
        private void BallEffect(Vector2 input)
        {
            BallEffectParameters parameters = new BallEffectParameters {SourceInput = input, SourceTeam = _team};
            
            _ballEffect.Raise(parameters);
        }
        private void SpinMap(float input)
        {
            SpinMapInputParameter parameters = new SpinMapInputParameter {SourceDirection = input, SourceTeam = _team};

            _spinMap.Raise(parameters);
            print(input);
        }
        public CustomShape GetDesiredShape()
        {
            CustomShape desiredShape = null;

            switch(DesiredShape.Value)
            {
                case Shape.Bar:
                    desiredShape = BarShape;
                    break;
                case Shape.C:
                    desiredShape = CShape;
                    break;
                default:
                    Debug.LogError("No existe clase para la forma pedida");
                    break;
            }
            if(desiredShape != _currentShape)
                EnableBody();

            return desiredShape;
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
        ///<summary> Retorna la rotacion deseada dependiendo del input apretado </summary> 
        public float ChangeRotationValue(float value, float amount)
        {
            if (LeftRotationInput) return value -= amount;
            else if (RightRotationInput) return value += amount;

            return value;
        }
        ///<summary> Reinicia el valor de los inputs que permiten la rotacion </summary>
        public void ResetRotationInput()
        {
            if (LeftRotationInput)
                LeftRotationInput = false;
            if (RightRotationInput)
                RightRotationInput = false;
        }
    }
}

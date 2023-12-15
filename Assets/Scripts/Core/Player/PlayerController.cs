using UnityEngine;

namespace RotaryPong
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Paint _team;
        private PlayerMovement _movement;
    #region State Machine
        [SerializeField] private ShapeVariable _shapeVariable;
        [SerializeField] private GameObject[] _bodies;
        private State _currentState;
        public Shape DesiredShape => _shapeVariable.Value;
        private Bar _bar = new Bar();
        private C _c = new C();
    #endregion
        public Paint Team => _team;
        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
        }
        private void OnEnable()
        {
            _currentState = GetDesiredShape();
        }
        private void Update()
        {
            _currentState = _currentState.SetState(this);
        }
        private void FixedUpdate()
        {
            _currentState.Movement(_movement);
            _currentState.Rotation(_movement);
        }
        private void EnableBody()
        {
            foreach(GameObject body in _bodies)
            {
                body.TryGetComponent(out Renderer renderer);
                renderer.material = GetMaterial(_team.ToString());
                body.SetActive(body.name == DesiredShape.ToString());
            }
            Debug.LogWarning("Player cambio su forma");
        }
        private Material GetMaterial(string materialName)
        {
            return Resources.Load<Material>(string.Format("01_Materials/{0}", materialName));
        }
        public State GetDesiredShape()
        {
            State desiredShape = null;

            switch(DesiredShape)
            {
                case Shape.Bar:
                    desiredShape = _bar;
                    break;
                case Shape.C:
                    desiredShape = _c;
                    break;
                default:
                    Debug.LogError("No existe clase para la forma pedida");
                    break;
            }
            if(desiredShape != _currentState) EnableBody();
            return desiredShape;
        }
    }
}

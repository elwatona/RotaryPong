using UnityEngine;
using Watona.Variables;

namespace RotaryPong
{
    [RequireComponent(typeof(TrailRenderer), typeof(Renderer))]
    public class PaintController : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;
        private Renderer _renderer;
        private Material _startingMaterial;
        [SerializeField] BooleanVariable _canChangeWithBounces;
        [SerializeField] private Material[] _playerMaterials;
        [SerializeField] FloatVariable _colorDurationSeconds;
        [SerializeField] FloatVariable _colorDurationBounces;
        [SerializeReference] PaintVariable _paint;
        private float _colorBounces;
        private float _colorTimer;
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _trailRenderer = GetComponent<TrailRenderer>();
        }
        private void Start()
        {
            _startingMaterial = _renderer.material;
        }
        private void Update()
        {
            if (_paint.Value != Paint.White) CheckPaint();
        }
        ///<summary> Cambia el color del objeto referenciando el index <paramref name="who"/> </summary>
        public void ColorChange(Paint team)
        {
            int who = (int)team - 1;

            _colorTimer = _colorDurationSeconds.Value;
            _colorBounces = _colorDurationBounces.Value;

            _paint.SetValue(team);

            _renderer.material = _playerMaterials[who];
            _trailRenderer.material = _renderer.material;
        }
        ///<summary> Configura <paramref name="Paint"/> a White, a la vez que vuelve las propiedades visuales del objeto a su estado inciail</summary>
        public void LosePaint()
        {
            Debug.Log("lose paint");
            _paint.SetValue(Paint.White);
            _renderer.material = _startingMaterial;
            _renderer.material.SetColor("_EmissionColor", _renderer.material.color);
            _trailRenderer.material = _renderer.material;
            _colorBounces = _colorDurationBounces.Value;
        }
        ///<summary> Comprueba la pintura actual, en caso que no sea blanca cambiara su color cuando <paramref name="_colorTimer"/> llegue a 0 </summary>
        public void CheckPaint()
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
        public void Bounce()
        {
            _colorBounces --;
        }
        public void SetTrailRendererActive(bool value)
        {
            _trailRenderer.enabled = value;
        }
        public Paint GetPaint() 
        {
            return _paint.Value;
        }
    }
}

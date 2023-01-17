using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Events;
using Watona.Variables;
using RotaryPong.Events;

namespace RotaryPong
{
    public class MapHandler : MonoBehaviour
    {
        [SerializeField] CodedGameEventListener<SpinMapInputParameter> _inputListener;
        [SerializeField] CodedEventListener _spinMapListener;

        [Header("Variables")]
        [SerializeField] PaintVariable _ballPaint;
        [SerializeField] BooleanVariable _canChangeSpinDirection;
        [SerializeField] FloatVariable _mapSpinSpeed;
        [SerializeField] private GameObject _map;
        private float _mapRotationDirection;

        ///<summary> Establece la direccion de rotacion del escenario </summary>
        public void SpinMapInput(SpinMapInputParameter parameter)
        {
            print(parameter.SourceDirection);
            if(_canChangeSpinDirection.Value && _ballPaint.Value != Paint.White && parameter.SourceTeam != _ballPaint.Value && parameter.SourceDirection != 0)
                _mapRotationDirection = parameter.SourceDirection;
        }
        public void ChangeMapDirection()
        {
            _mapRotationDirection *= -1;
        }

        ///<summary> Rota el mapa, definiendo su velocidad </summary>
        private void RotateMap()
        {
            float speedRot = _mapSpinSpeed.Value * Time.deltaTime * _mapRotationDirection;
            _map.transform.Rotate(0, 0, speedRot);
        }

        private void OnEnable()
        {
            _inputListener?.OnEnable(SpinMapInput);
            _spinMapListener?.OnEnable(ChangeMapDirection);
        }
        private void OnDisable()
        {
            _inputListener?.OnDisable();
            _spinMapListener?.OnDisable();
        }
        private void Awake()
        {
            _mapRotationDirection = -1;
        }
        private void FixedUpdate()
        {
            RotateMap();
        }

    }
}

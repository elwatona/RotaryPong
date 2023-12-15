using UnityEngine;
using UnityEngine.InputSystem;
using Watona.Variables;

namespace RotaryPong
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private FloatVariable _speed;
        [SerializeField] private FloatVariable _rotationAmmount;
        [SerializeField] private BooleanVariable _hasInterpolatedRotation;
        [SerializeField] private FloatVariable _interpolatedRotationSpeed;

        public float PlayerSpeed => _speed.Value;
        public float RotationAmmount => _rotationAmmount.Value;
        public float InterpolatedRotationSpeed => _interpolatedRotationSpeed.Value;
        public bool HasInterpolatedRotation => _hasInterpolatedRotation.Value;
        public bool CanMove {get; private set;}
        public Rigidbody Rigidbody {get; private set;}
        public Vector2 MovementInput {get; private set;}
        public bool LeftRotationInput {get; private set;}
        public bool RightRotationInput {get; private set;}
        
        public void OnMove(InputAction.CallbackContext ctx) => MovementInput = ctx.ReadValue<Vector2>();
        public void OnRotateLeft(InputAction.CallbackContext ctx) => LeftRotationInput = ctx.ReadValueAsButton();
        public void OnRotateRight(InputAction.CallbackContext ctx) => RightRotationInput = ctx.ReadValueAsButton();
        public void SetMoveBool(bool value)
        {
            CanMove = value;
        }
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
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
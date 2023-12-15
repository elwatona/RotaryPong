using UnityEngine;
using UnityEngine.InputSystem;
using Watona.Variables;
using RotaryPong.Events;

namespace RotaryPong
{
    public class Bar : State
    {
        public State SetState(PlayerController player)
        {
            return player.GetDesiredShape();
        }
        public void Movement(PlayerMovement player)
        {
            Rigidbody rigidbody = player.Rigidbody;
            Vector3 direction = player.MovementInput;
            float timeSpeed = Time.deltaTime * player.PlayerSpeed;
            
            rigidbody.velocity = Vector3.zero;
            direction *= timeSpeed;
            rigidbody.position += direction;
        }
        public void Rotation(PlayerMovement player)
        {
            bool hasInterpolatedRotation = player.HasInterpolatedRotation;

            Rigidbody rigidbody = player.Rigidbody;

            float currentRotation = player.transform.eulerAngles.z;
            float rotationAmount = player.RotationAmmount;
            float rotationTimeSpeed = Time.deltaTime * player.InterpolatedRotationSpeed;
            float newRotation = player.ChangeRotationValue(currentRotation, rotationAmount);

            Quaternion fixedRotation = Quaternion.Euler(0, 0, newRotation);
            Quaternion lerpRotation = Quaternion.Lerp(rigidbody.rotation, fixedRotation, rotationTimeSpeed);

            if (!hasInterpolatedRotation) player.ResetRotationInput();

            rigidbody.rotation = hasInterpolatedRotation ? lerpRotation : fixedRotation;        
        }
    }
}

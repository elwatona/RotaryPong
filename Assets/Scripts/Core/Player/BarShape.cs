using UnityEngine;

namespace RotaryPong
{
    public class BarShape : CustomShape
    {
        public CustomShape DoShape(PlayerController player)
        {
            return player.GetDesiredShape();
        }
        public void Movement(PlayerController player)
        {
            Rigidbody rigidbody = player.Rigidbody;
            Vector3 direction = player.MovementInput;
            float timeSpeed = Time.deltaTime * player.PlayerSpeed.Value;

            rigidbody.velocity = Vector3.zero;
            direction *= timeSpeed;
            rigidbody.position += direction;
        }
        public void Rotation(PlayerController player)
        {
            bool hasInterpolatedRotation = player.HasInterpolatedRotation.Value;

            Rigidbody rigidbody = player.Rigidbody;

            float currentRotation = player.transform.eulerAngles.z;
            float rotationAmount = player.RotationAmmount.Value;
            float rotationTimeSpeed = Time.deltaTime * player.InterpolatedRotationSpeed.Value;
            float newRotation = player.ChangeRotationValue(currentRotation, rotationAmount);

            Quaternion fixedRotation = Quaternion.Euler(0, 0, newRotation);
            Quaternion lerpRotation = Quaternion.Lerp(rigidbody.rotation, fixedRotation, rotationTimeSpeed);

            if (!hasInterpolatedRotation) player.ResetRotationInput();

            rigidbody.rotation = hasInterpolatedRotation ? lerpRotation : fixedRotation;        
        }
    }
}

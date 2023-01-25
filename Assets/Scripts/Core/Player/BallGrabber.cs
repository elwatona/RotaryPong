using UnityEngine;
using Watona.Events;
using RotaryPong.Events;

namespace RotaryPong
{
    public class BallGrabber : MonoBehaviour
    {
        [SerializeField] CodedGameEventListener<DropBallParameters> _dropBallListener;
        [SerializeField] BallGrabbedEvent _ballGrabbed;
        [SerializeField] float _pickupForce = 150;
        [SerializeField] Transform _holdArea;
        private Rigidbody _heldObjectRigidbody;
        private GameObject _heldObject;

        private void OnEnable()
        {
            _dropBallListener?.OnEnable(DropObject);
        }
        private void OnDisable()
        {
            _dropBallListener?.OnDisable();
        }
        private void Update()
        {
            if(_heldObject != null)
                MoveObject();   
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.name != "ball" && _heldObject != null)
                return;

            PickupObject(other.gameObject);
        }

        private void PickupObject(GameObject pickObj)
        {
            if(pickObj.TryGetComponent(out Rigidbody rigidbody))
            {
                _heldObjectRigidbody = rigidbody;
                _heldObjectRigidbody.drag = 10;
                _heldObjectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

                _heldObjectRigidbody.transform.parent = _holdArea;
                _heldObject = pickObj;

                BallGrabbedParameters parameters = new BallGrabbedParameters{SourceGrabber = this.gameObject};
                _ballGrabbed?.Raise(parameters);
            }
        }
        private void DropObject(DropBallParameters parameters)
        {
            if(_heldObject != null && parameters.SourceGrabber == this.gameObject)
            {
                _heldObjectRigidbody.drag = 1;
                _heldObjectRigidbody.constraints = RigidbodyConstraints.None;

                _heldObject.transform.parent = null;
                _heldObject = null;
            }
        }
        private void MoveObject()
        {
            if(Vector3.Distance(_heldObject.transform.position, _holdArea.position) > 0.1f)
            {
                Vector3 moveDirection = (_holdArea.position - _heldObject.transform.position);
                _heldObjectRigidbody.AddForce(moveDirection * _pickupForce);
            }
        }
    }
}

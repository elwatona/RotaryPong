using UnityEngine;

namespace RotaryPong
{
    public class PositionController : MonoBehaviour
    {        
        [SerializeField] private float _distanceFromCenter;
        [SerializeField] private bool _useScreenLimits;
        private float _timeOutside;
        private Vector3 _startingPoint;
        private void Start()
        {
            _startingPoint = transform.position;
        }
        private void Update()
        {
            if(!_useScreenLimits) CheckDistanceFromCenter();
            else CheckPositionRelativeToScreen(0);
        }
        ///<summary> Comprueba la distancia del jugador respecto al centro del mapa para considerar su posible reinicio de posicion </summary>
        private void CheckDistanceFromCenter()
        {
            float distanceFromZero = Vector2.Distance(transform.position, Vector2.zero);
            float distanceFromCenter = _distanceFromCenter;

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
        ///<summary> Comprueba la posicion en pantalla y cambia sus valores dependiendo de la misma </summary> 
        void CheckPositionRelativeToScreen(float offset)
        {
            float maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - offset;
            float maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - offset;
            float minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + offset;
            float minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y + offset;

            Vector3 currentPosition = transform.position;
            if(currentPosition.x > maxX) currentPosition.x = minX;
            if(currentPosition.x < minX) currentPosition.x = maxX;
            if(currentPosition.y > maxY) currentPosition.y = minY;
            if(currentPosition.y < minY) currentPosition.y = maxY;
            transform.position = currentPosition;
        }
    }
}
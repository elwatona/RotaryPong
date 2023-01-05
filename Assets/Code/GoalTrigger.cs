using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RotaryPong
{
    public class GoalTrigger : MonoBehaviour
    {
        [SerializeField]
        BallPaint paint;
        [SerializeField]
        GameManager gManager;

        private void OnTriggerStay(Collider other)
        {
            CheckForGoal(other.gameObject);
        }

        void CheckForGoal(GameObject obj)
        {
            if (obj.tag == "Ball")
            {
                Ball ball = obj.GetComponent<Ball>();
                if (ball.Paint != paint && ball.Paint != BallPaint.White && ball.CanScore)
                {
                    gManager.PlayerScore(ball);
                }
            }
        }
        
    }
}

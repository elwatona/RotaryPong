using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Utils.Events;

namespace RotaryPong
{
    public class GoalTrigger : MonoBehaviour
    {
        [SerializeField] Paint _paint;

        private void OnTriggerStay(Collider other)
        {
            if(other.tag == "Ball") CheckForGoal(other.GetComponent<Ball>());
        }

        private void CheckForGoal(Ball ball)
        {
            if (ball.CanScore && ball.Paint != _paint && ball.Paint != Paint.White)
                GameManager.Instance.PlayerScore(ball);
        }
        
    }
}

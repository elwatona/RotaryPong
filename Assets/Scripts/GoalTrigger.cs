using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Events;

namespace RotaryPong
{
    public class GoalTrigger : MonoBehaviour
    {
        [SerializeField] Paint _paint;
        [SerializeField] PaintVariable _ballPaint;

        private void OnTriggerStay(Collider other)
        {
            if(other.tag == "Ball") CheckForGoal(other.GetComponent<Ball>());
        }

        private void CheckForGoal(Ball ball)
        {
            if (ball.CanScore && _ballPaint.Value != _paint && _ballPaint.Value != Paint.White)
                GameManager.Instance.PlayerScore(ball);
        }
        
    }
}

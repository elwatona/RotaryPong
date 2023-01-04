using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (ball.paint != paint && ball.paint != BallPaint.noPaint && ball.canScore)
            {
                gManager.PlayerScore(ball);
            }
        }
    }
    
}

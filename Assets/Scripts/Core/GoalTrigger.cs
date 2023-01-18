using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Variables;
using Watona.Events;
using RotaryPong.Events;

namespace RotaryPong
{
    public class GoalTrigger : MonoBehaviour
    {
        [SerializeField] Paint _paint;
        [SerializeField] PaintVariable _ballPaint;
        [SerializeField] BooleanVariable _canScore;
        [SerializeField] ScoreEvent _scoreEvent;
        [SerializeField] BooleanVariable _didGameEnded;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Ball" && !_didGameEnded.Value) CheckForGoal(other.transform.position);
        }

        private void CheckForGoal(Vector3 otherPosition)
        {
            ScoreParameter parameter = new ScoreParameter {SourcePosition = otherPosition, SourceTeam = _ballPaint.Value};
            
            if (_canScore.Value && _ballPaint.Value != _paint && _ballPaint.Value != Paint.White)
                _scoreEvent.Raise(parameter);
        }
        
    }
}

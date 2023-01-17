using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Variables;
using Watona.Events;

namespace RotaryPong
{
    public class GoalHandler : MonoBehaviour
    {
        [SerializeField, Header("Variables")] BooleanVariable _enableGodWalls;
        [SerializeField] BooleanVariable _isUpdatingPostProcess;
        [SerializeField] PaintVariable _ballPaint;
        [SerializeField] GameObject[] _goals;
        [SerializeField] GameObject[] _walls;

        ///<sumary> Decide el comportamiento de los arcos </summary>
        private void SetWalls()
        {
            bool enableGodWalls = _enableGodWalls.Value;
            EnableGoals(!enableGodWalls);
            EnableWalls(enableGodWalls);
        }

        ///<summary> Activa o desactiva los arcos, dependiendo el valor de <paramref name="value"/> </summary>
        private void EnableGoals(bool value)
        {
            foreach (GameObject goal in _goals)
            {
                goal.SetActive(value);
            }
        }
        ///<summary> Activa o desactiva las paredes de los arcos, dependiendo el valor de <paramref name="value"/> </summary>
        private void EnableWalls(bool value)
        {
            foreach (GameObject goal in _walls)
            {
                goal.SetActive(value);
            }
        }

        private void Awake()
        {
            SetWalls();    
        }
    }
}

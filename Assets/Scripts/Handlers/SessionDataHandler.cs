using UnityEngine;
using Watona.Utils;
using Watona.Variables;
using Watona.Events;

namespace RotaryPong
{
    public class SessionDataHandler : MonoBehaviour
    {
        [SerializeField] CodedEventListener _endGameListener;
        private const string FILE_NAME = "session.json";
        IntVariable _winner => _currentBluePoints.Value > _currentPinkPoints.Value ? _blueGames : _pinkGames;
    #region SOVariables
        [SerializeField, Header("Data Variables")] IntVariable _bluePoints;
        [SerializeField] IntVariable _blueGames;
        [SerializeField] IntVariable _pinkPoints;
        [SerializeField] IntVariable _pinkGames;
        [SerializeField] IntVariable _gamesPlayed;
        [SerializeField, Header("Game Variables")] IntVariable _currentBluePoints;
        [SerializeField] IntVariable _currentPinkPoints;
    #endregion
        private void Awake()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(this.gameObject.tag);
            if(objs.Length > 1) { print("Ya existia este objeto"); Destroy(this.gameObject); }
            DontDestroyOnLoad(this.gameObject);

            SessionData data = FileHandler.ReadFromJSON<SessionData>(FILE_NAME);

            if(data != default(SessionData)) LoadValues(data);
        }
        private void OnEnable()
        {
            _endGameListener.OnEnable(UpdateValues);
        }
        private void OnDisable()
        {
            _endGameListener.OnDisable();
        }
        private void OnApplicationQuit()
        {
            SaveValues();
        }
        private void UpdateValues()
        {
            _bluePoints.ApplyChangeWithoutNotify(_currentBluePoints.Value);
            _pinkPoints.ApplyChangeWithoutNotify(_currentPinkPoints.Value);

            _winner.ApplyChangeWithoutNotify(1);

            _gamesPlayed.ApplyChangeWithoutNotify(1);

            SaveValues();
        }
        private void LoadValues(SessionData session)
        {
            _bluePoints.SetValueWithoutNotify(session.bluePoints);
            _blueGames.SetValueWithoutNotify(session.blueGames);
            _pinkPoints.SetValueWithoutNotify(session.pinkPoints);
            _pinkGames.SetValueWithoutNotify(session.pinkGames);
            _gamesPlayed.SetValueWithoutNotify(session.gamesPlayed);

            Debug.Log("Session Data loaded");
        }
        private void SaveValues()
        {
            SessionData currentSession = new SessionData
            {
                bluePoints = _bluePoints.Value,
                blueGames = _blueGames.Value,
                pinkPoints = _pinkPoints.Value,
                pinkGames = _pinkGames.Value,
                gamesPlayed = _gamesPlayed.Value
            };
            FileHandler.SaveToJSON<SessionData>(currentSession, FILE_NAME);

            Debug.Log("Session Data saved");
        }
    }
}

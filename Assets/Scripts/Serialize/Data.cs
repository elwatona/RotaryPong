using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace RotaryPong
{
    [Serializable]
    public struct Session
    {
        public SessionInfo Info;
        public PongConfig PongConfig;
    }

    [Serializable]
    public struct SessionInfo
    {
        public int Score;
        public int TimePlayed;
        public DateTime DatePlayed;
    }

    [Serializable]
    public struct PongConfig
    {
        public int BallSpeed;
        public int PaddleSpeed;
        public int MaxScore;
        public bool SoundEnabled;
    }

    [Serializable]
    public class Data : ISaveable, ILoadable
    {
        public List<SessionData> Sessions;
        public List<Achievement> Achievements;

        public Data()
        {
            Sessions = new List<SessionData>();
            Achievements = new List<Achievement>();
        }

        #region CRUD operations for SessionData

        /// <summary>
        /// Adds a new session to the list of sessions.
        /// </summary>
        public void AddSession(SessionData session)
        {
            Sessions.Add(session);
        }

        /// <summary>
        /// Updates the session at the specified index with the given session.
        /// </summary>
        public void UpdateSession(int index, SessionData session)
        {
            Sessions[index] = session;
        }

        /// <summary>
        /// Deletes the session at the specified index.
        /// </summary>
        public void DeleteSession(int index)
        {
            Sessions.RemoveAt(index);
        }

        /// <summary>
        /// Returns the session at the specified index.
        /// </summary>
        public SessionData GetSession(int index)
        {
            return Sessions[index];
        }

        #endregion

        #region CRUD operations for Achievement

        /// <summary>
        /// Adds a new achievement to the list of achievements.
        /// </summary>
        public void AddAchievement(Achievement achievement)
        {
            Achievements.Add(achievement);
        }

        /// <summary>
        /// Updates the achievement at the specified index with the given achievement.
        /// </summary>
        public void UpdateAchievement(int index, Achievement achievement)
        {
            Achievements[index] = achievement;
        }

        /// <summary>
        /// Deletes the achievement at the specified index.
        /// </summary>
        public void DeleteAchievement(int index)
        {
            Achievements.RemoveAt(index);
        }

        /// <summary>
        /// Returns the achievement at the specified index.
        /// </summary>
        public Achievement GetAchievement(int index)
        {
            return Achievements[index];
        }

        #endregion

        #region ISaveable implementation

        /// <summary>
        /// Saves the data to the specified file path.
        /// </summary>
        public void SaveData(string filePath)
        {
            string jsonData = JsonUtility.ToJson(this, true);
            File.WriteAllText(filePath, jsonData);
        }

        #endregion

        #region ILoadable implementation

        /// <summary>
        /// Loads the data from the specified file path.
        /// </summary>
        public void LoadData(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(jsonData, this);
            }
            else
            {
                Debug.LogError($"File not found: {filePath}");
            }
        }

        #endregion
    }

    public interface ISaveable
    {
        void SaveData(string filePath);
    }

    public interface ILoadable
    {
        void LoadData(string filePath);
}
}

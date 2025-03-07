using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Critsoft.CozyShip
{
    [Serializable]
    public class GameResult
    {
        public int AllCollisions;
        public float TotalElapsedTime;
    }

    public class GameResultsStorage
    {
        [Serializable]
        private class ScoreboardWrapper
        {
            public List<GameResult> Results;
        }

        #region Fields

        private const string SaveFileName = "game_results.json";
        
        private readonly string _filePath;

        #endregion

        #region Public Methods

        public GameResultsStorage()
        {
            _filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
        }

        public List<GameResult> LoadResults()
        {
            if (!File.Exists(_filePath))
            {
                return new List<GameResult>();
            }

            string json = File.ReadAllText(_filePath);
            return JsonUtility.FromJson<ScoreboardWrapper>(json).Results;
        }

        public void SaveResults(List<GameResult> results)
        {
            results.Sort((a, b) => a.TotalElapsedTime.CompareTo(b.TotalElapsedTime));

            ScoreboardWrapper wrapper = new ScoreboardWrapper { Results = results };
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(_filePath, json);
        }

        public void AddResult(int collisions, float time)
        {
            List<GameResult> results = LoadResults();
            results.Add(new GameResult { AllCollisions = collisions, TotalElapsedTime = time });

            SaveResults(results);
        }

        #endregion
    }
}

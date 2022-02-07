using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDeffense
{
    public class MapCompletion : SingletonBase<MapCompletion>
    {
        public const string fileName = "completion.dat";
        //public static void ResetSavedData()
        //{
        //    FileHandler.Reset(fileName);
        //}
        [Serializable]
        public class EpisodeScore
        {
            public Episode episode;
            public int score;
        }

        public static void SaveEpisodeResult(int levelScore)
        {
            if (Instance)
            {
                Instance.SaveResult(LevelSequenceController.Instance.CurrentEpisode, levelScore);
                Debug.Log($"Episode complete with score: {levelScore}");
            }
            else
            {
                Debug.Log($"Episode complete with score: {levelScore}");
            }
        }

        [SerializeField] private EpisodeScore[] completionData;
        public bool TryIndex(int id, out Episode episode, out int score)
        {
            if(id >= 0 && id < completionData.Length)
            {
                episode = completionData[id].episode;
                score = completionData[id].score;
                return true;
            }
            episode = null;
            score = 0;
            return false;
        }
        private new void Awake()
        {
            base.Awake();
            Saver<EpisodeScore[]>.TryLoad(fileName, ref completionData);
        }
        private void SaveResult(Episode currentEpisode,  int levelScore)
        {
            foreach( var item in completionData)
            {
                if(item.episode == currentEpisode)
                {
                    if (item.score < levelScore)
                    {
                        item.score = levelScore;
                        Saver<EpisodeScore[]>.Save(fileName,completionData);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDeffense
{
    public class MapCompletion : SingletonBase<MapCompletion>
    {
        [Serializable]
        public class EpisodeScore
        {
            public Episode episode;
            public int score;
        }

        public static void SaveEpisodeResult(int levelScore)
        {
            Instance.SaveResult(LevelSequenceController.Instance.CurrentEpisode, levelScore);
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
        private void SaveResult(Episode currentEpisode,  int levelScore)
        {
            foreach( var item in completionData)
            {
                if(item.episode == currentEpisode)
                {
                    if (item.score < levelScore)
                    {
                        item.score = levelScore;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffense
{
    public class LevelDisplayController : MonoBehaviour
    {
        [SerializeField] private MapLevel[] levels;
        [SerializeField] private MapLevel[] branchLevels;
        void Start()
        {
            var drawLevel = 0;
            var score = 1;
            while( score != 0 && drawLevel < levels.Length && 
                MapCompletion.Instance.TryIndex(drawLevel,out var episode,out score))
            {
                levels[drawLevel].SetLevelData(episode, score);
                drawLevel += 1;
                if(score == 0)
                {
                    break;
                }
            }
            for(int i = drawLevel; i < levels.Length; i++)
            {
                levels[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < branchLevels.Length; i++)
            {
                branchLevels[i].gameObject.SetActive(false);
            }
        }
    }
}
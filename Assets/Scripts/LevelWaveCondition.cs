using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDeffense
{
    public class LevelWaveCondition : MonoBehaviour, ILevelCondition
    {
        private bool isCompleted;
        private void Start()
        {
            FindObjectOfType<EnemyWavesManager>().OnAllWavesDead += () =>
            {
                isCompleted = true;
            };
        }
        public bool IsCompleted
        {
            get { return isCompleted; } 
        }
    }
}
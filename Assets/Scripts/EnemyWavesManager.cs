using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffense
{
    public class EnemyWavesManager : MonoBehaviour
    {
        [SerializeField] private Enemy m_EnemyPrefabs;
        [SerializeField] private Path[] paths;
        [SerializeField] private EnemyWave currentWaves;
        [SerializeField] private int activeEnemyCount = 0;
        public event Action OnAllWavesDead;
        private void RecordEnemyDead()
        {
            if(--activeEnemyCount == 0)
            {
                if(currentWaves)
                {
                    ForceNextWave();
                }
            }
        }
        private void Start()
        {
            currentWaves.Prepare(SpawnEnemies);
        }

        public void ForceNextWave()
        {
            if (currentWaves)
            {
                TDPlayer.Instance.ChangeGold((int)currentWaves.GetRemainingTime());
                SpawnEnemies();
            }
            else
            {
                if (activeEnemyCount == 0)
                {
                    OnAllWavesDead?.Invoke();
                }
            }
        }
        
        private void SpawnEnemies()
        {
            foreach ((EnemyAsset asset, int count, int pathIndex) in currentWaves.EnumerateSquads())
            {
                if (pathIndex < paths.Length)
                {
                 
                    for (int i = 0; i < count; i++)
                    {
                        var e = Instantiate(m_EnemyPrefabs, 
                            paths[pathIndex].StartArea.GetRandomInsideZone(), Quaternion.identity);
                        //событие когда заканчивается enemy
                        e.OnEnd += RecordEnemyDead;

                        e.Use(asset);
                        e.GetComponent<TDPatrolController>().SetPath(paths[pathIndex]);
                        activeEnemyCount += 1;
                        
                    }
                }
                else
                {
                    Debug.LogWarning($"invalid pathIndex in {name}");
                }
            }

            currentWaves = currentWaves.PrepareNext(SpawnEnemies);
        }
    }
}
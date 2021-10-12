using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDeffense;

namespace SpaceShooter
{
    public class EnemySpawner : Spawner
    {
        [SerializeField] private Enemy m_EnemyPrefabs;

        [SerializeField] private Path m_Path;

        [SerializeField] private EnemyAsset[] m_EnemyAsset;

        //protected override GameObject spawnedEntity => throw new System.NotImplementedException();

        protected override GameObject GenerateSpawnedEntity()
        {
            var e = Instantiate(m_EnemyPrefabs);
            e.Use(m_EnemyAsset[Random.Range(0, m_EnemyAsset.Length)]);
            e.GetComponent<TDPatrolController>().SetPath(m_Path);
            return e.gameObject;
        }
    }
}
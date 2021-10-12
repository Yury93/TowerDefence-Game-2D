using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDeffense;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        [SerializeField] private int m_NumLives;
        public int NumLives => m_NumLives;

        [SerializeField] private SpaceShip m_Ship;
        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private GameObject m_PlayerShipPrefab;

        protected override void Awake()
        {
            base.Awake();

            if (m_Ship != null)
            {
                Destroy(m_Ship.gameObject);
            }
        }

        protected void TakeDamage(int m_Damage)
        {
            m_NumLives -= m_Damage;
            if(m_NumLives<0)
            {
                //LevelSequenceController.Instance.FinishCurrentLevel(false);
                LevelSequenceController.Instance.RestartLevel();
            }
        }

        #region Score
        public int Score { get; private set; }

        public int NumKills { get; private set; }

        public void AddKiil(int num)
        {
            NumKills += num;
        }
        public void AddScore(int num)
        {
            Score += num;
        }
        #endregion
    }
}

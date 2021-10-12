using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDeffense
{

    public class TDPlayer : Player
    {
        public static new TDPlayer Instance
        {
            get
            {
                return Player.Instance as TDPlayer;
            }
        }

        public static event Action<int> OnGoldUpdate;
        public static void GoldUpdateSuscribe(Action<int> act)
        {
            OnGoldUpdate += act;
            act(Instance.m_Gold);
        }
        public static event Action<int> OnLifeUpdate;
        public static void LifeUpdateSuscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            act(Instance.NumLives);
        }

        [SerializeField] private int m_Gold;
        [SerializeField] private Tower m_towerPrefab;


       
        public void ChangeGold(int change)
        {
            m_Gold += change;
            OnGoldUpdate(m_Gold);
        }
        public void ReduceLife(int change)
        {
            TakeDamage(change);
            OnLifeUpdate(NumLives);
        }

        public void TryBuild(TowerAsset towerAsset, Transform m_BuildSite)
        {
            ChangeGold(-towerAsset.m_GoldCost);
            var tower = Instantiate(m_towerPrefab, m_BuildSite.position, Quaternion.identity);
            tower.GetComponentInChildren <SpriteRenderer>().sprite = towerAsset.m_Sprite;
            tower.GetComponentInChildren<Turret>().CreateOtherProjectile(towerAsset.turretProperties);

            Destroy(m_BuildSite.gameObject);
        }
    }
}
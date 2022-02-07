using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDeffense
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int m_Damage = 1;
        [SerializeField] private int m_Gold = 1;

        public event Action OnEnd;
        private void OnDestroy()
        {
            OnEnd?.Invoke();
        }

        public void Use(EnemyAsset asset)
        {
            var sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            sr.color = asset.color;
            sr.transform.localScale = new Vector3(asset.scaleSprite.x, asset.scaleSprite.y, 1);
            sr.GetComponent<Animator>().runtimeAnimatorController = asset.animations;
            GetComponent<SpaceShip>().Use(asset);
            m_Damage = asset.damage;
            m_Gold = asset.gold;
        }
        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(m_Damage);
        }
        public void GivePlayerGold()
        {
            TDPlayer.Instance.ChangeGold(m_Gold);
        }
    }
}
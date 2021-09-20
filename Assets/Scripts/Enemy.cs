using UnityEngine;
using SpaceShooter;

namespace TowerDeffense
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int m_Damage = 1;
        public void Use(EnemyAsset asset)
        {
            var sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            sr.color = asset.color;
            sr.transform.localScale = new Vector3(asset.scaleSprite.x, asset.scaleSprite.y, 1);
            sr.GetComponent<Animator>().runtimeAnimatorController = asset.animations;
            GetComponent<SpaceShip>().Use(asset);
            m_Damage = asset.damage;
        }
        public void DamagePlayer()
        {
            Player.Instance.TakeDamage(m_Damage);
        }
    }
}
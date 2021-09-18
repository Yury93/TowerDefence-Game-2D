using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Projectile : Entity
    {
        [SerializeField] private float m_Velocity;

        [SerializeField] private float m_VelocityAngle;

        [SerializeField] private float m_LifeTime;

        [SerializeField] private int m_Damage;

        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

        [SerializeField] private float m_DistanceShoot;

        [SerializeField] private float m_RadiusBoom;

        [SerializeField] private GameObject m_Partical;

        [SerializeField] private float m_ParticalLife;

        private GameObject m_Effect;

        private GameObject m_Enemy;//Поле для поиска по тегу

        private float m_Timer;

        private void Update()
        {

            float stepLenght = Time.deltaTime * m_Velocity;

            Vector2 step = transform.up * stepLenght;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLenght);
            if (hit)
            {
                var dest = hit.collider.transform.root.GetComponent<Destructible>();
                //Если снаряд не принадлежит нашему кораблю
                if (dest != null && dest != m_Parent)
                {
                    if (m_RadiusBoom > 0 && m_Velocity >= 1)
                    {
                        RadiusDamage();
                    }

                    dest.ApplyDamage(m_Damage);
                    Destroy(gameObject);
                    //OnProjectileLifeEnd(hit.collider, hit.point);
                    if (m_Partical == null) return;
                    m_Effect = Instantiate(m_Partical, transform.position, Quaternion.identity);

                    if (m_Parent == Player.Instance.ActiveShip)
                    {
                        Player.Instance.AddScore(dest.ScoreValue);

                        Player.Instance.AddKiil(dest.KillValue);
                    }
                }
                if (m_Effect != null)
                {
                    Destroy(m_Effect, m_ParticalLife);
                }
            }

            m_Timer += Time.deltaTime;

            if (m_Timer > m_LifeTime)
            {
                RadiusDamage();

                Destroy(gameObject);
            }

            transform.position += new Vector3(step.x, step.y, 0);

            //самонаводящиеся снаряды
            if (m_Velocity >= 1)
            {
                m_Enemy = GameObject.FindGameObjectWithTag("Enemy");
                if (m_Enemy != null)
                {

                    if (m_Enemy.tag == "Enemy")
                    {

                        AutoMissle();
                    }
                }
            }
        }
        private void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            print(gameObject);
            Destroy(gameObject);
        }

        /// <summary>
        /// Поле для нашего корабля
        /// </summary>
        private Destructible m_Parent;

        public void SetParentShooter(Destructible parent)
        {
            m_Parent = parent;
        }

        /// <summary>
        /// Cамонаводящиеся снаряды
        /// </summary>
        private void AutoMissle()
        {
            float dist = Vector2.Distance(transform.position, m_Enemy.transform.position);

            Vector3 direction = m_Enemy.transform.position - transform.position; //направление не нормализованное 

            float maxAngle = m_VelocityAngle * Time.deltaTime;

            float angle = Vector3.Angle(transform.up, direction);

            if (angle <= maxAngle)
            {

                transform.up = direction.normalized;
            }
            else
            {

                transform.up = Vector3.Slerp(transform.up, direction, maxAngle / angle);
            }
        }
        /// <summary>
        /// Подрыв врагов по радиусу
        /// </summary>
        private void RadiusDamage()
        {
            List<GameObject> Enemyes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
            for (int i = 0; i < Enemyes.Count; i++)
            {
                float dist = Vector3.Distance(transform.position, Enemyes[i].transform.position);
                if (dist <= m_RadiusBoom)
                {
                    Destructible des = Enemyes[i].GetComponent<Destructible>();
                    des.ApplyDamage(m_Damage);
                }
            }
        }
    }
}

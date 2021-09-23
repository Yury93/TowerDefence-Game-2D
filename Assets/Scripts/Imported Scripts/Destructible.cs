using System;
using System.Collections;
using System.Collections.Generic;
using TowerDeffense;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    /// <summary>
    /// Уничтоженный объект на сцене. То что может иметь хитпоинты.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties
        /// <summary>
        /// Объект игнорирует повреждения.
        /// </summary>
        [SerializeField]
        private bool m_Indestructible;
        public bool Indestructible => m_Indestructible;

        /// <summary>
        /// Стартовое Количество хитпоинтов.
        /// </summary>
        [SerializeField] private int m_HitPoints;
        /// <summary>
        /// Текущие хитпоинты.
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        [SerializeField] private GameObject m_Partical;

        [SerializeField] private float m_ParticalLife;


        #endregion

        #region Unity Events    
        protected virtual void Start()
        {

            m_CurrentHitPoints = m_HitPoints;

            //print(m_CurrentHitPoints);
        }
        #endregion

        #region  Public API

        /// <summary>
        /// 
        /// </summary>Приминение дамага к объекту
        /// <param name="damage">Урон наносимый объекту</param>
        public void ApplyDamage(int damage)
        {

            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            m_HitPoints = m_CurrentHitPoints;
            if (m_CurrentHitPoints <= 0)
            {
                OnDeath();

                if (m_Partical == null) return;
                GameObject m_Effect = Instantiate(m_Partical, transform.position, Quaternion.identity);
                if (m_Effect != null)
                {
                    Destroy(m_Effect, m_ParticalLife);
                }

            }

        }
        #endregion

        /// <summary>
        /// Уничтожение объекта когда хитпоинты ниже нуля
        /// </summary>
        protected virtual void OnDeath()
        {
            Destroy(gameObject);
            m_EventOnDeath?.Invoke();//Вызов события
        }
        /// <summary>
        /// списки(рид онли список,который можно прочитать но не задать) рид онли ссылается на хэш
        ///  "Static" означает, что данное поле, метод или свойство будет принадлежать не каждому объекту класса, а всем им вместе
        /// </summary>
        private static HashSet<Destructible> m_AllDestructibles;
        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
            {
                m_AllDestructibles = new HashSet<Destructible>();
            }
            m_AllDestructibles.Add(this);
        }
        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }
        /// <summary>
        /// Создание команд(ID Команды)
        /// </summary>
        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;
        /// <summary>
        /// событие
        /// </summary>
        [SerializeField] private UnityEvent m_EventOnDeath;//Ссылка на событие
        public UnityEvent EventOnDeath => m_EventOnDeath;


        ////  public IEnumerator Explosion ()
        //  {
        // /     yield return new WaitForSeconds(0.65f);
        //      Destroy(gameObject);

        //  }

        public void IndestructibleShipPlayer(bool destrOrNoDestr)
        {
            m_Indestructible = destrOrNoDestr;
        }


        #region Score

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        [SerializeField] private int m_KillValue;
        public int KillValue => m_KillValue;
        #endregion
        protected void Use(EnemyAsset asset)
        {
            m_HitPoints = asset.hp;
            m_ScoreValue = asset.score;
        }
    }

}



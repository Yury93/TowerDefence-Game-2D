using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{

    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float m_LifeTime;

        [SerializeField] private GameObject m_Effect;

        private float m_Timer;

        private GameObject m_Partical;
        private void Update()
        {

            if (m_Timer < m_LifeTime)
            {
                m_Timer += Time.deltaTime;
            }
            else
            {
                if (m_Effect != null)
                {
                    m_Partical = Instantiate(m_Effect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);


            }
            if (m_Partical != null)
            {
                //  Debug.Log("GameObj  не  null");
                Destroy(m_Partical, 1f);


            }
            //if(m_Effect != null)
            //{
            //    StartCoroutine(ParticalDestr());
            //}

        }
    }
}

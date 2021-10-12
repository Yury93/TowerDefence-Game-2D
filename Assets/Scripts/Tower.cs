using UnityEngine;
using SpaceShooter;


namespace TowerDeffense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius = 5f;
        private Turret[] turrets;
        private Destructible m_Target = null;

        private void Start()
        {
            turrets = GetComponentsInChildren<Turret>();
        }
        private void Update()
        {
            if (m_Target && !m_Target.GetComponentInParent<Hero>())
            {
                Vector2 targetVector = m_Target.transform.position - transform.position;
                if (targetVector.magnitude <= m_Radius)
                {

                    for (int i = 0; i < turrets.Length; i++)
                    {
                        turrets[i].transform.up = targetVector;
                        turrets[i].Fire();
                    }
                }
                else
                {
                    m_Target = null;
                }

            }
            else
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);

                if (enter)
                {
                    m_Target = enter.transform.root.GetComponent<Destructible>();

                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }
    }
}
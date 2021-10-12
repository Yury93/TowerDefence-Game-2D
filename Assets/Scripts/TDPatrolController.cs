using UnityEngine;
using UnityEngine.Events;
using SpaceShooter;


namespace TowerDeffense
{
    public class TDPatrolController : AIController
    {
        private Path m_Path;
        private int m_PathIndex;
        [SerializeField] private UnityEvent OnEndPath;
        internal void SetPath(Path newPath)
        {
            m_Path = newPath;
            m_PathIndex = 0;

            SetPatrolBehaviour(m_Path[m_PathIndex]);
        }
        protected override void GetNewPoint()
        {
            if (gameObject.GetComponent<Hero>())
            {
                gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.GetComponent<TDPatrolController>().SpeedShip(0);
                gameObject.GetComponentInChildren<Animator>().SetBool("point", true);
            }
            if (!gameObject.GetComponent<Hero>())
            {
                m_PathIndex += 1;
                if (m_Path.Length > m_PathIndex)
                {
                    SetPatrolBehaviour(m_Path[m_PathIndex]);
                }
                else
                {
                    OnEndPath.Invoke();
                    Destroy(gameObject);
                }
            }
        }
    }
}
using UnityEngine;
using SpaceShooter;


namespace TowerDeffense
{
    public class TDPatrolController : AIController
    {
        private Path m_Path;
        private int m_PathIndex;
        internal void SetPath(Path newPath)
        {
            m_Path = newPath;
            m_PathIndex = 0;

            SetPatrolBehaviour(m_Path[m_PathIndex]);
        }
        protected override void GetNewPoint()
        {
            m_PathIndex += 1;
            if (m_Path.Length > m_PathIndex)
            {
                SetPatrolBehaviour(m_Path[m_PathIndex]);
                print("»дЄм по точкам");
            }
            else
            {
                Destroy(gameObject);
                print("¬раг дошЄл до последней точки");
            }
        }
    }
}
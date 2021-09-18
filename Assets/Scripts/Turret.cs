using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Turret : MonoBehaviour
    {

        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;


        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;

        #region UnityEvent
        void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>(); // - transform.root - SpaceShip  на главном объкте
        }


        void Update()
        {
            if (m_RefireTimer > 0)
            {
                m_RefireTimer -= Time.deltaTime;
            }
            else if (m_Mode == TurretMode.AutoAmmo)
            {
                Fire();
            }
        }
        #endregion
        //Public API 
        public void Fire()
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;
            if (m_Ship != null)
            {
                if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false)
                {
                    return;
                }
                if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)
                {
                    return;
                }
            }
            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();

            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;
            projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOfFire;

        }
        public void AssignLoadOut(TurretProperties props)//Power up for turrel.
        {
            if (m_Mode != props.Mode) return;

            m_RefireTimer = 0;
            m_TurretProperties = props;
        }

    }
}

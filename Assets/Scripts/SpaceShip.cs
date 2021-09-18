using System;
using System.Collections;
using System.Collections.Generic;
using TowerDeffense;
using Unity.Profiling;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// ����������� ������� 2�.
    /// NOTE: ����� ������ ����������� ��� � ��������� ����� ���������� ������ �� ����� �������.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        /// <summary>
        /// ����� ��� �������������� ��������� � ������.
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;



        /// <summary>
        /// ��������� ������ ����.
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// ��������� ����.
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// ������������ �������� ��������.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;

        /// <summary>
        /// ������������ ������������ ��������. � ��������/���
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;

        /// <summary>
        /// ����������� ������ �� �����.
        /// </summary>
        private Rigidbody2D m_Rigid;
        public Rigidbody2D Rigid => m_Rigid;

        #region Public API

        /// <summary>
        /// ���������� �������� �����. -1.0 �� +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// ���������� ������������ �����. -1.0 �� +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        #region Unity events

        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            // ��������� ������� ��� ���� ����� ��������� ������ ��������.
            // ���� ������������� ���������� ����� ������� ��������
            // �������� ����������� ����� �� ������ �������
            m_Rigid.inertia = 1;

            //InitOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidbody();
            //UpdateEnergyRegen();
        }

        #endregion

        /// <summary>
        /// ����� ���������� ��� ������� ��� ��������.
        /// </summary>
        private void UpdateRigidbody()
        {
            // ���������� ��������� ����
            m_Rigid.AddForce(m_Thrust * ThrustControl * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            // �������� ������ ������ -V * C
            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            // ��������� ��������
            m_Rigid.AddTorque(m_Mobility * TorqueControl * Time.fixedDeltaTime, ForceMode2D.Force);

            // ������ ������������ ������
            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        #region Offensive

        private const int StartingAmmoCount = 10;

        /// <summary>
        /// ������ �� ������ �������. ������ ������ ���������� �������������.
        /// ������ ������ ��� ������� � �������.
        /// </summary>
        [SerializeField] private Turret[] m_Turrets;

        /// <summary>
        /// �������� ������� �� �������.
        /// </summary>
        [SerializeField] private int m_MaxEnergy;

        /// <summary>
        /// �������� �������� �� �������.
        /// </summary>
        [SerializeField] private int m_MaxAmmo;

        /// <summary>
        /// �������� ����������� ������� � �������.
        /// </summary>
        [SerializeField] private int m_EnergyRegenPerSecond;

        /// <summary>
        /// ���-�� ������� �� �������. float ���� ��� ����� � �������� ����� � �������.
        /// </summary>
        private float m_PrimaryEnergy;

        //public void AddEnergy(int e)
        //{
        //    m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        //}

        ///// <summary>
        ///// ���-�� ��������.
        ///// </summary>
        //private int m_SecondaryAmmo;

        //public void AddAmmo(int ammo)
        //{
        //    m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        //}

        /// <summary>
        /// ������������� ��������� ������� �������.
        /// </summary>
        //private void InitOffensive()
        //{
        //    m_PrimaryEnergy = m_MaxEnergy;
        //    m_SecondaryAmmo = StartingAmmoCount;
        //}

        ///// <summary>
        ///// ��������� ����� �������. ����� �������� �������� �����������.
        ///// </summary>
        //private void UpdateEnergyRegen()
        //{
        //    m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
        //}

        /// <summary>
        /// ����� ��������� �������� �� ��������� �������. ������������ ��������.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true ���� ������� ���� �������</returns>
        public bool DrawAmmo(int count)
        {
            return true;
        }

        /// <summary>
        /// ����� ��������� �������� �� ��������� �������. ������������ ��������.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true ���� ������� ���� �������</returns>
        public bool DrawEnergy(int count)
        {
            return true;
        }

        /// <summary>
        /// �������� ��������� ��� ���������.
        /// </summary>
        /// <param name="mode"></param>
        public void Fire(TurretMode mode)
        {
            foreach (var v in m_Turrets)
            {
                //if (v.Mode == mode)
                //    v.Fire();
                return;
            }
        }

        #endregion

        public void AssignWeapon(TurretProperties props)
        {
            foreach (var v in m_Turrets)
                v.AssignLoadOut(props);
        }
        public new void Use(EnemyAsset asset)
        {
            m_MaxLinearVelocity = asset.moveSpeed;
            base.Use(asset);//������� �������� ��  ENEMY � SpaceShip �  �� ����� �������� ��� � base, �� ��� ����� ��� ���� ������� �����  � destructible
        }
    }
}
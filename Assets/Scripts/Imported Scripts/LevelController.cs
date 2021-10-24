using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TowerDeffense;

namespace SpaceShooter
{

    /// <summary>
    /// ��������� ������� ����������� ������.
    /// </summary>
    public interface ILevelCondition
    {
        /// <summary>
        /// True ���� ������� ���������.
        /// </summary>
        bool IsCompleted { get; }
    }

    /// <summary>
    /// ���������� ����������� ������.
    /// ���������� ������ ���������� ������ ����������� �������� �������.
    /// �� ���������� ������� ������� ������ ����� �����������,
    /// ������� ����� ������������� ����������.
    /// 
    /// ���� ������� 0 �� ������� ����� ������ �����.
    /// ���������� ������ �������� ����������, �� �������� ����� m_DoNotDestroyOnLoad ������.
    /// �.�. �� ������ �������� ��� ����� ������.
    /// </summary>
    public class LevelController : SingletonBase<LevelController>
    {
        /// <summary>
        /// ����� ����������� � �������� �� ������� ����� ����������� ����.
        /// </summary>
        [SerializeField] private int m_ReferenceTime;
        public int ReferenceTime => m_ReferenceTime;

        /// <summary>
        /// ������� ������� ����� ������� ����� ������� ����� ��������. ���������� ���� ���.
        /// </summary>
        [SerializeField] protected UnityEvent m_EventLevelCompleted;

        /// <summary>
        /// ������ ������� ��� ��������� ����������� ������.
        /// </summary>
        private ILevelCondition[] m_Conditions;

        private bool m_IsLevelCompleted; // ���� ������� ������� ����������� ���� ���.

        private float m_LevelTime; // ������� ����� ����������� ������.
        public float LevelTime => m_LevelTime;

        #region Unity events

        protected void Start()
        {
            m_Conditions = GetComponentsInChildren<ILevelCondition>();
        }

        private void Update()
        {
            if (!m_IsLevelCompleted)
            {
                m_LevelTime += Time.deltaTime;

                CheckLevelConditions();
            }
        }

        #endregion

        /// <summary>
        /// ����� �������� ������� �����������.
        /// </summary>
        private void CheckLevelConditions()
        {
            if (m_Conditions == null || m_Conditions.Length == 0)
                return;

            int numCompleted = 0;

            foreach (var v in m_Conditions)
            {
                if (v.IsCompleted)
                    numCompleted++;
            }

            if (numCompleted == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;
                m_EventLevelCompleted?.Invoke();

                // Notify level sequence Unit3 code
                LevelSequenceController.Instance?.FinishCurrentLevel(true);
            }
        }
    }
}
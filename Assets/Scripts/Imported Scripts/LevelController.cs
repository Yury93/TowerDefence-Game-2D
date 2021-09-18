using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    public interface ILevelCondition
    {
        bool IsCompleted { get; }
    }
    public class LevelController : SingletonBase<LevelController>
    {
        [SerializeField] private int m_ReferensTime;
        public int ReferensTime => m_ReferensTime;

        [SerializeField] private UnityEvent m_EventLevelCompleted;

        private ILevelCondition[] m_Conditions;

        private bool m_IsLevelCompleted;
        private float m_LevelTime;
        public float LevelTime => m_LevelTime;


        private void Start()
        {
            m_Conditions = GetComponentsInChildren<ILevelCondition>();
        }
        private void Update()
        {
            if (m_IsLevelCompleted == false)
            {
                m_LevelTime += Time.deltaTime;

                CheckLevelCondition();
            }
        }

        private void CheckLevelCondition()
        {
            if (m_Conditions == null || m_Conditions.Length == 0)
            {
                return;
            }

            int numComplited = 0;

            foreach (var v in m_Conditions)
            {
                if (v.IsCompleted)
                {
                    numComplited++;

                }
                if (numComplited == m_Conditions.Length)
                {
                    m_IsLevelCompleted = true;
                    m_EventLevelCompleted?.Invoke();

                    LevelSequenceController.Instance?.FinishCurrentLevel(true);
                }
            }
        }
    }
}
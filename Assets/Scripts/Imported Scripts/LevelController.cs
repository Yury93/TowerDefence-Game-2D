using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SpaceShooter;

namespace TowerDeffense
{

    /// <summary>
    /// »нтерфейс услови€ прохождени€ уровн€.
    /// </summary>
    public interface ILevelCondition
    {
        /// <summary>
        /// True если условие выполнено.
        /// </summary>
        bool IsCompleted { get; }
    }

    /// <summary>
    ///  онтроллер прохождени€ уровн€.
    /// ќпредел€ет логику завершени€ уровн€ посредством проверки условий.
    /// ћы накидываем скрипты условий внутрь левел контроллера,
    /// которые потом автоматически подцеп€тс€.
    /// 
    /// ≈сли условий 0 то уровень будет играть вечно.
    ///  онтроллер уровн€ €вл€етс€ синглтоном, но включать опцию m_DoNotDestroyOnLoad нельз€.
    /// т.к. он должен удалитс€ при смене уровн€.
    /// </summary>
    public class LevelController : SingletonBase<LevelController>
    {
        /// <summary>
        /// ¬рем€ прохождени€ в секундах за которое будут начисл€тьс€ очки.
        /// </summary>
        [SerializeField] protected float m_ReferenceTime;
        public float ReferenceTime => m_ReferenceTime;

        /// <summary>
        /// —обытие которое будет вызвано когда уровень будет выполнен. ¬ызываетс€ один раз.
        /// </summary>
        [SerializeField] protected UnityEvent m_EventLevelCompleted;

        /// <summary>
        /// ћассив условий дл€ успешного прохождени€ уровн€.
        /// </summary>
        private ILevelCondition[] m_Conditions;

        private bool m_IsLevelCompleted; // флаг отсылки событи€ прохождени€ один раз.

        private float m_LevelTime; // текущее врем€ прохождени€ уровн€.
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
        /// ћетод проверки условий прохождени€.
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

                // Notify level sequence Unit3 code
                LevelSequenceController.Instance?.FinishCurrentLevel(true);
                m_EventLevelCompleted?.Invoke();
            }
        }
    }
}
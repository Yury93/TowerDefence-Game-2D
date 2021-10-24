using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TowerDeffense;

namespace SpaceShooter
{
    /// <summary>
    /// ������ ����������� ������. ������ ������ � ������ ������ ��� ������� DoNotDestroyOnLoad.
    /// </summary>
    public class LevelResultController : SingletonBase<LevelResultController>
    {

        [SerializeField] private GameObject m_PanelSuccess;
        [SerializeField] private GameObject m_PanelFailure;

        [SerializeField] private Text m_LevelTime;
        [SerializeField] private Text m_TotalPlayTime;
        [SerializeField] private Text m_TotalScore;
        [SerializeField] private Text m_TotalKills;

        /// <summary>
        /// ���������� ������ �����������. ���������� ������ �������� � ����������� �� ������.
        /// </summary>
        /// <param name="result"></param>
        public void Show(bool result)
        {
            //if (result)
            //{
            //    UpdateCurrentLevelStats();
            //    UpdateVisualStats();
            //}

            m_PanelSuccess?.gameObject.SetActive(result);
            m_PanelFailure?.gameObject.SetActive(!result);
        }

        /// <summary>
        /// ��������� ��������� ������. ��������� ������� � ������ play next.
        /// </summary>
        public void OnPlayNext()
        {
            LevelSequenceController.Instance.AdvanceLevel();
        }

        /// <summary>
        /// ������� ������. ��������� ������� � ������ restart � ������ ����� ������.
        /// </summary>
        public void OnRestartLevel()
        {
            LevelSequenceController.Instance.RestartLevel();
        }


        public class Stats
        {
            public int numKills;
            public float time;
            public int score;
        }

        /// <summary>
        /// ����� ���������� �� ������.
        /// </summary>
        public static Stats TotalStats { get; private set; }

        /// <summary>
        /// ����� ����� ���������� �� ������. ���������� ����� ������� �������.
        /// </summary>
        public static void ResetPlayerStats()
        {
            TotalStats = new Stats();
        }

        /// <summary>
        /// �������� ���������� �� �������� ������.
        /// </summary>
        /// <returns></returns>
        private void UpdateCurrentLevelStats()
        {
            TotalStats.numKills += Player.Instance.NumKills;
            TotalStats.time += LevelController.Instance.LevelTime;
            TotalStats.score += Player.Instance.Score;

            // ����� �� ����� �����������.
            int timeBonus = LevelController.Instance.ReferenceTime - (int)LevelController.Instance.LevelTime;

            if (timeBonus > 0)
                TotalStats.score += timeBonus;
        }

        /// <summary>
        /// ����� ���������� ������ ������.
        /// </summary>
        private void UpdateVisualStats()
        {
            m_LevelTime.text = System.Convert.ToInt32(LevelController.Instance.LevelTime).ToString();
            m_TotalScore.text = TotalStats.score.ToString();
            m_TotalPlayTime.text = System.Convert.ToInt32(TotalStats.time).ToString();
            m_TotalKills.text = TotalStats.numKills.ToString();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TowerDeffense;

namespace SpaceShooter
{
    /// <summary>
    /// ���������� ��������� ����� ��������. ������ ���� � �������� DoNotDetroyOnLoad
    /// � ������ � ����� � ������� ����. LevelController ������ ���������� ������.
    /// </summary>
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {
        public static string MainMenuSceneNickname = "LevelMap";

        /// <summary>
        /// ������� ������. ������������ ������������ ������ ������� ����� ������� ����.
        /// </summary>
        public Episode CurrentEpisode { get; private set; }

        /// <summary>
        /// ������� ������� �������. ������ ������������ �������� ������������� �������.
        /// </summary>
        public int CurrentLevel { get; private set; }

        /// <summary>
        /// ����� ������� ������� ������ �������.
        /// </summary>
        /// <param name="e"></param>
        public void StartEpisode(Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            // ���������� ����� ����� ������� �������.
            LevelResultController.ResetPlayerStats();

            // ��������� ������ ������� �������.
            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }

        /// <summary>
        /// �������������� ������� ������.
        /// </summary>
        public void RestartLevel()
        {
            //SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            SceneManager.LoadScene(1);
        }

        /// <summary>
        /// ��������� �������. � ����������� �� ���������� ����� �������� ������ �����������.
        /// </summary>
        /// <param name="success">���������� ��� ���������</param>
        public void FinishCurrentLevel(bool success)
        {
            // ����� ����������� ���������
            LevelResultController.Instance.Show(success);
        }

        /// <summary>
        /// ��������� ��������� ������� ��� ������� � ������� ���� ���� ������ ������� ����.
        /// </summary>
        public void AdvanceLevel()
        {
            CurrentLevel++;

            // ����� ������� ������������ � ������� ����.
            if (CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MainMenuSceneNickname);
            }
            else
            {
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            }
        }

        #region Ship select

        /// <summary>
        /// ��������� ������� ������� ��� �����������.
        /// </summary>
        public static SpaceShip PlayerShipPrefab { get; set; }

        #endregion
    }
}
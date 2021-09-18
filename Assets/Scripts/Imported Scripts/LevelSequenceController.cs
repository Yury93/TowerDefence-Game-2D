using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {
        public static string MainMenuSceneNickname = "main_menu";

        public Episode CurrentEpisode { get; private set; }

        public int CurrentLevel { get; private set; }

        public bool LastLevelResult { get; private set; }

        public PlayerStatistics levelStatistics { get; private set; }

        public static SpaceShip PlayerShip { get; set; }


        public void StartEpisode(Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            //levelStatistics = gameObject.AddComponent<PlayerStatistics>();
            //print($"level: {levelStatistics}");
            levelStatistics = GetComponent<PlayerStatistics>();

            levelStatistics.Reset();
            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        public void FinishCurrentLevel(bool success)
        {
            LastLevelResult = success;
            CalculateLevelStatisctic();

            ResultPanelController.Instance.ShowResults(levelStatistics, success);

            print($"level: {levelStatistics}");
        }

        public void AdvanceLevel()
        {
            //levelStatistics.Reset();
            CurrentLevel++;

            if (CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MainMenuSceneNickname);
            }
            else
            {
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            }
        }
        /// <summary>
        /// Метод считает статистику
        /// </summary>
        private void CalculateLevelStatisctic()
        {
            levelStatistics.score = Player.Instance.Score;
            levelStatistics.numKills = Player.Instance.NumKills;
            levelStatistics.time = (int)LevelController.Instance.LevelTime;
        }
    }
}

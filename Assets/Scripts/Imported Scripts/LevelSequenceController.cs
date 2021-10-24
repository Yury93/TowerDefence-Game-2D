using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TowerDeffense;

namespace SpaceShooter
{
    /// <summary>
    /// Контроллер переходов между уровнями. Должен быть с пометкой DoNotDetroyOnLoad
    /// И лежать в сцене с главным меню. LevelController дернет завершение уровня.
    /// </summary>
    public class LevelSequenceController : SingletonBase<LevelSequenceController>
    {
        public static string MainMenuSceneNickname = "LevelMap";

        /// <summary>
        /// Текущий эпизод. Выставляется контроллером выбора эпизода перед началом игры.
        /// </summary>
        public Episode CurrentEpisode { get; private set; }

        /// <summary>
        /// Текущий уровень эпизода. Идшник относительно текущего выставленного эпизода.
        /// </summary>
        public int CurrentLevel { get; private set; }

        /// <summary>
        /// Метод запуска первого уровня эпизода.
        /// </summary>
        /// <param name="e"></param>
        public void StartEpisode(Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            // сбрасываем статы перед началом эпизода.
            LevelResultController.ResetPlayerStats();

            // запускаем первый уровень эпизода.
            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }

        /// <summary>
        /// Принудительный рестарт уровня.
        /// </summary>
        public void RestartLevel()
        {
            //SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            SceneManager.LoadScene(1);
        }

        /// <summary>
        /// Завершаем уровень. В зависимости от результата будет показано окошко результатов.
        /// </summary>
        /// <param name="success">успешность или поражение</param>
        public void FinishCurrentLevel(bool success)
        {
            // после организации переходов
            LevelResultController.Instance.Show(success);
        }

        /// <summary>
        /// Запускаем следующий уровень или выходим в главное меню если больше уровней нету.
        /// </summary>
        public void AdvanceLevel()
        {
            CurrentLevel++;

            // конец эпизода вываливаемся в главное меню.
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
        /// Выбранный игроком корабль для прохождения.
        /// </summary>
        public static SpaceShip PlayerShipPrefab { get; set; }

        #endregion
    }
}
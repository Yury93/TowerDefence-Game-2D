
using UnityEngine;
using SpaceShooter;

namespace TowerDeffense
{
    public class TDLevelController : LevelController
    {
        private int levelScore = 3;
        private new void Start()
        {
            base.Start();
            TDPlayer.Instance.OnPlayerDead += EndLevel;

            m_ReferenceTime += Time.deltaTime;
            m_EventLevelCompleted.AddListener(
                () =>
                {
                    StopLevelActivity();
                    if(m_ReferenceTime <= Time.time)
                    {
                        levelScore -= 1;
                    }
                    Debug.Log($"Score: {levelScore}") ;
                    MapCompletion.SaveEpisodeResult(levelScore);
                });
            void LifeScoreChange(int _)
            {
                levelScore -= 1;
                TDPlayer.OnLifeUpdate -= LifeScoreChange;
            }
            TDPlayer.OnLifeUpdate += LifeScoreChange;
        }
        private void EndLevel()
        {
            StopLevelActivity();
            LevelResultController.Instance.Show(false);
        }
        private void StopLevelActivity()
        {
            //Находит все активные скрипты в сцене Enemy
            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.GetComponent<SpaceShip>().enabled = false;
                enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }

            void DisableAll<T>() where T : MonoBehaviour
            {
                foreach (var obj in FindObjectsOfType<T>())
                {
                    obj.enabled = false;
                }
            }
            DisableAll<Spawner>();
            DisableAll<Projectile>();
            DisableAll<Tower>();
            DisableAll<NextWaveGUI>();
        }
    }
}
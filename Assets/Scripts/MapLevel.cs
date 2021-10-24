using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDeffense
{
    public class MapLevel : MonoBehaviour
    {
        private Episode m_episode;
        [SerializeField] private Text text;
        public void LoadLevel()
        {
            LevelSequenceController.Instance.StartEpisode(m_episode);
        }
        public void SetLevelData(Episode episode, int score)
        {
            m_episode = episode;
            text.text = $"{score}/3";
        }
    }
}
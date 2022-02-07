using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDeffense
{
    public class MapLevel : MonoBehaviour
    {
        private Episode m_episode;
        //[SerializeField] private Text text;
        [SerializeField] private RectTransform m_ResultPanel;
        [SerializeField] private Image[] m_ResultsImage;
        public void LoadLevel()
        {
            LevelSequenceController.Instance.StartEpisode(m_episode);
        }
        public void SetLevelData(Episode episode, int score)
        {
            m_episode = episode;
            m_ResultPanel.gameObject.SetActive(score > 0);
            //���� ������ ������ ����� ���� ������ 3- �� ��������� m_ResultsImage[] - ������ ������� 3 ��������
            for (int i = 0; i < score; i++)
            {
                m_ResultsImage[0].color = Color.white;
            }
            //text.text = $"{score}/3";
        }
    }
}
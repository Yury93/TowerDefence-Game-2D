using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDeffense
{
    public class TowerBuyController : MonoBehaviour
    {

        [SerializeField] private TowerAsset m_ta;
        [SerializeField] private Text m_text;
        [SerializeField] private Button m_Button;
        [SerializeField] private Transform m_BuildSite;
        public void SetBuildSite(Transform value)
        { 
            m_BuildSite = value; 
        }
        
        private void Start()
        {
            TDPlayer.GoldUpdateSuscribe(GoldStatusCheck);
            m_text.text = m_ta.m_GoldCost.ToString();
            m_Button.GetComponent<Image>().sprite = m_ta.m_SpriteGUI;
        }
        private void OnDestroy()
        {
            TDPlayer.OnGoldUpdate -= GoldStatusCheck;

        }
        private void GoldStatusCheck(int gold)
        {
            if(gold >= m_ta.m_GoldCost != m_Button.interactable)
            {
                m_Button.interactable = !m_Button.interactable;
                m_text.color = m_Button.interactable ? Color.white : Color.red;
            }
        }
        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_ta, m_BuildSite);
            BuildSite.HideControls();
        }
    }
}
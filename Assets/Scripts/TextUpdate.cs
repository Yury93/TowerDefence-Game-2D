using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDeffense
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource
        {
            Gold,
            Life
        }
        public UpdateSource source = UpdateSource.Gold;
        #region Properties and fields class
        private Text m_Text;
        #endregion


        #region Private Methods
        private void Start()
        {
            m_Text = GetComponent<Text>();

            switch (source)
            {
                case UpdateSource.Gold: TDPlayer.GoldUpdateSuscribe(UpdateText);
                    break;
                case UpdateSource.Life: TDPlayer.LifeUpdateSuscribe(UpdateText);
                    break;
            }
        }

        private void OnDestroy()
        {
            switch(source)
            {
                case UpdateSource.Gold:
                    TDPlayer.OnGoldUpdate -= UpdateText;
                    break;
                case UpdateSource.Life:
                    TDPlayer.OnLifeUpdate -= UpdateText;
                    break;
            }
        }
        private void UpdateText(int money)
        {
            m_Text.text = money.ToString();
        }
        #endregion
    }
}
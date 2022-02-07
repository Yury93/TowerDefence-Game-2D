using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDeffense
{
    public class NextWaveGUI : MonoBehaviour
    {
        [SerializeField] private Text bonusAmount;
        private EnemyWavesManager manager;
        private float timeToNextWave;
        void Start()
        {
            manager = FindObjectOfType<EnemyWavesManager>();
            //анонимная функция
            EnemyWave.OnWavePrepare += (float time) =>
            {
                timeToNextWave = time;

            };
        }
        private void Update()
        {
            var bonus = (int)timeToNextWave;
            if(bonus<0)
            {
                bonus = 0;
            }
            bonusAmount.text = (bonus).ToString();
            timeToNextWave -= Time.deltaTime;
        }
        public void CallWave()
        {
            manager.ForceNextWave();
        }
    }
}
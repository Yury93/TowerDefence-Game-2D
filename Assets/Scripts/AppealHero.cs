using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDeffense
{
    public class AppealHero : MonoBehaviour
    {
        [SerializeField] public float timer;
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        [SerializeField] private GameObject prefabHero;
        [SerializeField] private Transform positionCreatePrefab;
        [SerializeField] private AIPointPatrol pointPatrolForHero;
        private float startTimer;
        private GameObject prefabHeroSave;

        private void Start()
        {
            prefabHeroSave = prefabHero;
            startTimer = timer;
            image.color = new Color(1, 1, 1, 0.5f);
            button.interactable = false;
        }
        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0 && !button.interactable)
            {
                image.color = new Color(1, 1, 1, 1);
                button.interactable = true;
            }
        }
        public void OnButtonClick()
        {
            if (prefabHero == null)
            {
                prefabHero = prefabHeroSave;
            }
            else
            {
                prefabHero = Instantiate(prefabHero, positionCreatePrefab.transform.position, Quaternion.identity);
                prefabHero.gameObject.GetComponent<TDPatrolController>().SetPatrolPoint(pointPatrolForHero);
                image.color = new Color(1, 1, 1, 0.5f);
                button.interactable = false;
                timer = startTimer;
            }
        }
    } }

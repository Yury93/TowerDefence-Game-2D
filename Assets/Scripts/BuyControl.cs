using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffense
{
    public class BuyControl : MonoBehaviour
    {
        private RectTransform t;
        private void Awake()
        {
            t = GetComponent<RectTransform>();
            BuildSite.OnclickEvent += MoveToBuildSite;
            gameObject.SetActive(false);
        }
        private void MoveToBuildSite(Transform buildSite)
        {
            if (buildSite)
            {
                var position = Camera.main.WorldToScreenPoint(buildSite.position);
                t.anchoredPosition = position;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
            foreach(var tbc in GetComponentsInChildren<TowerBuyController>())
            {
                tbc.SetBuildSite(buildSite);
            }
        }
    }
}
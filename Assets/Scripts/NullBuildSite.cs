using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDeffense
{
    public class NullBuildSite : BuildSite
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            HideControls();
        }
    }
}
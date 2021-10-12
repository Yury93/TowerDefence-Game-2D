using System;
using UnityEngine;
using SpaceShooter;

namespace TowerDeffense
{
    [CreateAssetMenu(fileName ="TowerSetting")]
    public class TowerAsset : ScriptableObject
       {
        public int m_GoldCost = 5;
        public Sprite m_SpriteGUI;
        public Sprite m_Sprite;
        public TurretProperties turretProperties;
       }
    
}
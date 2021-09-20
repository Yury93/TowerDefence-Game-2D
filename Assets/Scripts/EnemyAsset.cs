using UnityEngine;

namespace TowerDeffense
{
    [CreateAssetMenu(fileName = "Properties enemy", menuName = "Enemy settings/Enemy")]
    public sealed class EnemyAsset : ScriptableObject
    {
        [Header("������� ���")]
        public Color color = Color.white;
        public Vector2 scaleSprite = new Vector2(3, 3);
        public RuntimeAnimatorController animations;

        [Header("������� ���������")]
        public float moveSpeed = 1f;
        public int hp = 1;
        public int score = 1;
        public float radius = 0.32f;
        public int damage = 1;
    }
}


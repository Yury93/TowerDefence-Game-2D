using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDeffense
{
    public class StandUp : MonoBehaviour
    {
        private Rigidbody2D m_Rig;

        private SpriteRenderer m_Sr;

        private void Start()
        {
            m_Rig = transform.root.GetComponent<Rigidbody2D>();

            m_Sr = GetComponent<SpriteRenderer>();
        }
        private void LateUpdate()
        {
            transform.up = Vector2.up;
            var xMotion = m_Rig.velocity.x;
            if(xMotion > 0.01f)
            {
                m_Sr.flipX = false;
            }
            else if(xMotion< 0.01f && !gameObject.GetComponentInParent<Hero>())
            {
                m_Sr.flipX = true;
            }
        }
    }
}

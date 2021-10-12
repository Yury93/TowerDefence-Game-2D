using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDeffense
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float timer = 0;
        private bool timerStart = false;
        private void Update()
        {
            if(timerStart == true)
            {
                timer += Time.deltaTime;
                if(timer > 20)
                {
                    Destroy(gameObject);
                    timerStart = false;
                }
            }
        }
       
        private void OnCollisionEnter2D(Collision2D collision)
        {
            collision.gameObject.GetComponent<Enemy>().GivePlayerGold();
            gameObject.GetComponentInChildren<Animator>().SetBool("point", false);
            gameObject.GetComponentInChildren<Animator>().SetBool("NoAttack", false);
            StartCoroutine(CorDestroy(collision.gameObject));
            
            timerStart = true;
        }
       public IEnumerator CorDestroy(GameObject col)
        {
            yield return new WaitForSeconds(1f);
            Destroy(col);
            gameObject.GetComponentInChildren<Animator>().SetBool("NoAttack", true);
        }
    }
}
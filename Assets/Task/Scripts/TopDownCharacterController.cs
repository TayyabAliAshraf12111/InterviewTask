using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TopDownCharacterController : MonoBehaviour
{
    public float speed;
    private Animator animator;


    public int Keys=0;
    public Text KeyText;

    public int Health = 100;
    public Text HealthText;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(RegenerateMana());

    }


    private void FixedUpdate()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        GetComponent<Rigidbody2D>().velocity = speed * dir;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            KillNearestEnemy();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            HookButton();
        }
    }
    private void KillNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = float.MaxValue;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null && closestDistance < 3f)
        {
            Destroy(closestEnemy);
        }
    }

    GameObject TempKey = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Key")
        {
            if (TempKey == null||TempKey!=collision.gameObject)
            {
                TempKey = collision.gameObject;
                Keys++;
                KeyText.text = Keys + "/3";
                Destroy(collision.gameObject);
            }
        }

        if(collision.tag=="Door")
        {
            if(Keys>=3)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }else
            {
                Debug.Log("Not Enough Keys");
            }
        }

        if(collision.tag=="Food")
        {
            if (Health < 100)
            {
                Health += 10;
                HealthText.text = "Health : " + Health;
                Destroy(collision.gameObject);
            }

        }
    }


    public void HealthReduce(int health)
    {
        Health -= health;
        HealthText.text = "Health : " + Health;

        if(Health<=0)
             UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }


    // mana
    public Image manaBarImage;
    public int Mana = 100; 
    public int HookManaCost = 20;
    public float hookRange = 5f; 
    public float pullSpeed = 5f; 

    private bool isHooking = false; 
    private GameObject hookedEnemy;



    public void HookButton()
    {
        isHooking = true;
        Mana -= HookManaCost;
        Hook();
    }

    private void Hook()
    {
        // Find all game objects with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= hookRange)
            {
                hookedEnemy = enemy;
                StartCoroutine(PullEnemy());
                return; 
            }
        }
    }

    private IEnumerator PullEnemy()
    {
        hookedEnemy.GetComponent<BoxCollider2D>().isTrigger = false;
        hookedEnemy.GetComponent<BoxCollider2D>().isTrigger = true;

        while (hookedEnemy != null)
        {
            Vector2 direction = (transform.position - hookedEnemy.transform.position).normalized;
            hookedEnemy.GetComponent<Rigidbody2D>().velocity = direction * pullSpeed;

           
            float distance = Vector2.Distance(transform.position, hookedEnemy.transform.position);
            if (distance < 0.75f)
            {
                hookedEnemy.GetComponent<Rigidbody2D>().drag = 100000;
                Destroy(hookedEnemy, 0.25f);
                break;
            }

            yield return null;
        }
    }

    void EnableEnemyAgain()
    {

    }

    private IEnumerator RegenerateMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); 
  
                Mana += Mathf.RoundToInt(1); 
                Mana = Mathf.Clamp(Mana, 0, 100);
                UpdateManaBar();

        }
    }

    private void UpdateManaBar()
    {
        manaBarImage.fillAmount = (float)Mana / 100; 
    }
}



    


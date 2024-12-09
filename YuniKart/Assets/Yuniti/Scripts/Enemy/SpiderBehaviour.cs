using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class SpiderBehaviour : MonoBehaviour
{
    public static SpiderBehaviour instance;

    public NavMeshAgent enemyAgent;
    public GameObject waypoint;
    public float damageAmount = 0;
    public float attackRate = 0;

    //public int curHealth;

    private bool attackCooldown = false;
    private bool towerNearby = false;
    private Vector3 destination;
    public Vector3 originalWaypointVector3;
    private Animator anim;

    private TowerHealth towerHealth;
    private ProjectileBehaviour projectileBehaviour;
    private FWProjectileBehaviour fwProjectileBehaviour;

    private Vector3 lastPoint;
    private Vector3 currentAssignedPoint;
    private bool reachedFirstPoint = false;
    private int currentPathingIndex;
    public GameObject[] pathing;
    private bool finishedPathing = false;
    public bool isPathingNeeded = false;
    public GameObject[] spawnPoints;
    private Boom boom;
    private bool hasDecreasedEC = false;

    public GameObject hitTextPrefab;
    public Transform hitPosition;
    public IsometricController player;
    [Header("Particals")]
    public ParticleSystem[] particleSystems;
    public GameObject death;

    [Header("EnemyBody")]
    public GameObject[] enemyVisuals;
    private NavMeshAgent navMesh;

    [Header("Health")]
    public float healthAmount;
    public float maxHealth;

    public Slider healthBar;

    void Start()
    {
        currentPathingIndex = 0;

        if (!isPathingNeeded)
        {
            destination = waypoint.transform.position; // This sets the Vector3 destination (X, Y, Z) for the Enemy to go to.
        }
        else
        {
            destination = currentAssignedPoint;
        }

        originalWaypointVector3 = waypoint.transform.position; // This stores the waypoint of the assigned waypoint.


        healthAmount = maxHealth;
        anim = GetComponent<Animator>();
        SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

        if (!isPathingNeeded)
        {
            enemyAgent.SetDestination(originalWaypointVector3);
        }
        else
        {
            if (!reachedFirstPoint)
            {
                lastPoint = pathing[pathing.Length - 1].transform.position;
                currentAssignedPoint = pathing[0].transform.position;
            }


            if (finishedPathing == true)
            {
                enemyAgent.SetDestination(originalWaypointVector3);
            }
            else
            {
                enemyAgent.SetDestination(currentAssignedPoint);
            }
        }


        if (healthAmount <= 0 && RoundManager.instance.roundNumber <= 9)
        {
            StartCoroutine(Death());
        }
        else if (healthAmount <= 0)
        {
            GameManager.instance.coins++;
            healthAmount = 0;
            Destroy(gameObject);
        }
        SetHealth(healthAmount);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyPathing") && currentAssignedPoint != lastPoint && isPathingNeeded)
        {
            currentPathingIndex++;
            currentAssignedPoint = pathing[currentPathingIndex].transform.position;
        }

        if (other.gameObject.CompareTag("EnemyPathing") && other.gameObject.transform.position == pathing[0].transform.position && isPathingNeeded)
        {
            currentPathingIndex++;
            currentAssignedPoint = pathing[currentPathingIndex].transform.position;
            reachedFirstPoint = true;
        }

        if (other.gameObject.CompareTag("EnemyPathing") && other.gameObject.transform.position == lastPoint)
        {
            finishedPathing = true;
            enemyAgent.SetDestination(originalWaypointVector3);
        }

        if (other.gameObject.CompareTag("SpawnPoint"))
        {
            if (other.gameObject == spawnPoints[0])
            {
                pathing = Pathing(0);
            }
            else if (other.gameObject == spawnPoints[1])
            {
                pathing = Pathing(1);
            }
        }

        if (other.gameObject.CompareTag("Projectile"))
        {
            projectileBehaviour = other.gameObject.GetComponent<ProjectileBehaviour>();
            healthAmount = healthAmount - projectileBehaviour.damageAmount;
            // healthbar.value = CalculateHealth();
            HitPartical();
            FindAnyObjectByType<AudioManager>().Play("EnemyHit");
            // Debug.Log("Enemy Health: " + healthAmount);
        }

        if (other.gameObject.CompareTag("FWProjectile"))
        {
            fwProjectileBehaviour = other.gameObject.GetComponent<FWProjectileBehaviour>();
            healthAmount = healthAmount - fwProjectileBehaviour.damageAmount;
            // healthbar.value = CalculateHealth();
            HitPartical();
            FindAnyObjectByType<AudioManager>().Play("EnemyHit");
            //Debug.Log("Enemy Health: " + healthAmount);
        }

        if (other.gameObject.CompareTag("HitBox"))
        {
            healthAmount -= player.attackDamage;

            HitPartical();
            FindAnyObjectByType<AudioManager>().Play("EnemyHit");
            //Debug.Log("Enemy Health: " + healthAmount);
        }
        if (other.gameObject.CompareTag("SwordHitBox"))
        {
            healthAmount -= player.swordDamage;
            HitPartical();
            FindAnyObjectByType<AudioManager>().Play("EnemyHit");

        }

        if (other.gameObject.CompareTag("Bomb"))
        {
            boom = other.gameObject.GetComponent<Boom>();
            boom.StartEffectAndDeactivate();
            healthAmount = healthAmount / 2;
            healthAmount -= 5;

        }
    }

    float CalculateHealth()
    {
        return healthAmount;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Waypoint")) // It detects if the collider trigger has the tag "Waypoint".
        {
            towerNearby = true;
            StartCoroutine(TowerCheck());
        }

        if (other.gameObject.CompareTag("Tower") && !attackCooldown && other.gameObject != null)
        {
            towerHealth = other.gameObject.GetComponent<TowerHealth>();
            towerHealth.curHealth -= damageAmount;
            Debug.Log("Tower Health: " + towerHealth.curHealth);


            //anim.SetBool("EnemyAttack", true);

            StartCoroutine(StartAttackCooldown());

           

        }

    }

    void CreateHitAnimation(Vector3 position)
    {
        GameObject hitText = Instantiate(hitTextPrefab, position, Quaternion.identity);
        HitTextAnimation hitTextAnimation = hitText.AddComponent<HitTextAnimation>();
    }

    private void HitPartical()
    {
        if (particleSystems != null && particleSystems.Length > 0)
        {
            int randomIndex = Random.Range(0, particleSystems.Length);

            // Disable all particle systems in the array
            for (int i = 0; i < particleSystems.Length; i++)
            {
                if (particleSystems[i].isPlaying)
                {
                    particleSystems[i].Stop();
                }
            }

            // Activate the randomly selected particle system
            particleSystems[randomIndex].Play(); 
            anim.SetBool("Hit", true);
            GameManager.instance.score += 4;
            //anim.SetBool("Hit", false);
            StartCoroutine(HitAnimReset());
        }
        else
        {
            Debug.LogWarning("No particle systems found or added to the array!");
        }
    }

    IEnumerator StartAttackCooldown()
    {
        attackCooldown = true;
        yield return new WaitForSeconds(attackRate);
        attackCooldown = false;
        //anim.SetBool("EnemyAttack", false);

    }

    IEnumerator TowerCheck()
    {
        yield return new WaitForSeconds(3);
        towerNearby = false;
    }
    IEnumerator HitAnimReset()
    {
        yield return new WaitForSeconds(0.01f);
        anim.SetBool("Hit", false);
    }

    IEnumerator Death()
    {
        death.SetActive(true);
        navMesh = gameObject.GetComponent<NavMeshAgent>();
        navMesh.speed = 0;
        foreach (GameObject obj in enemyVisuals)
        {
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(1f);
        GameManager.instance.coins++;
        GameManager.instance.score += 99;
        GameManager.instance.killCount++;
        if (!hasDecreasedEC)
        {
            RoundManager.instance.remainingEnemies--;
            hasDecreasedEC = true;
        }
        healthAmount = 0;
        Destroy(gameObject);
    }
    /* ENEMY HEALTH SYSTEM */

    public void SetMaxHealth(float health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }
    public void SetHealth(float health)
    {
        healthBar.value = health;
    }

    public GameObject[] firstPathing;
    public GameObject[] secondPathing;
    public GameObject[] thirdPathing;
    public GameObject[] fourthPathing;

    public GameObject[] Pathing(int spawnPointChosen)
    {
        if (spawnPointChosen == 0)
        {
            int randomPathing = Random.Range(0, 2);
            if (randomPathing == 0)
            {
                return firstPathing;
            }
            else
            {
                return thirdPathing;
            }
        }
        else
        {
            int randomPathing = Random.Range(0, 2);
            if (randomPathing == 0)
            {
                return secondPathing;
            }
            else
            {
                return fourthPathing;
            }
        }
    }
}
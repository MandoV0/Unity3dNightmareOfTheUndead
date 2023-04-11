using Assets.GameProject.Scripts;
using Assets.GameProject.Scripts.Enemy;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using Wave_FPS_Mobile.Scripts;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : EnemyBase
{
    public string state;
    public State currentState;
    public WaitState waitState = new WaitState();

    [Title(label: "Settings")]
    public NavMeshAgent navMeshAgent;

    [Title(label: "Health")]
    public int health;
    public float multiplierHealth = 1;

    [Title(label: "Movement")]
    public float speed = 3.5f;

    [Title(label: "Attacking")]
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private int damageAmount = 45;
    [SerializeField]
    private float attackingDistance;

    [Title(label: "Audio")]
    public AudioSource audioSource;
    public AudioClip[] attackSounds;

    private float _attackTimer;

    [SerializeField] private bool isAlive;
    [SerializeField] private bool autoStartAi;

    /// <summary>
    /// Todo: Add gibbing.
    /// </summary>

    private void Start()
    {
        navMeshAgent.stoppingDistance = attackingDistance - 0.1f;

        if (autoStartAi)
        {
            Initialize(1500, 3);
        }
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }

        _attackTimer = Mathf.MoveTowards(_attackTimer, 0, Time.deltaTime);

        // Make sure that the Zombie stays inside the Playable Area
        if (transform.position.y < -15 || transform.position.y > 300)
        {
            Die();
        }
    }

    public void OnDrawGizmos()
    {
        // For debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    public override void Attack()
    {
        // if the attackTimer is not 0 return
        if (_attackTimer != 0)
        {
            return;
        }

        // Reset the attackTimer
        _attackTimer = attackDelay;

        // Play a random Attack Sound with a random pitch
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Length)]);

        // Get everything in the AttackSphere
        var hits = Physics.OverlapSphere(attackPoint.position, attackRadius);

        // Damage all Players in the AttackSphere
        foreach (var hit in hits)
        {
            if (hit.transform.root.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    public override void Die()
    {
        isAlive = false;
        health = 0;

        navMeshAgent.SetDestination(transform.position);

        if (!autoStartAi)
        {
            if (WaveSpawner.Instance != null)
            {
                WaveSpawner.Instance.EnemyDied(this);

                // Tries dropping power up

                if (PowerupManager.instance)
                {
                    PowerupManager.instance.DropPowerUp(transform.position);
                }
            }
        }

        gameObject.SetActive(false);
    }

    public override void FaceTarget(Transform target)
    {
        if (target)
        {
            if (Vector3.Distance(transform.position, target.position) <= attackingDistance + 0.5f)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 12f);
            }
        }
    }

    public override void Initialize(int baseHealth, float speedMultiplier)
    {
        isAlive = true;
        health = (int)(baseHealth * multiplierHealth);;

        navMeshAgent.speed = speed * speedMultiplier;

        currentState = waitState;
    }

    public override bool IsAlive()
    {
        return isAlive;
    }

    public override Transform SearchForTarget()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        if(playerStats != null)
        {
            if (playerStats.GetPlayerHealth().IsAlive())
            {
                return playerStats.transform;
            }
            else
            {
                return null;
            }
        }

        return null;
    }

    public override void SwitchState(State state)
    {
        // Set and Enter the new State
        currentState = state;
        currentState.EnterState(this);

        // For debugging
        this.state = currentState.GetStateName();
    }

    public override void TakeDamage(int damage, PlayerStats damageCauser, Vector3 damageCauserLook, HitLayer hitLayer, RaycastHit hit)
    {
        // if we are dead return so that we dont invoke any TakeDamage events
        if (!IsAlive()) return;

        health -= damage;

        // Get a blood splatter from the pool
        GameObject splatter = EffectPoolers.Instance.GetPooledObject("Splatter");

        if (splatter != null)
        {
            // Cast ray from the hit point with the direction of where the hit came from
            Vector3 spread = 0.3f * Random.insideUnitCircle;
            if (Physics.Raycast(hit.point, damageCauserLook + spread, out var rHit, 3, ~LayerMask.GetMask("Enemy", "Ignore Raycast")))
            {
                // Set its rotation to surface normal
                splatter.transform.rotation = Quaternion.LookRotation(-rHit.normal);

                // Apply offset so it doesnt clip
                splatter.transform.position = rHit.point + -splatter.transform.forward * 0.001f;                

                // Give the blood a random roation on the z axis
                splatter.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
                
                // Get the the bloodsplatter renderer
                Renderer renderer = splatter.GetComponent<Renderer>();

                // Apply random material offset from tilling texture
                float[] offsets = new float[] { 0f, 0.33f, 0.66f };
                renderer.material.mainTextureOffset = new Vector2(offsets[Random.Range(0, offsets.Length)], 0);
                
                splatter.SetActive(true);
            }
        }

        if (health <= 0)
        {
            health = 0;

            // If the player shot killed the enemy give him 30 Points
            // else give him 10
            if (damageCauser != null) 
            {
                damageCauser.AddPoints(30);
            }
            
            Die();
        }
        else
        {
            if (damageCauser != null)
            {
                damageCauser.AddPoints(10);
            }
        }
    }

    public override NavMeshAgent GetNavMeshAgent()
    {
        return navMeshAgent;
    }

    public override float GetAttackingDistance()
    {
        return attackingDistance;
    }
}
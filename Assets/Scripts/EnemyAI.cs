using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public VisionConica visionCono;
    public MaskController maskController;

    private AudioSource audioSource;

    bool isMaskDown;

    [Header("Patrol")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float waitTimeAtPoint = 2f;
    private bool isWaiting = false;

    [Header("Chase")]
    public Transform player;
    public float chaseSpeed = 4.5f;
    [Header("Chase Settings")]
    public float maxChaseTime = 5f;
    private float chaseTimer = 0f;

    [Header("Cooldown")]
    public float chaseCooldown = 5f;
    private bool isOnCooldown = false;

    private float patrolSpeed;

    public enum State
    {
        Patrolling,
        Chasing
    }

    public State currentState = State.Patrolling;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        patrolSpeed = agent.speed;

        if (waypoints.Length > 0)
            agent.destination = waypoints[currentWaypointIndex].position;
    }

    void Update()
    {
        isMaskDown = maskController.maskDown;

        // PRIORIDAD ABSOLUTA: si la máscara está bajada, el enemigo se queda quieto
        if (isMaskDown)
        {
            // Detener al agente si no está ya detenido
            if (!agent.isStopped)
            {
                agent.isStopped = true;
            }
            return;
        }
        else
        {
            // Si la máscara acaba de subirse, retrasar la reactivación 1 segundo
            if (agent.isStopped)
            {
                StartCoroutine(ResumeAfterDelay(1f));
            }
        }

        // Actualizar cooldown si está activo
        if (isOnCooldown)
            return; // Mientras está en cooldown, no hacer nada

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Chasing:
                Chase();
                break;
        }
    }

    // ------------------- NUEVO -------------------
    private IEnumerator ResumeAfterDelay(float delay)
    {
        // Esperar 1 segundo
        yield return new WaitForSeconds(delay);

        // Reanudar el agente solo si la máscara sigue subida
        if (!maskController.maskDown)
        {
            agent.isStopped = false;
        }
    }


    // ---------------- PATRULLA ----------------
    private void Patrol()
    {
        if (visionCono.CanSeePlayer && !isOnCooldown)
        {
            agent.speed = chaseSpeed;
            currentState = State.Chasing;
            return;
        }

        if (waypoints.Length == 0 || isWaiting)
            return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(WaitAndGoToNextPoint());
        }
    }

    private IEnumerator WaitAndGoToNextPoint()
    {
        isWaiting = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(waitTimeAtPoint);

        // Selecciona un waypoint aleatorio diferente al actual
        int nextWaypointIndex = currentWaypointIndex;
        if (waypoints.Length > 1) // Para evitar bucle infinito si solo hay un waypoint
        {
            while (nextWaypointIndex == currentWaypointIndex)
            {
                nextWaypointIndex = Random.Range(0, waypoints.Length);
            }
        }

        currentWaypointIndex = nextWaypointIndex;
        agent.destination = waypoints[currentWaypointIndex].position;

        agent.isStopped = false;
        isWaiting = false;
    }


    // ---------------- PERSECUCIÓN ----------------
    private void Chase()
    {
        if (player == null) return;

        if (!audioSource.isPlaying)
            audioSource.Play();

        if (!visionCono.CanSeePlayer)
        {
            StartCoroutine(StartCooldown());
            return;
        }

        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        chaseTimer += Time.deltaTime;

        if (chaseTimer >= maxChaseTime)
        {
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator StartCooldown()
    {
        currentState = State.Patrolling;
        agent.speed = patrolSpeed;
        chaseTimer = 0f;
        isOnCooldown = true;
        agent.destination = waypoints.Length > 0 ? waypoints[currentWaypointIndex].position : transform.position;

        yield return new WaitForSeconds(chaseCooldown);

        isOnCooldown = false;
    }
}

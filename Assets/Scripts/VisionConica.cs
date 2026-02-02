using UnityEngine;

public class VisionConica : MonoBehaviour
{
    [Header("Paràmetres de visió")]
    public float anguloVision = 60f;         // Angle del con en graus
    public float distanciaVision = 10f;      // Distància màxima de visió
    public LayerMask capaObjetivo;           // Quines capes poden ser detectades
    public LayerMask capaObstaculos;         // Quines capes bloquegen la visió
    public bool CanSeePlayer;

    [Header("Debug")]
    public bool mostrarGizmos = true;

    /// <summary>
    /// Comprova si un objectiu està dins del con de visió i sense obstacles.
    /// </summary>
    public bool EstaEnZonaDeVision(Transform objetivo)
    {
        // Calculem la direcció cap a l'objectiu
        Vector3 direccionAlObjetivo = (objetivo.position - transform.position).normalized;
        // Calculem l'angle entre la direcció del personatge i l'objectiu
        float angulo = Vector3.Angle(transform.forward, direccionAlObjetivo);

        // Si l'objectiu està dins de l'angle de visió...
        if (angulo < anguloVision / 2f)
        {
            // Calculem la distància fins a l'objectiu
            float distancia = Vector3.Distance(transform.position, objetivo.position);
            // Si està dins de la distància màxima...
            if (distancia < distanciaVision)
            {
                // Comprovem si hi ha obstacles entre el personatge i l'objectiu
                if (!Physics.Raycast(transform.position, direccionAlObjetivo, distancia, capaObstaculos))
                {
                    //Debug.Log("Objectiu a la vista: " + objetivo.name);
                    return true; // L'objectiu està dins del con de visió i sense obstacles
                }
            }
        }
        return false; // No està dins del con o hi ha obstacles
    }

    // Exemple d'ús: detectar si un objectiu està dins de la zona de visió
    void Update()
    {
        // Busquem tots els objectius dins de la distància màxima
        Collider[] objetivos = Physics.OverlapSphere(transform.position, distanciaVision, capaObjetivo);
        foreach (var objetivo in objetivos)
        {
            // Comprovem si cada objectiu està dins del con de visió
            if (EstaEnZonaDeVision(objetivo.transform))
            {
                CanSeePlayer = true;
                // Aquí pots posar la lògica de reacció del personatge
            }
            else
            {
                 CanSeePlayer = false;
            }
        }
    }

    // Visualització del con a l'escena
    void OnDrawGizmos()
    {
        if (!mostrarGizmos) return;

        Gizmos.color = Color.yellow;
        // Dibuixem els límits esquerre i dret del con
        Vector3 leftLimit = Quaternion.Euler(0, -anguloVision / 2, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, anguloVision / 2, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftLimit * distanciaVision);
        Gizmos.DrawRay(transform.position, rightLimit * distanciaVision);
        // Dibuixem una esfera per indicar la distància màxima
        Gizmos.DrawWireSphere(transform.position, distanciaVision);
    }
}

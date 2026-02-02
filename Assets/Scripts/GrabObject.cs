using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public KeyCode pickupKey = KeyCode.E; // Tecla para recoger
    private bool playerInRange = false;   // Para saber si el player está en el trigger

    void Update()
    {
        // Comprobar si el jugador está dentro del trigger y pulsa la tecla
        if (playerInRange && Input.GetKeyDown(pickupKey))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        // Acción al recoger el objeto
        Debug.Log("Objeto recogido: " + gameObject.name);
        // Aquí puedes hacer lo que quieras, por ejemplo desactivar el objeto
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Comprobar que el tag sea "Player"
        {
            playerInRange = true;
            Debug.Log("Player dentro del rango de pickup");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player salió del rango de pickup");
        }
    }
}

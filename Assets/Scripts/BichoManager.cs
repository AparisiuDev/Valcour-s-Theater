using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BichoManager : MonoBehaviour
{
    public MaskController maskController;

    // Objeto al que queremos mirar
    public Transform target;

    // Lista de GameObjects a alternar
    public List<GameObject> bichos;

    // Tiempo en segundos entre cambios
    public float intervaloCambio = 20f;

    private int indiceActual = 0;

    void Start()
    {
        InicializarBichos();
        StartCoroutine(CambiarBichoCadaIntervalo());
    }

    void Update()
    {
        if (target != null)
            transform.LookAt(target);

        ActualizarEstadoPorMascara();
    }

    // ------------------ INICIALIZACIÓN ------------------
    void InicializarBichos()
    {
        for (int i = 0; i < bichos.Count; i++)
        {
            if (bichos[i] == null) continue;

            bichos[i].SetActive(false);
            SetMeshRenderer(bichos[i], false);
        }

        if (bichos.Count > 0)
        {
            bichos[0].SetActive(true);
            SetMeshRenderer(bichos[0], true);
        }
    }

    // ------------------ LÓGICA MÁSCARA ------------------
    void ActualizarEstadoPorMascara()
    {
        if (maskController.maskDown)
        {
            // Todos activos, solo UNO visible
            for (int i = 0; i < bichos.Count; i++)
            {
                if (bichos[i] == null) continue;

                bichos[i].SetActive(true);
                SetMeshRenderer(bichos[i], i == indiceActual);
            }
        }
        else
        {
            // Máscara subida: ninguno visible
            for (int i = 0; i < bichos.Count; i++)
            {
                if (bichos[i] == null) continue;

                // Da igual si están activos o no, NO se ve ninguno
                SetMeshRenderer(bichos[i], false);
            }
        }
    }


    // ------------------ TURNOS ------------------
    IEnumerator CambiarBichoCadaIntervalo()
    {
        while (true)
        {
            float tiempo = 0f;

            // El tiempo no avanza si la máscara está abajo
            while (tiempo < intervaloCambio)
            {
                if (!maskController.maskDown)
                    tiempo += Time.deltaTime;

                yield return null;
            }

            indiceActual = (indiceActual + 1) % bichos.Count;

            // Esperar a que la máscara suba
            while (maskController.maskDown)
                yield return null;

            // Delay extra de 1 segundo
            yield return new WaitForSeconds(1f);
        }
    }

    // ------------------ UTILIDAD ------------------
    void SetMeshRenderer(GameObject obj, bool estado)
    {
        MeshRenderer mr = obj.GetComponent<MeshRenderer>();
        if (mr != null)
            mr.enabled = estado;
    }
}

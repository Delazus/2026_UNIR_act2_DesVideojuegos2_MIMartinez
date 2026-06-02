using UnityEngine;
using UnityEngine.InputSystem; // Evita el error InvalidOperationException

public class MecanicaRaycast : MonoBehaviour
{
    public float distanciaInteraccion = 10f;
    public float radioEsfera = 0.6f; // Tu margen de error cómodo para apuntar
    public GameObject contenedorIndicadorUI; // Tu letrero negro con fondo de la tecla [E]

    void Start()
    {
        if (contenedorIndicadorUI != null)
        {
            contenedorIndicadorUI.SetActive(false);
        }
    }

    void Update()
    {
        // Lanza un rayo grueso desde el centro de la pantalla hacia adelante
        Ray rayo = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        bool mirandoPalanca = false;

        if (Physics.SphereCast(rayo, radioEsfera, out hit, distanciaInteraccion))
        {
            // Busca únicamente si el objeto es una palanca de energía
            PalancaEnergia palanca = hit.collider.GetComponent<PalancaEnergia>();

            if (palanca != null)
            {
                mirandoPalanca = true;

                // Enciende el letrero visual en la interfaz
                if (contenedorIndicadorUI != null)
                {
                    contenedorIndicadorUI.SetActive(true);
                }

                // Detecta la tecla E usando el nuevo sistema de entradas
                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    palanca.AlternarPalanca(); // Acciona la palanca
                }
            }
        }

        // Si la mirada sale de la palanca, apaga el cartel de la interfaz de inmediato
        if (!mirandoPalanca && contenedorIndicadorUI != null)
        {
            contenedorIndicadorUI.SetActive(false);
        }
    }
}
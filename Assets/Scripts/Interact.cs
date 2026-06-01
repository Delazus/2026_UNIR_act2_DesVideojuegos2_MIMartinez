using UnityEngine;

public class Interact : MonoBehaviour
{
    public float distanciaInteraccion = 5f; // Qué tan cerca debe estar el jugador

    void Update()
    {
        // Si el jugador hace clic izquierdo con el ratón
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Lanza un rayo invisible hacia adelante
            if (Physics.Raycast(rayo, out hit, distanciaInteraccion))
            {
                // Si golpea una palanca, ejecuta su función
                PalancaEnergia palanca = hit.collider.GetComponent<PalancaEnergia>();
                if (palanca != null)
                {
                    palanca.AlternarPalanca();
                }
            }
        }
    }
}
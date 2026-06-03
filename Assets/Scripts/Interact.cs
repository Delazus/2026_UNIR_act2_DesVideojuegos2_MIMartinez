using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public float distanciaInteraccion = 10f;
    public float radioEsfera = 0.6f;
    public GameObject contenedorIndicadorUI;

    private ControlNave manager;

    void Start()
    {
        manager = FindAnyObjectByType<ControlNave>();
        if (contenedorIndicadorUI != null) contenedorIndicadorUI.SetActive(false);
    }

    void Update()
    {
        Ray rayo = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        bool mirandoInteractuable = false;

        if (Physics.SphereCast(rayo, radioEsfera, out hit, distanciaInteraccion))
        {
            PalancaEnergia palanca = hit.collider.GetComponent<PalancaEnergia>();

            // Detecta la consola principal por su Tag
            bool esConsola = hit.collider.CompareTag("Consola");

            // Se puede interactuar si es una palanca, o si es la consola Y la energía ya está lista
            if (palanca != null || (esConsola && manager != null && manager.panelEnergiaResuelto))
            {
                mirandoInteractuable = true;

                if (contenedorIndicadorUI != null) contenedorIndicadorUI.SetActive(true);

                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    if (palanca != null)
                    {
                        palanca.AlternarPalanca();
                    }
                    else if (esConsola)
                    {
                        manager.AbrirPanelVectores();
                    }
                }
            }
        }

        if (!mirandoInteractuable && contenedorIndicadorUI != null)
        {
            contenedorIndicadorUI.SetActive(false);
        }
    }
}
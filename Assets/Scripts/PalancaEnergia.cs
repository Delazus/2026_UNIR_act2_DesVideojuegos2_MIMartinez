using UnityEngine;

public class PalancaEnergia : MonoBehaviour
{
    public int vatiosQueAporta = 25; // Cuánta energía da esta palanca individual
    private bool estaActivada = false;
    private ControlNave manager;

    void Start()
    {
        // Busca automáticamente el script principal en la escena
        manager = Object.FindAnyObjectByType<ControlNave>();
    }

    public void AlternarPalanca()
    {
        estaActivada = !estaActivada;

        if (estaActivada)
        {
            manager.ModificarEnergia(vatiosQueAporta);
            // OPCIONAL: Mover la palanca visualmente un poco hacia abajo
            transform.Rotate(30, 0, 0);
        }
        else
        {
            manager.ModificarEnergia(-vatiosQueAporta);
            // OPCIONAL: Regresar la palanca a su posición original
            transform.Rotate(-30, 0, 0);
        }
    }
}

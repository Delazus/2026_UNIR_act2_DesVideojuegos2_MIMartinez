using UnityEngine;

public class PalancaEnergia : MonoBehaviour
{
    public int vatiosQueAporta = 25;
    private bool estaActivada = false;
    private ControlNave manager;

    [Header("Configuración de Animación")]
    // Hacemos el Animator público por si el componente está en un objeto hijo
    public Animator miAnimator;

    void Start()
    {
        manager = FindAnyObjectByType<ControlNave>();

        // Si se te olvida arrastrarlo en el Inspector, el código intentará buscarlo solo
        if (miAnimator == null)
        {
            miAnimator = GetComponent<Animator>();
        }
    }

    public void AlternarPalanca()
    {
        estaActivada = !estaActivada;

        if (estaActivada)
        {
            manager.ModificarEnergia(vatiosQueAporta);

            // ¡NUEVO! Fuerza al Animator a reproducir el clip de activación
            if (miAnimator != null)
            {
                miAnimator.Play("Lever_Activate");
            }
        }
        else
        {
            manager.ModificarEnergia(-vatiosQueAporta);

            // ¡NUEVO! Regresa la palanca a su estado original (apagado)
            if (miAnimator != null)
            {
                miAnimator.Play("Lever_Default");
            }
        }
    }
}
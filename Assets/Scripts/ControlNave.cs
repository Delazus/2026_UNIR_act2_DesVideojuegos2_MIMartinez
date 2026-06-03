using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlNave : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    public float tiempoRestante = 120f;
    public TMP_Text textoContador;

    [Header("Puzle 1: Energía")]
    public int energiaActual = 0;
    public int energiaObjetivo = 100;
    public TMP_Text textoEnergia;
    public GameObject panelVectoresUI;

    [Header("Puzle 2: Vectores")]
    public int vectorCorrectoX = 4;
    public int vectorCorrectoY = 7;
    public int vectorCorrectoZ = -2;
    public TMP_InputField inputX;
    public TMP_InputField inputY;
    public TMP_InputField inputZ;
    public TMP_Text textoAvisoUI;

    [Header("Paneles de Estado (UI)")]
    public GameObject panelBackground;
    public GameObject panelIntro;
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    [Header("Audio General")]
    public AudioSource[] sonidosParaApagar;

    [Header("Control del Jugador y Ratón")]
    public MonoBehaviour scriptMovimientoJugador;

    // Esta variable ahora es pública para que el Raycast sepa si la energía ya funciona
    [HideInInspector] public bool panelEnergiaResuelto = false;
    private bool juegoActivo = false;

    void Start()
    {
        panelVectoresUI.SetActive(false);
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);
        panelIntro.SetActive(true);
        panelBackground.SetActive(true);
        Time.timeScale = 0f;
        if (textoAvisoUI != null) textoAvisoUI.text = "";

        LiberarRaton(true);
    }

    public void IniciarJuego()
    {
        panelIntro.SetActive(false);
        panelBackground.SetActive(false);
        Time.timeScale = 1f;
        juegoActivo = true;
        LiberarRaton(false);
    }

    void Update()
    {
        if (!juegoActivo) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            textoContador.text = "Soporte de Vida: " + Mathf.Ceil(tiempoRestante).ToString() + "s";
            if (tiempoRestante < 30f) textoContador.color = Color.red;
        }
        else
        {
            Defeated();
        }
    }

    public void ModificarEnergia(int cantidad)
    {
        if (panelEnergiaResuelto || !juegoActivo) return;

        energiaActual += cantidad;
        textoEnergia.text = "Energía: " + energiaActual + " / " + energiaObjetivo + " W";

        if (energiaActual == energiaObjetivo)
        {
            panelEnergiaResuelto = true;
            textoEnergia.text = "SISTEMA ENERGIZADO";
            textoEnergia.color = Color.green;

            // Reemplazamos la activación directa por nuestra nueva función inteligente
            AbrirPanelVectores();
        }
    }

    // FUNCIONES DE CONTROL DE PANEL
    public void AbrirPanelVectores()
    {
        if (!panelEnergiaResuelto || !juegoActivo) return;

        panelVectoresUI.SetActive(true);
        LiberarRaton(true); // Libera el ratón automáticamente para poder escribir las coordenadas
    }

    public void CerrarPanelVectores()
    {
        if (!juegoActivo) return;

        panelVectoresUI.SetActive(false);
        LiberarRaton(false); // Vuelve a ocultar el ratón para que puedas caminar y ver las pistas
    }

    // FUNCIONES VECTOR

    public void ValidarVector()
    {
        int xIngresado = int.Parse(inputX.text);
        int yIngresado = int.Parse(inputY.text);
        int zIngresado = int.Parse(inputZ.text);

        if (xIngresado == vectorCorrectoX && yIngresado == vectorCorrectoY && zIngresado == vectorCorrectoZ)
        {
            Victory();
        }
        else
        {
            tiempoRestante -= 20f;
            if (textoAvisoUI != null) textoAvisoUI.text = "Ruta Errónea. Vector de trayectoria desviado.";
        }
    }

    void LiberarRaton(bool liberar)
    {
        if (liberar)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (scriptMovimientoJugador != null) scriptMovimientoJugador.enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (scriptMovimientoJugador != null) scriptMovimientoJugador.enabled = true;
        }
    }

    void Victory()
    {
        juegoActivo = false;
        Time.timeScale = 0f;
        panelVictoria.SetActive(true);
        LiberarRaton(true);
        foreach (AudioSource sonido in sonidosParaApagar)
        {
            if (sonido != null) sonido.Stop();
        }
    }

    void Defeated()
    {
        juegoActivo = false;
        Time.timeScale = 0f;
        panelDerrota.SetActive(true);
        LiberarRaton(true);
        foreach (AudioSource sonido in sonidosParaApagar)
        {
            if (sonido != null) sonido.Stop();
        }
    }

    public void ReiniciarNivel() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    public void IrAlMenu() { SceneManager.LoadScene("MenuPrincipal"); }
}
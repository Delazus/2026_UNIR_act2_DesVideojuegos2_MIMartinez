using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlNave : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    public float tiempoRestante = 120f; // 2 minutos de juego
    public TMP_Text textoContador;

    [Header("Puzle 1: Energía")]
    public int energiaActual = 0;
    public int energiaObjetivo = 100; // La meta exacta
    public TMP_Text textoEnergia;
    public GameObject panelVectoresUI; // El teclado numérico de la pantalla

    [Header("Puzle 2: Vectores")]
    public int vectorCorrectoX = 4;
    public int vectorCorrectoY = 7;
    public int vectorCorrectoZ = -2;
    public InputField inputX;
    public InputField inputY;
    public InputField inputZ;
    public TMP_Text textoAvisoUI; // Mensajes en pantalla (ej: "Coordenadas incorrectas")

    [Header("Paneles de Estado (UI)")]
    public GameObject panelBackground;
    public GameObject panelIntro;
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    [Header("Control del Jugador y Ratón")]
    public MonoBehaviour scriptMovimientoJugador; // Tu componente ThirdPersonController

    private bool panelEnergiaResuelto = false;
    private bool juegoActivo = false;

    void Start()
    {
        // Estado inicial de las pantallas
        panelVectoresUI.SetActive(false);
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);
        panelIntro.SetActive(true);
        panelBackground.SetActive(true);

        Time.timeScale = 0f; // Pausa el juego en la intro narrativa
        if (textoAvisoUI != null) textoAvisoUI.text = "";

        LiberarRaton(true); // Libera el cursor para poder pulsar "Entendido"
    }

    public void IniciarJuego()
    {
        panelIntro.SetActive(false);
        panelBackground.SetActive(false);
        Time.timeScale = 1f; // Arranca el tiempo del juego
        juegoActivo = true;

        LiberarRaton(false); // Oculta el ratón para empezar a mover al personaje
    }

    void Update()
    {
        if (!juegoActivo) return;

        // Cuenta regresiva del soporte de vida
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            textoContador.text = "Soporte de Vida: " + Mathf.Ceil(tiempoRestante).ToString() + "s";

            if (tiempoRestante < 30f)
            {
                textoContador.color = Color.red; // Tensión visual al final
            }
        }
        else
        {
            Defeated(); // Si el tiempo llega a 0, pierde
        }
    }

    // Esta función la llaman las palancas automáticamente al interactuar
    public void ModificarEnergia(int cantidad)
    {
        if (panelEnergiaResuelto || !juegoActivo) return;

        energiaActual += cantidad;
        textoEnergia.text = "Energía: " + energiaActual + " / " + energiaObjetivo + " W";

        // Comprobación de la condición del primer puzle
        if (energiaActual == energiaObjetivo)
        {
            panelEnergiaResuelto = true;
            textoEnergia.text = "SISTEMA ENERGIZADO";
            textoEnergia.color = Color.green;

            panelVectoresUI.SetActive(true); // Se enciende la pantalla de navegación automáticamente
        }
    }

    // Esta función la llamará tu botón de la UI "INGRESAR RUTA"
    public void ValidarVector()
    {
        // Convierte el texto que escribe el usuario en números
        int xIngresado = int.Parse(inputX.text);
        int yIngresado = int.Parse(inputY.text);
        int zIngresado = int.Parse(inputZ.text);

        if (xIngresado == vectorCorrectoX && yIngresado == vectorCorrectoY && zIngresado == vectorCorrectoZ)
        {
            Victory();
        }
        else
        {
            tiempoRestante -= 20f; // Penalización de tiempo por fallar la matemática
            if (textoAvisoUI != null) textoAvisoUI.text = "Ruta Errónea. Vector de trayectoria desviado.";
        }
    }

    // Control del cursor y el script del personaje en los menús
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
    }

    void Defeated()
    {
        juegoActivo = false;
        Time.timeScale = 0f;
        panelDerrota.SetActive(true);
        LiberarRaton(true);
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
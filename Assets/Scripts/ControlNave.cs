using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlNave : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    public float tiempoRestante = 120f;
    public Text textoContador;

    [Header("Puzle 1: Energía")]
    public int energiaActual = 0;
    public int energiaObjetivo = 100;
    public Text textoEnergia;
    public GameObject panelVectoresUI;

    [Header("Elemento Extra: Exploración")]
    public bool tieneTarjetaCoordenadas = false;
    public GameObject objetoTarjetaEnEscena; // La tarjeta 3D que el jugador debe buscar
    public Text textoAvisoUI; // Un texto auxiliar para dar mensajes al jugador

    [Header("Puzle 2: Vectores")]
    public int vectorCorrectoX = 4;
    public int vectorCorrectoY = 7;
    public int vectorCorrectoZ = -2;
    public InputField inputX;
    public InputField inputY;
    public InputField inputZ;

    [Header("Paneles de Estado (UI)")]
    public GameObject panelIntro;
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    [Header("Efectos de Sonido (SFX)")]
    public AudioClip sonidoPalanca;
    public AudioClip sonidoError;
    public AudioClip sonidoEnergiaOk;
    public AudioClip sonidoVictoria;

    private bool panelEnergiaResuelto = false;
    private bool juegoActivo = false;

    void Start()
    {
        panelVectoresUI.SetActive(false);
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);
        panelIntro.SetActive(true); // Empezamos con la intro narrativa
        Time.timeScale = 0f; // Pausa el juego durante la intro
        textoAvisoUI.text = "Busca la Tarjeta de Coordenadas y activa la energía.";
    }

    // Este método lo llamará el botón "ENTENDIDO" de tu panel de Intro
    public void IniciarJuego()
    {
        panelIntro.SetActive(false);
        Time.timeScale = 1f; // Reanuda el tiempo del juego
        juegoActivo = true;
    }

    void Update()
    {
        if (!juegoActivo) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            textoContador.text = "Soporte de Vida: " + Mathf.Ceil(tiempoRestante).ToString() + "s";

            // Efecto visual: Si queda menos de 30s, el texto parpadea en rojo
            if (tiempoRestante < 30f)
            {
                textoContador.color = Color.red;
            }
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

        // Reproduce el sonido de la palanca en la posición de la cámara
        if (sonidoPalanca != null) AudioSource.PlayClipAtPoint(sonidoPalanca, Camera.main.transform.position);

        if (energiaActual == energiaObjetivo)
        {
            panelEnergiaResuelto = true;
            textoEnergia.text = "SISTEMA ENERGIZADO";
            textoEnergia.color = Color.green;

            if (sonidoEnergiaOk != null) AudioSource.PlayClipAtPoint(sonidoEnergiaOk, Camera.main.transform.position);

            panelVectoresUI.SetActive(true);
        }
    }

    // El jugador usará el Raycast para hacer clic en la tarjeta 3D y llamará a esto
    public void RecogerTarjeta()
    {
        tieneTarjetaCoordenadas = true;
        objetoTarjetaEnEscena.SetActive(false); // Hace desaparecer la tarjeta del suelo/mesa
        textoAvisoUI.text = "¡Tarjeta recogida! Introdúcela en el panel de navegación.";
        if (sonidoPalanca != null) AudioSource.PlayClipAtPoint(sonidoPalanca, Camera.main.transform.position);
    }

    public void ValidarVector()
    {
        // Validación del elemento extra de exploración
        if (!tieneTarjetaCoordenadas)
        {
            textoAvisoUI.text = "ERROR: Inserta la Tarjeta de Coordenadas primero.";
            if (sonidoError != null) AudioSource.PlayClipAtPoint(sonidoError, Camera.main.transform.position);
            return;
        }

        int xIngresado = int.Parse(inputX.text);
        int yIngresado = int.Parse(inputY.text);
        int zIngresado = int.Parse(inputZ.text);

        if (xIngresado == vectorCorrectoX && yIngresado == vectorCorrectoY && zIngresado == vectorCorrectoZ)
        {
            Victory();
        }
        else
        {
            tiempoRestante -= 20f; // Penalización
            textoAvisoUI.text = "Coordenadas Incorrectas. Trayectoria Fallida.";
            if (sonidoError != null) AudioSource.PlayClipAtPoint(sonidoError, Camera.main.transform.position);
        }
    }

    void Victory()
    {
        juegoActivo = false;
        Time.timeScale = 0f;
        panelVictoria.SetActive(true);
        if (sonidoVictoria != null) AudioSource.PlayClipAtPoint(sonidoVictoria, Camera.main.transform.position);
    }

    void Defeated()
    {
        juegoActivo = false;
        Time.timeScale = 0f;
        panelDerrota.SetActive(true);
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
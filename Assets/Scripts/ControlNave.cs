using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con la interfaz
using UnityEngine.SceneManagement; // Necesario para reiniciar si pierde

public class ControlNave : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    public float tiempoRestante = 120f; // 2 minutos para escapar
    public Text textoContador; // Arrastra aquí un texto de la UI para el reloj

    [Header("Puzle 1: Energía")]
    public int energiaActual = 0;
    public int energiaObjetivo = 100; // El número exacto a conseguir
    public Text textoEnergia; // Texto UI que muestra la energía actual
    public GameObject panelVectoresUI; // Panel de UI del teclado numérico (empieza desactivado)

    [Header("Puzle 2: Vectores")]
    // Si la operación matemática da, por ejemplo, X=4, Y=7, Z=-2
    public int vectorCorrectoX = 4;
    public int vectorCorrectoY = 7;
    public int vectorCorrectoZ = -2;

    // Campos de texto de la UI donde el jugador escribirá los números
    public InputField inputX;
    public InputField inputY;
    public InputField inputZ;

    private bool panelEnergiaResuelto = false;

    void Start()
    {
        panelVectoresUI.SetActive(false); // Ocultar el teclado de vectores al inicio
        ActualizarUI();
    }

    void Update()
    {
        // Mecánica de cuenta regresiva (Condición de Derrota)
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            textoContador.text = "Soporte de Vida: " + Mathf.Ceil(tiempoRestante).ToString() + "s";
        }
        else
        {
            Defeated();
        }
    }

    // Lo llaman las palancas al ser pulsadas
    public void ModificarEnergia(int cantidad)
    {
        if (panelEnergiaResuelto) return;

        energiaActual += cantidad;
        ActualizarUI();

        // Si llega exactamente a la energía requerida
        if (energiaActual == energiaObjetivo)
        {
            panelEnergiaResuelto = true;
            textoEnergia.text = "SISTEMA ENERGIZADO";
            textoEnergia.color = Color.green;
            panelVectoresUI.SetActive(true); // Se enciende la pantalla de navegación
        }
    }

    void ActualizarUI()
    {
        textoEnergia.text = "Energía: " + energiaActual + " / " + energiaObjetivo + " W";
    }

    // Botón en la UI llamará a esta función cuando el jugador presione "INGRESAR RUTA"
    public void ValidarVector()
    {
        // Convertimos lo que escribió el usuario en números enteros
        int xIngresado = int.Parse(inputX.text);
        int yIngresado = int.Parse(inputY.text);
        int zIngresado = int.Parse(inputZ.text);

        if (xIngresado == vectorCorrectoX && yIngresado == vectorCorrectoY && zIngresado == vectorCorrectoZ)
        {
            Victory();
        }
        else
        {
            // Penalización por fallar la ruta matemática: pierde 20 segundos
            tiempoRestante -= 20f;
            Debug.Log("Coordenadas incorrectas. Rumbo fallido.");
        }
    }

    void Victory()
    {
        Debug.Log("¡Ruta calculada con éxito! Saltando al hiperespacio de vuelta a la Tierra.");
        // Aquí puedes cargar una escena que diga "Ganaste"
        // SceneManager.LoadScene("EscenaVictoria");
    }

    void Defeated()
    {
        Debug.Log("Oxigeno agotado o sistema colapsado. Fin de la partida.");
        // Reinicia la escena actual para volver a intentarlo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
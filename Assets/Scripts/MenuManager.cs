using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void IniciarJuego()
    {
        SceneManager.LoadScene("GamePlay_VECTOR-3_Lvl1"); // Carga la escena del juego
    }

    public void SalirDelJuego()
    {
        Application.Quit();
        Debug.Log("Has salido del juego.");
    }
}

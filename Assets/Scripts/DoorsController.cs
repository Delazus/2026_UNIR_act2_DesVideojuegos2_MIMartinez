using System;
using UnityEngine;

public class DoorsController : MonoBehaviour
{
    public Transform rightDoor, leftDoor;

    public Vector3 rightClosePosition, rightOpenPosition;

    public Vector3 leftClosePosition, leftOpenPosition;
    
    public float animationSpeed = 1f;

    Vector3 targetRightPosition, targetLeftPosition;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip DoorSound;

    [Header("Configuración de Escape Room")]
    // ¡NUEVO! Si está en True, se abre sola. Si está en False, se queda bloqueada.
    public bool puertaActiva = true;

    public GameObject panelUiBloqueo;

    private void Start()
    {
        targetRightPosition = rightClosePosition;
        targetLeftPosition = leftClosePosition;

        audioSource = GetComponent<AudioSource>();

        if (panelUiBloqueo != null)
        {
            panelUiBloqueo.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (puertaActiva)
            {
                OpenDoor();
            }
            else
            {
                // BLOQUEADA: Enciende el panel visual en la pantalla
                if (panelUiBloqueo != null)
                {
                    panelUiBloqueo.SetActive(true);
                }

                PlaySound(DoorSound);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (puertaActiva)
            {
                CloseDoor();
            }
            else
            {
                // AL ALEJARSE: Apaga el panel visual de la pantalla
                if (panelUiBloqueo != null)
                {
                    panelUiBloqueo.SetActive(false);
                }
            }
        }
    }

    private void CloseDoor()
    {
        targetRightPosition = rightClosePosition;
        targetLeftPosition = leftClosePosition;

    }

    private void OpenDoor()
    {
        targetRightPosition = rightOpenPosition;
        targetLeftPosition = leftOpenPosition;

        PlaySound(DoorSound);
    }

    private void Update()
    {
        rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, targetRightPosition, Time.deltaTime * animationSpeed);
        leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, targetLeftPosition, Time.deltaTime * animationSpeed);
    }
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }
}

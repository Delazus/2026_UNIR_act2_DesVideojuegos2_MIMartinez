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
    [SerializeField] private AudioClip openDoorSound;

    private void Start()
    {
        targetRightPosition = rightClosePosition;
        targetLeftPosition = leftClosePosition;

        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseDoor();
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

        PlaySound(openDoorSound);
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

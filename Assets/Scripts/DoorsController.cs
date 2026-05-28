using System;
using UnityEngine;

public class DoorsController : MonoBehaviour
{
    public Transform rightDoor, leftDoor;

    public Vector3 rightClosePosition, rightOpenPosition;

    public Vector3 leftClosePosition, leftOpenPosition;
    
    public float animationSpeed = 1f;

    Vector3 targetRightPosition, targetLeftPosition;

    private void Start()
    {
        targetRightPosition = rightClosePosition;
        targetLeftPosition = leftClosePosition;
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
    }

    private void Update()
    {
        rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, targetRightPosition, Time.deltaTime * animationSpeed);
        leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, targetLeftPosition, Time.deltaTime * animationSpeed);
    }
}

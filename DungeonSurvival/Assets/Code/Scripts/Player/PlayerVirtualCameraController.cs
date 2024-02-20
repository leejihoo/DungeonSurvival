using Cinemachine;
using UnityEngine;

public class PlayerVirtualCameraController : MonoBehaviour
{
    private void Awake()
    {
        var virtualCamera = GetComponent<CinemachineVirtualCamera>();
        var player = GameObject.FindWithTag("Player").transform;
        virtualCamera.Follow = player;
        virtualCamera.LookAt = player;
    }
}

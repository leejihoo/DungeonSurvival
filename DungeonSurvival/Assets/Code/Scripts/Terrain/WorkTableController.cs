using UnityEngine;
using UnityEngine.InputSystem;

public class WorkTableController : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    private void OnTriggerEnter2D(Collider2D col)
    {
        canvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        canvas.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            canvas.SetActive(false);
        }
    }
}

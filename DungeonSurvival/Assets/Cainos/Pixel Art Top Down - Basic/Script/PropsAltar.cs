using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

//when something get into the alta, make the runes glow
namespace Cainos.PixelArtTopDown_Basic
{

    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;
        public CinemachineVirtualCamera Camera;
        [SerializeField] private Transform player;
        [SerializeField] private Transform linkedAltar;

        private Color curColor;
        private Color targetColor;

        private void OnTriggerEnter2D(Collider2D other)
        {
            targetColor = new Color(1, 1, 1, 1);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (Keyboard.current.ctrlKey.wasPressedThisFrame)
            {
                Debug.Log("작동");
                player.transform.position = linkedAltar.position;
                linkedAltar.GetComponent<PropsAltar>().Camera.Priority = 10;
                Camera.Priority = 1;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            targetColor = new Color(1, 1, 1, 0);
        }

        private void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            foreach (var r in runes)
            {
                r.color = curColor;
            }
        }
    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Edited
{
    public class MouseCameraDrag : MonoBehaviour
    {
        private Vector3 startPosition;
        [SerializeField] private float force; 
        public bool dragMode;

        [SerializeField] private GameManager gameManager;
        // Start is called before the first frame update
        void Start()
        {
            startPosition = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            if (dragMode && Input.GetMouseButton(0))
            {
                Vector3 direction = Camera.main.ScreenToViewportPoint(Input.mousePosition - startPosition);
                Camera.main.transform.Translate(Time.deltaTime*force*(-direction));
            }
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }

        }
        private void LateUpdate()
        {
            startPosition = Input.mousePosition;
        }

        public void SetDragMode(bool value)
        {
            dragMode = value;
        }

    }
}

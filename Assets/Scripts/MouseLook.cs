using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity = 100f;

    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InputHandler.instance.OnMouseMove += HandleMouse;
    }

    private void OnDestroy ()
    {
        InputHandler.instance.OnMouseMove -= HandleMouse;
    }

    private void HandleMouse (float mouseX, float mouseY)
    {
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    // Update is called once per frame
    private void Update()
    {
        /* DEPRECATED
        if (canLook)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY; // vai reduzir bazeado no mouse Y
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // limita a rotacao pra 180  graus (a cabeça nao gira)

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
        } */
    }
}

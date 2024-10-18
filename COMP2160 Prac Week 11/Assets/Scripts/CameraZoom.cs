using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private Actions actions;
    private InputAction scrollAction;
    [SerializeField] float zoomModifier = 1;

    void Awake()
    {
        actions = new Actions();
        scrollAction = actions.camera.zoom;
    }

    void OnEnable()
    {
        actions.camera.Enable();
    }

    void OnDisable()
    {
        actions.camera.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cameraZoom();
    }

    void cameraZoom()
    {
        if (Camera.main.orthographic)
        {
            Camera.main.orthographicSize -= (scrollAction.ReadValue<float>() / zoomModifier);
            if (Camera.main.orthographicSize <= 0)
            {
                Camera.main.orthographicSize = 0.1f;
            }
        }
        else
        {
            Camera.main.fieldOfView -= (scrollAction.ReadValue<float>() / zoomModifier);
        }
    }
}

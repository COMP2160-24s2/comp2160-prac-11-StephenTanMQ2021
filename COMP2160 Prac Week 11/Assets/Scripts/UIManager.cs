/**
 * A singleton class to allow point-and-click movement of the marble.
 * 
 * It publishes a TargetSelected event which is invoked whenever a new target is selected.
 * 
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using System;
using UnityEngine;
using UnityEngine.InputSystem;

// note this has to run earlier than other classes which subscribe to the TargetSelected event
[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
    #region UI Elements
    [SerializeField] private Transform crosshair;
    [SerializeField] private Transform target;
    #endregion

    [SerializeField] bool deltaCrosshairMovement = false;
    [SerializeField] BoxCollider ground;
    [SerializeField] float mouseSensitivity = 10f;
    private Plane boardPlane;
    Vector2 mousePos = new Vector2(0, 0);

    #region Singleton
    static private UIManager instance;
    static public UIManager Instance
    {
        get { return instance; }
    }
    #endregion

    #region Actions
    private Actions actions;
    private InputAction mouseAction;
    private InputAction deltaAction;
    private InputAction selectAction;
    #endregion

    #region Events
    public delegate void TargetSelectedEventHandler(Vector3 worldPosition);
    public event TargetSelectedEventHandler TargetSelected;
    #endregion

    #region Init & Destroy
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one UIManager in the scene.");
        }

        instance = this;

        actions = new Actions();
        mouseAction = actions.mouse.position;
        deltaAction = actions.mouse.delta;
        selectAction = actions.mouse.select;

        Cursor.visible = false;
        target.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        actions.mouse.Enable();
    }

    void OnDisable()
    {
        actions.mouse.Disable();
    }
    #endregion Init

    void Start()
    {
        Vector3 planePosition = ground.transform.position + ground.transform.up * ((ground.size.y / 2) + 0.01f);
        boardPlane = new Plane(ground.transform.up, planePosition);
    }

    #region Update
    void Update()
    {
        MoveCrosshair();
        SelectTarget();
    }

    private void MoveCrosshair()
    {
        if (deltaCrosshairMovement)
        {
            mousePos = mousePos + deltaAction.ReadValue<Vector2>();
            crosshair.position = new Vector3(mousePos.x, crosshair.position.y, mousePos.y) / mouseSensitivity;

            // Clamp
            float x = Mathf.Clamp(Camera.main.WorldToScreenPoint(crosshair.position).x, 0f, Camera.main.pixelWidth);
            float y = Mathf.Clamp(Camera.main.WorldToScreenPoint(crosshair.position).y, 0f, Camera.main.pixelHeight);
            Vector3 treatedPosition = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 9.99f));
            crosshair.position = new Vector3(treatedPosition.x, crosshair.position.y, treatedPosition.z);
        }
        else
        {
            mousePos = mouseAction.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (boardPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                crosshair.position = hitPoint;
            }
        }
    }

    private void SelectTarget()
    {
        if (selectAction.WasPerformedThisFrame())
        {
            // set the target position and invoke 
            target.gameObject.SetActive(true);
            target.position = crosshair.position;
            TargetSelected?.Invoke(target.position);
        }
    }

    #endregion Update

}

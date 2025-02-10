using UnityEngine;
using UnityEngine.EventSystems;

public class RTSCameraController : MonoBehaviour
{
    public static RTSCameraController Instance;

    [Header("General")]
    [SerializeField] Transform CameraTransform;

    public Transform folloeTransform;

    Vector3 newPosition;
    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;

    [Header("Optional Functionality")]
    [SerializeField] bool moveWithKeyboard;
    [SerializeField] bool moveWithEdgeScrolling;
    [SerializeField] bool moveWithMouseDrag;

    [Header("Keyboard Movement")]
    [SerializeField] float fastSpeed = 0.05f;
    [SerializeField] float normalSpeed = 0.01f;
    [SerializeField] float movementSensitivity = 1f;
    float movementSpeed;

    [Header("Edge Scrolling Movement")]
    [SerializeField] float edgeSize = 50f;
    bool isCursorSet = false;
    public Texture2D cursorArrowUp;
    public Texture2D cursorArrowDown;
    public Texture2D cursorArrowLeft;
    public Texture2D cursorArrowRight;

    CursorArrow currentCursor = CursorArrow.DEFAULT;

    enum CursorArrow
    {
        UP, DOWN, LEFT, RIGHT, DEFAULT
    }

    private void Start()
    {
        Instance = this;
        newPosition = transform.position;
        movementSpeed = normalSpeed;
    }

    private void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        else
        {
            HandleCameraMovement();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }
    }

    void HandkeCameraMovement()
    {
        if (moveWithMouseDrag)
        {
            HandleMouseDragInput();
        }

        if (moveWithKeyboard)
        {
            if (Input.GetKey(KeyCode.LeftCommand))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * movementSpeed);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition -= (transform.forward * movementSpeed);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * movementSpeed);
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition -= (transform.forward * movementSpeed);
            }
        }

        if (moveWithEdgeScrolling)
        {
            if (isCursorSet)
            {
                ChangeCursor(CursorArrow.DEFAULT);
                isCursorSet = false;
            }
            else
            {
                if (Input.mousePosition.y > Screen.height - edgeSize)
                {
                    // Move Up
                    newPosition += (transform.forward * movementSpeed);
                    ChangeCursor(CursorArrow.UP);
                }
                else if (Input.mousePosition.y < edgeSize)
                {
                    // Move Down
                    newPosition -= (transform.forward * movementSpeed);
                    ChangeCursor(CursorArrow.DOWN);
                }
                else if (Input.mousePosition.x > Screen.width - edgeSize)
                {
                    // Move Right
                    newPosition += (transform.right * movementSpeed);
                    ChangeCursor(CursorArrow.RIGHT);
                }
                else if (Input.mousePosition.x < edgeSize)
                {
                    // Move Left
                    newPosition -= (transform.right * movementSpeed);
                    ChangeCursor(CursorArrow.RIGHT);
                }
                isCursorSet = true;
            }
        }
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementSensitivity);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void ChangeCursor(CursorArrow newCursor)
    {
        if (currentCursor != newCursor)
        {
            switch (newCursor)
            {
                case CursorArrow.UP:
                    Cursor.SetCursor(cursorArrowUp, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.DOWN:
                    Cursor.SetCursor(cursorArrowDown, new Vector2(cursorArrowDown.width, cursorArrowDown.height), CursorMode.Auto);
                    break;
                case CursorArrow.LEFT:
                    Cursor.SetCursor(cursorArrowLeft, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.RIGHT:
                    Cursor.SetCursor(cursorArrowRight, new Vector2(cursorArrowRight.width, cursorArrowDown.height), CursorMode.Auto);
                    break;
                case CursorArrow.DEFAULT:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto)
                    break;
            }
            currentCursor = newCursor;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionBox : MonoBehaviour
{
    Camera myCam;
    [SerializeField]

    RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    private void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            selectionBox = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
            if (boxVisual.rect.width > 0 || boxVisual.rect.height > 0)
            {
                UnitSelectionManager.Instance.DeselectAll();
                SelectUnits();
            }

            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectUnits();

            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual()
    {
        // calculate the starting and the ending position of the selection box
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;

        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if (Input.mousePosition.x < startPosition.x)
        {
            selectionBox.xMax = startPosition.x;
            selectionBox.xMin = Input.mousePosition.x;
        }
        else
        {
            selectionBox.xMax = Input.mousePosition.x;
            selectionBox.xMin = startPosition.x;
        }

        if (Input.mousePosition.y < startPosition.y)
        {
            selectionBox.yMax = startPosition.y;
            selectionBox.yMin = Input.mousePosition.y;
        }
        else
        {
            selectionBox.yMax = Input.mousePosition.y;
            selectionBox.yMin = startPosition.y;
        }
    }

    void SelectUnits()
    {
        foreach (var unit in UnitSelectionManager.Instance.allUnitsList)
        {
            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelectionManager.Instance.DragSelect(unit);
            }
        }
    }
}

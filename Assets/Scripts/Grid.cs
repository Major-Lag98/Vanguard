using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grid : ScriptableObject
{
    int _width;

    int _height;

    float _cellSize;

    Vector3 _originPosition;

    int[,] gridArray;

    TextMesh[,] debugTextArray;


    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int  x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetworldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 5, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetworldPosition(x, y), GetworldPosition(x, y + 1), Color.red, 100f);
                Debug.DrawLine(GetworldPosition(x, y), GetworldPosition(x + 1, y), Color.red, 100f);
            }
        }
        Debug.DrawLine(GetworldPosition(0, height), GetworldPosition(width,height), Color.red, 100f);
        Debug.DrawLine(GetworldPosition(width, 0), GetworldPosition(width, height), Color.red, 100f);

        SetValue(2, 1, 56);
    }

    public Vector3 GetworldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x/ _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < _width && y < _height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
        
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}

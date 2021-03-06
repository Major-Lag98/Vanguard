﻿using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
public class Grid : MonoBehaviour, ISerializationCallbackReceiver
{
    public enum GridCellType { None, Tower, Crops, Blocked}

    [SerializeField]
    //[HideInInspector]
    int _width;

    [SerializeField]
    //[HideInInspector]
    int _height;

    [SerializeField]
    //[HideInInspector]
    float _cellSize;

    [SerializeField]
    //[HideInInspector]
    Vector3 _originPosition;

    [SerializeField]
    [HideInInspector]
    int[,] gridArray;

    [SerializeField]
    [HideInInspector]
    int[] flatGrid;

    public static Grid Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (gridArray == null)
        {
            Generate(_width, _height, _cellSize, _originPosition);
            // Then deserialize the flat grid into the new grid
            LoadGrid(flatGrid);
            flatGrid = new int[0];
        }
    }

    public float GetCellSize()
    {
        return _cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < _width && y < _height)
        {
            gridArray[x, y] = value;
            //debugTextArray[x, y].text = gridArray[x, y].ToString();
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

    public void GenerateGrid()
    {
        Generate(_width, _height, _cellSize, _originPosition);
    }

    void Generate(int width, int height, float cellSize, Vector3 originPosition)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._originPosition = originPosition;

        gridArray = new int[width, height];
    }

    private void OnDrawGizmos()
    {
        if (gridArray == null)
            return;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var value = GetValue(x, y);
                var woorld = GetWorldPosition(x, y) + new Vector3(0.5f, 0.5f);
                Color color;

                if (value == 0)
                    color = new Color(0f, 0f, 0f, 0f);
                else if (value == 1)
                    color = Color.yellow;
                else if (value == 2)
                    color = Color.green;
                else
                    color = Color.red;

                color.a = 0.3f;

                Gizmos.color = color;
                Gizmos.DrawCube(woorld, new Vector3(0.9f, 0.9f, 1));
            }
        }
    }

    private int[] FlattenGrid()
    {
        var width = gridArray.GetLength(0);
        var flatArray = new int[width * gridArray.GetLength(1)];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                flatArray[x + y * width] = GetValue(x, y);

            }
        }

        return flatArray;
    }

    private void LoadGrid(int[] flatArray)
    {
        var width = gridArray.GetLength(0);
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                gridArray[x, y] = flatArray[x + y * width];

            }
        }
    }

    public void OnBeforeSerialize()
    {
        flatGrid = FlattenGrid();
    }

    public void OnAfterDeserialize()
    {
        if (gridArray == null)
        {
            Generate(_width, _height, _cellSize, _originPosition);
            LoadGrid(flatGrid);
            flatGrid = new int[0];
        }
    }
}

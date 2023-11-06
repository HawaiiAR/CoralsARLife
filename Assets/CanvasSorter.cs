using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSorter : MonoBehaviour
{
    [SerializeField] private Canvas[] _UICanvases;

    void Start()
    {
        _UICanvases[0].sortingOrder = 8;
        _UICanvases[1].sortingOrder = 6;
        _UICanvases[2].sortingOrder = 4;
    }

    public void ReOrderCanvases()
    {
        _UICanvases[0].sortingOrder = 2;
        _UICanvases[1].sortingOrder = 6;
        _UICanvases[2].sortingOrder = 4;
    }
}

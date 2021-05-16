using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draw2DLine : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject _line;

    private List<Vector3> _linePoints;

    private LineRenderer _renderer;

    [SerializeField]
    private Material _lineMaterial;

    private bool _drawing;

    private Vector3 GetMousePos() => Input.mousePosition;

    private int _indexes;

    private void SetLineRendererPos()
    {
        _renderer.SetPosition(_indexes, Camera.main.ScreenToWorldPoint(GetMousePos())); ;
    }

    public void OnPointerDown(PointerEventData eventData) => StartDrawing();
   
    private void StartDrawing()
    {
        _drawing = true;
    }

    public void OnPointerUp(PointerEventData eventData) => StopDrawing();

    private void StopDrawing()
    {
        _drawing = false;
        
        _renderer.useWorldSpace = false;

        _linePoints.Clear();

        Start();

        //Rigidbody rigidbody = _line.AddComponent<Rigidbody>();
        //rigidbody.useGravity = false;        
    }

    // Start is called before the first frame update
    void Start()
    {
        _indexes = 0;

        _linePoints = new List<Vector3>();
        _line = new GameObject();

        _renderer = _line.AddComponent<LineRenderer>();
        _renderer.startColor = _renderer.endColor = Color.red;
        _renderer.material = _lineMaterial;
        _renderer.startWidth = 0.2f;
    
    }

    // Update is called once per frame
    void Update()
    {
        if (_drawing)
        {
            Debug.Log(_indexes);
            _renderer.positionCount = _indexes++ + 1;
            SetLineRendererPos();
        }
        
    }

}

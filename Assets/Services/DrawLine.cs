using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawLine : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private GameObject _lineGameObject;

    private Vector3 _mousePosition;

    private LineRenderer _lineRenderer;

    [SerializeField]
    private Material _lineMat;

    private int _currentIndex;

    [SerializeField]
    private Camera _cam;

    [SerializeField]
    private Transform _colliderPrefab;

    private Transform _lastInstantiatedCollider;

    /**
     *  
     */
    private bool _drawingStarted;

    public void OnPointerDown(PointerEventData data)
    {
        
        _drawingStarted = true;
        _mousePosition = Input.mousePosition;

        _lineRenderer = _lineGameObject.AddComponent<LineRenderer>();

        _lineRenderer.startWidth = 0.2f;

        _lineRenderer.material = _lineMat;

    }

    public void OnPointerUp(PointerEventData data)
    {
        _drawingStarted = false;

        _lineGameObject.AddComponent<Rigidbody>();

        _lineGameObject.GetComponent<Rigidbody>().useGravity = false;

        _lineRenderer.useWorldSpace = false;

        Destroy(_lastInstantiatedCollider.gameObject);

        Start();

        _currentIndex = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        _lineGameObject = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        if(_drawingStarted)
        {

            // Vector3 Distance = _mousePosition - Input.mousePosition;
            // float DistanceSquareMagnitude = Distance.sqrMagnitude;

            if ((_mousePosition - Input.mousePosition).sqrMagnitude > 1000f)
            {
                SetLineRendererPos();

                if(_lastInstantiatedCollider != null)
                {
                    Vector3 currentWorldPosition = _lineRenderer.GetPosition(_currentIndex);
                    _lastInstantiatedCollider.gameObject.SetActive(true);

                    _lastInstantiatedCollider.LookAt(currentWorldPosition);

                    if (_lastInstantiatedCollider.rotation.y == 0)
                    {
                        _lastInstantiatedCollider.eulerAngles = new Vector3(_lastInstantiatedCollider.rotation.eulerAngles.x, 90, _lastInstantiatedCollider.rotation.eulerAngles.z);
                    }

                    _lastInstantiatedCollider.localScale = new Vector3(_lastInstantiatedCollider.localScale.x, _lastInstantiatedCollider.localScale.y, Vector3.Distance(_lastInstantiatedCollider.position, currentWorldPosition) * 2f);

                }

                _lastInstantiatedCollider = Instantiate(_colliderPrefab, _lineRenderer.GetPosition(_currentIndex), Quaternion.identity, _lineGameObject.transform);

                _lastInstantiatedCollider.gameObject.SetActive(false);

                _mousePosition = Input.mousePosition;

                _currentIndex++;

                _lineRenderer.positionCount = _currentIndex + 1;

                SetLineRendererPos();

            }

        }    
    }

    private void SetLineRendererPos()
    {
        _lineRenderer.SetPosition(_currentIndex, _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10f)));
    }
}

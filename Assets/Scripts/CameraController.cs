using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    private float minX, maxX, minY, maxY;
    private bool hasBounds = false;
    private Transform target;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetBounds(float _minX, float _maxX, float _minY, float _maxY)
    {
        minX = _minX; maxX = _maxX; minY = _minY; maxY = _maxY;
        hasBounds = true;
    }

    public void ClearBounds()
    {
        hasBounds = false;
    }

    private void LateUpdate()
    {
        if (!target) return;
        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.y = target.position.y;
        if (hasBounds)
        {
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        }
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
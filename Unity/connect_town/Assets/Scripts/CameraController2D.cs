using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    public float panSpeed = 1.0f; //カメラ移動の速さ
    public SpriteRenderer mapRenderer; // マップのSpriteRenderer参照
    private Vector3 lastPanPosition;
    private bool isPanning = false;
    void Start()
    {

    }

    void Update()
    {
        //PC
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
            isPanning = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }

        if (isPanning && Input.GetMouseButton(0))
        {
            Vector3 currentPanPosition = Input.mousePosition;
            PanCamera(currentPanPosition - lastPanPosition);
            lastPanPosition = currentPanPosition;
        }

        //Mobile
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
                isPanning = true;
            }
            else if (touch.phase == TouchPhase.Moved && isPanning)
            {
                Vector3 currentPanPosition = touch.position;
                PanCamera(currentPanPosition - lastPanPosition);
                lastPanPosition = currentPanPosition;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isPanning = false;
            }
        }
    }
    private void PanCamera(Vector3 delta)
    {
        Camera cam = GetComponent<Camera>();
        if (cam == null) return;

        // スクリーン座標の差分をワールド単位に変換
        float worldUnitsPerPixel = (cam.orthographicSize * 2f) / Screen.height;
        Vector3 move = new Vector3(-delta.x * worldUnitsPerPixel,
                                   -delta.y * worldUnitsPerPixel,
                                   0);

        transform.position += move * panSpeed;

        // カメラ位置をマップ内に制限
        if (mapRenderer != null)
        {
            Bounds mapBounds = mapRenderer.bounds;
            float halfHeight = cam.orthographicSize;
            float halfWidth = cam.orthographicSize * cam.aspect;

            float minX = mapBounds.min.x + halfWidth;
            float maxX = mapBounds.max.x - halfWidth;
            float minY = mapBounds.min.y + halfHeight;
            float maxY = mapBounds.max.y - halfHeight;

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;
        }
    }
}


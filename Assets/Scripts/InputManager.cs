using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Tilemap tilemap;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;

            Vector3Int cell = tilemap.WorldToCell(worldPos);

            Debug.Log(cell);
        }
    }
}
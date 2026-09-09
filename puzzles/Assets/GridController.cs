using UnityEngine.Tilemaps;
using UnityEngine;

public class GridController : MonoBehaviour
{
    // singleton instance
    public static GridController instance;


    private Grid grid;
    private Tilemap tilemap;

    private void Start()
    {
        // is there already a grid controller in our scene?
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        // otherwise, set ourselves as the singleton instance
        instance = this;

        grid = GetComponent<Grid>();
        if (grid == null)
        {
            Debug.LogError("there's no grid...did you add this to the wrong place???");
        }

        tilemap = transform.Find("Obstacles").GetComponent<Tilemap>();
    }


    public Vector3 GridToWorldPos(int x, int y)
    {
        return grid.CellToWorld(new Vector3Int(x, y, 0));
    }
}
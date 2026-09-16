using UnityEngine;
using UnityEngine.Tilemaps;

// singleton: a class that you can call from anywhere
public class GridController : MonoBehaviour
{
    public TileBase blockTile;

    // singleton instance
    public static GridController instance;


    private Grid grid;
    private Tilemap tilemap;

    private void Awake()
    {
        // is there already a grid controller in our scene?
        if (instance != null)
        {
            // if so, there should only be one per scene....this was a mistake!! destroy ourselves!!!
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

        // NOTE: make sure "Interactables" equals the name of the GameObject that contains your obstacles tilemap
        tilemap = transform.Find("Interactables").GetComponent<Tilemap>();
    }


    public Vector3 GridToWorldPos(int x, int y)
    {
        return grid.CellToWorld(new Vector3Int(x, y, 0));
    }


    public string GetTile(int x, int y)
    {
        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
        if (tile == null)
        {
            return null;
        }

        return tile.name;
    }


    public void PushBlock(Vector3Int start, Vector3Int end, int xmove, int ymove)
    {
        // if there's a block ahead of us...push it!
        if (GetTile(end.x, end.y) == "Box")
        {
            // calculate the block in front of us's start and end pos
            Vector3Int blockStart = end;
            Vector3Int blockEnd = blockStart + new Vector3Int(xmove, ymove, 0);
            // and push it!
            PushBlock(blockStart, blockEnd, xmove, ymove);
        }

        // erase where the block currently is
        tilemap.SetTile(start, null);
        // draw a box where it ends up
        tilemap.SetTile(end, blockTile);
    }


    public bool CanPushBlock(Vector3Int blockStart, Vector3Int blockEnd)
    {
        // keep looking in front of the block until we find something
        // that is NOT a block (empty / wall / etc)
        Vector3Int direction = blockEnd - blockStart;
        while (GetTile(blockEnd.x, blockEnd.y) == "Box")
        {
            blockEnd += direction;
        }

        // is the space in front of the blocks empty? 
        // if so, we can push!!
        string target = GetTile(blockEnd.x, blockEnd.y);
        if (target == null || target == "tree" || target == "goal")
        {
            return true;
        }

        return false;
    }
}
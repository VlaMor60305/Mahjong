using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameConfig gameConfig;

    public Slot[,,] slots;

    public Tile selectedTile { get; private set; }
    public int tilesCount { get; private set; } = 0;
    public bool isAutoSolving { get; private set; } = false;


    #region Initialization
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void OnEnable()
    {
        GameEvents.OnAutoSolvingStateChange.AddListener(SolvingState);
    }

    private void OnDisable()
    {

        GameEvents.OnAutoSolvingStateChange.RemoveListener(SolvingState);
    }
    #endregion
    #region Work with Tiles
    public void SelectTile(Tile newTile)
    {
        if(selectedTile != null && selectedTile.tileTypeIndex == newTile.tileTypeIndex && selectedTile != newTile)
        {
            selectedTile.DeleteTile();
            newTile.DeleteTile();
            selectedTile = null;
            ChangeTilesCount(-2);
            GameEvents.ChangeTiles();
        }
        else
        {
            selectedTile = newTile;
        }
    }

    public void ResetSelection()
    {
        selectedTile = null;
    }

    public void ChangeTilesCount(int delta)
    {
        tilesCount += delta;
    }

    void SolvingState(bool isSolving)
    {
        isAutoSolving = isSolving;
    }
    #endregion
    #region Checking
    public bool IsAvailable(int row, int col, int layer)
    {
        if (slots[row, col, layer] == null)
        {
            return false;
        }

        if (layer < slots.GetLength(2) - 1 && slots[row, col, layer + 1] != null && slots[row, col, layer + 1].tile != null)
        {
            return false;
        }

        if (IsBlockedOnBothSides(row, col, layer))
        {
            return false;
        }
        return true;
    }

    bool IsBlockedOnBothSides(int row, int col, int layer)
    {
        bool blockedLeft = row > 0 && slots[row-1, col, layer] != null && slots[row-1, col, layer].tile != null;
        bool blockedRight = row < slots.GetLength(0) - 1 && slots[row+1, col, layer] != null && slots[row+1, col, layer].tile != null;

        return blockedLeft && blockedRight;
    }
    #endregion
}

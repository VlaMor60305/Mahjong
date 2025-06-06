using UnityEngine;

public class Slot
{
    public Tile tile;
    public Vector3 position { get; private set; }

    public Slot(Vector3 position)
    {
        this.position = position;
    }
}

public class Generator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    private int width = 10;
    private int height = 10;
    private int depth = 1;
    
    private float offset = 0.5f;

    void Start()
    {
        width = GameManager.instance.gameConfig.width;
        height = GameManager.instance.gameConfig.height;
        depth = GameManager.instance.gameConfig.depth;
        offset = GameManager.instance.gameConfig.tileOffsetY;

        GenerateField();
    }

    public void RegenerateField()
    {
        if (GameManager.instance.isAutoSolving == true)
        {
            Debug.LogWarning("Don't allowed to generate level while autosolving");
            return;
        }

        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    if (GameManager.instance.slots[j, k, i] != null && GameManager.instance.slots[j, k, i].tile != null)
                    {
                        Destroy(GameManager.instance.slots[j, k, i].tile.gameObject);
                    }

                    GameManager.instance.slots[j, k, i] = null;
                }
            }
        }
        GameManager.instance.ChangeTilesCount(-GameManager.instance.tilesCount);
        GenerateField();
    }
    #region generation
    public void GenerateField()
    {
        if((width * height) % 4 != 0)
        {
            Debug.LogError("The field size isn't supported");
            return;
        }
        else
        {
            GameManager.instance.slots = new Slot[width , height, depth];
        }

        for (int i = 0; i < depth; i++)
        {
            for(int j = i; j < width - i; j++)
            {
                for(int k = i; k < height - i; k++)
                {
                    GameManager.instance.slots[j, k, i] = new Slot(new Vector3(j, k, i));
                }
            }
             
        }

        for (int i = 0; i < depth; i++)
        {
            for (int j = i; j < width; j++)
            {
                for (int k = i; k < height; k++)
                {
                    if (GameManager.instance.slots[j, k, i] != null)
                    {
                        if (GameManager.instance.slots[j, k, i].tile == null)
                        {
                            GameObject tileGO = Instantiate(tilePrefab, 
                                new Vector3(GameManager.instance.slots[j, k, i].position.x, GameManager.instance.slots[j, k, i].position.y + offset, GameManager.instance.slots[j, k, i].position.z), 
                                Quaternion.identity);
                            Tile tile = tileGO.GetComponent<Tile>();
                            int type = Random.Range(0, GameManager.instance.gameConfig.tileSpriteVariants.Length);
                            tile.Initialize(GameManager.instance.slots[j, k, i], type);
                            GameManager.instance.ChangeTilesCount(1);
                            GeneratePairTile(GameManager.instance.slots[j, k, i], type);
                        }
                    }
                }
            }
        }

        GameEvents.EndGeneration();
    }

    public void GeneratePairTile(Slot slot, int type)
    {
        Slot avaibleSlot = GameManager.instance.slots[width - (int)slot.position.x - 1, height - (int)slot.position.y - 1, (int)slot.position.z];
        GameObject tileGO = Instantiate(tilePrefab, avaibleSlot.position, Quaternion.identity);
        Tile tile = tileGO.GetComponent<Tile>();
        tile.Initialize(avaibleSlot, type);
        GameManager.instance.ChangeTilesCount(1);
    }
    #endregion
}

using UnityEngine;
using UnityEngine.EventSystems;



public class Tile : MonoBehaviour, IPointerClickHandler
{
    private SpriteRenderer _spriteRenderer;
    public bool isAvaible { get; private set; }

    private Slot _slot;
    public int tileTypeIndex { get; private set; }


    #region Initialization
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    public void DeleteTile()
    {
        _slot.tile = null;
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnGenerationEnded.AddListener(CheckMyAvailability);
        GameEvents.OnTilesChanged.AddListener(CheckMyAvailability);
    }

    private void OnDisable()
    {
        GameEvents.OnGenerationEnded.RemoveListener(CheckMyAvailability);
        GameEvents.OnTilesChanged.RemoveListener(CheckMyAvailability);
    }

    public void Initialize(Slot slot, int typeIndex)
    {
        slot.tile = this;
        tileTypeIndex = typeIndex;
        _spriteRenderer.sprite = GameManager.instance.gameConfig.tileSpriteVariants[typeIndex];
        _spriteRenderer.sortingOrder = (int)transform.position.z;
        _slot = slot;
        CheckMyAvailability();
    }
    #endregion
    public void CheckMyAvailability()
    {
        if (GameManager.instance.IsAvailable((int)_slot.position.x, (int)_slot.position.y, (int)_slot.position.z))
        {
            _spriteRenderer.color = new Color(1, 1, 1);
            isAvaible = true;
        }
        else
        {
            _spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
            isAvaible = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAvaible)
        {
            GameManager.instance.SelectTile(this);
        }
        else
        {
            CheckMyAvailability();
        }
    }
}

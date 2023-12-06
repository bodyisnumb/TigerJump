using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggablePower : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum PowerType
    {
        Battery,
        Bomb,
        Shield
    }

    public enum TreeColor
    {
        PinkTree,
        YellowTree,
        BlueTree
    }

    public PowerType powerType;
    private TreeBarDecayFill treeBarDecayFill;
    private UIManagerGame uIManagerGame;
    private EconomicManager economicManager;


    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Canvas canvas;
    private bool isDragging = false;
    private GameObject draggedObject;

    [Header("Powerd Objects")]
    public GameObject batteryObject;
    public GameObject bombObject;
    public GameObject shieldObject;
    public SoundPlayer soundPlayer;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        treeBarDecayFill = FindObjectOfType<TreeBarDecayFill>();
            if (treeBarDecayFill == null)
            {
                Debug.LogError("TreeBarDecayFill script not found!");
            }

        uIManagerGame = FindObjectOfType<UIManagerGame>();
            if (uIManagerGame == null)
            {
                Debug.LogError("UIManagerGame script not found!");
            }

        economicManager = FindObjectOfType<EconomicManager>();
            if (economicManager == null)
            {
                Debug.LogError("EconomicManager script not found!");
            }

        soundPlayer = FindObjectOfType<SoundPlayer>();
            if (soundPlayer == null)
            {
                Debug.LogError("SoundPlayer script not found!");
            }

        DisableIfCountZero();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        initialPosition = transform.position;
        initialScale = transform.localScale;

        draggedObject = Instantiate(gameObject, transform.position, Quaternion.identity);
        draggedObject.transform.SetParent(canvas.transform);

        Destroy(draggedObject.GetComponent<DraggablePower>());
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogError("Canvas is not in World Space mode!");
            return;
        }

        Vector3 worldPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position, eventData.pressEventCamera, out worldPosition
        );

        draggedObject.transform.position = worldPosition;
        draggedObject.transform.localScale = initialScale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (draggedObject != null)
        {
            Destroy(draggedObject);
        }

        CheckCollisionAtDropLocation(eventData.position);
    }

    private void CheckCollisionAtDropLocation(Vector3 dropPosition)
    {
        Vector3 worldPosition = Vector3.zero;
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform, dropPosition,
                canvas.worldCamera, out worldPosition
            );
        }
        else
        {
            Debug.LogError("Canvas is not in World Space mode!");
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapPointAll(worldPosition);

        foreach (Collider2D collider in colliders)
        {
            TreeColor treeColor;
            if (System.Enum.TryParse<TreeColor>(collider.gameObject.tag, out treeColor))
            {
                Debug.Log("Power " + powerType.ToString() + " dragged and dropped on " + treeColor.ToString());
                
                switch (powerType)
                {
                    case PowerType.Battery:
                        treeBarDecayFill.FillSliders(treeColor, 1f);
                        soundPlayer.BatterySound();
                        economicManager.DeductBattery();
                        uIManagerGame.UpdateUI();
                        DisableIfCountZero();
                        
                        break;
                    case PowerType.Bomb:
                        treeBarDecayFill.FillSliders(treeColor, 0.5f);
                        soundPlayer.BombSound();
                        economicManager.DeductBomb();
                        uIManagerGame.UpdateUI();
                        DisableIfCountZero();
                        
                        break;
                    case PowerType.Shield:
                        treeBarDecayFill.StopDecayFor(treeColor, 20f);
                        soundPlayer.ShieldSound();
                        economicManager.DeductShield();
                        uIManagerGame.UpdateUI();
                        DisableIfCountZero();
                        
                        break;
                    default:
                        Debug.LogWarning("Invalid power type provided.");
                        break;
                }
            }
        }
    }

    private void DisableIfCountZero()
    {
        int batteryCount = PlayerPrefs.GetInt("BatteryCount", 0);
        int bombCount = PlayerPrefs.GetInt("BombCount", 0);
        int shieldCount = PlayerPrefs.GetInt("ShieldCount", 0);

        if (batteryCount <= 0 && batteryObject != null)
        {
            DisablePowerObject(batteryObject);
        }
        if (bombCount <= 0 && bombObject != null)
        {
            DisablePowerObject(bombObject);
        }
        if (shieldCount <= 0 && shieldObject != null)
        {
            DisablePowerObject(shieldObject);
        }
    }


    private void DisablePowerObject(GameObject powerObject)
    {
        if (powerObject != null)
        {
            powerObject.SetActive(false);
        }
    }

    private void DecreasePowerCount(PowerType type)
    {
        switch (type)
        {
            case PowerType.Battery:
                int batteryCount = PlayerPrefs.GetInt("BatteryCount", 0);
                if (batteryCount > 0)
                {
                    batteryCount--;
                    PlayerPrefs.SetInt("BatteryCount", batteryCount);
                    PlayerPrefs.Save();
                }
                break;
            case PowerType.Bomb:
                int bombCount = PlayerPrefs.GetInt("BombCount", 0);
                if (bombCount > 0)
                {
                    bombCount--;
                    PlayerPrefs.SetInt("BombCount", bombCount);
                    PlayerPrefs.Save();
                }
                break;
            case PowerType.Shield:
                int shieldCount = PlayerPrefs.GetInt("ShieldCount", 0);
                if (shieldCount > 0)
                {
                    shieldCount--;
                    PlayerPrefs.SetInt("ShieldCount", shieldCount);
                    PlayerPrefs.Save();
                }
                break;
            default:
                Debug.LogWarning("Invalid power type provided for count decrease.");
                break;
        }
    }
}


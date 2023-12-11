using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DraggablePaw : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Canvas canvas;
    private bool isDragging = false;
    private GameObject draggedObject;
    private TreeBarDecayFill treeBarDecayFill;
    private UIManagerGame uIManagerGame;

    private static int totalConsecutiveDropsWithoutCollision = 0;
    private const int MAX_CONSECUTIVE_DROPS = 3;

    public GameObject trailPrefab; // Assign your trail prefab in the inspector
    private GameObject trailInstance;

    public ParticleSystem explosionParticlePrefab; // Assign your explosion particle prefab in the inspector
    private ParticleSystem explosionParticleInstance;

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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        initialPosition = transform.position;
        initialScale = transform.localScale;

        draggedObject = Instantiate(gameObject, transform.position, Quaternion.identity);
        draggedObject.transform.SetParent(canvas.transform);

        Destroy(draggedObject.GetComponent<DraggablePaw>());

        // Instantiate the trail prefab
        trailInstance = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        trailInstance.transform.SetParent(canvas.transform);
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

        // Update the trail position
        trailInstance.transform.position = worldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (draggedObject != null)
        {
            Destroy(draggedObject);
        }

        if (trailInstance != null)
        {
            Destroy(trailInstance);
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

        bool collisionDetected = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject.CompareTag(gameObject.tag))
            {
                DraggablePower.TreeColor treeColor = GetTreeColorTag(collider.gameObject.tag);
                Destroy(collider.gameObject);

                uIManagerGame.AddToScoreAndCoins(1);
                treeBarDecayFill.FillSliders(treeColor, 0.2f);
                ResetConsecutiveDropCount();
                collisionDetected = true;
                Debug.Log("Filled Tree Color: " + treeColor);

                // Create explosion particle effect
                CreateExplosionParticle(worldPosition);

                break;
            }
        }

        if (!collisionDetected)
        {
            IncrementConsecutiveDropCount();
        }
    }

    private void IncrementConsecutiveDropCount()
    {
        totalConsecutiveDropsWithoutCollision++;
        if (totalConsecutiveDropsWithoutCollision >= MAX_CONSECUTIVE_DROPS)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void ResetConsecutiveDropCount()
    {
        totalConsecutiveDropsWithoutCollision = 0;
    }

    private DraggablePower.TreeColor GetTreeColorTag(string pawTag)
    {
        switch (pawTag)
        {
            case "Pink":
                return DraggablePower.TreeColor.PinkTree;
            case "Yellow":
                return DraggablePower.TreeColor.YellowTree;
            case "Blue":
                return DraggablePower.TreeColor.BlueTree;
            default:
                return DraggablePower.TreeColor.PinkTree;
        }
    }

    private void CreateExplosionParticle(Vector3 position)
    {
        // Instantiate the explosion particle at the specified position
        explosionParticleInstance = Instantiate(explosionParticlePrefab, position, Quaternion.identity);
        // Play the particle effect
        explosionParticleInstance.Play();
        // Destroy the particle system after it has finished playing
        Destroy(explosionParticleInstance.gameObject, explosionParticleInstance.main.duration);
    }
}



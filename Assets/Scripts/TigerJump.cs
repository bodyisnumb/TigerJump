using System.Collections;
using UnityEngine;

public class TigerJump : MonoBehaviour
{
    public Transform[] jumpPoints;
    public float movementSpeed = 10f;

    public SoundPlayer soundPlayer;

    bool isMoving = false;

    void Start()
    {
        if (jumpPoints.Length != 3)
        {
            Debug.LogError("Please assign three jump points in the inspector.");
            return;
        }

        soundPlayer = FindObjectOfType<SoundPlayer>();
            if (soundPlayer == null)
            {
                Debug.LogError("SoundPlayer script not found!");
            }
    }

    public void JumpToSpecificPoint(int pointIndex)
    {
        if (pointIndex >= 0 && pointIndex < jumpPoints.Length)
        {
            if (!isMoving)
            {
                StartCoroutine(MoveToPoint(jumpPoints[pointIndex]));
                soundPlayer.BushSound();
            }
            else
            {
                Debug.LogWarning("Tiger is already moving.");
            }
        }
        else
        {
            Debug.LogError("Invalid point index.");
        }
    }

    private IEnumerator MoveToPoint(Transform targetPoint)
    {
        isMoving = true;

        while (Vector2.Distance(transform.position, targetPoint.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, movementSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
        Debug.Log("Tiger reached the point.");
    }
}

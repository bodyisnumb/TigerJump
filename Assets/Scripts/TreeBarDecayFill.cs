using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TreeBarDecayFill : MonoBehaviour
{
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public float decaySpeed = 0.1f;

    private float currentValue1;
    private float currentValue2;
    private float currentValue3;

    private bool decayInProgressPink = true;
    private bool decayInProgressYellow = true;
    private bool decayInProgressBlue = true;

    private TigerJump tigerJump;
    private UIManagerGame uIManagerGame;
    public SoundPlayer soundPlayer;

    private void Start()
    {
        slider1.value = 0.5f;
        slider2.value = 0.5f;
        slider3.value = 0.5f;

        currentValue1 = slider1.value;
        currentValue2 = slider2.value;
        currentValue3 = slider3.value;

        InvokeRepeating("DecaySliders", decaySpeed, decaySpeed);

        tigerJump = FindObjectOfType<TigerJump>();
            if (tigerJump == null)
            {
                Debug.LogError("TigerJump script not found!");
            }

        uIManagerGame = FindObjectOfType<UIManagerGame>();
            if (uIManagerGame == null)
            {
                Debug.LogError("UIManagerGame script not found!");
            }
        soundPlayer = FindObjectOfType<SoundPlayer>();
            if (soundPlayer == null)
            {
                Debug.LogError("SoundPlayer script not found!");
            }
    }

    private void Update()
    {
      SetCurrentScore(uIManagerGame.currentScore);  
    }

    public void SetCurrentScore(int score)
    {
        AdjustDecaySpeed(score);
    }

    private void AdjustDecaySpeed(int score)
    {
        decaySpeed = 0.1f - (score * 0.005f);
        decaySpeed = Mathf.Clamp(decaySpeed, 0.01f, 0.1f);
    }

    private void DecaySliders()
    {
        if (decayInProgressPink)
        {
            DecaySlider(ref currentValue1, slider1);
        }
        if (decayInProgressYellow)
        {
            DecaySlider(ref currentValue2, slider2);
        }
        if (decayInProgressBlue)
        {
            DecaySlider(ref currentValue3, slider3);
        }
    }

    private void DecaySlider(ref float currentValue, Slider slider)
    {
        currentValue -= 0.0001f * (1 / decaySpeed);

        slider.value = currentValue;

        if (currentValue <= 0)
        {
            StartCoroutine(FadeOutSlider(slider));
            slider.interactable = false;

            if (slider == slider1)
                decayInProgressPink = false;
            else if (slider == slider2)
                decayInProgressYellow = false;
            else if (slider == slider3)
                decayInProgressBlue = false;
        }

        if (slider1.value <= 0 && slider2.value <= 0 && slider3.value <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private System.Collections.IEnumerator FadeOutSlider(Slider slider)
    {
        CanvasGroup canvasGroup = slider.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = slider.gameObject.AddComponent<CanvasGroup>();
        }

        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }

    public void FillSliders(DraggablePower.TreeColor color, float fillAmount)
    {
        switch (color)
        {
            case DraggablePower.TreeColor.PinkTree:
                FillSlider(ref currentValue1, slider1, fillAmount);
                tigerJump.JumpToSpecificPoint(0);
                soundPlayer.SuccessSound();
                break;
            case DraggablePower.TreeColor.YellowTree:
                FillSlider(ref currentValue2, slider2, fillAmount);
                tigerJump.JumpToSpecificPoint(1);
                soundPlayer.SuccessSound();
                break;
            case DraggablePower.TreeColor.BlueTree:
                FillSlider(ref currentValue3, slider3, fillAmount);
                tigerJump.JumpToSpecificPoint(2);
                soundPlayer.SuccessSound();
                break;
            default:
                Debug.LogWarning("Invalid color provided.");
                break;
        }
    }

    private void FillSlider(ref float currentValue, Slider slider, float fillAmount)
    {
        if (currentValue > 0)
        {
            currentValue = Mathf.Clamp01(currentValue + fillAmount);
            slider.value = currentValue;
            Debug.Log($"{slider.gameObject.name} Filled");
        }
    }

    public void StopDecayFor(DraggablePower.TreeColor color, float duration)
    {
        switch (color)
        {
            case DraggablePower.TreeColor.PinkTree:
                StopCoroutine("ResumeDecayForTree");
                StartCoroutine(ResumeDecayForTree(color, duration));
                break;
            case DraggablePower.TreeColor.YellowTree:
                StopCoroutine("ResumeDecayForTree");
                StartCoroutine(ResumeDecayForTree(color, duration));
                break;
            case DraggablePower.TreeColor.BlueTree:
                StopCoroutine("ResumeDecayForTree");
                StartCoroutine(ResumeDecayForTree(color, duration));
                break;
            default:
                Debug.LogWarning("Invalid color provided.");
                break;
        }
    }

    private System.Collections.IEnumerator ResumeDecayForTree(DraggablePower.TreeColor color, float duration)
    {
        switch (color)
        {
            case DraggablePower.TreeColor.PinkTree:
                decayInProgressPink = false;
                break;
            case DraggablePower.TreeColor.YellowTree:
                decayInProgressYellow = false;
                break;
            case DraggablePower.TreeColor.BlueTree:
                decayInProgressBlue = false;
                break;
            default:
                yield break;
        }

        yield return new WaitForSeconds(duration);

        switch (color)
        {
            case DraggablePower.TreeColor.PinkTree:
                decayInProgressPink = true;
                break;
            case DraggablePower.TreeColor.YellowTree:
                decayInProgressYellow = true;
                break;
            case DraggablePower.TreeColor.BlueTree:
                decayInProgressBlue = true;
                break;
            default:
                yield break;
        }
    }
}

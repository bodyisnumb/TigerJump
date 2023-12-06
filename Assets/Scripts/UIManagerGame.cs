using UnityEngine;
using UnityEngine.UI;

public class UIManagerGame : MonoBehaviour
{
    public Text scoreText;
    public Text batteryCountText;
    public Text bombCountText;
    public Text shieldCountText;

    private EconomicManager economicManager;
    public int currentScore = 0;

    private void Start()
    {
        economicManager = EconomicManager.instance;

        UpdateUI();
    }

    public void UpdateUI()
    {
        scoreText.text = currentScore.ToString();

        batteryCountText.text = economicManager.GetBatteryCount().ToString();
        bombCountText.text = economicManager.GetBombCount().ToString();
        shieldCountText.text = economicManager.GetShieldCount().ToString();
    }

    public void AddToScoreAndCoins(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        economicManager.AddCoins(scoreToAdd);
        UpdateUI();
    }
}

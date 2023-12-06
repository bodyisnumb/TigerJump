using UnityEngine;
using UnityEngine.UI;

public class UIManagerMenu : MonoBehaviour
{
    public Text coinText1;
    public Text coinText2;
    public Text batteryCountText;
    public Text bombCountText;
    public Text shieldCountText;

    public Button buyBatteryButton;
    public Button buyBombButton;
    public Button buyShieldButton;

    private EconomicManager economicManager;

    private void Start()
    {
        economicManager = EconomicManager.instance;

        buyBatteryButton.onClick.AddListener(BuyBattery);
        buyBombButton.onClick.AddListener(BuyBomb);
        buyShieldButton.onClick.AddListener(BuyShield);

        UpdateUI();
    }

    private void UpdateUI()
    {
        coinText1.text = economicManager.GetCoinCount().ToString();
        coinText2.text = economicManager.GetCoinCount().ToString();
        batteryCountText.text = "Batteries:" + economicManager.GetBatteryCount().ToString();
        bombCountText.text = "Bombs:" + economicManager.GetBombCount().ToString();
        shieldCountText.text = "Shields:" + economicManager.GetShieldCount().ToString();
    }

    private void BuyBattery()
    {
        if (economicManager.DeductCoins(25))
        {
            economicManager.AddBattery();
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins to buy battery!");
        }
    }

    private void BuyBomb()
    {
        if (economicManager.DeductCoins(10))
        {
            economicManager.AddBomb();
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins to buy bomb!");
        }
    }

    private void BuyShield()
    {
        if (economicManager.DeductCoins(50))
        {
            economicManager.AddShield();
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins to buy shield!");
        }
    }
}

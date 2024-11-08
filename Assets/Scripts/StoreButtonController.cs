using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreButtonController : MonoBehaviour
{
    public enum ProductType
    {
        STRENGTH, BODIES, BLUE, GREEN, RED, PINK, YELLOW, BLACK, COINS
    };
    public ProductType productType;

    [Header("Product")]
    public int cost;
    public bool acquired;

    [Tooltip("Only if the button is a COLOR")]
    public Material playerMaterial;

    [Header("UI")]
    public TMP_Text nameText;
    public TMP_Text costText;
    Image costIcon;
    Color32 iconOriginalColor;
    public TMP_Text levelText;
    public Image buyBar;

    // Start is called before the first frame update
    void Start()
    {
        costIcon = costText.GetComponentInParent<Image>();
        iconOriginalColor = costIcon.color;
        GetAcquiredProduct();
    }

    // Update is called once per frame
    void Update()
    {
        ControlUI();
    }

    void ControlUI()
    {
        nameText.text = productType.ToString();
        costText.text = acquired ? "ACQUIRED" : cost.ToString();
        levelText.text = SetLevelText();
    }

    string SetLevelText()
    {
        string text = "";

        switch (productType)
        {
            case ProductType.STRENGTH:
                text = GameController.Instance.GetStrengthMultiplier().ToString() + "x";
                break;
            case ProductType.BODIES:
                int amount = GameController.Instance.GetMaxBodies();
                text = GameController.Instance.GetMaxBodies().ToString() + (amount > 1 ? " BODIES" : " BODY");
                break;
            case ProductType.BLUE:
                text = "Change your color to " + productType.ToString();
                break;
            case ProductType.GREEN:
                text = "Change your color to " + productType.ToString();
                break;
            case ProductType.RED:
                text = "Change your color to " + productType.ToString();
                break;
            case ProductType.PINK:
                text = "Change your color to " + productType.ToString();
                break;
            case ProductType.YELLOW:
                text = "Change your color to " + productType.ToString();
                break;
            case ProductType.BLACK:
                text = "Change your color to " + productType.ToString();
                break;
            case ProductType.COINS:
                text = "Dev only: Get more 9999 COINS";
                break;
            default:
                text = "[LEVEL: 0]";
                break;
        }

        return text;
    }

    void GetAcquiredProduct()
    {
        string stringAcquired = GameController.Instance.GetColors();
        int colorNumber;

        switch (productType)
        {
            case ProductType.STRENGTH:
                if (GameController.Instance.GetStrengthMultiplier() >= 2)
                    acquired = true;
                else
                    cost = 100 + GameController.Instance.GetStrengthLevel() * 100;
                    break;
            case ProductType.BODIES:
                if (GameController.Instance.GetMaxBodies() >= 99)
                    acquired = true;
                else
                    cost = GameController.Instance.GetMaxBodies() * 100;
                break;
            case ProductType.BLUE:
                colorNumber = (int)Char.GetNumericValue(stringAcquired[0]);
                if (colorNumber > 0)
                    acquired = true;
                else 
                    acquired = false;
                break;
            case ProductType.GREEN:
                colorNumber = (int)Char.GetNumericValue(stringAcquired[1]);
                if (colorNumber > 0)
                    acquired = true;
                else
                    acquired = false;
                break;
            case ProductType.RED:
                colorNumber = (int)Char.GetNumericValue(stringAcquired[2]);
                if (colorNumber > 0)
                    acquired = true;
                else
                    acquired = false;
                break;
            case ProductType.PINK:
                colorNumber = (int)Char.GetNumericValue(stringAcquired[3]);
                if (colorNumber > 0)
                    acquired = true;
                else
                    acquired = false;
                break;
            case ProductType.YELLOW:
                colorNumber = (int)Char.GetNumericValue(stringAcquired[4]);
                if (colorNumber > 0)
                    acquired = true;
                else
                    acquired = false;
                break;
            case ProductType.BLACK:
                colorNumber = (int)Char.GetNumericValue(stringAcquired[5]);
                if (colorNumber > 0)
                    acquired = true;
                else
                    acquired = false;
                break;
            case ProductType.COINS:
                acquired = false;
                break;
            default:
                break;
        }
    }

    public void BuyProduct()
    {
        // Deduct money
        if (GameController.Instance.CurrentMoney() < cost && !acquired) 
        {
            if (GameController.Instance.CurrentMoney() < cost)
            {
                StopCoroutine(NotEnoughCash());
                StartCoroutine(NotEnoughCash());
            }

            return; 
        }

        if (!acquired)
            GameController.Instance.AddSpendMoney(-cost);

        switch (productType)
        {
            case ProductType.STRENGTH:
                if (GameController.Instance.GetStrengthMultiplier() < 2)
                    GameController.Instance.SetStrengthLevel(0.1f);
                GetAcquiredProduct();
                break;
            case ProductType.BODIES:
                if (GameController.Instance.GetMaxBodies() < 99)
                    GameController.Instance.SetMaxBodies(1);
                GetAcquiredProduct();
                break;
            case ProductType.BLUE:
                if (!acquired)
                {
                    GameController.Instance.SetColorString('1', 0);
                    GetAcquiredProduct();
                }
                else
                {
                    FindFirstObjectByType<PlayerController>().SetPlayerColor(0);
                }
                GameController.Instance.SetCurrentColorIndex(0);
                break;
            case ProductType.GREEN:
                if (!acquired)
                {
                    GameController.Instance.SetColorString('1', 1);
                    GetAcquiredProduct();
                }
                else
                {
                    FindFirstObjectByType<PlayerController>().SetPlayerColor(1);
                }
                GameController.Instance.SetCurrentColorIndex(1);
                break;
            case ProductType.RED:
                if (!acquired)
                {
                    GameController.Instance.SetColorString('1', 2);
                    GetAcquiredProduct();
                }
                else
                {
                    FindFirstObjectByType<PlayerController>().SetPlayerColor(2);
                }
                GameController.Instance.SetCurrentColorIndex(2);
                break;
            case ProductType.PINK:
                if (!acquired)
                {
                    GameController.Instance.SetColorString('1', 3);
                    GetAcquiredProduct();
                }
                else
                {
                    FindFirstObjectByType<PlayerController>().SetPlayerColor(3);
                }
                GameController.Instance.SetCurrentColorIndex(3);
                break;
            case ProductType.YELLOW:
                if (!acquired)
                {
                    GameController.Instance.SetColorString('1', 4);
                    GetAcquiredProduct();
                }
                else
                {
                    FindFirstObjectByType<PlayerController>().SetPlayerColor(4);
                }
                GameController.Instance.SetCurrentColorIndex(4);
                break;
            case ProductType.BLACK:
                if (!acquired)
                {
                    GameController.Instance.SetColorString('1', 5);
                    GetAcquiredProduct();
                }
                else
                {
                    FindFirstObjectByType<PlayerController>().SetPlayerColor(5);
                }
                GameController.Instance.SetCurrentColorIndex(5);
                break;
            case ProductType.COINS:
                GameController.Instance.AddSpendMoney(9999);
                break;
            default:
                break;
        }

        GameController.Instance.SaveData();
    }
    IEnumerator NotEnoughCash()
    {
        costIcon.color = Color.red;
        costText.color = Color.red;

        yield return new WaitForSecondsRealtime(0.5f);

        costIcon.color = iconOriginalColor;
        costText.color = iconOriginalColor;
    }
}

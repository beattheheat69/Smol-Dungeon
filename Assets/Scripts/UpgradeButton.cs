using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField]
    int upgradeCost;
    [SerializeField]
    int id;
    [SerializeField]
    public TextMeshProUGUI costTMP;
    [SerializeField]
    MenuManager gM;
    [SerializeField]
    GameObject costText;
    [SerializeField]
    GameObject check;
    public bool fake;
    public bool bought;

    void OnEnable()
    {
        EvilXPCount.ExpAmountChange += CheckExp;
    }

    void OnDisable()
    {
        EvilXPCount.ExpAmountChange -= CheckExp;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (costTMP != null)
            costTMP.SetText(upgradeCost.ToString().PadLeft(2, '0'));

        if (upgradeCost > EvilXPCount.GetXP())
        {
            transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else if (EvilXPCount.upgrades[id] && !fake)
        {
            transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
            check.SetActive(true);
            costText.SetActive(false);
        }
    }

    public void BuyUpgrade()
    {
        if (EvilXPCount.GetXP() >= upgradeCost)
        {
            EvilXPCount.SpendEXP(upgradeCost);
            EvilXPCount.upgrades[id] = true;
            gM.UpdateEvilXP();
            transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
            check.SetActive(true);
            costText.SetActive(false);
            bought = true;
        }
    }


    void CheckExp(int eveilPoint)
    {
        if (eveilPoint >= upgradeCost && upgradeCost > 0 && !fake && !bought)
            transform.GetComponent<UnityEngine.UI.Button>().interactable = true;
        else if (upgradeCost > 0)
            transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
    }
}
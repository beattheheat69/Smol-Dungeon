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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        costTMP.SetText(upgradeCost.ToString().PadLeft(2, '0'));

        if (upgradeCost > EvilXPCount.GetXP())
        {
            transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else if (EvilXPCount.upgrades[id])
        {
            transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
            costText.SetActive(false);
        }
    }

    public void BuyUpgrade()
    {
        EvilXPCount.SpendEXP(upgradeCost);
        EvilXPCount.upgrades[id] = true;
        gM.UpdateEvilXP();
        transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
        costText.SetActive(false);
    }

    



}

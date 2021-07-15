using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImgr : MonoBehaviour
{
    public Transform canvas;
    public Transform main;

    public Button fishInformationButton;
    public Button sceneryShopButton;

    public Transform sceneryShop;
    public Transform fishInformation;

    public Transform fishInformationExpand;

    public GameObject[] contents = new GameObject[3];

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;
        main = canvas.Find("Main").transform;

        fishInformationButton = main.Find("FishInformationButton").GetComponent<Button>();
        sceneryShopButton = main.Find("SceneryShopButton").GetComponent<Button>();

        sceneryShop = main.Find("SceneryShop");
        fishInformation = main.Find("FishInformation");
        fishInformationExpand = main.Find("FishInformationExpand");

        sceneryShopButton.onClick.AddListener(() => sceneryShop.gameObject.SetActive(true));
        fishInformationButton.onClick.AddListener(() => fishInformation.gameObject.SetActive(true));
    }

    public void CancelButton()
    {
        sceneryShop.gameObject.SetActive(false);
        fishInformation.gameObject.SetActive(false);
    }

    public void ChangeContent(GameObject content)
    {
        foreach(GameObject ct in contents)
        {
            ct.SetActive(false);
        }
        content.SetActive(true);
    }
}

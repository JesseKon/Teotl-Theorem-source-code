using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveHud : MonoBehaviour
{
    private Text m_ShardsText;
    private Image m_ShardsImage;
    private Text m_RelicsText;
    private Image m_RelicsImage;

    GameManager m_GameManager;

    public bool AllowInventory {
        get { return m_AllowInventory; }
        set { m_AllowInventory = value; }
    }
    private bool m_AllowInventory = false;

    // Start is called before the first frame update
    private void Start() {
        m_ShardsText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        m_ShardsImage = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        m_RelicsText = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        m_RelicsImage = transform.GetChild(0).GetChild(3).GetComponent<Image>();
        m_ShardsImage.enabled = false;
        m_RelicsImage.enabled = false;


        m_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    private void Update() {
        if (!m_AllowInventory) {
            HideInventory();
            return;
        }

        if (Input.GetKey(KeyCode.Tab))
            ShowInventory();
        else
            HideInventory();
    }


    private void ShowInventory() {
        m_ShardsText.text = GameObject.FindGameObjectWithTag("ShardManager").GetComponent<ShardManager>().NumOfShards() + "  x  ";
        m_ShardsImage.enabled = true;
        m_RelicsText.text = GameObject.FindGameObjectWithTag("RelicManager").GetComponent<RelicManager>().NumOfRelics() + "  x  ";
        m_RelicsImage.enabled = true;
    }


    private void HideInventory() {
        m_ShardsText.text = "";
        m_ShardsImage.enabled = false;
        m_RelicsText.text = "";
        m_RelicsImage.enabled = false;
    }


}

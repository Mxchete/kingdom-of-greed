using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;
    // Start is called before the first frame update
    void Start()
    {
        ActivateTab(0);
    }

    // Update is called once per frame
    public void ActivateTab(int tabNo)
    {
        for(int i = 0; i< pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[tabNo].color = new Color(1f, 1f, 1f, tabImages[tabNo].color.a);

        }
        pages[tabNo].SetActive(true);
        tabImages[tabNo].color = new Color(1f, 1f, 1f, tabImages[tabNo].color.a);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LevelGroupController : MonoBehaviour
{
    Button[] btnLevels;

    void Awake()
    {
        btnLevels = GetComponentsInChildren<Button>();
        foreach (Button btnLevel in btnLevels)
        {
            btnLevel.gameObject.SetActive(false);
        }
    }

    public bool Init(int min,int max)
    {
        int count = 0;
        for (int i = min;i<=max;i++)
        {
            if (count > btnLevels.Length) return true;
            Button btnLevel = btnLevels[count];
            btnLevel.gameObject.SetActive(true);
            TextMeshProUGUI text = btnLevel.GetComponentInChildren<TextMeshProUGUI>();
            text.text = (i+1).ToString();
            btnLevel.onClick.AddListener(()=>
            {
                GlobalVar.currentMapLevel = i;
                SceneManager.LoadScene(1);
            });
            count++;
        }
        return false;
    }
}

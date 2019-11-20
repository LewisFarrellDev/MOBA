using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStateDisplayer : MonoBehaviour
{
    public Image stateLabel;
    public Slider healthBar;
    // Use this for initialization
    void Start()
    {
        stateLabel.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 statelabelPos = Camera.main.WorldToScreenPoint(this.transform.position);
        stateLabel.transform.position = statelabelPos;
        stateLabel.gameObject.SetActive(true);

        if (healthBar != null)
        {
            healthBar.transform.position = statelabelPos;
            healthBar.transform.position = statelabelPos;
            healthBar.gameObject.SetActive(true);
        }
    }
}

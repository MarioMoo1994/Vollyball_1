using UnityEngine;

public class HowToBtn : MonoBehaviour
{
    public GameObject HowToPanel;
    public GameObject title;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        HowToPanel.SetActive(false);
        title.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HowTo()
    {
        HowToPanel.SetActive(true);
        title.SetActive(false);
    }

    public void Back()
    {
        HowToPanel.SetActive(false);
        title.SetActive(true);
    }
}

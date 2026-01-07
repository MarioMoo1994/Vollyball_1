using UnityEngine;

public class HowToBtn : MonoBehaviour
{
    public GameObject HowToPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        HowToPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HowTo()
    {
        HowToPanel.SetActive(true);
    }

    public void Back()
    {
        HowToPanel.SetActive(false);
    }
}

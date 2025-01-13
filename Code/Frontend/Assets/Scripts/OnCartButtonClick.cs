using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCartButtonClick : MonoBehaviour
{
    public CanvasGroup ResultScreen;

    public CanvasGroup BackgroundScreen;

    public CanvasGroup CartScreen;

    public Text CartText;
    public GameObject ReadInput;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCartScreen()
    {
        ReadInput script = ReadInput.GetComponent<ReadInput>();
        CartText.text = script.runningList.text;
        CartText.text = CartText.text.Replace(":", "\n");
        CanvasGroupDisplayer.Hide(BackgroundScreen);
        CanvasGroupDisplayer.Hide(ResultScreen);
        CanvasGroupDisplayer.Show(CartScreen);
    }
}

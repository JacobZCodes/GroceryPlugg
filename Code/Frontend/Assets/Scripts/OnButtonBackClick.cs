using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnButtonBackClick : MonoBehaviour
{
    public CanvasGroup ResultScreen;
    public CanvasGroup BackgroundScreen;
    public CanvasGroup CartScreen;

    public void GoBack()
    {
        CanvasGroupDisplayer.Hide(ResultScreen);
        CanvasGroupDisplayer.Hide(CartScreen);
        CanvasGroupDisplayer.Show(BackgroundScreen);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

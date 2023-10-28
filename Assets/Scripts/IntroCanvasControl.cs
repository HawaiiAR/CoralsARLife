using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class IntroCanvasControl : MonoBehaviour
{
    [TextArea(1, 10)]
    [SerializeField] private string _textCardOne_txt;
    [TextArea(1, 10)]
    [SerializeField] private string _textCardTwo_txt;
    [TextArea(1, 10)]
    [SerializeField] private string _textCardThree_txt;
    [TextArea(1, 10)]
    [SerializeField] private string _textCardFour_txt;
    [TextArea(1, 10)]
    [SerializeField] private string _textCardFive_txt;

    [SerializeField] private CanvasGroup _introCanvasGroup;
    [SerializeField] private CanvasGroup _cardTextGroup;
    [SerializeField] private TMP_Text _textCard;
    private float alpha;
    private int cardTextNum;
    private bool deactivateCanvas;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 0;
        _cardTextGroup.alpha = alpha;
        cardTextNum = 1;
    }

    // Update is called once per frame

    public void Skip()
    {
        deactivateCanvas = true;
        Fade("FadeOut");
    }

    public void DisplayIntroText()
    {

        switch (cardTextNum)
        {
            case 1:
                _textCard.text = _textCardOne_txt;
                cardTextNum += 1;
                Fade("FadeIn");
                break;
            case 2:
                _textCard.text = _textCardTwo_txt;
                Fade("FadeIn");
                cardTextNum += 1;
                break;
            case 3:
                _textCard.text = _textCardThree_txt;
                Fade("FadeIn");
                cardTextNum += 1;
                break;
            case 4:
                _textCard.text = _textCardFour_txt;
                Fade("FadeIn");
                cardTextNum += 1;
                break;
            case 5:
                _textCard.text = _textCardFive_txt;
                Fade("FadeIn");
                cardTextNum += 1;
                break;
        }
    }

    public void Fade(string Fade)
    {
        switch (Fade)
        {
            case "FadeIn":
                StartCoroutine(FadeIn());
                break;
            case "FadeOut":
                StartCoroutine(FadeOut());
                break;
        }
    }

    IEnumerator FadeIn()
    {
         alpha = 0;   
        while (alpha < 1)
        {  
            _cardTextGroup.alpha = alpha;
            alpha += .5f * Time.deltaTime;
            yield return null;
        }
        Debug.Log("faded in");
       
    }

    IEnumerator FadeOut()
    {
        alpha = 1;
        while (alpha > 0)
        {
            alpha -=  1.5f * Time.deltaTime;
            _cardTextGroup.alpha = alpha;
            yield return null;
        }
        Debug.Log("Out");
        if (deactivateCanvas)
        {
            _introCanvasGroup.gameObject.SetActive(false);
        }
    }
}

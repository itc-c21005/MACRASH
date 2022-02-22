using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class CatchBottunScript : MonoBehaviour
{
    Button CatchButton;
    public Image CatchImage;

    public CanvasRenderer CBCR;

    private void Awake()
    {
        CatchButton = gameObject.GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CatchButton.interactable = false;
        CatchImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCatch()
    {

        //キャッチのボタンを出す
        CatchButton.interactable = true;
        CatchImage.enabled = true;
        OnClick();
    }

    public void OffCatch()
    {
        //キャッチのボタンを消す
        CatchButton.interactable = false;
        CatchImage.enabled = false;
    }

    public void OnClick()
    {
        CatchButton.interactable = false;
        CBCR.SetAlpha(0.5f);
        StartCoroutine(OnButton());
    }

    IEnumerator OnButton()
    {
        yield return new WaitForSeconds(1);
        CBCR.SetAlpha(1);
        CatchButton.interactable = true;
    }
}

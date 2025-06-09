using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CanvasModalImageTextPages : MonoBehaviour
{

    public Button buttonNext;
    public Button buttonBack;

    private List<string> pages = new List<string>();
    private int currentPage = 0;

    public List<Sprite> imagePages;
    public List<string> textPages;

    public Image imagePanel; // Asigna tu componente Image aquí
    public TMP_Text textPanel; // Asigna tu componente TextMeshPro aquí

    // Start is called before the first frame update
    void Start()
    {
        PaginateText();
        ShowPage(0);

        buttonNext.onClick.AddListener(NextPage);
        buttonBack.onClick.AddListener(PreviousPage);
    }

    void PaginateText()
    {
        pages.Clear();
        pages.AddRange(textPages);
    }

    void ShowPage(int page)
    {
        if (page >= 0 && page < imagePages.Count && page < textPages.Count)
        {
            imagePanel.sprite = imagePages[page];
            imagePanel.preserveAspect = true;
            textPanel.text = textPages[page];
            buttonBack.interactable = page > 0;
            buttonNext.interactable = page < pages.Count - 1;
        }
    }

    void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }
}

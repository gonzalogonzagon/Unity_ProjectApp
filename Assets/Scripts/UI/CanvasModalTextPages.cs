// CanvasModalTextPages.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


public class CanvasModalTextPages : MonoBehaviour
{
    public TMP_Text textPanel; // Asigna tu componente Text aquí
    public Button buttonNext;
    public Button buttonBack;
    [TextArea] public string longText;
    
    public enum PaginationMode
    {
        ByPeriod,
        ByNewLine
    }
    public PaginationMode paginationMode = PaginationMode.ByPeriod;

    private List<string> pages = new List<string>();
    private int currentPage = 0;

    void Start()
    {
        PaginateText();
        ShowPage(0);

        buttonNext.onClick.AddListener(NextPage);
        buttonBack.onClick.AddListener(PreviousPage);
    }

    public void PaginateText()
    {
        pages.Clear();
        if (paginationMode == PaginationMode.ByPeriod)
        {
            string[] sentences = longText.Split(new[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var sentence in sentences)
            {
                string trimmed = sentence.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    pages.Add(trimmed + ".");
            }
        }
        else if (paginationMode == PaginationMode.ByNewLine)
        {
            string[] lines = longText.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                string trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    pages.Add(trimmed);
            }
        }
    }

    public void ShowPage(int page)
    {
        if (pages.Count == 0) return;
        textPanel.text = pages[page];
        buttonBack.interactable = page > 0;
        buttonNext.interactable = page < pages.Count - 1;
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
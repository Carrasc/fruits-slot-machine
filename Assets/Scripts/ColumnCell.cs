using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;


public class ColumnCell : EnhancedScrollerCellView
{
    [SerializeField] private Image image;
    [SerializeField] private CanvasGroup highlightWinImage;
    [HideInInspector] public int index;

    /// <summary>
    /// When creating the prefab, set cell data and the corresponding sprite.
    /// Also when re-utiliing the cell, set the proper data.
    /// </summary>
    public void InitializeCell(CellData cellData)
    {
        //cellIndex = index;
        //symbolData = cellData.symbolData;

        // Set the sprite at initialization
        image.sprite = cellData.symbolData.symbolSprite;
    }

    public void HighlightCell()
    {
        highlightWinImage.alpha = 1;
    }
}

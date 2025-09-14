using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;


public class ColumnCell : EnhancedScrollerCellView
{
    [SerializeField] private Image image;
    [SerializeField] private CanvasGroup highlightWinImage;
    [HideInInspector] public int index;
    private CellData _currentData;

    /// <summary>
    /// When creating the prefab, set cell data and the corresponding sprite.
    /// Also when re-utiliing the cell, set the proper data.
    /// </summary>
    public void InitializeCell(CellData cellData)
    {
        if (_currentData != null)
        {
            _currentData.OnHighlightStateChanged -= HighlightCell;
        }

        _currentData = cellData;

        // Method SetHighlight will run automatically when _currentData.IsHighlighted is changed
        _currentData.OnHighlightStateChanged += HighlightCell;

        // Update from the start the state of the cell
        HighlightCell(_currentData.IsHighlighted);

        // Set the sprite at initialization
        image.sprite = cellData.symbolData.symbolSprite;
    }

    public void HighlightCell(bool isHighlighted)
    {
        highlightWinImage.alpha = isHighlighted ? 1 : 0;
    }

    private void OnDisable()
    {
        if (_currentData != null)
        {
            _currentData.OnHighlightStateChanged -= HighlightCell;
        }
    }
}

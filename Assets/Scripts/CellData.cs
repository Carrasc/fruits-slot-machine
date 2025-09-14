using UnityEngine;

// Delegate to communicate to the cell that its a "winner" and we can highlight
public delegate void HighlightStateChangedDelegate(bool isHighlighted);

/// <summary>
/// A simple class that holds the information (just data itself) for each cell of the slot, like:
/// payout amounts, that fruit it is, etc.
/// </summary>
public class CellData
{
    public SymbolData symbolData;
    public int cellIndex;
    public HighlightStateChangedDelegate OnHighlightStateChanged;

    // Used to control the state of the highlight
    private bool _isHighlighted;
    public bool IsHighlighted
    {
        get { return _isHighlighted; }
        set
        {
            if (_isHighlighted != value)
            {
                _isHighlighted = value;
                OnHighlightStateChanged?.Invoke(_isHighlighted);
            }
        }
    }
}

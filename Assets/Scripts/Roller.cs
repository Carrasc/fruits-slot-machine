using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class Roller : MonoBehaviour, IEnhancedScrollerDelegate
{
    [Header("Roller defined symbols")]
    [SerializeField] private List<SymbolData> rollerSymbols = new List<SymbolData>();
    
    [Header("Animation values")]
    [SerializeField][Range(2500, 8000)] private float spinSpeed;

    // The list of the DATA for the slot cells
    public EnhancedUI.SmallList<CellData> RollerCellsData = new EnhancedUI.SmallList<CellData>();

    // Scroller that can re-utilize cells, to simulate a loop and snap to a specific cell
    public EnhancedScroller scroller;

    // Cell prefab
    public EnhancedScrollerCellView slotCellViewPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // set this controller as the scroller's delegate
        scroller.Delegate = this;

        InitializeRoller();
    }

    /// <summary>
    /// With a simpole list we can set in the inspector, initialize the roller with a certain pattern 
    /// </summary>
    private void InitializeRoller()
    {
        // reset the data list
        RollerCellsData.Clear();

        for (int i = 0; i < rollerSymbols.Count; i++)
        {
            CellData newData = new CellData()
            {
                symbolData = rollerSymbols[i],
                cellIndex = i
            };

            RollerCellsData.Add(newData);
        }

        // reload the scroller with the new items we just created
        scroller.ReloadData();
    }

    public IEnumerator SpinRoller(int stopIndex, float spinTime)
    {
        // Fast spin for X seconds
        float elapsed = 0f;
        while (elapsed < spinTime)
        {
            elapsed += Time.deltaTime;

            // Scroll continuously
            scroller.ScrollPosition -= spinSpeed * Time.deltaTime;

            yield return null;
        }


        // Slow final stop with a short tween
        float slowTweenTime = 1f; // controls how slow it eases into the stop
        scroller.JumpToDataIndex(stopIndex, 0f, 0f, true, EnhancedScroller.TweenType.easeOutBounce, slowTweenTime);
    }


    /// <summary>
    /// This makes the scroller move without having an explicit touch event
    /// </summary>
    /// <param name="amount"></param>
    public void StartRollerSpin(int stopIndex, float spinTime)
    {
        StartCoroutine(SpinRoller(stopIndex, spinTime));
    }

    #region EnhancedScroller Callbacks

    /// <summary>
    /// This callback tells the scroller how many slot cells to expect
    /// </summary>
    /// <param name="scroller">The scroller requesting the number of cells</param>
    /// <returns>The number of cells</returns>
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return RollerCellsData.Count;
    }

    /// <summary>
    /// This callback tells the scroller what size each cell is.
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell size</param>
    /// <param name="dataIndex">The index of the data list</param>
    /// <returns>The size of the cell (Height for vertical scrollers, Width for Horizontal scrollers)</returns>
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 125f;
    }

    /// <summary>
    /// This callback gets the cell to be displayed by the scroller
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell</param>
    /// <param name="dataIndex">The index of the data list</param>
    /// <param name="cellIndex">The cell index (This will be different from dataindex if looping is involved)</param>
    /// <returns>The cell to display</returns>
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // get the cell view from the scroller, recycling if possible
        ColumnCell cellView = scroller.GetCellView(slotCellViewPrefab) as ColumnCell;

        // set the data for the cell
        cellView.InitializeCell(RollerCellsData[dataIndex]);

        // return the cell view to the scroller
        return cellView;
    }

    #endregion
}

using UnityEngine;

[CreateAssetMenu(fileName = "SymbolData", menuName = "Scriptable Objects/SymbolData")]
public class SymbolData : ScriptableObject
{
    public string symbolName;
    public Sprite symbolSprite;
    public int[] payoutAmounts = new int[4]; // Payout for 2, 3, 4 and 5 in a row
}

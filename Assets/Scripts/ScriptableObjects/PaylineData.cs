using UnityEngine;

[CreateAssetMenu(fileName = "PaylineData", menuName = "Scriptable Objects/PaylineData")]
public class PaylineData : ScriptableObject
{
    // This SO holds the pattern that can form a winning formation (starting from the first roller)
    public int[] linePositions = new int[5];
}

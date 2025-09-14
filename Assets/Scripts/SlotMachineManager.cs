using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class SlotMachineManager : MonoBehaviour
{
    public Roller[] rollers = new Roller[5];
    public List<PaylineData> paylines;
    [SerializeField] private float rollerStartSpinDelay = 0.5f;
    [SerializeField] private float minSpinTime = 0.5f;
    [SerializeField] private float maxSpinTime = 1f;
    [SerializeField] private Button spinButton;
    [SerializeField] private TextMeshProUGUI winAmountText;

    private int credits;
    private int currentBet;

    // Game states, so we can manager button states and give out rewards when appropriate
    private enum GameState { Idle, Spinning, Evaluating, Win, Lose }
    private GameState currentState;

    void Start()
    {
        currentState = GameState.Idle;

        // Show credits here, or cool start animation, etc...
    }

    private void Update()
    {
        // Handle the button, only activate when in idle (not spinning nor evaluating results)
        spinButton.interactable = currentState == GameState.Idle;
    }

    // When the spin button is pressed
    public void OnSpinButtonPressed()
    {
        if (currentState == GameState.Idle)
        {
            if (credits >= currentBet)
            {
                credits -= currentBet;

                // Remove any winning highlights from previous bet
                ResetHighlightsAndWinnings();
                StartCoroutine(StartSpinning());
            }
        }
    }

    private IEnumerator StartSpinning()
    {
        currentState = GameState.Spinning;

        // We generate the winning results at the very start, so the machine is fair and not dependant on animation.
        // Animation is just a cool show for the player
        List<int> stopIndexes = new List<int>();
        CellData[,] finalResult = GenerateFinalResult(out stopIndexes);

        // Calculate the spin time, for all rollers between 2 and 4 seconds
        float spinTime = Random.Range(minSpinTime, maxSpinTime);

        for (int i = 0; i < rollers.Length; i++)
        {
            rollers[i].StartRollerSpin(stopIndexes[i], spinTime);

            // Small delay between the start spin of each roller
            yield return new WaitForSeconds(rollerStartSpinDelay);
        }

        // Wait for the rollers to finish the animation
        yield return new WaitForSeconds(spinTime);

        // After all rollers stop, evaluate the result and show winnings if there are any
        EvaluateResult(finalResult);
    }

    private CellData[,] GenerateFinalResult(out List<int> stopRollerIndexes)
    {
        // Matriz para guardar el resultado [carrete, fila]
        CellData[,] result = new CellData[rollers.Length, 3]; // 3 filas visibles

        // So we know where to stop the roller, to match the result
        List<int> indexes = new List<int>();

        string output = "";
        for (int i = 0; i < rollers.Length; i++)
        {
            // Pick one random starting index for the 3 visible symbols
            var rollerSymbols = rollers[i].GetComponent<Roller>().RollerCellsData;
            int randomIndex = Random.Range(0, rollerSymbols.Count);
            indexes.Add(randomIndex);

            for (int j = 0; j < 3; j++)
            {
                // Wrap around with modulo so we dont go out of range
                int symbolIndex = (randomIndex + j) % rollerSymbols.Count;

                result[i, j] = new CellData()
                {
                    symbolData = rollerSymbols[symbolIndex].symbolData,
                    cellIndex = symbolIndex
                };

                output += result[i, j] + " - " + symbolIndex + "\t";
            }

            output += "\n"; // new row
        }

        Debug.Log(output);

        stopRollerIndexes = indexes;

        return result;
    }

    private void EvaluateResult(CellData[,] result)
    {
        Debug.Log("Evaluating results...");
        currentState = GameState.Evaluating;
        int totalWinnings = 0;

        foreach (PaylineData payline in paylines)
        {
            CellData firstSymbol = result[0, payline.linePositions[0]];
            int consecutiveSymbols = 1;

            for (int i = 1; i < rollers.Length; i++)
            {
                CellData currentSymbol = result[i, payline.linePositions[i]];
                if (currentSymbol.symbolData == firstSymbol.symbolData)
                {
                    consecutiveSymbols++;
                }
                else
                {
                    break; // Sequence broke
                }
            }

            // Calculate based on the first symbol, and how many consecutive symbols there are
            if (consecutiveSymbols >= 2)
            {
                totalWinnings += firstSymbol.symbolData.payoutAmounts[consecutiveSymbols - 2];

                for (int rollerIndex = 0; rollerIndex < consecutiveSymbols; rollerIndex++)
                {
                    CellData winningCellData = result[rollerIndex, payline.linePositions[rollerIndex]];

                    // Find the original cell from the roller and change the state
                    var originalData = rollers[rollerIndex].RollerCellsData[winningCellData.cellIndex];
                    originalData.IsHighlighted = true;
                }

                Debug.Log($"Win with {firstSymbol.symbolData.symbolName}! you won +{firstSymbol.symbolData.payoutAmounts[consecutiveSymbols - 2]}");
            }
        }

        if (totalWinnings > 0)
        {
            currentState = GameState.Win;
            credits += totalWinnings;

            // Show victory animation / winning amounts
            winAmountText.text = totalWinnings.ToString();
            Debug.Log($"Total win: ! +{totalWinnings}");
        }
        else
        {
            currentState = GameState.Lose;

            Debug.Log("Lost spin. No winning symbols");
        }

        // Back to idle after evaluation (we could make animations fancier and wait longer)
        currentState = GameState.Idle;
    }

    private void ResetHighlightsAndWinnings()
    {
        // Remove the highlights from each cell
        foreach (var roller in rollers)
        {
            for (int i = 0; i < roller.RollerCellsData.Count; i++)
            {
                roller.RollerCellsData[i].IsHighlighted = false;
            }
        }

        // Reset the winning text
        winAmountText.text = "0";
    }
}
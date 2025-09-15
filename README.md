# 🎰 Unity slot machine

A simple Unity project that simulates a classic 5-reel slot machine. Reels spin with realistic timing, loop seamlessly, and stop on pre-determined winning results for fair gameplay. Includes highlighting of winning symbols and configurable paylines.

## 🛠️ Features
- 5 reels with custom symbol patterns
- Seamless looping reels with EnhancedScroller
- Randomized spin times (2s–4s)
- Realistic left-to-right reel start and stop delays
- Pre-calculated results (animation is just visual)
- Highlighting system for winning symbols (works with recycled cells)
- Configurable paylines for win evaluation
- Modular setup: easily add/remove reels and symbols

## ▶️ How to Run

1. Clone or download this repo.
2. Open the project in Unity (preferably **Unity 6 LTS or later**).
3. Open the scene named **`SlotScene`**.
4. Press **Play** to test the slot machine.
5. Adjust reel symbols and timings in the inspector (`Roller` components).

## 🎮 Controls

- **Click SPIN** to start the reels.
- Reels spin from left to right with a slight delay.
- Once all reels stop, winning symbols (if any) are highlighted automatically.

## 🧩 Dependencies
- [EnhancedScrollerV2](https://assetstore.unity.com/packages/tools/gui/enhanced-scroller-v2-72736) for performant looping reels

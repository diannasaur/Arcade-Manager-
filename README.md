# Arcade High Score Manager

A C# console program that reads raw arcade game scores from a file, calculates each player's final score using **polymorphism** (game-specific scoring rules), sorts players into a leaderboard by score, and outputs the results to both the console and a file.

## Overview

The program models three arcade games, each implementing a shared `IArcadeGame` interface with its own scoring formula. An abstract `PlayerScore` class defines the shape of a player's result, and `ArcadePlayerScore` determines which game's rules to apply at runtime and calculates the player's final score accordingly.

## Features

- **Reads raw score data** from `raw_scores.txt` (CSV format, no header)
- **Polymorphic scoring:** each game calculates a player's final score using its own rules via `IArcadeGame.CalculateScore`
- **Sorts the leaderboard** by final score, descending
- **Outputs the leaderboard** to the console and to `processed_leaderboard.txt`
- **Graceful error handling** for a missing input file or malformed data

## Input File Format

`raw_scores.txt` is a comma-separated file with **no header row**, one score entry per line:

```
PlayerName,GameName,Points,Bonus
```

Example:

```
Lara,DragonSlayer,1000,250
Samus,SpaceRacer,300,50
Mario,RetroPuzzle,120,60
```

> The array that stores parsed entries is fixed at 20 slots, so the input file should contain at most 20 lines.

## Scoring Rules

| Game | Formula |
|---|---|
| **DragonSlayer** | `(points × 2) + bonus` |
| **SpaceRacer** | `points + (bonus × 3)` |
| **RetroPuzzle** | `points + bonus` |

`GameName` in the input file must exactly match one of: `DragonSlayer`, `SpaceRacer`, `RetroPuzzle`.

## Output

- **Console:** prints the leaderboard header `*** THE POLYMORPHIC ARCADE LEADERBOARD ***` followed by each player's name, game, and final score, sorted highest to lowest.
- **File:** writes the same sorted leaderboard to `processed_leaderboard.txt` in the format:
  ```
  PlayerName,GameName,FinalScore
  ```

## Error Handling

- If `raw_scores.txt` is missing, the program catches `FileNotFoundException` and prints a friendly message instead of crashing.
- Other exceptions (e.g. malformed lines, parsing errors) are caught and their message is printed to the console.

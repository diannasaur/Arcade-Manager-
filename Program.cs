using System;
using System.IO;

namespace ArcadeHighScoreManager;

interface IArcadeGame
{
    string GetGameName(); //returns the name of the game

    int CalculateScore(int points, int bonus); //calculates the final score based on game-specific rules.
}

public abstract class PlayerScore
{
    public string PlayerName //proptery 
    {
        get;
        set;
    }

    public string GameName //proptery 
    {
        get;
        set;
    }

    public int FinalScore //proptery 
    {
        get;
        set;
    }

    public PlayerScore(string playerName, string gameName) //constructor, initialize the player's name and the game name to the according propterties
    {
        PlayerName = playerName;
        GameName = gameName;
    }

    public abstract void ProcessScore(int points, int bonus); //abstract method that will use a game's CalculateScore method to set the FinalScore
}

public class DragonSlayerGame : IArcadeGame //derived game class that implements IArcadeGame
{
    public string GetGameName()
    {
        return "DragonSlayer";
    }

    public int CalculateScore(int points, int bonus)
    {
        return (points * 2) + bonus;
    }

}

public class SpaceRacerGame : IArcadeGame //derived game class that implements IArcadeGame
{
    public string GetGameName()
    {
        return "SpaceRacer";
    }

    public int CalculateScore(int points, int bonus)
    {
        return points + (bonus * 3);
    }
}

public class RetroPuzzleGame : IArcadeGame //derived game class that implements IArcadeGame
{
    public string GetGameName()
    {
    return "RetroPuzzle";
    }


    public int CalculateScore(int points, int bonus)
    {
        return points + bonus;
    }
}

public class ArcadePlayerScore : PlayerScore
{
    public ArcadePlayerScore(string playerName, string gameName) : base(playerName, gameName) {} //constructor that calls the base constructor from PlayerScore

    public override void ProcessScore(int points, int bonus) //overrides the abstract method from PlayerScore
    {

        IArcadeGame dragonSlayer = new DragonSlayerGame();
        IArcadeGame spaceRacer = new SpaceRacerGame();
        IArcadeGame retroPuzzle = new RetroPuzzleGame();

        //The if-else statements find the correct game type and calls its CalculateScore method 
        if (GameName.Equals(dragonSlayer.GetGameName()))
        {
            FinalScore = dragonSlayer.CalculateScore(points, bonus);
        }
        else if (GameName.Equals(spaceRacer.GetGameName()))
        {
            FinalScore = spaceRacer.CalculateScore(points, bonus);
        }
        else if (GameName.Equals(retroPuzzle.GetGameName()))
        {
            FinalScore = retroPuzzle.CalculateScore(points, bonus);
        }
            
    }
}

public class Program
{
    static void Main(string[] args)
    {
        PlayerScore[] allScores = new PlayerScore[20]; 
        int count = 0; //counts number of valid entries

        //trys to read the file & process the data
        try
        {
            //reads each line in the file and stores the values in the array
            using (StreamReader reader = new StreamReader("raw_scores.txt"))
            {
                string line = "";
                //this while loop helps locate each value by the positions of the commas and assigns them to variables
                while ((line = reader.ReadLine()) != null)
                {

                    int charPos = line.IndexOf(',');
                    int charPos2 = line.IndexOf(',', charPos + 1);
                    int charPos3 = line.IndexOf(',', charPos2 + 1);

                    string playerName = line.Substring(0, charPos);
                    string gameName = line.Substring(charPos + 1, charPos2 - (charPos + 1));
                    int points = int.Parse(line.Substring(charPos2 + 1, charPos3 - (charPos2 + 1)));
                    int bonus = int.Parse(line.Substring(charPos3 + 1));

                    ArcadePlayerScore score = new ArcadePlayerScore(playerName, gameName);

                    score.ProcessScore(points, bonus);

                    allScores[count] = score;
                    count++;
                }
            }

            //sorts the array based on FinalScore in descending order by comparing each entry
            for (int i = 0; i < count - 1; i++) 
            {
                for (int j = i + 1; j < count; j++) 
                {
                    if (allScores[i].FinalScore < allScores[j].FinalScore) 
                    {
                        PlayerScore temp = allScores[i];
                        allScores[i] = allScores[j];
                        allScores[j] = temp;
                    }
                }
            }

            //writes each entry from allScores array in a new file
            using (StreamWriter writer = new StreamWriter("processed_leaderboard.txt"))
            {
                for (int i = 0; i < count; i++)
                {
                    PlayerScore score = allScores[i];
                    writer.WriteLine($"{score.PlayerName},{score.GameName},{score.FinalScore}");
                }
            }

            Console.WriteLine("*** THE POLYMORPHIC ARCADE LEADERBOARD ***");

            //writes each entry from allScores array to the console
            foreach (PlayerScore score in allScores)
            {
                if (score == null)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"{score.PlayerName} ({score.GameName}): {score.FinalScore}");
                }
            }

        }
        catch (FileNotFoundException) //catches if the file is not found
        {
            Console.WriteLine("raw_scores.txt was not found.");
        }
        catch (Exception ex) //catches any other exceptions
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }
}
using System.Diagnostics;

interface ICharacter{
    void Draw();
    void Update();
}

public class Character{
    public int[]? xPosition, yPosition;
    public ConsoleColor Colour;
    public int Speed;
}

public class Apple : Character, ICharacter{
    Apple(ConsoleColor colour){
        Colour = colour;
        Speed = 0;
    }   
    public void Draw(){
        //Insert spawning behaviour
    }

    public void Update(){
        //Insert update behaviour
    }
}

public class Snake : Character, ICharacter{
    Snake(ConsoleColor colour) {
        Colour = colour;
        Speed = 8;
    }
    public void Draw(){
        //Insert movement behaviour
    }

    public void Update(){
        static void Grow(Apple apple, Score score){
            //if(Appel x en y in Slang x en y, Lengte++){

            //}
            AddScore(score);
        }

        static void AddScore(Score score){
            score.CurrentScore += 10;
        }
    }  
}

public class Wall{
    int Height, Width;

    Wall(int height, int width){
        Height = height;
        Width = width;
    }
    public void Draw(){
        for(int i = 0; i < Height; i++){
            //Insert wall creation behaviour
        }
    }
}

public class State{
    public bool gameOver = false;

    public void Start(){
        //Insert 'Setup' logic
    }

    public void GameOver(){
        //Game is over
    }

    public void Pause(){
        //Game is paused
    }
}

public class Score{
    public int CurrentScore = 0;
    //Dictionary<string, int> Leaderboard = [];
    //Dictionary kan gebruikt worden voor een leaderboard. Leadboard.Add("Naam", Score);

    public void Display(){
        //Insert behaviours for current score and high score
    }
}

class Program {
    static int width = 50, height = 15;
    static int score = 0;
    static int speed = 8;
    static bool gameOver = false;
    static int[] xPosition = new int[50], yPosition = new int[50];
    static int fruitX, fruitY;
    static int nTail = 0;
    static ConsoleKeyInfo keyInfo = new();
    static Random random = new();

    // Stopwatch voor het bijhouden van de tijd
    static Stopwatch stopwatch = new();

    static void Main() {
        Console.SetWindowSize(width + 1, height + 1);
        Console.CursorVisible = false;
        Setup();
        stopwatch.Start();
        while (!gameOver){
            Draw();
            Pause();
            Movement();
            Thread.Sleep(speed);
        }
        stopwatch.Stop();
        Console.SetCursorPosition(width / 2 - 4, height / 2);
        Console.WriteLine("Game Over!");
        Console.WriteLine("Your score: " + score);
        Console.WriteLine("Time: " + stopwatch.Elapsed.ToString(@"mm\:ss"));
        Console.ReadKey();
    }

    static void Setup(){
        xPosition[0] = width / 2;
        yPosition[0] = height / 2;
        fruitX = random.Next(1, width - 1);
        fruitY = random.Next(1, height - 1);
    }

    static void Draw(){
        Console.Clear();

        for (int i = 0; i < width + 2; i++)
        Console.Write("=");
        Console.WriteLine();

        for (int i = 0; i < height; i++){
            for (int j = 0; j < width; j++){
                if (j == 0){ 
                    Console.Write("\u2551");
                }
                if (i == yPosition[0] && j == xPosition[0]) {
                    Console.ForegroundColor = ConsoleColor.Green; // Kleur van het hoofd van de slang wordt groen
                    Console.Write("\u2588");
                    Console.ResetColor();
                }
                else if (i == fruitY && j == fruitX){
                    Console.ForegroundColor = ConsoleColor.Red; // Kleur van het fruit wordt rood
                    Console.Write("\u2588");
                    Console.ResetColor();
                }
                else{
                    bool print = false;
                    for (int k = 0; k < nTail; k++) {
                        if (xPosition[k] == j && yPosition[k] == i) {
                            Console.ForegroundColor = ConsoleColor.Green; // Kleur van de staart van de slang wordt groen
                            Console.Write("\u2588");
                            Console.ResetColor(); // Reset de kleur
                            print = true;
                        }
                    }
                    if (!print) {
                        Console.Write(" ");
                        }
                    }

                    if (j == width - 1){ 
                        Console.Write("\u2551");
                    }
            }
            Console.WriteLine();
        }

        for (int i = 0; i < width + 2; i++){
            Console.Write("=");
        }
        Console.WriteLine();
        Console.WriteLine("Score: " + score);
    }

    static void Pause(){
        if (Console.KeyAvailable) {
            keyInfo = Console.ReadKey(true); // true om te voorkomen dat de toets wordt weergegeven in de console
            if (keyInfo.Key == ConsoleKey.P){
                PauseGame();
            }
        }
    }

    static void PauseGame(){
        Console.WriteLine("\nSpel gepauzeerd. Druk op 'P' om te hervatten.");
        bool paused = true;
        while (paused){
            if (Console.KeyAvailable){
                ConsoleKeyInfo keyPress = Console.ReadKey(true);
                if (keyPress.Key == ConsoleKey.P){
                    paused = false;
                }
            }
        }
        Console.Clear(); // Het scherm wissen om de tekeningen van de pauze te verwijderen
    }

    static void Movement(){
        int prevX = xPosition[0], prevY = yPosition[0];
        int prev2X, prev2Y;

        // Beweging van het hoofd van de slang
        switch (keyInfo.Key){
            case ConsoleKey.RightArrow:
                xPosition[0]++;
                Thread.Sleep((1000/speed)); 
                break;
            case ConsoleKey.LeftArrow:
                xPosition[0]--;
                Thread.Sleep((1000/speed));
                break;
            case ConsoleKey.DownArrow:
                yPosition[0]++;
                Thread.Sleep((1000/speed)); 
                break;
            case ConsoleKey.UpArrow:
                yPosition[0]--;
                Thread.Sleep((1000/speed)); 
                break;
        }

        // Beweging van de staart van de slang
        for (int i = 1; i < nTail; i++){
            prev2X = xPosition[i];
            prev2Y = yPosition[i];
            xPosition[i] = prevX;
            yPosition[i] = prevY;
            prevX = prev2X;
            prevY = prev2Y;
        }

        // Checkt of de slang tegen de muur aan botst
        if (xPosition[0] < 0 || xPosition[0] >= width || yPosition[0] < 0 || yPosition[0] >= height){
            gameOver = true;
        }

        // Checkt of de slang tegen zichzelf aan botst
        for (int i = 1; i < nTail; i++){
            if (xPosition[0] == xPosition[i] && yPosition[0] == yPosition[i]){
                gameOver = true;
            }
        }

        // Checkt of de slang de fruit eet
        if (xPosition[0] == fruitX && yPosition[0] == fruitY){
            score += 10;
            fruitX = random.Next(1, width - 1);
            fruitY = random.Next(1, height - 1);
            nTail++;
        }
    }
}
using System.Diagnostics;

interface ICharacter{
    void Draw();
    void Update();
}

public class Character{
    public int[] xPosition, yPosition;
    public ConsoleColor Colour;
    public int Speed;
    public int Length;
}

public class Apple : Character, ICharacter {
    public static List<Apple> Apples = [];

    public Apple(ConsoleColor colour) {
        Colour = colour;
        Speed = 0;
        Length = 1;
    }

    public void Draw() {
        static void RemoveOld(Apple apple) {
            if(Apples.Count() > 1) {
                //Ga naar positie en teken een zwart blokje, return daarna terug naar oude positie
                //Hij moet alleen nog naar de oude positie (Waar de slang heen wilt dus)
                Console.SetCursorPosition(apple.xPosition[0], apple.yPosition[0]);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\u2558");
                Console.ResetColor();
                Apples.Remove(apple);
                //Zet de cursor terug
                //Console.SetCursorPosition(oude positie);
            }

            static void DrawNew(Wall wall, Apple apple) {
                apple.xPosition[0] = new Random().Next(1, wall.Width - 1);
                apple.yPosition[0] = new Random().Next(1, wall.Height - 1);
                if(Apples.Count() == 0) {
                    Console.SetCursorPosition(apple.xPosition[0], apple.yPosition[0]);
                    Console.ForegroundColor = apple.Colour;
                    Console.Write("\u2558");
                    Console.ResetColor();
                    //Ze de cursor terug
                    //Console.SetCursorPosition(oude positie);
                }
            }
        }
    }

    public void Update() {
        
    }
}

public class Snake : Character, ICharacter {
    public Snake(ConsoleColor colour) {
        Colour = colour;
        Speed = 8;
        Length = 1;
    }
    public void Draw(){
        static void DrawBody(Snake snake) {
            Console.ForegroundColor = snake.Colour;
            Console.Write("\u2558");
            Console.ResetColor();
        }

        static void RemoveTail(Snake snake) {
            //Omdat je de console niet elke keer meer cleart moet je dit handmatig doen door naar de slang zijn staart
            //Te gaan en het handmatig over te kleuren. Vergeet niet de cursor terug te doen.
            if(snake.xPosition.Length > snake.Length){
                (int x, int y) currentPosition = (snake.xPosition[0], snake.yPosition[0]);
                Console.SetCursorPosition(snake.xPosition[snake.Length], snake.yPosition[snake.Length]);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("\u2558");
                Console.ResetColor();
                Console.SetCursorPosition(currentPosition.x, currentPosition.y);
            }
        }
    }

    public void Update(){
        static void Move(Snake snake) {
            
        }

        static void Grow(Snake snake, Apple apple, Score score){
            if(apple.xPosition == snake.xPosition && apple.yPosition == snake.yPosition) {
                snake.Length++;
                snake.Speed++;
                //Gebaseerd op de slang zn snelheid krijgt hij meet punten
                score.CurrentScore += (10+(10*(snake.Speed/100)));
            }
        }
    }  
}

public class Wall{
    public int Height, Width;

    public Wall(int width,int height) {
        Height = height + 2;
        Width = width + 2;
    }

    public void Draw() {
        for(int h = 0; h <= Height; h++) {
            for(int w = 0; w <= Width; w++) {
                if((w == 0 || w == Width) && !(h == 0 || h == Height)) {
                    Console.Write("\u2551");
                }
                if(!(w == 0 || w == Width) && (h == 0 || h == Height)) {
                    Console.Write("\u2550");
                }
                if(!(w == 0 || w == Width) && !(h == 0 || h == Height)) {
                    Console.Write(" ");
                }
                if(w == 0 && h == 0) {
                    Console.Write("\u2554");
                }
                if(w == Width && h == 0) {
                    Console.Write("\u2557");
                }
                if(w == 0 && h == Height) {
                    Console.Write("\u255A");
                }
                if(w == Width && h == Height) {
                    Console.Write("\u255D");
                }
            }
            Console.Write("\n");
        }
    }
}

public class Game {
    public bool gameOver = false;

    public void Start(Snake snake, Wall wall) {
        //Plaatst de Slang in het midden van het speelveld om te beginnen met het spel :)
        snake.xPosition[0] = wall.Width / 2;
        snake.yPosition[0] = wall.Height / 2;
    }

    public void GameOver(Snake snake, Wall wall) {
        if (snake.xPosition[0] < 0 || snake.xPosition[0] >= wall.Width || snake.yPosition[0] < 0 || snake.yPosition[0] >= wall.Height){
            gameOver = true;
        }

        // Checkt of de slang tegen zichzelf aan botst
        for (int i = 1; i < snake.Length; i++){
            if (snake.xPosition[0] == snake.xPosition[i] && snake.yPosition[0] == snake.yPosition[i]){
                gameOver = true;
            }
        }
    }

    public void Pause() {
        static void CheckInput(ConsoleKeyInfo keyInfo){
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
        }
    }
}

public class Score{
    public int CurrentScore = 0;
    //Dictionary<string, int> Leaderboard = [];
    //Dictionary kan gebruikt worden voor een leaderboard. Leadboard.Add("Naam", Score);

    public void AddToLeaderboard(Score score){
        //Gebruiker krijgt de mogelijkheid om zijn naam in te vullen met zijn huidige score.
        //"Vul je naam in op op het leaderboard te komen!"
        //Leaderboard.Add(Console.ReadLine(), score.CurrentScore);
    }

    public void Display(){
        //Insert behaviours for current score and high score
    }

    public void Reset(){
        //Wanneer de game opnieuw start
        //score.currentScore = 0;
    }
}

class Program {
    static int width = 45, height = 15;
    //Score moet uiteindelijk in Score class komen.
    static int score = 0;
    //Speed zit al in Snake class, maar dingen verwijderen breekt veel
    static int speed = 8;
    //Game over logic is succesvol overgeplaatst naar Game.GameOver();
    //static bool gameOver = false;
    static int[] xPosition = new int[50], yPosition = new int[50];
    static int fruitX, fruitY;
    static int nTail = 0;
    static ConsoleKeyInfo keyInfo = new();
    static Random random = new();

    // Stopwatch voor het bijhouden van de tijd
    static Stopwatch stopwatch = new();

    static void Main() {
        Console.SetWindowSize(width + 5, height + 8);
        Console.CursorVisible = false;
        Setup();
        //Creeert een nieuw speelveld. Roep dit opnieuw aan als een nieuwe game start.
        new Wall(width, height).Draw();

        //Creer een score om te tracken, logica hiervoor zit gedeeltelijk in de Snake class
        Score currentScore = new();

        //Nieuwe slang wordt aangemaakt met de kleur groen, want slangen zijn groen. Mag alles zijn tho
        Snake Snake = new(ConsoleColor.Green);

        //Tekent de slang, deze logica moet in de while loop komen later
        Snake.Draw();

        //Nieuwe appel wordt aangemaakt met de kleur ROOD, want appels zijn over het algemeen rood? toch?
        Apple Apple = new(ConsoleColor.Red);

        //Apple wordt toegevoegd aan de appel lijst, om te tellen hoeveel appels er zijn.
        Apple.Apples.Add(Apple);

        //Teken de appel, de logica om te kijken of er 1 of 0 appels zijn zit in de klasse zelf
        Apple.Draw();

        ////////////////////////////
        // IK KAN NIET TEGEN HET FLIKKKEREN VAN MIJN SCHERM dus ik heb deze maar laten slapen.
        // We moeten een manier zoeken voor multithreading zodat Slang en Appel asynchroon kunnen draaien.
        // Als we dit niet doen krijgen we probleempjes, zoals dat je slang elke keer teleport naar een appel als die tevoorschijn komt :)
        //stopwatch.Start();
        //while (!gameOver){
        //    Draw();
        //    Pause();
        //    Movement();
        //    Thread.Sleep(speed);
        //}
        //stopwatch.Stop();
        //Console.SetCursorPosition(width / 2 - 4, height / 2);
        //Console.WriteLine("Game Over!");
        //Console.WriteLine("Your score: " + score);
        //Console.WriteLine("Time: " + stopwatch.Elapsed.ToString(@"mm\:ss"));
        //Console.ReadKey();
        ////////////////////////////
    }

    //Fruit posities moet in de Appel class komen, Start positie kan in Slang.Draw();
    static void Setup(){
        xPosition[0] = width / 2;
        yPosition[0] = height / 2;
        fruitX = random.Next(1, width - 1);
        fruitY = random.Next(1, height - 1);
    }

    // Draw methodes moeten opgesplitst worden bij hun bijbehorende class Snake/Apple/Wall/Score. MUUR IS AL GEDAAN! 
    static void Draw(){
        Console.Clear();

        for (int i = 0; i < width + 2; i++){ 
            Console.Write("=");
        }
        for (int i = 0; i < height; i++){
            for (int j = 0; j < width; j++){
                if (j == 0){ 
                    Console.Write("\u2551");
                }
                //if (i == yPosition[0] && j == xPosition[0]) {
                //    Console.ForegroundColor = ConsoleColor.Green; // Kleur van het hoofd van de slang wordt groen
                //    Console.Write("\u2588");
                //    Console.ResetColor();
                //}
                //else if (i == fruitY && j == fruitX){
                //    Console.ForegroundColor = ConsoleColor.Red; // Kleur van het fruit wordt rood
                //    Console.Write("\u2588");
                //    Console.ResetColor();
                //}
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

    static void Movement() {
        int prevX = xPosition[0], prevY = yPosition[0];
        int prev2X, prev2Y;

        // Dit moet naar Snake.Update() onder Move(), het moet ook niet mogelijk zijn om in tegenovergestelde richting te gaan
        // Anders ga je instant dood als je lengte meer dan 1 is :)
        // Beweging van het hoofd van de slang
        switch(keyInfo.Key) {
            case ConsoleKey.RightArrow:
                xPosition[0]++;
                Thread.Sleep((1000 / speed) / 2);
                break;
            case ConsoleKey.LeftArrow:
                xPosition[0]--;
                Thread.Sleep((1000 / speed) / 2);
                break;
            case ConsoleKey.DownArrow:
                yPosition[0]++;
                Thread.Sleep((1000 / speed));
                break;
            case ConsoleKey.UpArrow:
                yPosition[0]--;
                Thread.Sleep((1000 / speed));
                break;
        }

        //Ik snap dit compleet niet maar waarschijlijk moet het naar Snake.Update() ergens;
        // Beweging van de staart van de slang
        for(int i = 1;i < nTail;i++) {
            prev2X = xPosition[i];
            prev2Y = yPosition[i];
            xPosition[i] = prevX;
            yPosition[i] = prevY;
            prevX = prev2X;
            prevY = prev2Y;
        }

        ////////////////////////
        //Deze logica zit in Game.GameOver();
        // Checkt of de slang tegen de muur aan botst
        //if(xPosition[0] < 0 || xPosition[0] >= width || yPosition[0] < 0 || yPosition[0] >= height) {
        //    gameOver = true;
        //}

        //// Checkt of de slang tegen zichzelf aan botst
        //for(int i = 1;i < nTail;i++) {
        //    if(xPosition[0] == xPosition[i] && yPosition[0] == yPosition[i]) {
        //        gameOver = true;
        //    }
        //}
        ////////////////////////
        // DEZE LOGICA ZIT NU IN SLANG.GROW()
        // Checkt of de slang de fruit eet
        //if (xPosition[0] == fruitX && yPosition[0] == fruitY){
        //    score += 10;
        //    fruitX = random.Next(1, width - 1);
        //    fruitY = random.Next(1, height - 1);
        //    nTail++;
        //}
    }
}
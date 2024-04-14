    using System.Diagnostics;

    namespace Snake{

    public class Character{
        //Standaard waardes voor Characters
        public int[] xPosition, yPosition;
        public ConsoleColor Colour;
        public int Speed;
        public int Length;
        public (int x, int y) StartPosition;
    }

    public class Apple : Character {
        //Lijst van appels zodat ze makkelijk bijgehouden kunnen worden
        public static List<Apple> Apples = [];

        //Een constructor voor een appel, enige parameter is kleur
        public Apple(Wall wall, ConsoleColor colour){
            Colour = colour;
            Speed = 0;
            Length = 1;  
            StartPosition = (new Random().Next(1, wall.Width - 1), new Random().Next(1, wall.Height - 1));
        }

        //Tekenfunctie voor appel, als hij aangeroepen wordt zoekt hij een willekeurige plek om een appel te tekenen en voeg hem toe aan de appel lijst!
        public void Draw(Wall wall, Apple apple) {
            apple.xPosition[0] = new Random().Next(1, wall.Width - 1);
            apple.yPosition[0] = new Random().Next(1, wall.Height - 1);
            Console.SetCursorPosition(apple.xPosition[0], apple.yPosition[0]);
            Console.ForegroundColor = apple.Colour;
            Console.Write("\u2558");
            Console.ResetColor();
            Apples.Add(apple);
            //Ze de cursor terug naar die waar de slang zou zijn
            //Console.SetCursorPosition(oude positie);
        }

        public void Update(Wall wall, Apple apple) {
            //Als er meer dan 1 appel zou zijn, verwijder de appel uit de appel lijst
            if(Apples.Count() > 1) {
                Apples.Remove(apple);
            }
            //Als er geen appels zijn, roep de Draw() methode aan
            if(Apples.Count() == 0) {
                Draw(wall, apple);
            }
        }
    }

    

    //Simpele public class Snake : Character {
                      //Constructor voor slang, Wall om te kiezen tussen welke wall hij komt en kleur omdat we allemaal andere kleuren slang hebben
                      public Snake(Wall wall, ConsoleColor colour) {
                          Colour = colour;
                          Speed = 8;
                          Length = 1;
                          StartPosition = (wall.Width / 2, wall.Height / 2);
                      }
              
                      //Tekent de slang in slangenkleur en tekent over zijn spoor
                      public void Draw(Snake snake) {
                          if(snake.xPosition == null && snake.yPosition == null) {
                              Console.SetCursorPosition(StartPosition.x, StartPosition.y);
                          }
                          if(snake.xPosition != null && snake.yPosition != null){
                              Console.SetCursorPosition(snake.xPosition[0], snake.yPosition[0]);
                          }
                          Console.ForegroundColor = snake.Colour;
                          Console.Write("\u2558");
                          Console.ResetColor();
              
                          //Omdat je de console niet elke keer meer cleart moet je dit handmatig doen door naar de slang zijn staart
                          //Te gaan en het handmatig over te kleuren. Vergeet niet de cursor terug te doen.
                          if(snake.xPosition != null && snake.xPosition.Count() > snake.Length){
                              (int x, int y) currentPosition = (snake.xPosition[0], snake.yPosition[0]);
                              Console.SetCursorPosition(snake.xPosition[snake.Length], snake.yPosition[snake.Length]);
                              Console.ForegroundColor = ConsoleColor.Black;
                              Console.Write("\u2558");
                              Console.ResetColor();
                              Console.SetCursorPosition(currentPosition.x, currentPosition.y);
                          }
                      }
              
                      public void Update(Snake snake, Apple apple, Score score, ConsoleKeyInfo keyInfo){
                          int prevX = snake.xPosition[0], prevY = snake.yPosition[0];
                          int prev2X, prev2Y;
              
                          // Beweging van het hoofd van de slang
                          switch(keyInfo.Key) {
                              case ConsoleKey.RightArrow:
                                  snake.xPosition[0]++;
                                  Thread.Sleep((1000 / snake.Speed) / 2);
                                  break;
                              case ConsoleKey.LeftArrow:
                                  snake.xPosition[0]--;
                                  Thread.Sleep((1000 / snake.Speed) / 2);
                                  break;
                              case ConsoleKey.DownArrow:
                                  snake.yPosition[0]++;
                                  Thread.Sleep((1000 / snake.Speed));
                                  break;
                              case ConsoleKey.UpArrow:
                                  snake.yPosition[0]--;
                                  Thread.Sleep((1000 / snake.Speed));
                                  break;
                          }
              
                          // Beweging van de staart van de slang
                          for(int i = 1; i < snake.Length; i++) {
                              prev2X = snake.xPosition[i];
                              prev2Y = snake.yPosition[i];
                              snake.xPosition[i] = prevX;
                              snake.yPosition[i] = prevY;
                              prevX = prev2X;
                              prevY = prev2Y;
                          }   
              
                          //Als de slang positie over een appel positie gaat, meer lengte, meer snelhid, meer punten, nog meer punten hoe sneller je bent!
                          if(apple.xPosition == snake.xPosition && apple.yPosition == snake.yPosition) {
                              snake.Length++;
                              snake.Speed++;
                              //Gebaseerd op de slang zijn snelheid krijgt hij meer punten
                              score.CurrentScore += (10+(10*(snake.Speed/100)));
                          }
                      }  
                  }class voor muur tekenen
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

    //Class bevat alles wat over de game status gaat
    public class Game {
        public bool gameOver = false;
        Stopwatch timer = new();

        //Wordt aan het begin aangeroepen om het spel te starten
        //Op het moment zorgt het alleen dat ze slang in het midden komt
        public void Start(Snake snake, Score score) {
            //Plaatst de Slang in het midden van het speelveld om te beginnen met het spel :)
            Console.SetCursorPosition(snake.StartPosition.x, snake.StartPosition.y);
                if(Console.KeyAvailable) {
                    timer.Start();
                    score.CurrentScore = 0;
                    Apple.Apples.Clear();
                }
            }

            public void GameOver(Snake snake, Wall wall, Score score) {
                if (snake.xPosition[0] < 0 || snake.xPosition[0] >= wall.Width || snake.yPosition[0] < 0 || snake.yPosition[0] >= wall.Height){
                    gameOver = true;
                }

                // Checkt of de slang tegen zichzelf aan botst
                for (int i = 1; i < snake.Length; i++){
                    if (snake.xPosition[0] == snake.xPosition[i] && snake.yPosition[0] == snake.yPosition[i]){
                        gameOver = true;
                    }
                }
                timer.Stop();
                Console.WriteLine($"Score: {score.CurrentScore}");
                Console.WriteLine($"Time: {timer.Elapsed.ToString(@"mm\:ss")}");
            }

        public void Pause(ConsoleKeyInfo keyInfo) {
            if (Console.KeyAvailable) {
                keyInfo = Console.ReadKey(true); // true om te voorkomen dat de toets wordt weergegeven in de console
                if (keyInfo.Key == ConsoleKey.P){
                    Console.WriteLine("\nSpel gepauzeerd. Druk op 'P' om te hervatten.");
                    bool paused = true;
                    while(paused) {
                        if(Console.KeyAvailable) {
                            ConsoleKeyInfo keyPress = Console.ReadKey(true);
                            if(keyPress.Key == ConsoleKey.P) {
                                paused = false;
                            }
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
        //Game over logic is succesvol overgeplaatst naar Game.GameOver();
        //static bool gameOver = false;
        static int[] xPosition = new int[50], yPosition = new int[50];

        static void Main() {
            Console.SetWindowSize(width + 5, height + 8);
            Console.CursorVisible = false;

            //Creeert een nieuw speelveld. Roep dit opnieuw aan als een nieuwe game start.
            Wall wall = new(width, height);
            wall.Draw();

            //Creer een score om te tracken, logica hiervoor zit gedeeltelijk in de Snake class
            Score currentScore = new();

            //Nieuwe slang wordt aangemaakt met de kleur groen, want slangen zijn groen. Mag alles zijn tho
            Snake snake = new(wall, ConsoleColor.Green);

            //Nieuwe appel wordt aangemaakt met de kleur ROOD, want appels zijn over het algemeen rood? toch?
            Apple apple = new(wall, ConsoleColor.Red);

            //Apple wordt toegevoegd aan de appel lijst, om te tellen hoeveel appels er zijn.
            Apple.Apples.Add(apple);

            Game Game = new();
            ////////////////////////////
            // IK KAN NIET TEGEN HET FLIKKKEREN VAN MIJN SCHERM dus ik heb deze maar laten slapen.
            // We moeten een manier zoeken voor multithreading zodat Slang en Appel asynchroon kunnen draaien.
            // Als we dit niet doen krijgen we probleempjes, zoals dat je slang elke keer teleport naar een appel als die tevoorschijn komt :)
            //stopwatch.Start();
            while(!Game.gameOver) {
                snake.Draw(snake);
                apple.Draw(wall, apple);
                snake.Update(snake, apple, currentScore, Console.ReadKey());
                apple.Update(wall, apple);
                Game.Pause(Console.ReadKey());
            }
            //stopwatch.Stop();
            //Console.SetCursorPosition(width / 2 - 4, height / 2);
            //Console.WriteLine("Game Over!");
            //Console.WriteLine("Your score: " + score);
            //Console.WriteLine("Time: " + stopwatch.Elapsed.ToString(@"mm\:ss"));
            //Console.ReadKey();
            ////////////////////////////
        }

        //Fruit posities moet in de Appel class komen, Start positie kan in Slang.Draw();
        //static void Setup(){
        //    xPosition[0] = width / 2;
        //    yPosition[0] = height / 2;
        //    fruitX = random.Next(1, width - 1);
        //    fruitY = random.Next(1, height - 1);
        //}

        // Draw methodes moeten opgesplitst worden bij hun bijbehorende class Snake/Apple/Wall/Score. MUUR IS AL GEDAAN! 
        //static void Draw(){
        //    Console.Clear();

        //    for (int i = 0; i < width + 2; i++){ 
        //        Console.Write("=");
        //    }
        //    for (int i = 0; i < height; i++){
        //        for (int j = 0; j < width; j++){
        //            if (j == 0){ 
        //                Console.Write("\u2551");
        //            }
        //            if(i == yPosition[0] && j == xPosition[0]) {
        //                Console.ForegroundColor = ConsoleColor.Green; // Kleur van het hoofd van de slang wordt groen
        //                Console.Write("\u2588");
        //                Console.ResetColor();
        //            }
        //            else if(i == fruitY && j == fruitX) {
        //                Console.ForegroundColor = ConsoleColor.Red; // Kleur van het fruit wordt rood
        //                Console.Write("\u2588");
        //                Console.ResetColor();
        //            }
        //            else {
        //                bool print = false;
        //                for (int k = 0; k < nTail; k++) {
        //                    if (xPosition[k] == j && yPosition[k] == i) {
        //                        Console.ForegroundColor = ConsoleColor.Green; // Kleur van de staart van de slang wordt groen
        //                        Console.Write("\u2588");
        //                        Console.ResetColor(); // Reset de kleur
        //                        print = true;
        //                    }
        //                }
        //                if (!print) {
        //                    Console.Write(" ");
        //                    }
        //                }

        //                if (j == width - 1){ 
        //                    Console.Write("\u2551");
        //                }
        //        }
        //        Console.WriteLine();
        //    }

        //    for (int i = 0; i < width + 2; i++){
        //        Console.Write("=");
        //    }
        //    Console.WriteLine();
        //    Console.WriteLine("Score: " + score);
        //}

        //static void Movement(ConsoleKeyInfo keyInfo, Snake snake) {
        //    int prevX = xPosition[0], prevY = yPosition[0];
        //    int prev2X, prev2Y;

        // Dit moet naar Snake.Update() onder Move(), het moet ook niet mogelijk zijn om in tegenovergestelde richting te gaan
        // Anders ga je instant dood als je lengte meer dan 1 is :)
        // Beweging van het hoofd van de slang
        //switch(keyInfo.Key) {
        //    case ConsoleKey.RightArrow:
        //        xPosition[0]++;
        //        Thread.Sleep((1000 / snake.Speed) / 2);
        //        break;
        //    case ConsoleKey.LeftArrow:
        //        xPosition[0]--;
        //        Thread.Sleep((1000 / speed) / 2);
        //        break;
        //    case ConsoleKey.DownArrow:
        //        yPosition[0]++;
        //        Thread.Sleep((1000 / speed));
        //        break;
        //    case ConsoleKey.UpArrow:
        //        yPosition[0]--;
        //        Thread.Sleep((1000 / speed));
        //        break;
        //}

        //Ik snap dit compleet niet maar waarschijlijk moet het naar Snake.Update() ergens;
        // Beweging van de staart van de slang
        //for(int i = 1;i < nTail;i++) {
        //    prev2X = xPosition[i];
        //    prev2Y = yPosition[i];
        //    xPosition[i] = prevX;
        //    yPosition[i] = prevY;
        //    prevX = prev2X;
        //    prevY = prev2Y;
        //}

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

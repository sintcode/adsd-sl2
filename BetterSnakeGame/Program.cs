using System.Xml.Linq;

namespace BetterSnakeGame {
    public class Character {
        public (int x, int y) Position;
        public ConsoleColor Color;
        public int Speed;
        public int Length;
    }

    class Snake : Character {
        public Snake((int x, int y) position, ConsoleColor color, int speed, int length) {
            Position = (position.x, position.y);
            Color = color;
            Speed = speed;
            Length = length;
        }

        public static void Draw(Snake snake){
            Console.ForegroundColor = snake.Color;
            Console.Write("\u2588");
            Console.ResetColor();
        }

        public void Update(){

        }
    }

    class Apple : Character {
        public Apple((int x, int y) position, ConsoleColor color, int speed, int length) {
            Position = (position.x, position.y);
            Color = color;
            Speed = 0;
            Length = 1;
        }

        public static void Draw(Apple apple) {
            Console.ForegroundColor = apple.Color;
            Console.Write("\u2588");
            Console.ResetColor();
        }

        public void Update() {

        }
    }

    public class Game {
        void Start(){

        }

        void Pause(){

        }

        void Over(){

        }
    }

    public class Score {
        public int CurrentScore = 0;
        public List<(string name, int score, int time)> Leaderboard = [];

        void DisplayScore() {
            Console.WriteLine($"Current score: {CurrentScore}");
        }

        void DisplayLeaderBoard(){
            Leaderboard.Sort();
            foreach((string name, int score, int time) in Leaderboard) {
                Console.WriteLine($"{name} | {score} | {time}");
            }
        }

        void AddToLeaderboard() {
            Console.WriteLine("Your Name: ");
            string? name = Console.ReadLine();
            if(name != null){
                Leaderboard.Add((name, 2, 2));
                Console.WriteLine("Succesfully added to leaderboard");
            }
            else {
                Console.WriteLine("Please provide a valid name");
            }
            Reset();
        }

        void Reset(){
            CurrentScore = 0;
        }
    }

    public class Map {
        public int Height, Width;

        public Map(int width,int height) {
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

    internal class Program {
        static int mapSize = 45;

        static void Main() {
            Map playArea = new(mapSize, mapSize/3);
            playArea.Draw();

            Snake snake = new((mapSize/2, mapSize/6), ConsoleColor.Green, 8, 1);
            Apple apple = new((new Random().Next(1, mapSize - 1),(new Random().Next(1, (mapSize/3) - 1)), ConsoleColor.Red, 0, 1);

            while(1==1) {
                Console.SetCursorPosition(snake.Position.x + 1, snake.Position.y + 1);
                Snake.Draw(snake);
                Apple.Draw(apple);
            }
        }
    }
}

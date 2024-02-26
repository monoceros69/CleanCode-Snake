using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            Random random = new Random();
            int score = 5;
            int gameOver = 0;

            Pixel head = new Pixel
            {
                XPos = screenWidth / 2,
                YPos = screenHeight / 2,
                ScreenColor = ConsoleColor.Red
            };

            string movement = "RIGHT";

            List<int> xPosLijf = new List<int>();
            List<int> yPosLijf = new List<int>();

            int berryX = random.Next(1, screenWidth - 2);
            int berryY = random.Next(1, screenHeight - 2);

            DateTime startTime = DateTime.Now;
            DateTime currentTime = DateTime.Now;
            string buttonPressed = "no";

            while (true)
            {
                Console.Clear();

                if (head.XPos == screenWidth - 1 || head.XPos == 0 || head.YPos == screenHeight - 1 || head.YPos == 0)
                {
                    gameOver = 1;
                }

                DrawBorders(screenWidth, screenHeight);

                Console.ForegroundColor = ConsoleColor.Green;

                if (berryX == head.XPos && berryY == head.YPos)
                {
                    score++;
                    berryX = random.Next(1, screenWidth - 2);
                    berryY = random.Next(1, screenHeight - 2);
                }

                DrawSnakeBody(xPosLijf, yPosLijf, head);

                if (gameOver == 1)
                {
                    break;
                }

                Console.SetCursorPosition(head.XPos, head.YPos);
                Console.ForegroundColor = head.ScreenColor;
                Console.Write("■");

                Console.SetCursorPosition(berryX, berryY);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");

                startTime = DateTime.Now;
                buttonPressed = "no";

                while (true)
                {
                    currentTime = DateTime.Now;

                    if (currentTime.Subtract(startTime).TotalMilliseconds > 500)
                    {
                        break;
                    }

                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                        if (IsValidKeyPress(keyInfo.Key, movement, buttonPressed))
                        {
                            movement = GetNewDirection(keyInfo.Key);
                            buttonPressed = "yes";
                        }
                    }
                }

                xPosLijf.Add(head.XPos);
                yPosLijf.Add(head.YPos);

                MoveHead(movement, head);

                if (xPosLijf.Count > score)
                {
                    xPosLijf.RemoveAt(0);
                    yPosLijf.RemoveAt(0);
                }

                Thread.Sleep(50); // Adjust sleep time for smoother animation
            }

            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        }

        static void DrawBorders(int screenWidth, int screenHeight)
        {
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }
        }

        static void DrawSnakeBody(List<int> xPos, List<int> yPos, Pixel head)
        {
            for (int i = 0; i < xPos.Count(); i++)
            {
                Console.SetCursorPosition(xPos[i], yPos[i]);
                Console.Write("■");

                if (xPos[i] == head.XPos && yPos[i] == head.YPos)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(head.XPos, head.YPos);
                    Console.Write("■");
                }
            }
        }

        static bool IsValidKeyPress(ConsoleKey key, string currentMovement, string buttonPressed)
        {
            return key switch
            {
                ConsoleKey.UpArrow => currentMovement != "DOWN" && buttonPressed == "no",
                ConsoleKey.DownArrow => currentMovement != "UP" && buttonPressed == "no",
                ConsoleKey.LeftArrow => currentMovement != "RIGHT" && buttonPressed == "no",
                ConsoleKey.RightArrow => currentMovement != "LEFT" && buttonPressed == "no",
                _ => false
            };
        }

        static string GetNewDirection(ConsoleKey key)
        {
            return key switch
            {
                ConsoleKey.UpArrow => "UP",
                ConsoleKey.DownArrow => "DOWN",
                ConsoleKey.LeftArrow => "LEFT",
                ConsoleKey.RightArrow => "RIGHT",
                _ => ""
            };
        }

        static void MoveHead(string direction, Pixel head)
        {
            switch (direction)
            {
                case "UP":
                    head.YPos--;
                    break;
                case "DOWN":
                    head.YPos++;
                    break;
                case "LEFT":
                    head.XPos--;
                    break;
                case "RIGHT":
                    head.XPos++;
                    break;
            }
        }

        class Pixel
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor ScreenColor { get; set; }
        }
    }
}

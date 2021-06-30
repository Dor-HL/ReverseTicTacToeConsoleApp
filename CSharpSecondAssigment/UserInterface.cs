using System;
using System.Text;

namespace B21_Ex02
{
    public class UserInterface
    {
        private readonly GameLogic r_LogicOfGame;

        public UserInterface()
        {
            r_LogicOfGame = new GameLogic();
        }

        private static bool presentMenuAfterGameEnd()
        {
            bool toContinueGame = true;
            Console.WriteLine("press 1 to play another round");
            Console.WriteLine("press 2 to exit the game");
            string userInput = Console.ReadLine();
            bool isInt = int.TryParse(userInput, out int userInputAsInt);
            while ((isInt == false) || (userInputAsInt != 1 && userInputAsInt != 2))
            {
                Console.WriteLine("Please enter a valid input");
                userInput = Console.ReadLine();
                isInt = int.TryParse(userInput, out userInputAsInt);
            }

            if (userInputAsInt == 2)
            {
                toContinueGame = false;
            }

            return toContinueGame;
        }

        public void RunProgram() 
        {
            bool isInputIllegal = true;

            Console.WriteLine("Please choose the width and length of the board - A number between 3 - 9");
            string readValue = Console.ReadLine();
            int.TryParse(readValue, out int sizeOfBoard);

            while(isInputIllegal)
            {
                if(sizeOfBoard < 3 || sizeOfBoard > 9)
                {
                    Console.WriteLine("This input is illegal! please enter a valid input, a number between 3 - 9");
                    readValue = Console.ReadLine();
                    int.TryParse(readValue, out sizeOfBoard);
                }
                else
                {
                    isInputIllegal = false;
                }
            }

            r_LogicOfGame.GameBoard = new GameBoard(sizeOfBoard);
            isInputIllegal = true;
            Console.WriteLine("press 1 for player VS player mode");
            Console.WriteLine("press 2 for player VS computer mode");
            readValue = Console.ReadLine();
            int.TryParse(readValue, out int gameMode);

            while(isInputIllegal)
            {
                if((gameMode != 1) && (gameMode != 2))
                {
                    Console.WriteLine("This input is illegal! please enter a valid input");
                    readValue = Console.ReadLine();
                    int.TryParse(readValue, out gameMode);
                }
                else
                {
                    isInputIllegal = false;
                }
            }

            runReverseTicTacToeGame(gameMode);
        }
       
        private void runReverseTicTacToeGame(int i_GameMode)
        {
            int gameBoardCounter = 0;
            bool isGameStillOn = true;
            bool isQPressed = false;
            int rowUserInput = 0;
            int colUserInput = 0;
            Console.Clear();
            printBoard();

            if(i_GameMode == 2)
            {
               r_LogicOfGame.PlayerTwo.IsPlayerHuman = false;
            }

            while(isGameStillOn)
            {
                if(r_LogicOfGame.IsFirstPlayerTurn)
                {
                   r_LogicOfGame.PlayerOne.PlayerMove(ref rowUserInput, ref colUserInput, r_LogicOfGame, r_LogicOfGame.PlayerTwo.Symbol);
                    if(r_LogicOfGame.PlayerOne.IsPlayerHuman)
                    {
                        isQPressed = getAndCheckUserInput(ref rowUserInput, ref colUserInput, i_GameMode);
                    }

                    if(!isQPressed)
                    {
                        r_LogicOfGame.GameBoard.AddSymbolToBoard(r_LogicOfGame.PlayerOne.Symbol, rowUserInput, colUserInput);
                        gameBoardCounter++;

                        if(r_LogicOfGame.CheckIfLost(r_LogicOfGame.PlayerOne.Symbol))
                        {
                            r_LogicOfGame.PlayerTwo.CurrentScore++;
                            isGameStillOn = false;
                        }
                        else if(r_LogicOfGame.IsTie(gameBoardCounter))
                        {
                            isGameStillOn = false;
                        }

                        r_LogicOfGame.IsFirstPlayerTurn = false;
                        Console.Clear();
                        printBoard();
                    }
                    else
                    {
                        r_LogicOfGame.PlayerTwo.CurrentScore++;
                    }
                }
                else
                {
                    r_LogicOfGame.PlayerTwo.PlayerMove(ref rowUserInput, ref colUserInput, r_LogicOfGame, r_LogicOfGame.PlayerOne.Symbol);
                    if(r_LogicOfGame.PlayerTwo.IsPlayerHuman)
                    {
                        isQPressed = getAndCheckUserInput(ref rowUserInput, ref colUserInput, i_GameMode);
                    }

                    if(!isQPressed)
                    {
                        r_LogicOfGame.GameBoard.AddSymbolToBoard(r_LogicOfGame.PlayerTwo.Symbol, rowUserInput, colUserInput);
                        gameBoardCounter++;
                        if(r_LogicOfGame.CheckIfLost(r_LogicOfGame.PlayerTwo.Symbol))
                        {
                            r_LogicOfGame.PlayerOne.CurrentScore++;
                            isGameStillOn = false;
                        }
                        else if(r_LogicOfGame.IsTie(gameBoardCounter))
                        {
                            isGameStillOn = false;
                        }

                        r_LogicOfGame.IsFirstPlayerTurn = true;
                        Console.Clear();
                        printBoard();
                    }
                    else
                    {
                        r_LogicOfGame.PlayerOne.CurrentScore++;
                    }
                }

                if(isGameStillOn == false || isQPressed)
                {
                    displayScores();
                    if(presentMenuAfterGameEnd())
                    {
                        r_LogicOfGame.GameBoard.EmptyBoard();
                        gameBoardCounter = 0;
                        Console.Clear();
                        printBoard();
                        r_LogicOfGame.IsFirstPlayerTurn = true;
                        isGameStillOn = true;
                    }
                    else
                    {
                        Console.WriteLine("Thank you for playing!");
                        System.Environment.Exit(1);
                    }
                }
            }
        }

        private bool getAndCheckUserInput(ref int io_RowUserInput, ref int io_ColUserInput, int i_GameMode)
        {
            bool isQPressed = false;

            do
            {
                Console.WriteLine("Please enter row number");
                string userInputAsString = Console.ReadLine();
                bool isInt = int.TryParse(userInputAsString, out int rowUserInput);
                if(isInt == false)
                {
                    isExitSymbolPressed(userInputAsString, i_GameMode, ref isQPressed);
                }

                if(!isQPressed)
                {
                    io_RowUserInput = rowUserInput;
                    Console.WriteLine("Please enter a col number");
                    userInputAsString = Console.ReadLine();
                    isInt = int.TryParse(userInputAsString, out int colUserInput);
                    if(isInt == false)
                    {
                        isExitSymbolPressed(userInputAsString, i_GameMode, ref isQPressed);
                    }

                    io_ColUserInput = colUserInput;
                }

                if(isQPressed)
                {
                    break;
                }
            }
            while(!r_LogicOfGame.IsMoveLegal(io_RowUserInput, io_ColUserInput));

            return isQPressed;
        }

        private void displayScores() // change name to game mode
        {
            if(r_LogicOfGame.PlayerTwo.IsPlayerHuman)
            {
                string gameResult = string.Format("The current scores are:{0}Player One has:{1} points{2}player Two has:{3} points", Environment.NewLine, r_LogicOfGame.PlayerOne.CurrentScore, Environment.NewLine, r_LogicOfGame.PlayerTwo.CurrentScore);
                Console.WriteLine(gameResult);
                if(r_LogicOfGame.PlayerOne.CurrentScore > r_LogicOfGame.PlayerTwo.CurrentScore)
                {
                    Console.WriteLine("The Winner is player one!");
                    Console.WriteLine();
                }
                else if(r_LogicOfGame.PlayerOne.CurrentScore == r_LogicOfGame.PlayerTwo.CurrentScore)
                {
                    Console.WriteLine("The game is currently tied!");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("The Winner is player two!");
                    Console.WriteLine();
                }
            }
            else
            {
                string gameResult = string.Format("The current scores are:{0}the player has {1} points ,the computer has {2} points", Environment.NewLine, r_LogicOfGame.PlayerOne.CurrentScore, r_LogicOfGame.PlayerTwo.CurrentScore);
                Console.WriteLine(gameResult);
                if(r_LogicOfGame.PlayerOne.CurrentScore > r_LogicOfGame.PlayerTwo.CurrentScore)
                {
                    Console.WriteLine("The Winner is the player!");
                }
                else if(r_LogicOfGame.PlayerOne.CurrentScore == r_LogicOfGame.PlayerTwo.CurrentScore)
                {
                    Console.WriteLine("The game is currently tied!");
                }
                else
                {
                    Console.WriteLine("The Winner is the computer - unstoppable AI!");
                }
            }
        }

     
        private void isExitSymbolPressed(string i_UserInputAsString, int i_GameMode, ref bool io_IsQPressed)
        {
            bool isChar = char.TryParse(i_UserInputAsString, out char charInput);
            if(isChar)
            {
                if(charInput == 'Q')
                {
                    io_IsQPressed = true;
                    Console.WriteLine("You have pressed 'Q', quitting game now");
                    Console.WriteLine();
                }
            }
        }

        private void printBoard()
        {
            StringBuilder stringToPrint = new StringBuilder();
            int sizeOfGameBoard = r_LogicOfGame.GameBoard.SizeOfBoard;
            for (int i = 1; i <= sizeOfGameBoard; i++)
            {
                stringToPrint.Append("   " + i);
            }

            stringToPrint.Append(Environment.NewLine);
            for (int i = 0; i < sizeOfGameBoard; i++)
            {
                stringToPrint.Append((i + 1) + "|");
                for (int j = 0; j < sizeOfGameBoard; j++)
                {
                    stringToPrint.Append(" " + r_LogicOfGame.GameBoard.Board[i, j] + " |");
                }

                stringToPrint.Append(Environment.NewLine);
                int howManyTimesToPrint = (4 * sizeOfGameBoard) + 1;
                stringToPrint.Append(" ");
                for (int j = 0; j < howManyTimesToPrint; j++)
                {
                    stringToPrint.Append("=");
                }

                stringToPrint.Append(Environment.NewLine);
            }

            Console.WriteLine(stringToPrint);
        }
    }
}

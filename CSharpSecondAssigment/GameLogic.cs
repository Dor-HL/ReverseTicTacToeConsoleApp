using System;

namespace B21_Ex02
{
    public class GameLogic
    {
        private GameBoard m_ReverseTicTacToeBoard;
        private Player m_PlayerOne;
        private Player m_PlayerTwo;
        private bool m_IsFirstPlayerTurn;

        public GameLogic()
        {
            bool isHuman = true;
            int defaultSize = 3;
            m_PlayerOne = new Player('X', isHuman, 1);
            m_PlayerTwo = new Player('O', isHuman, 2);
            m_IsFirstPlayerTurn = true;
            GameBoard = new GameBoard(defaultSize);
        }

        public bool IsFirstPlayerTurn
        {
            get
            {
                return m_IsFirstPlayerTurn;
            }

            set
            {
                m_IsFirstPlayerTurn = value;
            }
        }

        public Player PlayerOne
        {
            get
            {
                return m_PlayerOne;
            }

            set
            {
                m_PlayerOne = value;
            }
        }

        public Player PlayerTwo
        {
            get
            {
                return m_PlayerTwo;
            }

            set
            {
                m_PlayerTwo = value;
            }
        }

        public GameBoard GameBoard// change to get cell 
        {
            get
            {
                return m_ReverseTicTacToeBoard;
            }

            set
            {
                m_ReverseTicTacToeBoard = value;
            }
        }

        public bool IsMoveLegal(int i_Row, int i_Column)
        {
            bool isLegal = true;
            int sizeOfBoard = m_ReverseTicTacToeBoard.SizeOfBoard;

            if(i_Row > sizeOfBoard || i_Row < 1)
            {
                Console.WriteLine("This Row is illegal! please choose a valid row on the board");
                isLegal = false;
            }

            if(i_Column > sizeOfBoard || i_Column < 1)
            {
                Console.WriteLine("This col is illegal! please choose a valid col on the board");
                isLegal = false;
            }

            if(isLegal)
            {
                if(checkIfPlacementIsAlreadyTaken(i_Row, i_Column))
                {
                    Console.WriteLine("This location is already taken! please choose a valid location on the board");
                    isLegal = false;
                }
            }

            return isLegal;
        }

        private bool checkIfPlacementIsAlreadyTaken(int i_Row, int i_Column)
        {
            bool isPlaceTaken = m_ReverseTicTacToeBoard.Board[i_Row - 1, i_Column - 1] != ' ';

            return isPlaceTaken;
        }

        public bool CheckIfLost(char i_Symbol)
        {
            bool isLose = false;
            int symbolCounter = 0;
            int sizeOfBoard = m_ReverseTicTacToeBoard.SizeOfBoard;

            // checks for full rows
            for(int r = 1; r <= sizeOfBoard; r++)
            {
                int c = 1;
                symbolCounter = 0;
                while(c <= sizeOfBoard)
                {
                    if(m_ReverseTicTacToeBoard.Board[r - 1, c - 1] == i_Symbol)
                    {
                        symbolCounter++;
                        c++;
                    }
                    else
                    {
                        break;
                    }
                }

                if(symbolCounter == sizeOfBoard)
                {
                    isLose = true;
                    break;
                }
            }

            // checks for full column
            for(int c = 1; c <= sizeOfBoard; c++)
            {
                int r = 1;
                symbolCounter = 0;
                while(r <= sizeOfBoard)
                {
                    if(m_ReverseTicTacToeBoard.Board[r - 1, c - 1] == i_Symbol)
                    {
                        symbolCounter++;
                        r++;
                    }
                    else
                    {
                        break;
                    }
                }

                if(symbolCounter == sizeOfBoard)
                {
                    isLose = true;
                    break;
                }
            }

            int currentRow = 1;
            int currentCol = 1;
            symbolCounter = 0;

            // check for diagonal line
            while(currentRow <= sizeOfBoard && currentCol <= sizeOfBoard)
            {
                if(m_ReverseTicTacToeBoard.Board[currentRow - 1, currentCol - 1] == i_Symbol)
                {
                    symbolCounter++;
                    currentRow++;
                    currentCol++;
                }
                else
                {
                    break;
                }

                if(symbolCounter == sizeOfBoard)
                {
                    isLose = true;
                    break;
                }
            }

            currentRow = 1;
            currentCol = sizeOfBoard;
            symbolCounter = 0;

            // check for diagonal line
            while(currentRow <= sizeOfBoard && currentCol >= 1)
            {
                if(m_ReverseTicTacToeBoard.Board[currentRow - 1, currentCol - 1] == i_Symbol)
                {
                    symbolCounter++;
                    currentRow++;
                    currentCol--;
                }
                else
                {
                    break;
                }

                if(symbolCounter == sizeOfBoard)
                {
                    isLose = true;
                    break;
                }
            }

            return isLose;
        }

        public bool IsTie(int i_GameBoardCounter)
        {
            int fullGameBoard = m_ReverseTicTacToeBoard.SizeOfBoard * m_ReverseTicTacToeBoard.SizeOfBoard;
            return i_GameBoardCounter == fullGameBoard;
        }
   
        public void RandomComputerMove(char i_Symbol, ref int o_RowInput, ref int o_ColInput)
        {
            bool moveLegal = false;
            Random rnd = new Random();
            int generateRandomRow = 0;
            int generateRandomCol = 0;
            int sizeOfBoard = m_ReverseTicTacToeBoard.SizeOfBoard;

            while(moveLegal == false)
            {
                generateRandomRow = rnd.Next(1, sizeOfBoard + 1);
                generateRandomCol = rnd.Next(1, sizeOfBoard + 1);
                if(!checkIfPlacementIsAlreadyTaken(generateRandomRow, generateRandomCol))
                {
                    moveLegal = true;
                }
            }

            o_RowInput = generateRandomRow;
            o_ColInput = generateRandomCol;
        }

        // This function finds the ideal placement for the computer's move. The function looks for a placement that isn't going to disqualify the computer
        // as well as not occupy a location that can lead to the player's lose. if he cannot find a placment that does both, than he prioritize 
        // not losing over not blocking the other player. if non of those two options are found, the move will be randomized (will happen when the only option is 
        // either to lose or there is only one place left on the board)
        public bool CalculateComputerMoveAi(char i_Symbol, char i_OpponentSymbol, ref int[] io_ChosenIdealPlacement)
        {
            bool isDiagonalFull = false;
            int[] chosenPlacementBasedOnlyOnMySymbol = new int[2];
            bool isIdealPlacementFound = false;
            bool isPlacementBasedOnPcSymbolFound = false;
            int sizeOfBoard = m_ReverseTicTacToeBoard.SizeOfBoard;

            for(int r = 1; r <= sizeOfBoard; r++)
            { 
                    if(isIdealPlacementFound)
                    {
                        break;
                    }

                    for(int c = 1; c <= sizeOfBoard; c++)
                    {
                        if(m_ReverseTicTacToeBoard.Board[r - 1, c - 1] == ' ')
                        {
                            bool isCurrentRowOrColFull = isRowOrColFullOfMySymbol(i_Symbol, r, c);
                            if(r == c || (r + c == sizeOfBoard + 1))
                            {
                                isDiagonalFull = isDiagonalFullOfMySymbol(i_Symbol, r, c);
                            }

                            if(!isCurrentRowOrColFull && !isDiagonalFull)
                            {
                                isPlacementBasedOnPcSymbolFound = true;
                                chosenPlacementBasedOnlyOnMySymbol[0] = r;
                                chosenPlacementBasedOnlyOnMySymbol[1] = c;
                                isCurrentRowOrColFull = isRowOrColFullOfMySymbol(i_OpponentSymbol, r, c);
                                if(r == c || (r + c == sizeOfBoard + 1))
                                {
                                    isDiagonalFull = isDiagonalFullOfMySymbol(i_OpponentSymbol, r, c);
                                }

                                if(!isCurrentRowOrColFull && !isDiagonalFull)
                                {
                                    io_ChosenIdealPlacement[0] = r;
                                    io_ChosenIdealPlacement[1] = c;
                                    isIdealPlacementFound = true;
                                    break;
                                }
                            }
                        }
                    }
            }
            
            if(!isIdealPlacementFound && isPlacementBasedOnPcSymbolFound)
            {
                io_ChosenIdealPlacement[0] = chosenPlacementBasedOnlyOnMySymbol[0];
                io_ChosenIdealPlacement[1] = chosenPlacementBasedOnlyOnMySymbol[1];
                isIdealPlacementFound = true;
            }

            return isIdealPlacementFound;
        }

        private bool isRowOrColFullOfMySymbol(char i_Symbol, int i_Row, int i_Col)
        {
            int rowSymbolCounter = 0;
            int colSymbolCounter = 0;
            int currentColAndRow = 1;
            bool isCurrentRowOrColFull = false;
            int sizeOfBoard = m_ReverseTicTacToeBoard.SizeOfBoard;
            
            while (currentColAndRow <= sizeOfBoard)
            {
                if (m_ReverseTicTacToeBoard.Board[i_Row - 1, currentColAndRow - 1] == i_Symbol)
                {
                    rowSymbolCounter++;
                }

                if (m_ReverseTicTacToeBoard.Board[currentColAndRow - 1, i_Col - 1] == i_Symbol)
                {
                    colSymbolCounter++;
                }

                currentColAndRow++;
            }

            if(rowSymbolCounter == sizeOfBoard - 1 || colSymbolCounter == sizeOfBoard - 1)
            {
                isCurrentRowOrColFull = true;
            }

            return isCurrentRowOrColFull;
        }

        private bool isDiagonalFullOfMySymbol(char i_Symbol, int i_Row, int i_Col)
        {
            int boardHalfPoint = m_ReverseTicTacToeBoard.SizeOfBoard / 2;
            int sizeOfBoard = m_ReverseTicTacToeBoard.SizeOfBoard;
            int currentRow = 1;
            int currentCol = 1;
            bool isDiagonalFull = false;
            int symbolCounter = 0;

            if(i_Col <= boardHalfPoint)
            {
                while(currentRow <= sizeOfBoard && currentCol <= sizeOfBoard)
                {
                    if(m_ReverseTicTacToeBoard.Board[currentRow - 1, currentCol - 1] == i_Symbol)
                    {
                        symbolCounter++;
                    }

                    currentRow++;
                    currentCol++;
                }

                if(symbolCounter == sizeOfBoard - 1)
                {
                    isDiagonalFull = true;
                }
            }
            else if(i_Col >= boardHalfPoint)
            {
                symbolCounter = 0;
                currentCol = sizeOfBoard;
                currentRow = 1;
                while(currentRow <= sizeOfBoard && currentCol >= 1)
                {
                    if(m_ReverseTicTacToeBoard.Board[currentRow - 1, currentCol - 1] == i_Symbol)
                    {
                        symbolCounter++;
                    }

                    currentRow++;
                    currentCol--;
                }

                if(symbolCounter == sizeOfBoard - 1)
                {
                    isDiagonalFull = true;
                }
            }

            return isDiagonalFull;
        }
    }
}

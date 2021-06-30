using System;

namespace B21_Ex02
{
    public class Player
    {
        private int m_PlayerScore;
        private char m_PlayerSymbol;
        private bool m_IsHuman;
        private int m_PlayerNumber;

        public int PlayerNumber
        {
            get
            {
                return m_PlayerNumber;
            }

            set
            {
                m_PlayerNumber = value;
            }
        }

        public int CurrentScore
        {
            get
            {
                return m_PlayerScore;
            }

            set
            {
                m_PlayerScore = value;
            }
        }

        public bool IsPlayerHuman
        {
            get
            {
                return m_IsHuman;
            }

            set
            {
                m_IsHuman = value;
            }
        }

        public char Symbol
        {
            get
            {
                return m_PlayerSymbol;
            }

            set
            {
                m_PlayerSymbol = value;
            }
        }

        public Player(char i_MySymbol, bool i_IsHuman, int i_PlayerNumber)
        { 
            m_PlayerScore = 0;
            m_PlayerSymbol = i_MySymbol;
            m_IsHuman = i_IsHuman;
            m_PlayerNumber = i_PlayerNumber;
        }

        public void PlayerMove(ref int io_RowInput, ref int io_ColInput, GameLogic i_Logic, char i_OpponentSymbol)
        {
            if(m_IsHuman)
            {
                getMoveFromHumanPlayer();
            }
            else
            {
                computerMove(i_OpponentSymbol, i_Logic, ref io_RowInput, ref io_ColInput);
            }
        }

        private void getMoveFromHumanPlayer()
        {
            Console.WriteLine("Player{0} please choose your placements based on the rows and cols on the board", m_PlayerNumber);
            Console.WriteLine("You can choose to quit the game by pressing 'Q'");
        }

        private void computerMove(char i_OpponentSymbol, GameLogic i_Logic, ref int io_ChosenRow, ref int io_ChosenCol)
        {
            int[] chosenLocation = new int[2];

            if (!i_Logic.CalculateComputerMoveAi(m_PlayerSymbol, i_OpponentSymbol, ref chosenLocation))
            {
                i_Logic.RandomComputerMove(m_PlayerSymbol, ref io_ChosenRow, ref io_ChosenCol);
            }

            io_ChosenRow = chosenLocation[0];
            io_ChosenCol = chosenLocation[1];
        }
    }
}
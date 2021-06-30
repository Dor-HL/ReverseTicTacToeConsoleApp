# ReverseTicTacToeConsoleApp
A reverse tic tac toe console  application (reverse meaning that the winner is the player that doesn't get the row/col/diagonal full of their sign), this game 
can be played with 2 players or 1 player vs the computer which uses an AI to decide his ideal location.
The functionality of the AI: the computer checks the locations on a matrix and uses 2 parameters which are:
1) will I disqualify myself.
2)Am I preventing my opponent from losing if i shall place my sign in said location.
If the computer finds a location that both the parameters are true for he then continues to assign his sign to that "ideal location", if only 1 of the conditions is met he will always 
prioritize not losing, if in all locations there isnt a single place that said prioritized parameter isnt good for therefor there is only one place 
or all choices lead to defeat - therefor will choose a location randomly

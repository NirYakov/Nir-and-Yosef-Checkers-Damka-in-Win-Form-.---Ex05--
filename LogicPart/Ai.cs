namespace LogicPart
{
    public static class Ai
    {
        public static MoveToDo TheBestMoveToDoForPlayer2(Logic i_TheGameNow)
        {
            bool foundActiveToDo = false;
            MoveToDo activeTheBestMove = null;
            foundActiveToDo = player2CanToEat(i_TheGameNow, out activeTheBestMove);
            if (!foundActiveToDo)
            {
                foundActiveToDo = player2CanToMove(i_TheGameNow, out activeTheBestMove);
            }

            return activeTheBestMove;
        }

        public static MoveToDo TheBestMoveToDoForPlayer1(Logic i_TheGameNow)
        {
            bool foundActiveToDo = false;
            MoveToDo o_ActiveTheBestMove = null;
            foundActiveToDo = player1CanToEat(i_TheGameNow, out o_ActiveTheBestMove);
            if (!foundActiveToDo)
            {
                foundActiveToDo = Player1CanToMove(i_TheGameNow, out o_ActiveTheBestMove);
            }

            return o_ActiveTheBestMove;
        }

        private static bool player2CanToEat(Logic i_TheGameNow, out MoveToDo o_ActiveToEat)
        {
            o_ActiveToEat = null;
            bool playerCanToEat = false;
            Locat destintionIndex = new Locat();
            foreach (Locat sourceIndex in i_TheGameNow.r_VellsOfPlayer2)
            {
                if (i_TheGameNow.Player2CanToEat(sourceIndex, ref destintionIndex))
                {
                    o_ActiveToEat = new MoveToDo(sourceIndex, destintionIndex);
                    playerCanToEat = true;
                    break;
                }
            }

            return playerCanToEat;
        }

        private static bool player1CanToEat(Logic i_TheGameNow, out MoveToDo o_ActiveToEat)
        {
            o_ActiveToEat = null;
            bool playerCanToEat = false;
            Locat destintionIndex = new Locat();
            foreach (Locat sourceIndex in i_TheGameNow.r_VellsOfPlayer1)
            {
                if (i_TheGameNow.Player1CanToEat(sourceIndex, ref destintionIndex))
                {
                    o_ActiveToEat = new MoveToDo(sourceIndex, destintionIndex);
                    playerCanToEat = true;
                    break;
                }
            }

            return playerCanToEat;
        }

        private static bool player2CanToMove(Logic i_TheGameNow, out MoveToDo o_ActiveToMove)
        {
            o_ActiveToMove = null;
            bool playerCanToMove = false;
            Locat destintionIndex = new Locat();
            foreach (Locat sourceIndex in i_TheGameNow.r_VellsOfPlayer2)
            {
                if (i_TheGameNow.Player2CanToMove(sourceIndex, ref destintionIndex))
                {
                    o_ActiveToMove = new MoveToDo(sourceIndex, destintionIndex);
                    playerCanToMove = true;
                    break;
                }
            }

            return playerCanToMove;
        }

        private static bool Player1CanToMove(Logic i_TheGameNow, out MoveToDo o_ActiveToMove)
        {
            o_ActiveToMove = null;
            bool playerCanToMove = false;
            Locat destintionIndex = new Locat();
            foreach (Locat sourceIndex in i_TheGameNow.r_VellsOfPlayer1)
            {
                if (i_TheGameNow.Player1CanToMove(sourceIndex, ref destintionIndex))
                {
                    o_ActiveToMove = new MoveToDo(sourceIndex, destintionIndex);
                    playerCanToMove = true;
                    break;
                }
            }

            return playerCanToMove;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LogicPart
{
    public class Logic
    {
        private const bool k_Player1 = true;
        private bool m_NowPlaying = k_Player1;

        private byte[,] m_Mat;
        private bool m_GameOn;

        public readonly List<Locat> r_VellsOfPlayer1 = new List<Locat>();
        public readonly List<Locat> r_VellsOfPlayer2 = new List<Locat>();

        private bool m_IsTurnPass = false;
        private bool m_IsEated = false;

        public event Action<Locat, eCheckers> BoardLogicMove;

        public Logic(byte i_Size = 8)
        {
            m_Mat = new byte[i_Size, i_Size];
            ResetGame();
        }

        public bool NowPlaying
        {
            get
            {
                return m_NowPlaying;
            }
        }

        public bool IsTurnPass
        {
            get
            {
                return m_IsTurnPass;
            }
        } 
        
        protected virtual void OnBoardLogicMove(Locat i_ThePlaceThatChange, eCheckers i_ChangeToType)
        {
            if(BoardLogicMove != null)
            {
                BoardLogicMove.Invoke(i_ThePlaceThatChange, i_ChangeToType);
            }
        }

        private void changeSoildersInList(List<Locat> i_VellsOfPlayer, Locat i_CurrentVessel, Locat i_NewVessel)
        {
            i_VellsOfPlayer.Remove(i_CurrentVessel);
            i_VellsOfPlayer.Add(i_NewVessel);
        }

        private void sortListOfVesselBecomingFirst(List<Locat> i_ListToSort)
        {
            i_ListToSort.Sort(delegate(Locat locat1, Locat locat2)
            {
                if (locat1.Y == locat2.Y) return 0;
                else if (locat1.Y < locat2.Y) return -1;
                else if (locat1.Y > locat2.Y) return 1;
                else return locat1.Y.CompareTo(locat2.Y);
            });
        }

        public void ResetGame()
        {
            m_GameOn = true;
            m_NowPlaying = k_Player1;
            Locat currentVeesel = new Locat();
            r_VellsOfPlayer1.Clear();
            r_VellsOfPlayer2.Clear();
            for (int i = 0; i < m_Mat.GetLength(0); i++)
            {
                for (int j = 0; j < m_Mat.GetLength(0); j++)
                {
                    if (i < ((m_Mat.GetLength(0) / 2) - 1) && (i + j) % 2 != 0)
                    {
                        m_Mat[i, j] = (byte)eCheckers.CheckerO;
                        currentVeesel.X = (byte)j;
                        currentVeesel.Y = (byte)i;
                        r_VellsOfPlayer1.Add(currentVeesel);
                    }
                    else if (i >= ((m_Mat.GetLength(0) / 2) + 1) && (i + j) % 2 != 0)
                    {
                        m_Mat[i, j] = (byte)eCheckers.CheckerX;
                        currentVeesel.X = (byte)j;
                        currentVeesel.Y = (byte)i;
                        r_VellsOfPlayer2.Add(currentVeesel);
                    }
                    else
                    {
                        m_Mat[i, j] = (byte)eCheckers.Non;
                    }
                }
            }
        }

        public void ChangePlayer()
        {
            m_NowPlaying = m_NowPlaying == k_Player1 ? !k_Player1 : k_Player1;
        }

        public void PlayingMove(Locat i_SourceIndex, Locat i_DestintionIndex)
        {
            m_IsEated = false;
            m_IsTurnPass = false;
            eCheckers checkers = (eCheckers)m_Mat[i_SourceIndex.Y, i_SourceIndex.X];
            eCheckers soilderToPlay = soilderKind();
            if ((checkers & soilderToPlay) == checkers && checkers != eCheckers.Non)
            {
                choisesToPlay(checkers, i_SourceIndex, i_DestintionIndex);
            }
            else
            {
                MessageBox.Show("not your vessel, try another", "Damka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if(m_IsEated)
            {
                checkingBounderisAndAbilityToEatAndEatingAutomatic(i_DestintionIndex);
            }

            checkIfBecomeKing(i_DestintionIndex);
        }

        private void choisesToPlay(eCheckers i_SoilderPlay, Locat i_SourceIndex, Locat i_DestintionIndex)
        {
            switch (i_SoilderPlay)
            {
                case eCheckers.CheckerO:
                case eCheckers.CheckerX:
                    playRegularSoilderAndCheckMoveDirection(i_SourceIndex, i_DestintionIndex);
                    break;
                case eCheckers.CheckerU:
                case eCheckers.CheckerK:
                    eatOrMoveSoilder(i_SourceIndex, i_DestintionIndex);
                    break;
            }
        }

        private void playRegularSoilderAndCheckMoveDirection(Locat i_SourceIndex, Locat i_DestintionIndex)
        {
            if (isMoveFront(i_SourceIndex.Y, i_DestintionIndex.Y) == true)
            {
                eatOrMoveSoilder(i_SourceIndex, i_DestintionIndex);
            }
            else
            {
                MessageBox.Show("you can not go back", "Damka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void eatOrMoveSoilder(Locat i_SourceIndex, Locat i_DestintionIndex)
        {
            const byte oneStepMoveInCross = 1, twoStepsMoveInCross = 2;
            if (checkMoveInCross(oneStepMoveInCross, i_SourceIndex, i_DestintionIndex))
            {
                moveSoilder(i_SourceIndex, i_DestintionIndex);
            }
            else if (checkMoveInCross(twoStepsMoveInCross, i_SourceIndex, i_DestintionIndex))
            {
                eatEnemySoilder(i_SourceIndex, i_DestintionIndex);
            }
            else
            {
                MessageBox.Show("illegal move , you can only move in cross one step or eat in cross . try again.", "Damka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private bool checkIfBecomeKing(Locat i_DestintionIndex)
        {
            bool becomeKing = true;
            if (i_DestintionIndex.Y == 0 && m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X] == (byte)eCheckers.CheckerX)
            {
                m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X] = (byte)eCheckers.CheckerK;
                OnBoardLogicMove(i_DestintionIndex, eCheckers.CheckerK);
            }
            else if (i_DestintionIndex.Y == (m_Mat.GetLength(0) - 1) && m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X] == (byte)eCheckers.CheckerO)
            {
                m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X] = (byte)eCheckers.CheckerU;
                OnBoardLogicMove(i_DestintionIndex, eCheckers.CheckerU);
            }
            else
            {
                becomeKing = false;
            }

            return becomeKing;
        }

        private void moveSoilder(Locat i_SourceIndex, Locat i_DestintionIndex)
        {
            if (m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X] == (byte)eCheckers.Non)
            {
                executeJump(i_SourceIndex, i_DestintionIndex);
                m_IsTurnPass = true;
                Locat original = i_SourceIndex;
                Locat newVessel = i_DestintionIndex;
                if (m_NowPlaying == k_Player1)
                {
                    changeSoildersInList(r_VellsOfPlayer1, original, newVessel);
                    sortListOfVesselBecomingFirst(r_VellsOfPlayer1);
                    OnBoardLogicMove(i_SourceIndex, (eCheckers)m_Mat[i_SourceIndex.Y, i_SourceIndex.X]);
                    OnBoardLogicMove(i_DestintionIndex, (eCheckers)m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X]);
                }
                else
                {
                    changeSoildersInList(r_VellsOfPlayer2, original, newVessel);
                    sortListOfVesselBecomingFirst(r_VellsOfPlayer2);
                    OnBoardLogicMove(i_SourceIndex, (eCheckers)m_Mat[i_SourceIndex.Y, i_SourceIndex.X]);
                    OnBoardLogicMove(i_DestintionIndex, (eCheckers)m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X]);
                }
            }
        }

        private void eatEnemySoilder(Locat i_SourceIndex, Locat i_DestintionIndex)
        {
            Locat middleIndex = new Locat();
            middleIndex.X = (byte)((i_SourceIndex.X + i_DestintionIndex.X) / 2);
            middleIndex.Y = (byte)((i_SourceIndex.Y + i_DestintionIndex.Y) / 2);

            if (isHaveEnemyInCrossToEat(middleIndex, i_DestintionIndex))
            {
                m_IsEated = true;
                executeJump(i_SourceIndex, i_DestintionIndex);
                m_Mat[middleIndex.Y, middleIndex.X] = (byte)eCheckers.Non;
                m_IsTurnPass = true;
                Locat original = i_SourceIndex;
                Locat newVessel = i_DestintionIndex;
                Locat isEatedNow = new Locat(middleIndex.X, middleIndex.Y);
                if (m_NowPlaying == k_Player1)
                {
                    changeSoildersInList(r_VellsOfPlayer1, original, newVessel);
                    sortListOfVesselBecomingFirst(r_VellsOfPlayer1);
                    r_VellsOfPlayer2.Remove(isEatedNow);

                    OnBoardLogicMove(i_SourceIndex, (eCheckers) m_Mat[i_SourceIndex.Y, i_SourceIndex.X]);
                    OnBoardLogicMove(i_DestintionIndex, (eCheckers) m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X]);
                    OnBoardLogicMove(isEatedNow, eCheckers.Non);
                }
                else
                {
                    changeSoildersInList(r_VellsOfPlayer2, original, newVessel);
                    sortListOfVesselBecomingFirst(r_VellsOfPlayer2);
                    r_VellsOfPlayer1.Remove(isEatedNow);

                    OnBoardLogicMove(i_SourceIndex, (eCheckers)m_Mat[i_SourceIndex.Y, i_SourceIndex.X]);
                    OnBoardLogicMove(i_DestintionIndex, (eCheckers)m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X]);
                    OnBoardLogicMove(isEatedNow, eCheckers.Non);
                }

                checkIfBecomeKing(i_DestintionIndex);
            }
            else
            {
                MessageBox.Show("Cant jump so far without Eat. - you cant eat nothing or yourself - . try again.", "Damka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void executeJump(Locat i_SourceIndex, Locat i_DestintionIndex)
        {
            m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X] = m_Mat[i_SourceIndex.Y, i_SourceIndex.X];
            m_Mat[i_SourceIndex.Y, i_SourceIndex.X] = (byte)eCheckers.Non;
        }

        private void eatWithSameSoilder(Locat i_SourceIndex, Locat i_DestintionIndex)
        {
            m_IsEated = false;
            if (checkMoveInCross(2, i_SourceIndex, i_DestintionIndex))
            {
                eatEnemySoilder(i_SourceIndex, i_DestintionIndex);
            }
        }

        private bool checkMoveInCross(byte i_MoveDist, Locat i_SourceIndex, Locat i_DestintionIndex)
        {
            byte stepsLineX = (byte)Math.Abs(i_SourceIndex.X - i_DestintionIndex.X), stepsLineY = (byte)Math.Abs(i_SourceIndex.Y - i_DestintionIndex.Y);
            return stepsLineX == i_MoveDist && stepsLineY == i_MoveDist;
        }
        
        private bool checkingBounderisAndAbilityToEatAndEatingAutomatic(Locat i_DestintionIndex)
        {
            byte start = 0, end = (byte)(m_Mat.GetLength(0) - 1);
            bool isRightUpSpotLegal = (i_DestintionIndex.X + 2 <= end) && (i_DestintionIndex.Y - 2 >= start);
            bool isRightDownSpotLegal = (i_DestintionIndex.X + 2 <= end) && (i_DestintionIndex.Y + 2 <= end);
            bool isLeftUpSpotLegal = (i_DestintionIndex.X - 2 >= start) && (i_DestintionIndex.Y - 2 >= start);
            bool isLeftDownSpotLegal = (i_DestintionIndex.X - 2 >= start) && (i_DestintionIndex.Y + 2 <= end);
            bool isCanEatAgain = false;

            if (isRightUpSpotLegal == true && isCanEatAgain == false)
            {
                Locat middleRightUpSpot = new Locat((byte)(i_DestintionIndex.X + 1), (byte)(i_DestintionIndex.Y - 1));
                Locat destRightUpSpot = new Locat((byte)(i_DestintionIndex.X + 2), (byte)(i_DestintionIndex.Y - 2));
                isCanEatAgain = isHaveEnemyInCrossToEat(middleRightUpSpot, destRightUpSpot);
                if (isCanEatAgain)
                {
                    multiEatingAutomatic(i_DestintionIndex, destRightUpSpot);
                }
            }

            if (isRightDownSpotLegal == true && isCanEatAgain == false)
            {
                Locat middleRightDownSpot = new Locat((byte)(i_DestintionIndex.X + 1), (byte)(i_DestintionIndex.Y + 1));
                Locat destRightDownSpot = new Locat((byte)(i_DestintionIndex.X + 2), (byte)(i_DestintionIndex.Y + 2));
                isCanEatAgain = isHaveEnemyInCrossToEat(middleRightDownSpot, destRightDownSpot);
                if (isCanEatAgain)
                {
                    multiEatingAutomatic(i_DestintionIndex, destRightDownSpot);
                }
            }

            if (isLeftUpSpotLegal == true && isCanEatAgain == false)
            {
                Locat middleLeftUpSpot = new Locat((byte)(i_DestintionIndex.X - 1), (byte)(i_DestintionIndex.Y - 1));
                Locat destLeftUpSpot = new Locat((byte)(i_DestintionIndex.X - 2), (byte)(i_DestintionIndex.Y - 2));
                isCanEatAgain = isHaveEnemyInCrossToEat(middleLeftUpSpot, destLeftUpSpot);
                if (isCanEatAgain)
                {
                    multiEatingAutomatic(i_DestintionIndex, destLeftUpSpot);
                }
            }

            if (isLeftDownSpotLegal == true && isCanEatAgain == false)
            {
                Locat middleLeftDownSpot = new Locat((byte)(i_DestintionIndex.X - 1), (byte)(i_DestintionIndex.Y + 1));
                Locat destLeftDownSpot = new Locat((byte)(i_DestintionIndex.X - 2), (byte)(i_DestintionIndex.Y + 2));
                isCanEatAgain = isHaveEnemyInCrossToEat(middleLeftDownSpot, destLeftDownSpot);
                if (isCanEatAgain)
                {
                    multiEatingAutomatic(i_DestintionIndex, destLeftDownSpot);
                }
            }

            return isCanEatAgain;
        }

        private void multiEatingAutomatic(Locat i_LastDestintion, Locat i_DestintionToEat)
        {
            eatWithSameSoilder(i_LastDestintion, i_DestintionToEat);
            checkingBounderisAndAbilityToEatAndEatingAutomatic(i_DestintionToEat);
        }

        public bool Player2CanToEat(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            bool isHaveFreeSpot = false;
            byte end = (byte)(m_Mat.GetLength(0) - 1);

            byte indexX = i_IndexesToPlay.X, indexY = i_IndexesToPlay.Y;
            if (canToEatInRightUpInY(i_IndexesToPlay, ref io_IndexesThatLegal))
            {
                isHaveFreeSpot = true;
            }

            if (isHaveFreeSpot == false)
            {
                if (canToEatInLeftUpInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                {
                    isHaveFreeSpot = true;
                }
            }

            eCheckers kings = eCheckers.CheckerK | eCheckers.CheckerU, currentSoilder = (eCheckers)m_Mat[indexY, indexX];
            if (isHaveFreeSpot == false && ((currentSoilder & kings) == currentSoilder))
            {
                if (canToEatInRightDownInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                {
                    isHaveFreeSpot = true;
                }

                if (isHaveFreeSpot == false)
                {
                    if (canToEatInLeftDownInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                    {
                        isHaveFreeSpot = true;
                    }
                }
            }

            return isHaveFreeSpot;
        }

        public bool Player1CanToEat(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            bool isHaveFreeSpot = false;
            byte end = (byte)(m_Mat.GetLength(0) - 1);
            byte indexX = i_IndexesToPlay.X, indexY = i_IndexesToPlay.Y;
            if (canToEatInRightDownInY(i_IndexesToPlay, ref io_IndexesThatLegal))
            {
                isHaveFreeSpot = true;
            }

            if (isHaveFreeSpot == false)
            {
                if (canToEatInLeftDownInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                {
                    isHaveFreeSpot = true;
                }
            }

            eCheckers kings = eCheckers.CheckerK | eCheckers.CheckerU, currentSoilder = (eCheckers)m_Mat[indexY, indexX];
            if (isHaveFreeSpot == false && ((currentSoilder & kings) == currentSoilder))
            {
                if (canToEatInRightUpInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                {
                    isHaveFreeSpot = true;
                }

                if (isHaveFreeSpot == false)
                {
                    if (canToEatInLeftUpInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                    {
                        isHaveFreeSpot = true;
                    }
                }
            }

            return isHaveFreeSpot;
        }

        private bool canToEatInRightUpInY(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            bool isCanToEatInRightUpInY = false;
            byte start = 0, end = (byte)(m_Mat.GetLength(0) - 1);
            bool isRightUpSpotLegal = (i_IndexesToPlay.X + 2 <= end) && (i_IndexesToPlay.Y - 2 >= start);
            Locat rightUpIndexMiddle = new Locat((byte)(i_IndexesToPlay.X + 1), (byte)(i_IndexesToPlay.Y - 1));
            Locat rightUpIndexDest = new Locat((byte)(i_IndexesToPlay.X + 2), (byte)(i_IndexesToPlay.Y - 2));
            if (isRightUpSpotLegal)
            {
                if (isHaveEnemyInCrossToEat(rightUpIndexMiddle, rightUpIndexDest))
                {
                    io_IndexesThatLegal.X = rightUpIndexDest.X;
                    io_IndexesThatLegal.Y = rightUpIndexDest.Y;
                    isCanToEatInRightUpInY = true;
                }
            }

            return isCanToEatInRightUpInY;
        }

        private bool canToEatInLeftUpInY(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            bool isCanToEatLeftUpInY = false;
            byte start = 0, end = (byte)(m_Mat.GetLength(0) - 1);
            bool isLeftUpSpotLegal = (i_IndexesToPlay.X - 2 >= start) && (i_IndexesToPlay.Y - 2 >= start);
            Locat rightUpIndexMiddle = new Locat((byte)(i_IndexesToPlay.X - 1), (byte)(i_IndexesToPlay.Y - 1));
            Locat rightUpIndexDest = new Locat((byte)(i_IndexesToPlay.X - 2), (byte)(i_IndexesToPlay.Y - 2));
            if (isLeftUpSpotLegal)
            {
                if (isHaveEnemyInCrossToEat(rightUpIndexMiddle, rightUpIndexDest))
                {
                    io_IndexesThatLegal.X = rightUpIndexDest.X;
                    io_IndexesThatLegal.Y = rightUpIndexDest.Y;
                    isCanToEatLeftUpInY = true;
                }
            }

            return isCanToEatLeftUpInY;
        }

        private bool canToEatInRightDownInY(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            bool isCanToEatRightDownInY = false;
            byte end = (byte)(m_Mat.GetLength(0) - 1);
            bool isRightDownSpotLegal = (i_IndexesToPlay.X + 2 <= end) && (i_IndexesToPlay.Y + 2 <= end);
            Locat rightUpIndexMiddle = new Locat((byte)(i_IndexesToPlay.X + 1), (byte)(i_IndexesToPlay.Y + 1));
            Locat rightUpIndexDest = new Locat((byte)(i_IndexesToPlay.X + 2), (byte)(i_IndexesToPlay.Y + 2));
            if (isRightDownSpotLegal)
            {
                if (isHaveEnemyInCrossToEat(rightUpIndexMiddle, rightUpIndexDest))
                {
                    io_IndexesThatLegal.X = rightUpIndexDest.X;
                    io_IndexesThatLegal.Y = rightUpIndexDest.Y;
                    isCanToEatRightDownInY = true;
                }
            }

            return isCanToEatRightDownInY;
        }

        private bool canToEatInLeftDownInY(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            bool isCanToEatLeftDownInY = false;
            byte start = 0, end = (byte)(m_Mat.GetLength(0) - 1);
            bool isLeftDownSpotLegal = (i_IndexesToPlay.X - 2 >= start) && (i_IndexesToPlay.Y + 2 <= end);
            Locat rightUpIndexMiddle = new Locat((byte)(i_IndexesToPlay.X - 2), (byte)(i_IndexesToPlay.Y + 2));
            Locat rightUpIndexDest = new Locat((byte)(i_IndexesToPlay.X - 2), (byte)(i_IndexesToPlay.Y + 2));
            if (isLeftDownSpotLegal)
            {
                if (isHaveEnemyInCrossToEat(rightUpIndexMiddle, rightUpIndexDest))
                {
                    io_IndexesThatLegal.X = rightUpIndexDest.X;
                    io_IndexesThatLegal.Y = rightUpIndexDest.Y;
                    isCanToEatLeftDownInY = true;
                }
            }

            return isCanToEatLeftDownInY;
        }

        public bool Player2CanToMove(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            byte end = (byte)(m_Mat.GetLength(0) - 1);
            bool isHaveFreeSpot = false;
            byte indexX = i_IndexesToPlay.X, indexY = i_IndexesToPlay.Y;
            eCheckers kings = eCheckers.CheckerK | eCheckers.CheckerU, currentSoilder = (eCheckers)m_Mat[indexY, indexX];
            if (canToMoveRightUpInY(i_IndexesToPlay, ref io_IndexesThatLegal))
            {
                isHaveFreeSpot = true;
            }

            if (isHaveFreeSpot == false)
            {
                if (canToMoveLeftUpInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                {
                    isHaveFreeSpot = true;
                }
            }

            if (isHaveFreeSpot == false && ((currentSoilder & kings) == currentSoilder))
            {
                if (canToMoveRightDownInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                {
                    isHaveFreeSpot = true;
                }

                if (isHaveFreeSpot == false)
                {
                    if (canToMoveLeftDownInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                    {
                        isHaveFreeSpot = true;
                    }
                }
            }

            return isHaveFreeSpot;
        }

        public bool Player1CanToMove(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            byte end = (byte)(m_Mat.GetLength(0) - 1);
            bool isHaveFreeSpot = false;
            byte indexX = i_IndexesToPlay.X, indexY = i_IndexesToPlay.Y;
            eCheckers kings = eCheckers.CheckerK | eCheckers.CheckerU, currentSoilder = (eCheckers)m_Mat[indexY, indexX];
            if (canToMoveRightDownInY(i_IndexesToPlay, ref io_IndexesThatLegal))
            {
                isHaveFreeSpot = true;
            }

            if (isHaveFreeSpot == false)
            {
                if (canToMoveLeftDownInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                {
                    isHaveFreeSpot = true;
                }
            }

            if (isHaveFreeSpot == false && ((currentSoilder & kings) == currentSoilder))
            {
                if (canToMoveRightUpInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                {
                    isHaveFreeSpot = true;
                }

                if (isHaveFreeSpot == false)
                {
                    if (canToMoveLeftUpInY(i_IndexesToPlay, ref io_IndexesThatLegal))
                    {
                        isHaveFreeSpot = true;
                    }
                }
            }

            return isHaveFreeSpot;
        }

        private bool canToMoveRightUpInY(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            byte start = 0, end = (byte)(m_Mat.GetLength(0) - 1);
            bool isCanMoveRightUpinYLine = false;
            bool isRightUpSpotLegal = (i_IndexesToPlay.X + 1 <= end) && (i_IndexesToPlay.Y - 1 >= start);
            if (isRightUpSpotLegal)
            {
                eCheckers spotToCheck = (eCheckers)m_Mat[i_IndexesToPlay.Y - 1, i_IndexesToPlay.X + 1];
                if (spotToCheck == eCheckers.Non)
                {
                    io_IndexesThatLegal.X = (byte)(i_IndexesToPlay.X + 1);
                    io_IndexesThatLegal.Y = (byte)(i_IndexesToPlay.Y - 1);
                    isCanMoveRightUpinYLine = true;
                }
            }

            return isCanMoveRightUpinYLine;
        }

        private bool canToMoveLeftUpInY(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            byte start = 0, end = (byte)(m_Mat.GetLength(0) - 1);
            bool isCanMoveLefttUpinYLine = false;
            bool isLeftUpSpotLegal = (i_IndexesToPlay.X - 1 >= start) && (i_IndexesToPlay.Y - 1 >= start);
            if (isLeftUpSpotLegal)
            {
                eCheckers spotToCheck = (eCheckers)m_Mat[i_IndexesToPlay.Y - 1, i_IndexesToPlay.X - 1];
                if (spotToCheck == eCheckers.Non)
                {
                    io_IndexesThatLegal.X = (byte)(i_IndexesToPlay.X - 1);
                    io_IndexesThatLegal.Y = (byte)(i_IndexesToPlay.Y - 1);
                    isCanMoveLefttUpinYLine = true;
                }
            }

            return isCanMoveLefttUpinYLine;
        }

        private bool canToMoveRightDownInY(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            byte end = (byte)(m_Mat.GetLength(0) - 1);
            bool isCanMoveRightDowninYLine = false;
            byte indexX = i_IndexesToPlay.X, indexY = i_IndexesToPlay.Y;
            bool isRightDownSpotLegal = (indexX + 1 <= end) && (indexY + 1 <= end);
            if (isRightDownSpotLegal)
            {
                eCheckers spotToCheck = (eCheckers)m_Mat[indexY + 1, indexX + 1];
                if (spotToCheck == eCheckers.Non)
                {
                    io_IndexesThatLegal.X = (byte)(indexX + 1);
                    io_IndexesThatLegal.Y = (byte)(indexY + 1);
                    isCanMoveRightDowninYLine = true;
                }
            }

            return isCanMoveRightDowninYLine;
        }

        private bool canToMoveLeftDownInY(Locat i_IndexesToPlay, ref Locat io_IndexesThatLegal)
        {
            byte start = 0, end = (byte)(m_Mat.GetLength(0) - 1);
            bool isCanMoveleftDowninYLine = false;
            bool isLeftDownSpotLegal = (i_IndexesToPlay.X - 1 >= start) && (i_IndexesToPlay.Y + 1 <= end);
            if (isLeftDownSpotLegal)
            {
                eCheckers spotToCheck = (eCheckers)m_Mat[i_IndexesToPlay.Y + 1, i_IndexesToPlay.X - 1];
                if (spotToCheck == eCheckers.Non)
                {
                    io_IndexesThatLegal.X = (byte)(i_IndexesToPlay.X - 1);
                    io_IndexesThatLegal.Y = (byte)(i_IndexesToPlay.Y + 1);
                    isCanMoveleftDowninYLine = true;
                }
            }

            return isCanMoveleftDowninYLine;
        }

        private bool isHaveEnemyInCrossToEat(Locat i_MiddleIndex, Locat i_DestintionIndex)
        {
            eCheckers soildersTeam = soilderKind();
            eCheckers checker = (eCheckers)m_Mat[i_MiddleIndex.Y, i_MiddleIndex.X], freeSpot = (eCheckers)m_Mat[i_DestintionIndex.Y, i_DestintionIndex.X];
            return freeSpot == eCheckers.Non && ((checker & soildersTeam) != checker && checker != eCheckers.Non);
        }

        private bool isMoveFront(byte i_SouruceIndexY, byte i_DestintionIndexY)
        {
            sbyte distLineY = (sbyte)(i_SouruceIndexY - i_DestintionIndexY);
            if (m_NowPlaying != k_Player1)
            {
                distLineY *= -1;
            }

            return distLineY < 0 ? true : false;
        }

        private eCheckers soilderKind()
        {
            return m_NowPlaying == k_Player1 ? eCheckers.CheckerO | eCheckers.CheckerU : eCheckers.CheckerX | eCheckers.CheckerK;
        }

        public bool GameOn()
        {
            if (m_NowPlaying == k_Player1)
            {
                m_GameOn = Ai.TheBestMoveToDoForPlayer1(this) != null;
            }
            else
            {
                m_GameOn = Ai.TheBestMoveToDoForPlayer2(this) != null;
            }

            return m_GameOn;
        }
    }
}

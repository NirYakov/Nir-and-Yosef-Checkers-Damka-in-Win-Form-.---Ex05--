using System.Windows.Forms;
using LogicPart;
using BoardPart;

namespace GameManager
{
    public class GamePlay 
    {
        private const string k_NameOfComputer = "[Computer]";
        private const bool k_Player1 = true;
        private Player m_Player1, m_Player2;
        private bool m_PlayVSComputer;
        private Logic m_ActiveGame;
        private BoardDamkaWinForm m_Ui;

        public GamePlay(byte i_SizeOfGame, string i_NameOfPlayer1, string i_NameOfPlayer2)
        {
            m_Player1 = new Player(i_NameOfPlayer1);
            m_Player2 = new Player(i_NameOfPlayer2);
            m_PlayVSComputer = i_NameOfPlayer2 == k_NameOfComputer;
            m_ActiveGame = new Logic(i_SizeOfGame);
            m_Ui = new BoardDamkaWinForm(i_SizeOfGame, i_NameOfPlayer1, i_NameOfPlayer2);
            m_Ui.BoardUiMove += boardUi_Move;
            m_ActiveGame.BoardLogicMove += boardLogic_Move;
        }

        public void StartGame()
        {
            m_Ui.ShowBoard();
        }

        private void boardLogic_Move(Locat i_LocateThatChange, eCheckers i_ChangeToThisType)
        {
            string newTextToButton = string.Empty;
            switch(i_ChangeToThisType)
            {
                case eCheckers.Non:
                    newTextToButton = string.Empty;
                    break;
                case eCheckers.CheckerX:
                    newTextToButton = "X";
                    break;
                case eCheckers.CheckerO:
                    newTextToButton = "O";
                    break;
                case eCheckers.CheckerU:
                    newTextToButton = "U";
                    break;
                case eCheckers.CheckerK:
                    newTextToButton = "K";
                    break;
            }

            m_Ui.ChangeTextOnButton(i_LocateThatChange, newTextToButton);
        }

        private void boardUi_Move(Locat i_Source, Locat i_Dest)
        {
            if(m_ActiveGame.GameOn())
            {
                if(m_PlayVSComputer)
                {
                    m_ActiveGame.PlayingMove(i_Source, i_Dest);
                    if (m_ActiveGame.IsTurnPass)
                    {
                        computerPlayingMove();
                    }
                }
                else
                {
                    m_ActiveGame.PlayingMove(i_Source, i_Dest);
                }
                
                if(m_ActiveGame.IsTurnPass)
                {
                    updateAfterTurnPass();
                }
            }

            if(!m_ActiveGame.GameOn())
            {
                gameOver();
            }
        }

        private void computerPlayingMove()
        {
            m_ActiveGame.ChangePlayer();
            
            if(m_ActiveGame.GameOn())
            {
                MoveToDo bestActive = Ai.TheBestMoveToDoForPlayer2(m_ActiveGame);
                m_ActiveGame.PlayingMove(bestActive.Source, bestActive.Dest);
            }
            else
            {
                m_ActiveGame.ChangePlayer();
            }
        }

        private void updateAfterRound()
        {
            m_Ui.RestBoard();
            m_ActiveGame.ResetGame();

            if (m_ActiveGame.NowPlaying == k_Player1)
            {
                m_Player2.Points++;
                m_Ui.LabelOfPlayer2 = string.Format("{0}:{1}", m_Player2.Name, m_Player2.Points.ToString());
            }
            else
            {
                m_Player1.Points++;
                m_Ui.LabelOfPlayer1 = string.Format("{0}:{1}", m_Player1.Name, m_Player1.Points.ToString());
            }

            m_Ui.LabelNameOfPlayingNow = m_Player1.Name;
        }

        private void gameOver()
        {
            string nameOfPlayerWin = m_ActiveGame.NowPlaying == k_Player1 ? m_Player2.Name : m_Player1.Name;
            string messageForUser = string.Format("{0} won another round ?", nameOfPlayerWin);

            DialogResult result = MessageBox.Show(messageForUser, "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            updateAfterRound();

            if (result == DialogResult.No)
            {
                m_Ui.CloseBoard();
            }
        }

        private void updateAfterTurnPass()
        {
            m_ActiveGame.ChangePlayer();
            string nameOfPlayingNow = m_ActiveGame.NowPlaying == k_Player1 ? m_Player1.Name : m_Player2.Name;
            m_Ui.LabelNameOfPlayingNow = nameOfPlayingNow;
        }
    }
}

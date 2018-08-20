using System;
using System.Windows.Forms;
using System.Drawing;
using LogicPart;

namespace BoardPart
{
    public class BoardDamkaWinForm 
    {
        private const string k_Player1Sign = "O", k_Player2Sign = "X", k_EmptyPlace = "";
        private readonly byte r_SizeOfBoard;
        private Form m_FormOfBoard = new Form();
        private Locat? m_Dest, m_Source;
        private ButtonLocat[,] m_MatOfButton;
        private Label m_LabelPlayer1, m_LabelPlayer2, m_LabelNowPlaying, m_LabelNameOfPlayingNow;

        public event Action<Locat, Locat> BoardUiMove;

        public BoardDamkaWinForm(byte i_SizeOfBoardGame, string i_NameOfPlayer1, string i_NameOfPlayer2)
        {
            r_SizeOfBoard = i_SizeOfBoardGame;
            initializeBoardOfGame(i_NameOfPlayer1, i_NameOfPlayer2);
            m_FormOfBoard.FormClosing += form_Closing;
        }

        private void form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure that you want exit ?", "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        public string LabelOfPlayer1
        {
            get
            {
                return m_LabelPlayer1.Text;
            }

            set
            {
                m_LabelPlayer1.Text = value;
            }
        }

        public string LabelOfPlayer2
        {
            get
            {
                return m_LabelPlayer2.Text;
            }

            set
            {
                m_LabelPlayer2.Text = value;
            }
        }

        public string LabelNameOfPlayingNow
        {
            get
            {
                return m_LabelNameOfPlayingNow.Text;
            }

            set
            {
                m_LabelNameOfPlayingNow.Text = value;
            }
        }

        public void ChangeTextOnButton(Locat i_LocateOfButton, string i_NewTextToButton)
        {
            m_MatOfButton[i_LocateOfButton.Y, i_LocateOfButton.X].Text = i_NewTextToButton;
        }

        public void ShowBoard()
        {
            m_FormOfBoard.ShowDialog();
        }

        private void initializeBoardOfGame(string i_NameOfPlayer1, string i_NameOfPlayer2)
        {
            initializeLabels(i_NameOfPlayer1, i_NameOfPlayer2);
            initializeFormStyleAndSize();
            initializeButtonBoard();
        }

        private void initializeLabels(string i_NameOfPlayer1, string i_NameOfPlayer2)
        {
            m_LabelNowPlaying = new Label();
            m_LabelNowPlaying.Text = "Now Playing:";
            m_LabelNowPlaying.Top = 12;
            m_LabelNowPlaying.Height = 50;
            m_LabelNowPlaying.Width = 70;
            m_LabelNowPlaying.Left = 0;
            m_LabelNowPlaying.Font = new Font("Segoe Print", 10);
            m_FormOfBoard.Controls.Add(m_LabelNowPlaying);

            m_LabelNameOfPlayingNow = new Label();
            m_LabelNameOfPlayingNow.Text = i_NameOfPlayer1;
            m_LabelNameOfPlayingNow.Top = m_LabelNowPlaying.Top;
            m_LabelNameOfPlayingNow.Height = m_LabelNowPlaying.Height;
            m_LabelNameOfPlayingNow.Width = 150;
            m_LabelNameOfPlayingNow.Left = m_LabelNowPlaying.Right;
            m_LabelNameOfPlayingNow.Font = m_LabelNowPlaying.Font;
            m_FormOfBoard.Controls.Add(m_LabelNameOfPlayingNow);

            m_LabelPlayer1 = new Label();
            m_LabelPlayer1.Text = string.Format("{0}:{1}", i_NameOfPlayer1, "0");
            m_LabelPlayer1.Top = m_LabelNameOfPlayingNow.Top;
            m_LabelPlayer1.Height = m_LabelNameOfPlayingNow.Height;
            m_LabelPlayer1.Width = m_LabelNameOfPlayingNow.Width;
            m_LabelPlayer1.Left = m_LabelNameOfPlayingNow.Right + 20;
            m_LabelPlayer1.Font = m_LabelNameOfPlayingNow.Font;
            m_FormOfBoard.Controls.Add(m_LabelPlayer1);

            m_LabelPlayer2 = new Label();
            m_LabelPlayer2.Text = string.Format("{0}:{1}", i_NameOfPlayer2, "0");
            m_LabelPlayer2.Top = m_LabelPlayer1.Top;
            m_LabelPlayer2.Height = m_LabelPlayer1.Height;
            m_LabelPlayer2.Width = m_LabelPlayer1.Width;
            m_LabelPlayer2.Left = m_LabelPlayer1.Right + 20;
            m_LabelPlayer2.Font = m_LabelPlayer1.Font;
            m_FormOfBoard.Controls.Add(m_LabelPlayer2);
        }

        private void initializeFormStyleAndSize()
        {
            int spaceForLabels = m_LabelPlayer1.Top + m_LabelPlayer1.Height;
            Size sizeToClient = new Size();
            sizeToClient.Width = 600 + r_SizeOfBoard;
            sizeToClient.Height = sizeToClient.Width + spaceForLabels;

            m_FormOfBoard.ClientSize = sizeToClient;
            m_FormOfBoard.FormBorderStyle = FormBorderStyle.FixedSingle;
            m_FormOfBoard.StartPosition = FormStartPosition.CenterScreen;
            m_FormOfBoard.MaximizeBox = false;
        }

        private void initializeButtonBoard()
        {
            int lengthAndWidthOfButton = m_FormOfBoard.ClientSize.Width / r_SizeOfBoard;
            Locat locateForButtons = new Locat();
            int spaceForLabels = m_LabelPlayer1.Top + m_LabelPlayer1.Height;
            m_MatOfButton = new ButtonLocat[r_SizeOfBoard, r_SizeOfBoard];

            for (int i = 0; i < r_SizeOfBoard; i++)
            {
                for (int j = 0; j < r_SizeOfBoard; j++)
                {
                    if (i < ((r_SizeOfBoard / 2) - 1) && (i + j) % 2 != 0)
                    {
                        locateForButtons.X = (byte)j;
                        locateForButtons.Y = (byte)i;
                        ButtonLocat buttonOfPlayr1 = new ButtonLocat(locateForButtons);
                        buttonOfPlayr1.Text = k_Player1Sign;
                        buttonOfPlayr1.Width = lengthAndWidthOfButton;
                        buttonOfPlayr1.Height = lengthAndWidthOfButton;
                        buttonOfPlayr1.Left = j * lengthAndWidthOfButton;
                        buttonOfPlayr1.Top = (i * lengthAndWidthOfButton) + spaceForLabels;
                        buttonOfPlayr1.Click += button_Cliked;

                        m_FormOfBoard.Controls.Add(buttonOfPlayr1);
                        m_MatOfButton[i, j] = buttonOfPlayr1;
                    }
                    else if (i >= ((r_SizeOfBoard / 2) + 1) && (i + j) % 2 != 0)
                    {
                        locateForButtons.X = (byte)j;
                        locateForButtons.Y = (byte)i;
                        ButtonLocat buttonOfPlayr2 = new ButtonLocat(locateForButtons);
                        buttonOfPlayr2.Text = k_Player2Sign;
                        buttonOfPlayr2.Width = lengthAndWidthOfButton;
                        buttonOfPlayr2.Height = lengthAndWidthOfButton;
                        buttonOfPlayr2.Left = j * lengthAndWidthOfButton;
                        buttonOfPlayr2.Top = (i * lengthAndWidthOfButton) + spaceForLabels;
                        buttonOfPlayr2.Click += button_Cliked;

                        m_FormOfBoard.Controls.Add(buttonOfPlayr2);
                        m_MatOfButton[i, j] = buttonOfPlayr2;
                    }
                    else
                    {
                        locateForButtons.X = (byte)j;
                        locateForButtons.Y = (byte)i;
                        ButtonLocat buttonOfEmptyPlace = new ButtonLocat(locateForButtons);
                        buttonOfEmptyPlace.Text = k_EmptyPlace;
                        buttonOfEmptyPlace.Width = lengthAndWidthOfButton;
                        buttonOfEmptyPlace.Height = lengthAndWidthOfButton;
                        buttonOfEmptyPlace.Left = j * lengthAndWidthOfButton;
                        buttonOfEmptyPlace.Top = (i * lengthAndWidthOfButton) + spaceForLabels;

                        if ((i + j) % 2 == 0)
                        {
                            buttonOfEmptyPlace.BackColor = Color.Gray;
                            buttonOfEmptyPlace.Enabled = false;
                        }
                        else
                        {
                            buttonOfEmptyPlace.Click += button_Cliked;
                        }

                        m_FormOfBoard.Controls.Add(buttonOfEmptyPlace);
                        m_MatOfButton[i, j] = buttonOfEmptyPlace;
                    }
                }
            }
        }

        public void RestBoard()
        {
            for (int i = 0; i < r_SizeOfBoard; i++)
            {
                for (int j = 0; j < r_SizeOfBoard; j++)
                {
                    if (i < ((r_SizeOfBoard / 2) - 1) && (i + j) % 2 != 0)
                    {
                        m_MatOfButton[i, j].Text = k_Player1Sign;
                        m_MatOfButton[i, j].BackColor = Color.Empty;
                    }
                    else if (i >= ((r_SizeOfBoard / 2) + 1) && (i + j) % 2 != 0)
                    {
                        m_MatOfButton[i, j].Text = k_Player2Sign;
                        m_MatOfButton[i, j].BackColor = Color.Empty;
                    }
                    else
                    {
                        if (m_MatOfButton[i, j].Enabled)
                        {
                            m_MatOfButton[i, j].Text = k_EmptyPlace;
                            m_MatOfButton[i, j].BackColor = Color.Empty;
                        }
                    }
                }
            }
        }

        public void CloseBoard()
        {
            m_FormOfBoard.Close();
        }

        private void button_Cliked(object sender, EventArgs e)
        {
            ButtonLocat currentButton = sender as ButtonLocat;
            if (currentButton != null)
            {
                if (currentButton.BackColor != Color.CornflowerBlue)
                {
                    currentButton.BackColor = Color.CornflowerBlue;
                    if (m_Source == null)
                    {
                        m_Source = currentButton.LocatOfButton;
                    }
                }
                else
                {
                    currentButton.BackColor = Color.Empty;
                    m_Source = null;
                }

                if (m_Source != null && !m_Source.Value.Equals(currentButton.LocatOfButton))
                {
                    m_Dest = currentButton.LocatOfButton;
                    OnBoardUiMove(m_Source.Value, m_Dest.Value);
                    cleanAfterMove();
                }
            }
        }

        private void cleanAfterMove()
        {
            m_MatOfButton[m_Source.Value.Y, m_Source.Value.X].BackColor = Color.Empty;
            m_MatOfButton[m_Dest.Value.Y, m_Dest.Value.X].BackColor = Color.Empty;
            m_Source = null;
            m_Dest = null;
        }

        protected virtual void OnBoardUiMove(Locat i_Source, Locat i_Dest)
        {
            if (BoardUiMove != null)
            {
                BoardUiMove.Invoke(i_Source, i_Dest);
            }
        }
    }
}
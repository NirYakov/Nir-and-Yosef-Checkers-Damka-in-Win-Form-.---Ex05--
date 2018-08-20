using System;
using System.Windows.Forms;

namespace GameManager
{
    public partial class FormSettings : Form
    {
        private const string k_Computer = "[Computer]";

        public FormSettings()
        {
            InitializeComponent();
        }

        public string Player1Name
        {
            get
            {
                return textBoxPlayer1Name.Text;
            }
        }

        public string Player2Name
        {
            get
            {
                return textBoxPlayer2Name.Text;
            }
        }

        public byte BoardSize
        {
            get
            {
                byte boardSize = 8;

                if (radioButtenBoardSize6.Checked == true)
                {
                    boardSize = 6;
                }
                else if (radioButtenBoardSize8.Checked == true)
                {
                    boardSize = 8;
                }
                else if (radioButtenBoardSize10.Checked == true)
                {
                    boardSize = 10;
                }

                return boardSize;
            }
        }

        private void labelExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format(
@"Welcome,
To play against friend click on the check box,
if not just leave the check box unchecked.
Please insert names without spaces and max 20 letters.")
, "Helper");
        }

        private void checkBoxVsPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            string player2BoxStr = k_Computer;
            bool vsComputer = false;
            if (checkBoxVsPlayer2.Checked == true)
            {
                player2BoxStr = string.Empty;
                vsComputer = true;
            }

            textBoxPlayer2Name.Text = player2BoxStr;
            textBoxPlayer2Name.Enabled = vsComputer;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            formMove(e);
        }

        private void formMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Capture = false;
                Message msg = Message.Create(this.Handle, 0XA1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            }
        }

        private bool playerNameCheck(string i_Str)
        {
            bool isVaildName = true;

            if (i_Str.Contains(" ") || i_Str.Length > 20 || i_Str == string.Empty)
            {
                isVaildName = false;
            }

            return isVaildName;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            if (playerNameCheck(textBoxPlayer1Name.Text) && playerNameCheck(textBoxPlayer2Name.Text))
            {
                this.DialogResult = DialogResult.OK;
                MessageBox.Show(
                   string.Format("{0}  -VS-  {1}{2} Good Luck, Size => {3}{2} Continue to the game now", Player1Name, Player2Name, Environment.NewLine, BoardSize), "Yay!");
                this.Hide();
                GamePlay newGame = new GamePlay(BoardSize, Player1Name, Player2Name);
                newGame.StartGame();
                this.Close();
            }
            else
            {
                MessageBox.Show(
@"Plase insert all the fields in the right format
to continue to the Game. help in the '?' .", "Fields Missing/Incorrect", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
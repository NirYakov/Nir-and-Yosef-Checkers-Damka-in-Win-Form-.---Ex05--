namespace GameManager
{
    public partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelExit = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtenBoardSize10 = new System.Windows.Forms.RadioButton();
            this.radioButtenBoardSize8 = new System.Windows.Forms.RadioButton();
            this.radioButtenBoardSize6 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxVsPlayer2 = new System.Windows.Forms.CheckBox();
            this.textBoxPlayer1Name = new System.Windows.Forms.TextBox();
            this.textBoxPlayer2Name = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Controls.Add(this.labelExit);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // labelTitle
            // 
            resources.ApplyResources(this.labelTitle, "labelTitle");
            this.labelTitle.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelTitle.Name = "labelTitle";
            // 
            // labelExit
            // 
            resources.ApplyResources(this.labelExit, "labelExit");
            this.labelExit.ForeColor = System.Drawing.SystemColors.Window;
            this.labelExit.Name = "labelExit";
            this.labelExit.Click += new System.EventHandler(this.labelExit_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButtenBoardSize10
            // 
            resources.ApplyResources(this.radioButtenBoardSize10, "radioButtenBoardSize10");
            this.radioButtenBoardSize10.Name = "radioButtenBoardSize10";
            this.radioButtenBoardSize10.UseVisualStyleBackColor = true;
            // 
            // radioButtenBoardSize8
            // 
            resources.ApplyResources(this.radioButtenBoardSize8, "radioButtenBoardSize8");
            this.radioButtenBoardSize8.Checked = true;
            this.radioButtenBoardSize8.Name = "radioButtenBoardSize8";
            this.radioButtenBoardSize8.TabStop = true;
            this.radioButtenBoardSize8.UseVisualStyleBackColor = true;
            // 
            // radioButtenBoardSize6
            // 
            resources.ApplyResources(this.radioButtenBoardSize6, "radioButtenBoardSize6");
            this.radioButtenBoardSize6.Name = "radioButtenBoardSize6";
            this.radioButtenBoardSize6.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // checkBoxVsPlayer2
            // 
            resources.ApplyResources(this.checkBoxVsPlayer2, "checkBoxVsPlayer2");
            this.checkBoxVsPlayer2.Name = "checkBoxVsPlayer2";
            this.checkBoxVsPlayer2.UseVisualStyleBackColor = true;
            this.checkBoxVsPlayer2.CheckedChanged += new System.EventHandler(this.checkBoxVsPlayer2_CheckedChanged);
            // 
            // textBoxPlayer1Name
            // 
            resources.ApplyResources(this.textBoxPlayer1Name, "textBoxPlayer1Name");
            this.textBoxPlayer1Name.Name = "textBoxPlayer1Name";
            // 
            // textBoxPlayer2Name
            // 
            resources.ApplyResources(this.textBoxPlayer2Name, "textBoxPlayer2Name");
            this.textBoxPlayer2Name.Name = "textBoxPlayer2Name";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // button2
            // 
            this.button2.ForeColor = System.Drawing.SystemColors.Highlight;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxPlayer2Name);
            this.Controls.Add(this.textBoxPlayer1Name);
            this.Controls.Add(this.checkBoxVsPlayer2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioButtenBoardSize10);
            this.Controls.Add(this.radioButtenBoardSize8);
            this.Controls.Add(this.radioButtenBoardSize6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelExit;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtenBoardSize10;
        private System.Windows.Forms.RadioButton radioButtenBoardSize8;
        private System.Windows.Forms.RadioButton radioButtenBoardSize6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxVsPlayer2;
        private System.Windows.Forms.TextBox textBoxPlayer1Name;
        private System.Windows.Forms.TextBox textBoxPlayer2Name;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
    }
}
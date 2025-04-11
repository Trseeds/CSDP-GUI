namespace CSDP_GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Cover = new PictureBox();
            Play_Pause = new Button();
            StatusWindow = new Label();
            Stop = new Button();
            Next = new Button();
            Previous = new Button();
            Seekbar = new TrackBar();
            Download = new Button();
            Menu = new Button();
            Eject = new Button();
            Quit = new Button();
            panel1 = new Panel();
            DevBox = new Label();
            ((System.ComponentModel.ISupportInitialize)Cover).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Seekbar).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // Cover
            // 
            Cover.AccessibleDescription = "Song cover";
            Cover.AccessibleName = "Song Cover";
            Cover.Image = Properties.Resource1.None;
            Cover.Location = new Point(0, 0);
            Cover.Name = "Cover";
            Cover.Size = new Size(250, 250);
            Cover.SizeMode = PictureBoxSizeMode.StretchImage;
            Cover.TabIndex = 0;
            Cover.TabStop = false;
            // 
            // Play_Pause
            // 
            Play_Pause.BackColor = Color.Transparent;
            Play_Pause.ForeColor = Color.Transparent;
            Play_Pause.Image = Properties.Resource1.Play;
            Play_Pause.Location = new Point(12, 256);
            Play_Pause.Name = "Play_Pause";
            Play_Pause.Size = new Size(33, 33);
            Play_Pause.TabIndex = 2;
            Play_Pause.UseVisualStyleBackColor = false;
            // 
            // StatusWindow
            // 
            StatusWindow.AutoSize = true;
            StatusWindow.BorderStyle = BorderStyle.FixedSingle;
            StatusWindow.Location = new Point(0, 0);
            StatusWindow.Name = "StatusWindow";
            StatusWindow.Size = new Size(101, 17);
            StatusWindow.TabIndex = 3;
            StatusWindow.Text = "No disc detected.";
            // 
            // Stop
            // 
            Stop.BackColor = Color.Transparent;
            Stop.Image = Properties.Resource1.Stop;
            Stop.Location = new Point(51, 256);
            Stop.Name = "Stop";
            Stop.Size = new Size(33, 33);
            Stop.TabIndex = 4;
            Stop.UseVisualStyleBackColor = false;
            // 
            // Next
            // 
            Next.Image = Properties.Resource1.Next;
            Next.Location = new Point(129, 256);
            Next.Name = "Next";
            Next.Size = new Size(33, 33);
            Next.TabIndex = 5;
            Next.UseVisualStyleBackColor = true;
            // 
            // Previous
            // 
            Previous.Image = Properties.Resource1.Previous;
            Previous.Location = new Point(90, 256);
            Previous.Name = "Previous";
            Previous.Size = new Size(33, 33);
            Previous.TabIndex = 6;
            Previous.UseVisualStyleBackColor = true;
            // 
            // Seekbar
            // 
            Seekbar.LargeChange = 2250;
            Seekbar.Location = new Point(0, 0);
            Seekbar.Name = "Seekbar";
            Seekbar.Size = new Size(250, 45);
            Seekbar.SmallChange = 75;
            Seekbar.TabIndex = 7;
            Seekbar.TickStyle = TickStyle.None;
            // 
            // Download
            // 
            Download.Image = Properties.Resource1.Download;
            Download.Location = new Point(68, 311);
            Download.Name = "Download";
            Download.Size = new Size(33, 33);
            Download.TabIndex = 8;
            Download.UseVisualStyleBackColor = true;
            // 
            // Menu
            // 
            Menu.Image = Properties.Resource1.Menu;
            Menu.Location = new Point(178, 311);
            Menu.Name = "Menu";
            Menu.Size = new Size(33, 33);
            Menu.TabIndex = 9;
            Menu.UseVisualStyleBackColor = true;
            // 
            // Eject
            // 
            Eject.Image = Properties.Resource1.Eject;
            Eject.Location = new Point(168, 256);
            Eject.Name = "Eject";
            Eject.Size = new Size(33, 33);
            Eject.TabIndex = 10;
            Eject.UseVisualStyleBackColor = true;
            // 
            // Quit
            // 
            Quit.Image = Properties.Resource1.Quit;
            Quit.Location = new Point(207, 256);
            Quit.Name = "Quit";
            Quit.Size = new Size(33, 33);
            Quit.TabIndex = 11;
            Quit.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(Seekbar);
            panel1.Location = new Point(0, 225);
            panel1.Name = "panel1";
            panel1.Size = new Size(250, 25);
            panel1.TabIndex = 12;
            // 
            // DevBox
            // 
            DevBox.AutoSize = true;
            DevBox.BorderStyle = BorderStyle.FixedSingle;
            DevBox.Location = new Point(139, 300);
            DevBox.Name = "DevBox";
            DevBox.Size = new Size(51, 17);
            DevBox.TabIndex = 13;
            DevBox.Text = "Dev Box";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CausesValidation = false;
            ClientSize = new Size(250, 330);
            Controls.Add(DevBox);
            Controls.Add(panel1);
            Controls.Add(Quit);
            Controls.Add(Eject);
            Controls.Add(Menu);
            Controls.Add(Download);
            Controls.Add(Previous);
            Controls.Add(Next);
            Controls.Add(Stop);
            Controls.Add(StatusWindow);
            Controls.Add(Play_Pause);
            Controls.Add(Cover);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximumSize = new Size(266, 369);
            Name = "Form1";
            Text = "CSharp Disc Player";
            ((System.ComponentModel.ISupportInitialize)Cover).EndInit();
            ((System.ComponentModel.ISupportInitialize)Seekbar).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox Cover;
        private Button Play;
        private Button Play_Pause;
        private Label StatusWindow;
        private Button Stop;
        private Button Next;
        private Button Previous;
        private TrackBar Seekbar;
        private Button Download;
        private Button Menu;
        private Button Eject;
        private Button Quit;
        private Panel panel1;
        private Label DevBox;
    }
}

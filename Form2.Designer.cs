namespace CSDP_GUI
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            Save = new Button();
            Album = new TextBox();
            Artist = new TextBox();
            Year = new TextBox();
            Genre = new TextBox();
            Songs = new TextBox();
            CoverPath = new TextBox();
            SuspendLayout();
            // 
            // Save
            // 
            Save.Location = new Point(12, 164);
            Save.Name = "Save";
            Save.Size = new Size(196, 25);
            Save.TabIndex = 0;
            Save.Text = "Save!";
            Save.UseVisualStyleBackColor = true;
            // 
            // Album
            // 
            Album.Location = new Point(12, 12);
            Album.Name = "Album";
            Album.Size = new Size(95, 23);
            Album.TabIndex = 1;
            Album.Text = "Album";
            // 
            // Artist
            // 
            Artist.Location = new Point(113, 12);
            Artist.Name = "Artist";
            Artist.Size = new Size(95, 23);
            Artist.TabIndex = 2;
            Artist.Text = "Artist";
            // 
            // Year
            // 
            Year.Location = new Point(12, 41);
            Year.Name = "Year";
            Year.Size = new Size(95, 23);
            Year.TabIndex = 4;
            Year.Text = "Year";
            // 
            // Genre
            // 
            Genre.Location = new Point(113, 41);
            Genre.Name = "Genre";
            Genre.Size = new Size(95, 23);
            Genre.TabIndex = 5;
            Genre.Text = "Genre";
            // 
            // Songs
            // 
            Songs.Location = new Point(12, 99);
            Songs.Multiline = true;
            Songs.Name = "Songs";
            Songs.Size = new Size(196, 59);
            Songs.TabIndex = 6;
            Songs.Text = "Song names (one per line)";
            // 
            // CoverPath
            // 
            CoverPath.Location = new Point(12, 70);
            CoverPath.Name = "CoverPath";
            CoverPath.Size = new Size(196, 23);
            CoverPath.TabIndex = 7;
            CoverPath.Text = "Path to cover image (opt.)";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(224, 201);
            Controls.Add(CoverPath);
            Controls.Add(Songs);
            Controls.Add(Genre);
            Controls.Add(Year);
            Controls.Add(Artist);
            Controls.Add(Album);
            Controls.Add(Save);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(240, 240);
            MinimizeBox = false;
            MinimumSize = new Size(240, 240);
            Name = "Form2";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Save CD";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Save;
        private TextBox Artist;
        private TextBox Year;
        private TextBox Genre;
        private TextBox Songs;
        private TextBox Album;
        private TextBox CoverPath;
    }
}
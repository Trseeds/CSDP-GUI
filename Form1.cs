using System.ComponentModel;
using System.Runtime.Serialization;
using CSDP_GUI.Properties;
using Main;
using NHotkey;
using NHotkey.WindowsForms;
//TODO: Reimplement the god awful shuffle logic.
//Over write library entries instead of just appending them.
//Note: Keep all CD calls outside of This.(Begin)Invoke() if possible, for the future,
//move CD calls out of the UI thread for button presses.
//Also avoid expensive calls like loading/disposing images or reading the library file unless they've yet to be done already.
namespace CSDP_GUI
{
    public partial class Form1 : Form
    {
        bool DiscPresent = false;
        bool DevMode = false;
        bool CDLoaded = false;
        bool MenuOpen = true;
        bool DiscClosedMenu = false;
        bool ShuffleMode = false;
        int[] Identifiers = null;
        string CDID = null;
        string[] Tags = null;
        string[] Songs = null;
        string CoverReqState = "empty";
        string SongCheck = null;
        int QueuePos = 0;
        private System.Windows.Forms.Timer Loopy;
        CD CD = new CD();
        CDDB CDDB = new CDDB();
        Helpers Helpers = new Helpers();
        public Form1()
        {
            InitializeComponent();
            CD.Init();
            CDDB.InitConfig();
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            this.Play_Pause.Click += new System.EventHandler(this.Play_Pause_Click);
            this.Next.Click += new System.EventHandler(this.Next_Click);
            this.Previous.Click += new System.EventHandler(this.Previous_Click);
            this.Shuffle.Click += new System.EventHandler(this.Shuffle_Click);
            this.Seekbar.Scroll += new System.EventHandler(this.Seekbar_Scroll);
            this.TagsMenu.Click += new System.EventHandler(this.TagsMenu_Click);
            this.OpenCloseMenu.Click += new System.EventHandler(this.OpenCloseMenu_Click);
            this.Load.Click += new System.EventHandler(this.Load_Click);
            this.Eject.Click += new System.EventHandler(this.Eject_Click);
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            try
            {
                HotkeyManager.Current.AddOrReplace("Quit", Keys.Q | Keys.Control, Quit_Hotkey);
                HotkeyManager.Current.AddOrReplace("PlayPause", Keys.MediaPlayPause, Play_Pause_Hotkey);
                HotkeyManager.Current.AddOrReplace("Stop", Keys.MediaStop, Stop_Hotkey);
                HotkeyManager.Current.AddOrReplace("Next", Keys.MediaNextTrack, Next_Hotkey);
                HotkeyManager.Current.AddOrReplace("Previous", Keys.MediaPreviousTrack, Previous_Hotkey);
                HotkeyManager.Current.AddOrReplace("Eject", Keys.E | Keys.Control | Keys.Shift, Eject_Hotkey);
            }
            catch
            {
                ;
            }
            AsyncWorker.DoWork += AsyncWorker_DoWork;
            AsyncWorker.RunWorkerAsync();
        }
        public void AsyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (CD.IsDiscPresent() == false)// && !DevMode)
                {
                    CDLoaded = false;
                    Identifiers = null;
                    Tags = null;
                    Songs = null;
                    CDID = null;
                    CoverReqState = "empty";
                    this.BeginInvoke(() =>
                    {
                        this.MaximumSize = new Size(266, 289);
                        this.Size = new Size(266, 289);
                        DiscClosedMenu = true;
                        if (CoverReqState == "empty" && Cover.Image != Properties.Resource1.Empty)
                        {
                            Cover.Image.Dispose();
                            Cover.Image = Properties.Resource1.Empty;
                        }
                    });
                    CD.Stop(null);
                }
                else if (CD.IsDiscPresent() == true)
                {
                    if (DiscClosedMenu)
                    {
                        this.BeginInvoke(() =>
                        {
                            this.MaximumSize = new Size(266, 399);
                            this.Size = new Size(266, 399);
                            DiscClosedMenu = false;
                        });
                    }
                    if (!DiscPresent)
                    {
                        CD.Init();
                        if (CD.IsDiscPresent())
                        {
                            DiscPresent = true;
                        }                        
                    }
                    SongCheck = CD.GetPlayHeadPosition()[1];
                    if (CD.GetDiscStatus() == "stopped")
                    {
                        this.BeginInvoke(() =>
                        {
                            CoverReqState = "stopped";
                            StatusWindow.Text = "Stopped";
                            if (CoverReqState == "stopped" && Cover.Image != null || Cover.Image != Properties.Resource1.None)
                            {
                                Cover.Image.Dispose();
                                if (Tags != null)
                                {
                                    Cover.Image = Image.FromFile(Tags[5]);
                                }
                                else
                                {
                                    Cover.Image = Properties.Resource1.None;
                                }
                            }
                        });
                    }
                    else if (CD.GetDiscStatus() == "playing")
                    {
                        int Max;
                        string Text;
                        if (ShuffleMode)
                        {
                            Text = $"Track: {CD.GetPlayHeadPosition()[1]}/{CD.GetShuffledTrackPositions().Length}, Timestamp: {CD.GetPlayHeadPosition()[2].Substring(0, 5)}/{CD.GetShuffledTrackLengths()[Helpers.ToInt(CD.GetPlayHeadPosition()[1]) - 1].Substring(0, 5)}";
                            Max = CD.TimeToInt(CD.GetShuffledTrackLengths()[Helpers.ToInt(CD.GetPlayHeadPosition()[1]) - 1]);
                        }
                        else
                        {
                            Text = $"Track: {CD.GetPlayHeadPosition()[1]}/{CD.GetTrackPositions().Length}, Timestamp: {CD.GetPlayHeadPosition()[2].Substring(0, 5)}/{CD.GetTrackLengths()[Helpers.ToInt(CD.GetPlayHeadPosition()[1]) - 1].Substring(0, 5)}";
                            Max = CD.TimeToInt(CD.GetTrackLengths()[Helpers.ToInt(CD.GetPlayHeadPosition()[1]) - 1]);
                        }
                        int Frames = CD.TimeToInt(CD.GetPlayHeadPosition()[2]);
                        this.BeginInvoke(() =>
                        {
                            CoverReqState = "playing";
                            StatusWindow.Text = Text;
                            if (CoverReqState == "playing" && Cover.Image != null || Cover.Image != Properties.Resource1.Playing)
                            {
                                Cover.Image.Dispose();
                                if (Tags != null)
                                {
                                    Cover.Image = Image.FromFile(Tags[5]);
                                }
                                else
                                {
                                    Cover.Image = Properties.Resource1.Playing;
                                }
                            }
                            Seekbar.Maximum = Max;
                        });
                        this.BeginInvoke(() =>
                        {
                            try
                            {
                                Seekbar.Value = Frames;
                            }
                            catch
                            {
                                Seekbar.Value = 0;
                                Play_Pause.Image.Dispose();
                                Play_Pause.Image = Resource1.Play;
                                CD.Stop("end");
                            }
                        });
                    }
                    if (Identifiers == null)
                    {
                        Identifiers = [CD.GetTrackLengths().Length, CD.TimeToInt(CD.GetTrackPositions()[0]), CD.TimeToInt(CD.GetRunTime())];
                        CDID = CDDB.GenerateCDID(Identifiers);
                        if (Tags == null)
                        {
                            if (CDLoaded == false)
                            {
                                Tags = CDDB.LoadCD("Tags", CDID);
                                Songs = CDDB.LoadCD("Songs", CDID);
                                CDLoaded = true;
                                
                            }
                        }
                    }
                    if (CDLoaded)
                    {
                        string SongBoxText = Songs[Helpers.ToInt(CD.GetPlayHeadPosition()[1])-1];
                        this.BeginInvoke(() =>
                        {
                            TagsBox.Text = $"{Tags[1]} - {Tags[3]} - {Tags[4]} - {Tags[2]}";
                            SongBox.Text = SongBoxText;
                            TagsBox.Show();
                            SongBox.Show();
                        });
                    }
                    else
                    {
                        this.BeginInvoke(() =>
                        {
                            TagsBox.Hide();
                            SongBox.Hide();
                        });
                    }
                    if (ShuffleMode)
                    {
                        if (CD.TimeToInt(CD.GetPlayHeadPosition()[0]) > CD.TimeToInt(CD.GetShuffledTrackLengths()[QueuePos]))
                        {
                            RegulateQueuePos("add");
                            Helpers.Log(QueuePos.ToString());
                            CD.Play(CD.GetShuffledTrackPositions()[QueuePos],"end");
                        }
                    }
                }
                if (!CDLoaded)
                {
                    this.BeginInvoke(() =>
                    {
                        TagsBox.Hide();
                        SongBox.Hide();
                    });
                }
                else
                {
                    this.BeginInvoke(() =>
                    {
                        TagsBox.Show();
                        SongBox.Show();
                    });
                }
                Thread.Sleep(100);
            }
        }
        private void RegulateQueuePos(string Type)
        {
            if (Type == "add" && QueuePos < CD.GetTrackPositions().Length - 1)
            {
                QueuePos++;
            }
            else if (Type == "add" && QueuePos == CD.GetTrackPositions().Length - 1)
            {
                QueuePos = 0;
            }
            else if (Type == "sub" && QueuePos > 0)
            {
                QueuePos--;
            }
            else
            {
                QueuePos = CD.GetTrackPositions().Length - 1;
            }
        }
        private void Stop_Click(object sender, System.EventArgs e)
        {
            Play_Pause.Image.Dispose();
            Play_Pause.Image = Properties.Resource1.Play;
            Seekbar.Value = 0;
            CD.Stop("end");
        }
        private void Stop_Hotkey(object sender, HotkeyEventArgs e)
        {

            Stop_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        private void Play_Pause_Click(object sender, System.EventArgs e)
        {
            if (CD.GetDiscStatus() == "stopped")
            {
                Play_Pause.Image.Dispose();
                Play_Pause.Image = Properties.Resource1.Pause;
                CD.Play("null", "null");
            }
            else if (CD.GetDiscStatus() == "playing")
            {
                Play_Pause.Image.Dispose();
                Play_Pause.Image = Properties.Resource1.Play;
                CD.Pause();
            }
        }
        private void Play_Pause_Hotkey(object sender, HotkeyEventArgs e)
        {
            Play_Pause_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        private void Next_Click(object sender, System.EventArgs e)
        {
            if (ShuffleMode)
            {
                RegulateQueuePos("add");
                CD.Seek(CD.GetShuffledTrackPositions()[QueuePos]);
            }
            else
            {
                CD.Seek((Helpers.ToInt(CD.GetPlayHeadPosition()[1]) + 1).ToString());
            }
        }
        private void Next_Hotkey(object sender, HotkeyEventArgs e)
        {
            Next_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        private void Previous_Click(object sender, System.EventArgs e)
        {
            if (Helpers.ToInt(CD.GetPlayHeadPosition()[2].Split(":")[1]) <= 3 && Helpers.ToInt(CD.GetPlayHeadPosition()[2].Split(":")[0]) == 0)
            {
                if (ShuffleMode)
                {
                    RegulateQueuePos("sub");
                    CD.Seek(CD.GetShuffledTrackPositions()[QueuePos]);
                }
                else
                {
                    CD.Seek((Helpers.ToInt(CD.GetPlayHeadPosition()[1]) - 1).ToString());
                }
            }
            else
            {
                if (ShuffleMode)
                {
                    RegulateQueuePos("sub");
                    CD.Seek(CD.GetShuffledTrackPositions()[QueuePos]);
                }
                else
                {
                    CD.Seek(CD.GetPlayHeadPosition()[1]);
                }
            }
        }
        private void Previous_Hotkey(object sender, HotkeyEventArgs e)
        {
            Previous_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        private void Shuffle_Click(object sender, System.EventArgs e)
        {
            ShuffleMode = !ShuffleMode;
            MessageBox.Show("Warning!: Shuffle mode is buggy!", "Warning!");
        }
        private void Seekbar_Scroll(object sender, System.EventArgs e)
        {
            CD.SeekInTrack(CD.IntToTime(Seekbar.Value));
        }
        private void OpenCloseMenu_Click(object sender, System.EventArgs e)
        {
            if (MenuOpen)
            {
                this.MaximumSize = new Size(266, 289);
                this.Size = new Size(266, 289);
                MenuOpen = false;
                OpenCloseMenu.Text = "Open";
                OpenCloseMenu.Location = new Point(90,210);
                Play_Pause.Location = new Point(12, 210);
                Stop.Location = new Point(207, 210);
                Next.Location = new Point(168, 210);
                Previous.Location = new Point(51, 210);
            }
            else
            {
                this.MaximumSize = new Size(266, 399);
                this.Size = new Size(266, 399);
                MenuOpen = true;
                OpenCloseMenu.Text = "Close";
                OpenCloseMenu.Location = new Point(90, 320);
                Play_Pause.Location = new Point(12, 281);
                Stop.Location = new Point(51, 281);
                Next.Location = new Point(129, 281);
                Previous.Location = new Point(90, 281);
            }
        }
        private void TagsMenu_Click(object sender, System.EventArgs e)
        {
            using (Form2 Settings = new Form2())
            {
                Settings.ShowDialog();
            }
        }
        private void Load_Click(object sender, System.EventArgs e)
        {
            Identifiers = [CD.GetTrackLengths().Length, CD.TimeToInt(CD.GetTrackPositions()[0]), CD.TimeToInt(CD.GetRunTime())];
            CDID = CDDB.GenerateCDID(Identifiers);
            Tags = CDDB.LoadCD("Tags", CDID);
            Songs = CDDB.LoadCD("Songs", CDID);
            CDLoaded = true;
        }
        private void Eject_Click(object sender, System.EventArgs e)
        {
            CD.Tray("open");
        }
        private void Eject_Hotkey(object sender, HotkeyEventArgs e)
        {
            Eject_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        private void Quit_Click(object sender, System.EventArgs e)
        {
            CD.End();
            System.Windows.Forms.Application.Exit();

        }
        private void Quit_Hotkey(object sender, HotkeyEventArgs e)
        {
            Quit_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
    }
}

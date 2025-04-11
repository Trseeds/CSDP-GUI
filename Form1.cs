using CSDP_GUI.Properties;
using Main;
using NHotkey;
using NHotkey.WindowsForms;
namespace CSDP_GUI
{
    public partial class Form1 : Form
    {
        bool DiscPresent = false;
        bool MenuOpen = false;
        bool DevMode = false;
        private System.Windows.Forms.Timer Loopy;
        CD CD = new CD();
        Config Labler = new Config();
        public int ToInt(string Input)
        {
            return (System.Convert.ToInt16(Input));
        }
        public Form1()
        {
            InitializeComponent();
            this.MaximumSize = new Size(266, 334);
            this.Size = new Size(266, 339);
            Loopy = new System.Windows.Forms.Timer();
            Loopy.Interval = 100;
            Loopy.Tick += Loopdy_Loop;
            Loopy.Start();
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            this.Play_Pause.Click += new System.EventHandler(this.Play_Pause_Click);
            this.Next.Click += new System.EventHandler(this.Next_Click);
            this.Previous.Click += new System.EventHandler(this.Previous_Click);
            this.Seekbar.Scroll += new System.EventHandler(this.Seekbar_Scroll);
            this.Download.Click += new System.EventHandler(this.Download_Click);
            this.Menu.Click += new System.EventHandler(this.Menu_Click);
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
        }
        public void Loopdy_Loop(object sender, EventArgs e)
        {
            if (DevMode)
            {
                DevBox.Show();
            }
            else
            {
                DevBox.Hide();
            }
            if (!CD.IsDiscPresent())
            {
                if (!DevMode)
                {
                    CD.Stop("empty");
                    DiscPresent = false;
                    StatusWindow.Text = "No disc detected.";
                    Cover.Image.Dispose();
                    Cover.Image = Properties.Resource1.Empty;
                    Stop.Hide();
                    Play_Pause.Hide();
                    Next.Hide();
                    Previous.Hide();
                    Seekbar.Hide();
                    Download.Hide();
                    panel1.Hide();
                    Eject.Hide();
                    Quit.Hide();
                    Menu.Hide();
                }
            }
            else
            {
                if (!DiscPresent)
                {
                    CD.Init();
                    DiscPresent = true;
                }
                Stop.Show();
                Play_Pause.Show();
                Next.Show();
                Previous.Show();
                Seekbar.Show();
                Download.Show();
                panel1.Show();
                Eject.Show();
                Quit.Show();
                Menu.Show();
                if (CD.GetDiscStatus() == "stopped")
                {
                    StatusWindow.Text = "Stopped";
                    Cover.Image.Dispose();
                    Cover.Image = Properties.Resource1.None;
                }
                else if (CD.GetDiscStatus() == "playing")
                {
                    StatusWindow.Text = $"Track: {CD.GetPlayHeadPosition()[1]}/{CD.GetTrackPositions().Length}, Timestamp: {CD.GetPlayHeadPosition()[2].Substring(0, 5)}/{CD.GetTrackLengths()[ToInt(CD.GetPlayHeadPosition()[1])-1].Substring(0, 5)}";
                    Cover.Image.Dispose();
                    Cover.Image = Properties.Resource1.Playing;
                    Seekbar.Maximum = CD.TimeToInt(CD.GetTrackLengths()[ToInt(CD.GetPlayHeadPosition()[1]) - 1]);
                    int Frames = CD.TimeToInt(CD.GetPlayHeadPosition()[2]);

                    try
                    {
                        Seekbar.Value = Frames;
                    }
                    catch
                    {
                        Seekbar.Value = 0;
                        CD.Stop("end");
                        Play_Pause.Image.Dispose();
                        Play_Pause.Image = Resource1.Play;
                    }
                }
            }
        }
        private void Stop_Click(object sender, System.EventArgs e)
        {
            CD.Stop("end");
            Play_Pause.Image.Dispose();
            Play_Pause.Image = Properties.Resource1.Play;
            Seekbar.Value = 0;
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
                CD.Play("null", "null");
                Play_Pause.Image.Dispose();
                Play_Pause.Image = Properties.Resource1.Pause;
            }
            else if (CD.GetDiscStatus() == "playing")
            {
                CD.Pause();
                Play_Pause.Image.Dispose();
                Play_Pause.Image = Properties.Resource1.Play;
            }
        }
        private void Play_Pause_Hotkey(object sender, HotkeyEventArgs e)
        {
            Play_Pause_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        private void Next_Click(object sender, System.EventArgs e)
        {
            CD.Seek((ToInt(CD.GetPlayHeadPosition()[1]) + 1).ToString());
        }
        private void Next_Hotkey(object sender, HotkeyEventArgs e)
        {
            Next_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        private void Previous_Click(object sender, System.EventArgs e)
        {
            if (ToInt(CD.GetPlayHeadPosition()[2].Split(":")[1]) <= 3 && ToInt(CD.GetPlayHeadPosition()[2].Split(":")[0]) == 0)
            {
                CD.Seek((ToInt(CD.GetPlayHeadPosition()[1]) - 1).ToString());
            }
            else
            {
                CD.Seek(CD.GetPlayHeadPosition()[1]);
            }
        }
        private void Previous_Hotkey(object sender, HotkeyEventArgs e)
        {
            Previous_Click(sender, EventArgs.Empty);
            e.Handled = true;
        }
        private void Seekbar_Scroll(object sender, System.EventArgs e)
        {
            CD.SeekTrack(CD.IntToTime(Seekbar.Value));
        }
        private void Download_Click(object sender, System.EventArgs e)
        {
            Labler.FileHandler();
        }
        private void Menu_Click(object sender, System.EventArgs e)
        {
            if (MenuOpen == false)
            {
                this.MaximumSize = new Size(266, 369);
                this.Size = new Size(266, 369);
                MenuOpen = true;
                Download.Show();
            }
            else
            {
                this.MaximumSize = new Size(266, 334);
                this.Size = new Size(266, 334);
                MenuOpen = false;
                Download.Hide();
            }
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

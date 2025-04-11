using System.Runtime.InteropServices;
using System.Text;

namespace Main
{
    class CD
    {
        public int ToInt(string Input)
        {
            return (System.Convert.ToInt16(Input));
        }
        public int TimeToInt(string Time)
        {
            int Minute = ToInt(Time.Split(":")[0]);
            int Second = ToInt(Time.Split(":")[1]);
            int Frame = ToInt(Time.Split(":")[2]);
            return (Minute * 60 * 75 + Second * 75 + Frame);
        }
        public string IntToTime(int Time)
        {
            int Minute = 0;
            int Second = 0;
            int TotalFrames = Time;
            while (TotalFrames > 74)
            {
                Second++;
                TotalFrames -= 75;
            }
            while (Second > 59)
            {
                Minute++;
                Second -= 60;
            }
            return ($"{Minute.ToString().PadLeft(2, '0')}:{Second.ToString().PadLeft(2, '0')}:{TotalFrames.ToString().PadLeft(2, '0')}");
        }
        string PauseFrom = "00:00:00";
        string PauseTo = "00:00:00";
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);
        StringBuilder Buffer = new StringBuilder(128);
        public void Init()
        {
            mciSendString("open cdaudio shareable", null, 0, IntPtr.Zero);
            if (IsDiscPresent())
            {
                PauseFrom = GetTrackPositions()[0];
                PauseTo = GetRunTime();
            }
            else
            {
                PauseTo = "00:00:00";
                PauseFrom = "00:00:00";
            }
        }
        public string GetDiscStatus()
        {
            mciSendString("status cdaudio mode", Buffer, Buffer.Capacity, IntPtr.Zero);
            return(Buffer.ToString());
        }
        public bool IsDiscPresent()
        {
           if (GetDiscStatus() == "open")
           {
               return(false);
           }
           return (true);
        }
        public void Tray(string Type)
        {
            if (Type == "open")
            {
                mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
            }
            if (Type == "close")
            {
                mciSendString("set cdaudio door closed", null, 0, IntPtr.Zero);
            }
        }
        public void End()
        {
            Stop("end");
            mciSendString("close cdaudio", null, 0, IntPtr.Zero);
        }
        public void Play(string From, string To)
        {
            if (From == "null" || To == "null")
            {
                From = PauseFrom;
                To = PauseTo;
            }
            mciSendString($"play cdaudio from {From} to {To}", null, 0, IntPtr.Zero);
        }
        public void Pause()
        {
            mciSendString("status cdaudio position", Buffer, Buffer.Capacity, IntPtr.Zero);
            PauseFrom = Buffer.ToString();
            Stop("pause");
        }
        public void Stop(string Type)
        {
            mciSendString("stop cdaudio", null, 0, IntPtr.Zero);
            if (Type == "end")
            {
                PauseFrom = GetTrackPositions()[0];
            }
            else if (Type == "empty")
            {
                ;
            }
        }
        public void Seek(string UsrInp)
        {
            try
            {
                string From = GetTrackPositions()[ToInt(UsrInp)-1];
                Play(From, PauseTo);
            }
            catch
            {
                try
                {
                    string From = $"{ToInt(UsrInp.Split(':')[0]):00}:{ToInt(UsrInp.Split(':')[1]):00}:{ToInt(UsrInp.Split(':')[2]):00}";
                    Play(From, PauseTo);
                }
                catch
                {
                    ;
                }
            }
        }
        public void SeekTrack(string UsrInp)
        {
            int Track = ToInt(GetPlayHeadPosition()[1]);
            int FrameTrack = TimeToInt(GetTrackPositions()[Track - 1]);
            int FrameFrom = TimeToInt(UsrInp);
            int FrameFin = FrameFrom + FrameTrack;
            Seek(IntToTime(FrameFin));
        }
        public string[] GetTrackLengths()
        {
            mciSendString("status cdaudio number of tracks", Buffer, Buffer.Capacity, IntPtr.Zero);
            int TrackCount = ToInt(Buffer.ToString());
            List<string> TrackDuration = new List<string>();
            for (int i = 1; i <= TrackCount; i++)
            {
                Buffer.Clear();
                mciSendString($"status cdaudio length track {i}", Buffer, Buffer.Capacity, IntPtr.Zero);
                TrackDuration.Add(Buffer.ToString());
            }
            return (TrackDuration.ToArray());
        }
        public string[] GetTrackPositions()
        {
            List<string> TrackPositions = new List<string>();
            for (int i = 1; i <= GetTrackLengths().Length; i++)
            {
                Buffer.Clear();
                mciSendString($"status cdaudio track {i} position", Buffer, Buffer.Capacity, IntPtr.Zero);
                TrackPositions.Add(Buffer.ToString());
            }
            return (TrackPositions.ToArray());
        }
        public string GetRunTime()
        {
            string LastTrackPosition = GetTrackPositions()[GetTrackPositions().Length - 1];
            string LastTrackDuration = GetTrackLengths()[GetTrackPositions().Length - 1];
            int FramePos = TimeToInt(LastTrackPosition);
            int FrameDur = TimeToInt(LastTrackDuration);
            int FrameFin = FramePos + FrameDur;
            return (IntToTime(FrameFin));
        }
        public string[] GetPlayHeadPosition()
        {
            List<string> Return = new List<string>();
            mciSendString("status cdaudio position", Buffer, Buffer.Capacity, IntPtr.Zero);
            Return.Add(Buffer.ToString());
            int Frame = TimeToInt(Return[0]);
            mciSendString("status cdaudio current track", Buffer, Buffer.Capacity, IntPtr.Zero);
            int Track = ToInt(Buffer.ToString());
            Return.Add(Track.ToString());
            int FrameTrack = TimeToInt(GetTrackPositions()[Track - 1]);
            int FrameFin = Frame - FrameTrack;
            Return.Add(IntToTime(FrameFin));
            return (Return.ToArray());
        }
    }
}
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace Main
{
    class CD
    {
        Helpers Helpers = new Helpers();
        public int TimeToInt(string Time)
        {
            int Minute = Helpers.ToInt(Time.Split(":")[0]);
            int Second = Helpers.ToInt(Time.Split(":")[1]);
            int Frame = Helpers.ToInt(Time.Split(":")[2]);
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
        public bool IsValidTimeString(string UsrInp)
        {
            try
            {
                int Test = Helpers.ToInt(UsrInp.Split(":")[0]);
                Test = Helpers.ToInt(UsrInp.Split(":")[1]);
                Test = Helpers.ToInt(UsrInp.Split(":")[2]);
                return (true);
            }
            catch
            {
                Console.WriteLine($"'{UsrInp}' was not formatted correctly. (not enough numbers or missing ':')");
                return (false);
            }
        }
        string PauseFrom = "00:00:00";
        string PauseTo = "00:00:00";
        string[] ShuffledTrackPositions = null;
        string[] ShuffledTrackLengths = null;
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);
        //StringBuilder Buffer = new StringBuilder(128);
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
            StringBuilder Buffer = new StringBuilder(128);
            mciSendString("status cdaudio mode", Buffer, Buffer.Capacity, IntPtr.Zero);
            Helpers.Log(Buffer.ToString());
            return (Buffer.ToString());
        }
        public bool IsDiscPresent()
        {
           if (GetDiscStatus() == "open" || GetDiscStatus() == "" || GetDiscStatus() == null)
           {
                ShuffledTrackPositions = null;
                return (false);
           }
           return (true);
        }
        public void Tray(string Type)
        {
            if (Type == "open")
            {
                Stop("end");
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
            StringBuilder Buffer = new StringBuilder(128);
            mciSendString("status cdaudio position", Buffer, Buffer.Capacity, IntPtr.Zero);
            PauseFrom = Buffer.ToString();
            Stop(null);
        }
        public void Stop(string Type)
        {
            mciSendString("stop cdaudio", null, 0, IntPtr.Zero);
            if (Type == "end")
            {
                PauseFrom = GetTrackPositions()[0];
                ShuffledTrackPositions = null;
                ShuffledTrackLengths = null;
            }
        }
        public void Seek(string UsrInp)
        {
            try
            {
                string From = GetTrackPositions()[Helpers.ToInt(UsrInp)-1];
                Play(From, PauseTo);
            }
            catch
            {
                if (IsValidTimeString(UsrInp))
                {
                    string From = UsrInp;
                    Play(From, PauseTo);
                }
            }
        }
        public void SeekInTrack(string UsrInp)
        {
            int Track = Helpers.ToInt(GetPlayHeadPosition()[1]);
            int FrameTrack = TimeToInt(GetTrackPositions()[Track - 1]);
            int FrameFrom = TimeToInt(UsrInp);
            int FrameFin = FrameFrom + FrameTrack;
            Seek(IntToTime(FrameFin));
        }
        public string[] GetTrackLengths()
        {
            StringBuilder Buffer = new StringBuilder(128);
            mciSendString("status cdaudio number of tracks", Buffer, Buffer.Capacity, IntPtr.Zero);
            int TrackCount = Helpers.ToInt(Buffer.ToString());
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
            StringBuilder Buffer = new StringBuilder(128);
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
            StringBuilder Buffer = new StringBuilder(128);
            List<string> Return = new List<string>();
            mciSendString("status cdaudio position", Buffer, Buffer.Capacity, IntPtr.Zero);
            int TotalFrames = TimeToInt(Buffer.ToString());
            Return.Add(Buffer.ToString()); //0, current time
            int Frame = TimeToInt(Return[0]);
            mciSendString("status cdaudio current track", Buffer, Buffer.Capacity, IntPtr.Zero);
            int Track = Helpers.ToInt(Buffer.ToString());
            Return.Add(Track.ToString());//1, current track
            int FrameTrack = TimeToInt(GetTrackPositions()[Track - 1]);
            int FrameFin = Frame - FrameTrack;
            Return.Add(IntToTime(FrameFin));//2, current time on track
            Return.Add(TotalFrames.ToString());//3, current time in frames
            return (Return.ToArray());
        }
        public void ShuffleTracks()
        {
            Random Random = new Random();
            List<string> ShuffledTrackPositionsTemp = new List<string>();
            List<string> ShuffledTrackLengthsTemp = new List<string>();
            List<string> TrackPositions = new List<string>();
            List<string> TrackLengths = new List<string>();
            int Index = 0;
            foreach (string i in GetTrackPositions())
            {
                TrackPositions.Add(i);
            }
            foreach (string i in GetTrackLengths())
            {
                TrackLengths.Add(i);
            }
            for (int i = 0; i < GetTrackLengths().Length; i++)
            {
                Index = Random.Next(0, TrackPositions.ToArray().Length);
                ShuffledTrackPositionsTemp.Add(TrackPositions[Index]);
                TrackPositions.Remove(TrackPositions[Index]);
            }
            for (int i = 0; i < GetTrackLengths().Length; i++)
            {
                Index = Random.Next(0, TrackLengths.ToArray().Length);
                ShuffledTrackLengthsTemp.Add(TrackLengths[Index]);
                TrackLengths.Remove(TrackLengths[Index]);
            }
            ShuffledTrackPositions = ShuffledTrackPositionsTemp.ToArray();
            ShuffledTrackLengths = ShuffledTrackLengthsTemp.ToArray();
        }
        public string[] GetShuffledTrackPositions()
        {
            if (ShuffledTrackPositions != null)
            {
                return (ShuffledTrackPositions);
            }
            else
            {
                ShuffleTracks();
                return (ShuffledTrackPositions);
            }
        }
        public string[] GetShuffledTrackLengths()
        {
            if (ShuffledTrackLengths != null)
            {
                return (ShuffledTrackLengths);
            }
            else
            {
                ShuffleTracks();
                return (ShuffledTrackLengths);
            }
        }
    }
}
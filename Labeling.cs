using System.Reflection.Metadata.Ecma335;
using System.Windows.Forms.VisualStyles;

namespace Main 
{
    class CDDB
    {
        CD CD = new CD();
        Helpers Helpers = new Helpers();
        public string GetRandomEmail()
        {
            string RandomTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            string[] RandomDomainTable = {".com",".net",".org"};
            string RandomEmail = "";
            Random Random = new Random();
            for (int i = 0; i < 32; i++)
            {
                RandomEmail += RandomTable[Random.Next(0,61)];
            }
            RandomEmail += "@";
            for (int i = 0; i < 8; i++)
            {
                RandomEmail += RandomTable[Random.Next(0,61)];
            }
            RandomEmail += RandomDomainTable[Random.Next(0,2)];
            return (RandomEmail);
        }
        public void InitConfig()
        {
            //FileHandler("CDDB.txt", "delete", null, null, 0);
            if (!Helpers.ToBool(FileHandler("CDDB.txt", "check", null, null, 0)))
            {
                string[] WriteLines = {GetRandomEmail()};
                FileHandler("CDDB.txt", "create", null, null, 0);
                FileHandler("CDDB.txt", "write", "WARNING!: Do not modify this file yourself unless you are sure of what you are doing!", null, 0);
                FileHandler("CDDB.txt", "append", null, WriteLines, 0);
            }
        }
        //Identifiers layout; Number of tracks, Pregap length (in frames), Runtime (in frames)
        public string GenerateCDID(int[] Identifiers)
        {
            int Tracks = Identifiers[0];
            int Pregap = Identifiers[1];
            int Runtime = Identifiers[2];
            string CDID = $"{Tracks}-{Pregap}-{Runtime}";
            return (CDID);
        }
        //Tag layout; CDID, Record name, Release year, Artist(s), Genre, Cover Path
        public void SaveCD(string[] Tags, string[] SongNames)
        {
            if (OverWriteNeeded(Tags[0]))
            {
                RemoveCD(Tags[0]);
            }
            string[] AppendCDBeg = {"Begin CD", Tags[0], Tags[1], Tags[2], Tags[3], Tags[4], Tags[5] };
            FileHandler("CDDB.txt","append",null,AppendCDBeg,0);
            for (int i = 0; i <= SongNames.Length - 1; i++)
            {
                string[] AppendSong = { "Begin song", SongNames[i], "End song" };
                FileHandler("CDDB.txt", "append", null, AppendSong, 0);
            }
            string[] AppendCDEnd = {"End CD"};
            FileHandler("CDDB.txt", "append", null, AppendCDEnd, 0);
        }
        public bool OverWriteNeeded(string CDID)
        {
            List<string> LoadedFile = new List<string>();
            foreach (string i in File.ReadAllLines("CDDB.txt"))
            {
                LoadedFile.Add(i);
            }
            foreach (string i in LoadedFile)
            {
                if (i == CDID)
                {
                    return (true);
                }
            }
            return (false);
        }
        public void RemoveCD(string CDID)
        {
            int StartLine = 0;
            int EndLine = 0;
            bool OnReqCD = false;
            List<string> LoadedFile = new List<string>();
            foreach (string i in File.ReadAllLines("CDDB.txt"))
            {
                LoadedFile.Add(i);
            }
            foreach (string i in LoadedFile)
            {
                EndLine++;
                if (!OnReqCD)
                {
                    StartLine++;
                }
                if (i == CDID)
                {
                    OnReqCD = true;
                }
                if (i == "End CD" && OnReqCD)
                {
                    EndLine -= StartLine;
                    break;
                }
            }
            LoadedFile.RemoveRange(StartLine-2,EndLine+2);
            FileHandler("CDDB.txt","delete",null,null,0);
            FileHandler("CDDB.txt", "append", null, LoadedFile.ToArray(), 0);
        }
        public string[] LoadCD(string Type,string CDID)
        {
            List<string> LoadedTags = new List<string>();
            List<string> LoadedSongs = new List<string>();
            int ReadHeadPos = 0;
            string ReadHeadCont = FileHandler("CDDB.txt","read line", null, null, ReadHeadPos);
            bool OnRequestedCD = false;
            while (ReadHeadCont != "End of file")
            {
                ReadHeadCont = FileHandler("CDDB.txt", "read line", null, null, ReadHeadPos);
                ReadHeadPos++;
                if (ReadHeadCont == "Begin CD")
                {
                    ReadHeadPos++;
                }
                if (ReadHeadCont == CDID)
                {
                    OnRequestedCD = true;
                }
                if (ReadHeadCont == "End CD")
                {
                    OnRequestedCD = false;
                }
                if (OnRequestedCD)
                {
                    LoadedTags.Add(CDID);
                    for (int a = 0; a < 5; a++)
                    {
                        ReadHeadCont = FileHandler("CDDB.txt", "read line", null, null, ReadHeadPos);
                        LoadedTags.Add(ReadHeadCont);
                        ReadHeadPos++;
                    }
                    ReadHeadPos++;
                    for (int i = 1; i <= Helpers.ToInt(CDID.Split("-")[0]); i++)
                    {
                        ReadHeadCont = FileHandler("CDDB.txt", "read line", null, null, ReadHeadPos);
                        LoadedSongs.Add(ReadHeadCont);
                        ReadHeadPos += 3;
                    }
                    if (Type == "Tags")
                    {
                        return (LoadedTags.ToArray());
                    }
                    else
                    {
                        return (LoadedSongs.ToArray());
                    }
                }
                ReadHeadPos++;
            }
            return (null);
        }
        public string FileHandler(string Filename, string OperType, string OverWrite, string[] AppendLines, int DesiredLine)
        {
            if (OperType == "check")
            {
                if (File.Exists(Filename))
                {
                    return ("true");
                }
                else
                {
                    return "false";
                }
            }
            if (OperType == "create")
            {
                using (FileStream fs = File.Create(Filename)) { }
            }
            if (OperType == "delete")
            {
                File.Delete(Filename);
            }
            if (OperType == "write")
            {
                File.WriteAllText(Filename, OverWrite);
            }
            if (OperType == "append")
            {
                File.AppendAllLines(Filename, AppendLines);
            }
            if (OperType == "read")
            {
                return(File.ReadAllText(Filename));
            }
            if (OperType == "read line")
            {
                string[] Lines = File.ReadAllLines(Filename);
                return (Lines[DesiredLine]);
            }
            return (null);
        }
    }
}
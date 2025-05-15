namespace Main
{
    public class Helpers()
    {
        public int ToInt(string Input)
        {
            try
            {
                return (System.Convert.ToInt16(Input));
            }
            catch
            {
                Log(Input);
                return (System.Convert.ToInt16(Input)); //throws intentionally to halt exec
            }
        }
        public bool ToBool(string In)
        {
            if (In == "true" || In == "True")
            {
                return (true);
            }
            else if (In == "false" || In == "False" || In == null || In == "null")
            {
                return (false);
            }
            else
            {
                return (false);
            }
        }
        public void Log(string Input)
        {
            CDDB CDDB = new CDDB();
            string[] HelpMe = { Input };
            CDDB.FileHandler("LOG.txt", "append", null, HelpMe, 0);
        }
    }
}
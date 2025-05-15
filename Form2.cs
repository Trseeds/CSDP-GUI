using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Main;

namespace CSDP_GUI
{
    public partial class Form2 : Form
    {
        CD CD = new CD();
        CDDB CDDB = new CDDB();
        Helpers Helpers = new Helpers();
        public Form2()
        {
            InitializeComponent();
            this.Save.Click += new System.EventHandler(this.Save_Click);
        }
        public void Save_Click(Object sender, EventArgs e)
        {
            int[] Identifiers = { CD.GetTrackLengths().Length, CD.TimeToInt(CD.GetTrackPositions()[0]), CD.TimeToInt(CD.GetRunTime()) };
            string[] Tags = { CDDB.GenerateCDID(Identifiers), Album.Text, Year.Text, Artist.Text, Genre.Text, CoverPath.Text};
            if (Songs.Lines.Length != Helpers.ToInt(CDDB.GenerateCDID(Identifiers).Split("-")[0]))
            {
                Songs.Text = "Number of songs provided did not match number of tracks on disc.";
            }
            else
            {
                CDDB.SaveCD(Tags, Songs.Lines);
            }
        }
    }
}

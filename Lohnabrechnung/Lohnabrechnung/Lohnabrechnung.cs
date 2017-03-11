using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basisklasse;
using System.Data.OleDb;

namespace Lohnabrechnung
{
    public partial class Lohnabrechnung : Form
    {
        Form1 hk = new Form1();
        Klasse bk = new Klasse();
        OleDbDataReader dr;
        OleDbDataAdapter da;
        DataTable dt;
        int mitnr;
        int mon;
        int jahr;

        public Lohnabrechnung()
        {
            InitializeComponent();
        }
       
        private void Lohnabrechnung_Load(object sender, EventArgs e)
        {
            mon = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("MM"));
            jahr = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("yyyy"));
            
            try
            {
                mitnr = Logik.GetMitnr();
                dr = bk.Reader("SELECT * FROM Mitarbeiter, Abteilung WHERE (" + mitnr + " = MLaNr) AND MAbtNr = AbtNr;");
                dr.Read();

                textBox1.Text = "" + dr["MVorname"] + " " + dr["MName"];
                textBox3.Text = "" + dr["MLaNr"];
                textBox8.Text = "" + dr["AbtName"];
                Logik.SetLgbetrag();
                Logik.SetAstunden_betrag();
                textBox2.Text = Logik.GetAstunden_betrag().ToString();
                Logik.SetUstunden();
                textBox4.Text = Logik.GetUstunden().ToString();
                Logik.SetGesamtlohn();
                textBox6.Text = Logik.GetGesamtlohn().ToString();
            }
            catch (Exception)
            {

          
            }
          
        }
       
    }
}

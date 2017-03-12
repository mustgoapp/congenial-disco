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
    public partial class Mitarbeiter : Form
    {
        #region Variablen
        Klasse bk = new Klasse();
        Form1 hk = new Form1();
        OleDbCommand cmd;
        OleDbDataReader dr;      
        string sql;
        string status;
        string aktiv = "false";
        bool check = false;
        bool checkTabPageOne = false;
        bool checkTabPage2 = false;
        #endregion
        public Mitarbeiter()
        {
            InitializeComponent();
         
        }
    
        private void button2_Click_1(object sender, EventArgs e)
        {  
            CheckTabPageOne();
            LoadTabPageOne();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFormular();
            SetFormularStatus(); 
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            CheckTabPage2();
            LoadTabPageTwo();
            LoadFormular();
        }
       
        private void button5_Click(object sender, EventArgs e)
        {
            dr = bk.Reader(String.Format("select Status from Mitarbeiter where MNr = {0}",comboBox1.SelectedItem));
            while (dr.Read())
            {
                status = Convert.ToString(dr["Status"]);
            }
            if (status == aktiv)
            {
                cmd = bk.Command("update Mitarbeiter set Status = 'true'where MNr = "+comboBox1.SelectedItem+"");
                button5.Text = "Deaktivieren";
                textBox14.Text = "true";
            }
            else
            {
                cmd = bk.Command("update Mitarbeiter set Status = 'false' where MNr = " + comboBox1.SelectedItem + "");
                button5.Text = "Aktivieren";
                textBox14.Text = "false";
            }
            SetFormularStatus();
        }
        #region Methode Formular laden
        private void LoadFormular()
        {
            dr = bk.Reader("select * from Mitarbeiter where MNr =" + comboBox1.SelectedItem + ";");
            while (dr.Read())
            {
                textBox10.Text = Convert.ToString(dr["MVorname"]);
                textBox9.Text = Convert.ToString(dr["MName"]);
                textBox13.Text = Convert.ToString(dr["stadt"]);
                textBox8.Text = Convert.ToString(dr["strasse"]);
                textBox12.Text = Convert.ToString(dr["plz"]);
                textBox7.Text = Convert.ToString(dr["hausnr"]);
                textBox14.Text = "";
                textBox16.Clear();
                status = Convert.ToString(dr["Status"]);
                listBox8.SelectedItem = Convert.ToInt32(dr["MLgNr"]);
                listBox6.SelectedItem = Convert.ToInt32(dr["MAbtNr"]);
                textBox14.Text = (dr["Status"].ToString());
                textBox16.Text = (dr["MLaNr"].ToString());

            }
            if (status == aktiv)
            {
                button5.Text = "Aktivieren";
            }
            else
            {
                button5.Text = "Deaktivieren";
            }
        }
        #endregion

        #region Methode Formular-Status
        private void SetFormularStatus()
        {
            if (textBox14.Text == aktiv)
            {
                textBox10.ReadOnly = true;
                textBox9.ReadOnly = true;
                textBox13.ReadOnly = true;
                textBox8.ReadOnly = true;
                textBox12.ReadOnly = true;
                textBox7.ReadOnly = true;
                listBox6.Enabled = false;
                listBox8.Enabled = false;
            }
            else
            {
                textBox10.ReadOnly = false;
                textBox9.ReadOnly = false;
                textBox13.ReadOnly = false;
                textBox8.ReadOnly = false;
                textBox12.ReadOnly = false;
                textBox7.ReadOnly = false;
                listBox6.Enabled = true;
                listBox8.Enabled = true;
            }
        }
        #endregion

        #region Methode LoadTabPageTwo
        private void LoadTabPageTwo()
        {
            if (checkTabPage2 == true)
            {
                try
                {
                    int plz = Convert.ToInt32(textBox12.Text);
                    int haunr = Convert.ToInt32(textBox7.Text);
                    int lgnr = Convert.ToInt32(listBox8.SelectedItem);
                    int abtnr = Convert.ToInt32(listBox6.SelectedItem);

                    cmd = bk.Command(String.Format("update Mitarbeiter set MVorName = '{0}', MName = '{1}', MLgNr = {2}, MAbtNr = {3}, stadt = '{4}', strasse = '{5}', plz = {6}, hausnr = {7} where MNr = {8}", textBox10.Text, textBox9.Text, lgnr, abtnr, textBox13.Text, textBox8.Text, plz, haunr, comboBox1.SelectedItem));
                    int mitnr = Convert.ToInt32(comboBox1.SelectedItem);
                    ClearMitUpdate();
                    Msg(String.Format("Der Mitarbeiter mit der Nummer {0} wurde erfolgreich Bearbeitet", mitnr));
                }
                catch (Exception)
                {
                    textBox12.Text = "";
                    textBox7.Text = "";
                    Msg("Plz und HausNr dürfen nur Zahlen enthalten!");
                }
            }
        }
        #endregion

        #region Methode LoadTabPageOne
        private void LoadTabPageOne()
        {
            if (checkTabPageOne == true)
            {

                try
                {
                    int Plz = Convert.ToInt32(textBox4.Text);
                    int HausNr = Convert.ToInt32(textBox5.Text);
                    int LaNr = Convert.ToInt32(textBox11.Text);
                    int LgNr = Convert.ToInt32(listBox1.SelectedItem);
                    int AbtNr = Convert.ToInt32(listBox2.SelectedItem);
                    string Status = "true";

                    sql = String.Format("insert into Mitarbeiter (MVorname, MName, MLgNr, MAbtNr, MLaNr, stadt, strasse, plz, hausnr, Status) values ('{0}', '{1}', {2}, {3}, {4}, '{5}', '{6}', {7}, {8}, '{9}')", textBox1.Text, textBox2.Text, LgNr, AbtNr, LaNr, textBox3.Text, textBox6.Text, Plz, HausNr, Status);
                    cmd = bk.Command(sql);
                    ClearMitHinz();
                    Msg("Erfolgreich angelegt");
                }
                catch (Exception)
                {
                    textBox5.Text = "";
                    textBox4.Text = "";
                    Msg("Plz und Hausnr dürfen nur Zahlen enthalten!");
                }
            }
        }
        #endregion

        #region Methode Clear
        private void Clear()
        {
            try
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                listBox1.SelectedIndex = 0;
                listBox2.SelectedIndex = 0;
                listBox1.ClearSelected();
                listBox2.ClearSelected();
            }
            catch (Exception)
            {
                MessageBox.Show("Beim leeren der Felder trat ein Problem auf", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearMitHinz()
        {
            try
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox11.Text = "";
                listBox1.SelectedIndex = 0;
                listBox2.SelectedIndex = 0;
            }
            catch (Exception)
            {


            }
        }

        private void ClearMitUpdate()
        {
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox16.Clear();
            textBox14.Clear();
            listBox6.SelectedIndex = 0;
            listBox8.SelectedIndex = 0;

        }
        #endregion

        #region Methode MsgBox;
        private void Msg(string msg)
        {
            MessageBox.Show(msg, "Hinweis", MessageBoxButtons.OK);
        }
        #endregion

        #region Methode Check
        private void Check()
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || listBox1.SelectedIndex == -1 || listBox2.SelectedIndex == -1 || textBox11.Text == "")
            {
                Msg("Alle Felder müssen ausgefüllt sein!");
                check = false;
            }
            else
            {
                check = true;
            }

        }
        private void CheckTabPageOne()
        {


            if (textBox11.Text == "" || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || listBox1.SelectedIndex == -1 || listBox2.SelectedIndex == -1)
            {

                Msg("Alle Felder müssen ausgefüllt sein!");
                checkTabPageOne = false;
            }
            else
            {
                try
                {
                    dr = bk.Reader("select MLaNr from Mitarbeiter where MLaNr = " + textBox11.Text + ";");
                    dr.Read();
                    if (dr.HasRows)
                    {
                        checkTabPageOne = false;
                        textBox11.Text = "";
                        Msg("Die Lohnabrechnungsnummer ist bereits vergeben!");
                    }
                    else
                    {
                        checkTabPageOne = true;
                    }

                }
                catch (Exception)
                {


                }
            }
        }

        private void CheckTabPage2()
        {
            if (textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "" || textBox10.Text == "" || textBox12.Text == "" || textBox13.Text == "" || listBox6.SelectedIndex == -1 || listBox8.SelectedIndex == -1)
            {
                Msg("Alle Felder müssen ausgefüllt sein!");
                checkTabPage2 = false;
            }
            else
            {
                checkTabPage2 = true;
            }
        }
        #endregion

        #region FormLoad
        private void Mitarbeiter_Load(object sender, EventArgs e)
        {
            dr = bk.Reader("SELECT MNr FROM Mitarbeiter");
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["MNr"]);

            }
            LoadLgNrListbox();
            LoadAbtNrListbox();
            LoadLgNrListboxTab2();
            LoadAbtNrListBoxTab2();

        }

        #endregion

        #region LoadMethoden
        private void LoadLgNrListbox()
        {
            dr = bk.Reader("select LgNr from Lohngruppen");
            while (dr.Read())
            {
                listBox1.Items.Add(dr["LgNr"]);
            }
        }

        private void LoadAbtNrListbox()
        {
            dr = bk.Reader("select AbtNr from Abteilung");
            while (dr.Read())
            {
                listBox2.Items.Add(dr["AbtNr"]);
            }
        }

        private void LoadLgNrListboxTab2()
        {
            dr = bk.Reader("select LgNr from Lohngruppen");
            while (dr.Read())
            {
                listBox8.Items.Add(dr["LgNr"]);
            }
        }
        private void LoadAbtNrListBoxTab2()
        {
            dr = bk.Reader("select AbtNr from Abteilung");
            while (dr.Read())
            {
                listBox6.Items.Add(dr["AbtNr"]);
            }
        }
        #endregion
    }

}

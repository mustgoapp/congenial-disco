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
using System.Globalization;


namespace Lohnabrechnung
{
    public partial class Form1 : Form
    {
        Klasse bk = new Klasse();
        OleDbDataReader dr;
        OleDbCommand cmd;
        int mon; //Monat
        int jahr; //Jahr
        double arbeitstunden; //Arbeitsstunden
        double ustunden; //Ueberstunden
        private int mitnr; //MitLaNr
        int unr; //UeberstundenNr
        int mlgnr; //MitLgNr
        int selindex; //Selected Index listBox3
        int lb2index;
        int item; //MLaNr
        string vname; //Vorname
        string name; //Name
        string stadt; //Stadt
        string strasse; //Hausnr
        int plz; //Hausnr
        int hausnr; //Hausnr
        int abtnr; //Abteilungsnr
        bool check2; //bool für den Speichern-Button, falls eine Abr existiert false = vorhanden true = Abrechnung kann erstellt werden
        double bonus = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetDateTime();
            LoadListBox1();
            LoadListBox2();
            LoadListBox3();
            LoadBonusComb1();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            dr = bk.Reader("SELECT * FROM Mitarbeiter, Lohngruppen where MLgNr = LgNr;");
            dr.Read();

            if (listBox1.SelectedIndex == 0)
            {
                Mitarbeiter mitarbeiter = new Mitarbeiter();
                mitarbeiter.ShowDialog();
                LoadListBox1();
            }
            SelWorker();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Logik.SetUstunden();
            Logik.SetAstunden();
            Logik.SetAstunden_betrag();
            Logik.SetGesamtlohn();
            Lohnabrechnung lar = new Lohnabrechnung();
            lar.ShowDialog();

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            LoadDateTime();
        }
        
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBox3.SelectedIndex == 0)
            {
                Lohngruppen lohngruppen = new Lohngruppen();
                lohngruppen.ShowDialog();
                listBox3.Items.Clear();
                LoadListBox3();
            }
            else
            {
                listBox3.SelectedItem = mlgnr;

            }
            
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            textBox2.Enabled = true;
            lb2index = Convert.ToInt32(listBox2.SelectedIndex) + 1;
            dr = bk.Reader("SELECT * FROM Ueberstunden WHERE '" + listBox2.SelectedItem + "' = UeName;");
            dr.Read();
            try
            {

                textBox5.Text = "" + dr["UeBetrag"].ToString();
                unr = Convert.ToInt32(dr["UeNr".ToString()]);
                textBox2.ReadOnly = false;

            }
            catch (Exception)
            {

                throw;
            }

            try
            {

                dr = bk.Reader("SELECT * FROM MUeberstunden WHERE " + lb2index + " = MUeNr AND MLaNr = " + mitnr + " AND MDatMon = " + mon + " AND MDatJahr = " + jahr + " ;");
                dr.Read();

                textBox2.Clear();
                textBox2.Text = "" + dr["MUeberstundenAnzahl"].ToString();
                textBox2.ReadOnly = true;

            }
            catch (Exception)
            {
                textBox2.Text = "0";

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox2.Text) == false)
            {
                if (check2 == false)
                {
                    MessageBox.Show("Es kann keine Abrechnung erstellt werden\rda zu diesem Monat bereits eine Abrechnung existiert!", "Hinweis", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult res;
                    bool check = true;

                    try
                    {
                        ustunden = Convert.ToDouble(textBox2.Text);
                        mitnr = Convert.ToInt32(textBox3.Text);
                        mon = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("MM"));
                        jahr = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("yyyy"));
                        check = true;

                    }
                    catch (Exception)
                    {
                        res = MessageBox.Show("Bitte Überprüfen Sie Ihre Eingabe.\r\nEs dürfen nur Zahlen eingegeben werden!", "Hinweis", MessageBoxButtons.OK);
                        if (res == DialogResult.OK)
                        {
                            textBox2.Clear();
                            check = false;
                        }
                        else
                        {
                            textBox2.Clear();
                            check = false;
                        }

                    }

                    if (check == true)
                    {
                        try
                        {
                            if (ustunden >= 1)
                            { 
                            cmd = bk.Command("INSERT INTO MUeberstunden (MUeNr, MUeberstundenAnzahl, MLaNr, MDatMon, MDatJahr) VALUES (" + unr + ", " + ustunden + "," + mitnr + ", " + mon + "," + jahr + ");");
                            MessageBox.Show("Die Überstunden wurden erfolgreich gespeichert!", "Hinweis");
                        }
                            else
                            {
                                MessageBox.Show("Die Überstunden wurden erfolgreich gespeichert!", "Hinweis");

                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Die Überstunden wurden nicht gespeichert!", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        AbrCheck();
                    }

                }
            }
            else
            {
                if (check2 == false)
                {
                    MessageBox.Show("Es kann keine Abrechnung erstellt werden\rda zu diesem Monat bereits eine Abrechnung existiert!", "Hinweis", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult res;
                    bool check = true;

                    try
                    {
                        arbeitstunden = Convert.ToInt32(textBox4.Text);
                       // Logik.SetAstunden(Convert.ToDouble(textBox4.Text));
                        mitnr = Convert.ToInt32(textBox3.Text);
                        mon = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("MM"));
                        jahr = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("yyyy"));
                        check = true;

                    }
                    catch (Exception)
                    {
                        res = MessageBox.Show("Bitte Überprüfen Sie Ihre Eingabe.\r\nEs dürfen nur Zahlen eingegeben werden!", "Hinweis", MessageBoxButtons.OK);
                        if (res == DialogResult.OK)
                        {

                            textBox4.Clear();
                            check = false;
                        }
                        else
                        {

                            textBox4.Clear();
                            check = false;
                        }

                    }
                    if (check == true)
                    {
                        try
                        {

                            cmd = bk.Command("INSERT INTO Lohnabrechnung (LaNr, LaDatMon, LaDatJahr, LaStunden) VALUES (" + mitnr + ", " + mon + "," + jahr + "," + arbeitstunden + ");");

                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Es wurde kein Betrag für die Überstunden eingetragen.", "Hinweis",MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        AbrCheck();
                    }

                }

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            mitnr = Convert.ToInt32(textBox3.Text);
        }
    

        private void button5_Click(object sender, EventArgs e)
        {

            if (check2 == false)
            {
                MessageBox.Show("Es kann keine Abrechnung erstellt werden\rda zu diesem Monat bereits eine Abrechnung existiert!", "Hinweis", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult res;
                bool check = true;

                try
                {
                    arbeitstunden = Convert.ToDouble(textBox4.Text);
                   // Logik.SetAstunden(Convert.ToDouble(textBox4.Text));
                    mitnr = Convert.ToInt32(textBox3.Text);
                    mon = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("MM"));
                    jahr = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("yyyy"));
                    check = true;

                }
                catch (Exception)
                {
                    res = MessageBox.Show("Bitte Überprüfen Sie Ihre Eingabe.\r\nEs dürfen nur Zahlen eingegeben werden!", "Hinweis", MessageBoxButtons.OK);
                    if (res == DialogResult.OK)
                    {

                        textBox4.Clear();
                        check = false;
                    }
                    else
                    {

                        textBox4.Clear();
                        check = false;
                    }

                }
                if (check == true)
                {
                    try
                    {
                        if (bonus == 0)
                        {
                            cmd = bk.Command(String.Format(CultureInfo.InvariantCulture,"insert into Lohnabrechnung (LaNr, LaDatMon, LaDatJahr, LaStunden, LaLgNr, LaMitAbt, LaMitVName, LaMitName, LaStadt, LaStrasse, LaPlz, LaHausnr) values ({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}','{9}',{10},{11})",mitnr,mon,jahr,arbeitstunden,mlgnr,abtnr,vname,name,stadt,strasse,plz,hausnr));
                            button4.Visible = true;
                        }
                        if (bonus >= 1)
                        {
                            cmd = bk.Command(String.Format(CultureInfo.InvariantCulture,"insert into Lohnabrechnung (LaNr,LaDatMon, LaDatJahr, LaStunden, LaLgNr, LaMitAbt, LaMitVName, LaMitName, LaStadt, LaStrasse, LaPlz, LaHausnr, LaBonus) values ({0},{1},{2},{3},{4},{5},'{6}','{7}',{8}','{9}',{10},{11},{12})",mitnr,mon,jahr,arbeitstunden,mlgnr,abtnr,vname,name,stadt,strasse,plz,hausnr,bonus));
                            button4.Visible = true;
                        }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der Datensatz wurde fast erfolgreich eingespeichert", "Hinweis");
                    }
                    AbrCheck();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                FormBonus formBonus = new FormBonus();
                formBonus.ShowDialog();
                LoadBonusComb1();
               
            }
            else
            {
               string combobox = Convert.ToString(comboBox1.SelectedItem);
               string combo = combobox.Substring(combobox.IndexOf(',') + 1);
                // bonus = Double.Parse(CultureInfo.InvariantCulture, combo);
                if (!double.TryParse(combo, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out bonus) &&
         // Then try in US english
         !double.TryParse(combo, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("de-DE"), out bonus) &&
         // Then in neutral language
         !double.TryParse(combo, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out bonus))
                { bonus = 0; }
            }
        }
        #region Methode DateTimeLaden
        private void LoadDateTime()
        {
            mon = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("MM"));
            jahr = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("yyyy"));
            Logik.SetMon(mon);
            Logik.SetJahr(jahr);
            if (listBox1.SelectedIndex > 0)
            {
                textBox4.Clear();
                try
                {

                    dr = bk.Reader("SELECT LaStunden FROM Lohnabrechnung WHERE (" + jahr + " = LaDatJahr AND " + mon + " = LaDatMon AND " + mitnr + " = LaNr );");

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            textBox4.Text = dr["LaStunden"].ToString();
                           // Logik.SetAstunden(Convert.ToDouble(dr["LaStunden"]));

                        }
                        textBox2.ReadOnly = true;
                        textBox4.ReadOnly = true;
                    }
                    else
                    {
                        textBox4.Text = " ";
                        textBox4.ReadOnly = false;
                        textBox2.ReadOnly = false;
                        check2 = true;
                    }

                }
                catch (Exception)
                {

                    MessageBox.Show("Beim Laden der Stunden aus der Lohnabrechnung trat ein Problem auf", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }
        #endregion

        #region Methode DateTime setzen
        private void SetDateTime()
        {
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "MM-yyyy";
        }
        #endregion

        #region Methode listBox1 laden
        private void LoadListBox1()
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("[Mitarbeiter verwalten]");
            dr = bk.Reader("SELECT MLaNr FROM Mitarbeiter where Status = 'true';");
            while (dr.Read())
            {
                listBox1.Items.Add(dr["MLaNr".ToString()]);
            }
        }
        #endregion

        #region Methode listBox2 laden
        private void LoadListBox2()
        {
            dr = bk.Reader("SELECT * FROM Ueberstunden;");
            while (dr.Read())
            {
                listBox2.Items.Add(dr["UeName".ToString()]);

            }
        }
        #endregion

        #region Methode listBox3 laden
        private void LoadListBox3()
        {
            listBox3.Items.Add("[Lohngruppen verwalten]");
            dr = bk.Reader("SELECT * FROM Lohngruppen;");
            while (dr.Read())
            {

                listBox3.Items.Add(dr["LgNr".ToString()]);
            }
        }
        #endregion

        #region Methode Ausgewählter Mitarbeiter()
        private void SelWorker()
        {
            try
            {
                if (listBox1.SelectedIndex > 0)
                {
                    item = Convert.ToInt32(listBox1.SelectedItem);
                    Logik.SetMitnr(item);

                    dr = bk.Reader("SELECT * FROM Mitarbeiter, Abteilung WHERE (" + item + " = MLaNr) AND MAbtNr = AbtNr;");
                    dr.Read();
                    textBox1.Text = "" + dr["MVorname"] + " " + dr["MName"];
                    textBox3.Text = "" + dr["MLaNr"];
                    textBox8.Text = "" + dr["AbtName"];
                    vname = dr["MVorname"].ToString();
                    name = dr["MName"].ToString();
                    stadt = dr["stadt"].ToString();
                    strasse = dr["strasse"].ToString();
                    plz = Convert.ToInt32(dr["plz"]);
                    hausnr = Convert.ToInt32(dr["hausnr"]);
                    abtnr = Convert.ToInt32(dr["MAbtNr"]);
                    textBox2.Text = "0";
                    dr = bk.Reader("SELECT MLgNr FROM Mitarbeiter WHERE MLaNr = " + mitnr + ";");
                    dr.Read();
                    mlgnr = Convert.ToInt32(dr["MLgNr"]);
                    selindex = mlgnr - 1;
                    listBox3.SelectedItem = mlgnr;
                    dr = bk.Reader("SELECT * FROM Lohngruppen WHERE LgNr = " + mlgnr + ";");

                    while (dr.Read())
                    {
                        textBox7.Text = dr["LgName"].ToString();
                        textBox10.Text = dr["LgBetrag"].ToString();

                    }

                    button3.Enabled = true;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                    AbrCheck();

                }

            }
            catch (Exception)
            {
                MessageBox.Show("Beim Laden des Mitarbeiters ist ein Problem aufgetreten", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Methode AbrCheck
        private void AbrCheck()
        {
            mon = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("MM"));
            jahr = Convert.ToInt32(dateTimePicker2.Value.Date.ToString("yyyy"));
            Logik.SetMon(mon);
            Logik.SetJahr(jahr);
            if (listBox1.SelectedIndex > 0)
            {
                textBox4.Clear();
                try
                {
                    dr = bk.Reader("SELECT LaStunden FROM Lohnabrechnung WHERE (" + jahr + " = LaDatJahr AND " + mon + " = LaDatMon AND " + mitnr + " = LaNr );");
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            textBox4.Text = dr["LaStunden"].ToString();
                            //Logik.SetAstunden(Convert.ToDouble(dr["LaStunden"]));

                        }
                        textBox4.ReadOnly = true;
                    }
                    else
                    {
                        textBox4.Text = " ";
                        textBox4.ReadOnly = false;
                        textBox2.ReadOnly = false;
                        check2 = true;
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Beim Prüfen der aktuellen Abrechnung trat ein Problem auf", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }

        }
        #endregion

        #region Methode Load Combobox1
        private void LoadBonusComb1()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("[Bonus verwalten]");
            dr = bk.Reader("SELECT * from Bonus");
            while (dr.Read())
            {
                comboBox1.Items.Add(String.Format("{0}, {1}", dr["BonusName"].ToString(), dr["BonusBetrag"].ToString()));

            }
            
        }
        #endregion
    }
}


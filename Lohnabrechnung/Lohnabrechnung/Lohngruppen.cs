using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Basisklasse;
using System.Globalization;

namespace Lohnabrechnung
{
    public partial class Lohngruppen : Form
    {
        OleDbCommand cmd;
        OleDbDataReader dr;
        Klasse bk = new Klasse();

        public Lohngruppen()
        {
            InitializeComponent();
        }

        private void Lohngruppen_Load(object sender, EventArgs e)
        {
            LoadLgNr();
        }

        #region Load

        private void LoadLgNr()
        {
            dr = bk.Reader("select LgNr from Lohngruppen");
            while (dr.Read())
            {
                listBox1.Items.Add(dr["LgNr"].ToString());
            }
        }

        #endregion

        #region Selected LgBetrag, Lg Name
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dr = bk.Reader(String.Format("select LgName, LgBetrag where LgNr = '{0}'", listBox1.SelectedItem.ToString()));
            dr.Read();
            textBox2.Text = dr["LgName"].ToString();
            textBox1.Text = dr["LgBetrag"].ToString();
        }


        #endregion

        #region Safe
        private void button1_Click(object sender, EventArgs e)
        {
            double betrag;
            string name;
            try
            {
                if (!double.TryParse(textBox1.Text, out betrag))
                {
                    MessageBox.Show("Der Betrag darf nur Zahlen enhalten!","Hinweis");
                }
                name = Convert.ToString(textBox2.Text);
                if (listBox1.SelectedIndex == -1)
                {
                    cmd = bk.Command(String.Format(CultureInfo.InvariantCulture,"insert into Lohngruppen (LgName, LgBetrag) values ('{0}', {1})", name, betrag));
                    MessageBox.Show("Die Lohngruppe wurde erfolgreich Hinzugefügt.","Hinweis");
                }
                else
                {
                    cmd = bk.Command(String.Format(CultureInfo.InvariantCulture,"update Lohngruppen set LgName = '{0}', LgBetrag = {1} where LgNr = {2}", name, betrag, listBox1.SelectedItem));
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Der Betrag darf nur Zahlen enthalten!","Hinweis");
            }
        }


        #endregion

        #region Hinzufügen
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            listBox1.SelectedIndex = -1;
            listBox1.Enabled = true;
            button3.Enabled = true;
            button2.Enabled = true;
        }

        private void Lohngruppen_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode== Keys.Escape)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                listBox1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                LoadLgNr();
                e.Handled = true;
            }
        }

        #endregion
    }
}

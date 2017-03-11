﻿using System;
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

namespace Lohnabrechnung
{
    public partial class FormBonus : Form
    {
        OleDbCommand cmd;
        OleDbDataReader dr;
        Klasse bk = new Klasse();
        public FormBonus()
        {
            InitializeComponent();


        }

        private void FormBonus_Load(object sender, EventArgs e)
        {
            LoadBonusNameList();
        }

        #region Load

        private void LoadBonusNameList()
        {
            dr = bk.Reader("select BonusName from Bonus");
            while (dr.Read())
            {
                listBox1.Items.Add(dr["BonusName"].ToString());
            }
        }

        #endregion

        #region Selected Bonus
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dr = bk.Reader(String.Format("select BonusBetrag from Bonus where BonusName = '{0}'", listBox1.SelectedItem.ToString()));
            dr.Read();
            textBox2.Text = listBox1.SelectedItem.ToString();
            textBox1.Text = dr["BonusBetrag"].ToString();
        }


        #endregion

        #region Safe
        private void button1_Click(object sender, EventArgs e)
        {

            dr = bk.Reader(String.Format("select BonusName from Bonus where BonusName = '{0}'", textBox2.Text));
            dr.Read();
            double bonus;
            try
            {
                bonus = Convert.ToDouble(textBox1.Text);
                if (dr.HasRows)
                {
                    cmd = bk.Command(String.Format("update Bonus set BonusBetrag = {0} where BonusName = '{1}' ", Convert.ToInt32(textBox1.Text), textBox2.Text));
                    MessageBox.Show("Der BonusBetrag wurde erfolgreich verändert!", "Hinweis");
                }
                else
                {
                    cmd = bk.Command(String.Format("insert into Bonus (BonusBetrag) values ({0}) where BonusName = '{1}'", Convert.ToInt32(textBox1.Text), textBox2.Text));
                    MessageBox.Show("Der Bonus wurde erfolgreich Hinzugefügt!", "Hinweis");
                }
            }
            catch (Exception)
            {
                textBox1.Clear();
                MessageBox.Show("Das Bonusfeld darf nur Zahlen enthalten!", "Hinweis");
            }


        }

        #endregion

        #region loeschen

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                cmd = bk.Command(String.Format("delete from Bonus where BonusName = '{0}'", Convert.ToString(listBox1.SelectedItem)));
                MessageBox.Show("Bonus erfolgreich gelöscht!", "Hinweis");
            }
            catch (Exception)
            {

                MessageBox.Show("Bitte wählen sie einen Bonus zum löschen aus!","Hinweis");
            }
        }

        #endregion


    }

}
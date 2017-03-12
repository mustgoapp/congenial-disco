using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Basisklasse;

namespace Lohnabrechnung
{
    class Logik
    {
       static OleDbDataReader  dr;
       static Klasse bk = new Klasse();
  
        private static int mon;
        public static int GetMon()
        { return mon; }
        public static void SetMon(int value)
        { mon = value; }
        private static int jahr;
        public static int GetJahr()
        { return jahr; }
        public static void SetJahr(int value)
        { jahr = value; }
        private static double astunden;
        public static double GetAstunden()
        { return astunden; }
        public static void SetAstunden()
        { dr = bk.Reader("SELECT LaStunden FROM Lohnabrechnung WHERE LaNr = "+GetMitnr()+" AND LaDatMon = "+GetMon()+" AND LaDatJahr = "+GetJahr()+"");
            dr.Read();
            astunden = Convert.ToDouble(dr["LaStunden"]);
        }
        private static double ustunden;
        public static double GetUstunden()
        { return ustunden; }
        public static void SetUstunden()
        {  dr =  bk.Reader("SELECT sum(MUeberstundenAnzahl*UeBetrag) from MUeberstunden, Ueberstunden WHERE MLaNr = "+mitnr+" AND MDatMon = "+mon+" AND MDatJahr = "+jahr+" AND MUeNr = UeNr;");
            try
            {
                while (dr.Read())
                {
                    ustunden = Convert.ToInt32(dr["Expr1000"]);
                }
            }
            catch (Exception)
            {

            }

        }
        private static double astunden_betrag;
        public static  double GetAstunden_betrag()
        { return astunden_betrag; }
        public static void SetAstunden_betrag()
        {
            astunden_betrag = Convert.ToDouble(GetAstunden()) * Convert.ToDouble(GetLgbetrag());
        }
        private static double gesamtlohn;
        public static double GetGesamtlohn()
        { return gesamtlohn; }
        public static void SetGesamtlohn()
        { if (mon == 3)
            {
                double prozent = (astunden_betrag + ustunden) / 100 * 10;
                gesamtlohn = astunden_betrag + ustunden + prozent;
            }
            else
            {
                gesamtlohn = astunden_betrag + ustunden;
            }
            }
        private static double bonus;
        public static double GetBonus()
        { return bonus; }
        public static void SetBonus()
        { }
        private static int mitnr;
        public static int GetMitnr()
        { return mitnr; }
        public static void SetMitnr(int value)
        { mitnr = value; }
        private static double lgbetrag;
        public static double GetLgbetrag()
        { return lgbetrag; }
        public static void SetLgbetrag()
        {
            dr = bk.Reader("SELECT * FROM Lohngruppen, Lohnabrechnung WHERE LaNr = " + GetMitnr() + " AND LaDatMon = " + GetMon() + " AND LaDatJahr = " + GetJahr() + " AND LaLgNr = LgNr;");
            while (dr.Read())
            {
                lgbetrag = Convert.ToDouble(dr["LgBetrag"]);
            }
        }
    }
}

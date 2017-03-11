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
        public static void SetAstunden(double value)
        { astunden = value;  }
        private static double ustunden;
        public static double GetUstunden()
        { return ustunden; }
        public static void SetUstunden()
        {  dr =  bk.Reader("SELECT sum(MUeberstundenAnzahl*UeBetrag) from MUeberstunden, Ueberstunden WHERE MLaNr = "+GetMitnr()+" AND MDatMon = "+GetMon()+" AND MDatJahr = "+GetJahr()+" AND MUeNr = UeNr;");
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
            dr = bk.Reader("SELECT LgBetrag FROM Lohnabrechnung, Lohngruppen WHERE LaNr = " + mitnr + " AND LaDatMon = " + mon + " AND LaDatJahr = " + jahr + " AND LaLgNr = LgNr;");
            while (dr.Read())
            {
                lgbetrag = Convert.ToDouble(dr["LgBetrag"]);
            }
        }
    }
}

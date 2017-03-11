using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;


namespace Basisklasse
{
    public class Klasse
    {
        #region Variablen
        OleDbConnection con = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = DBProjekt.accdb");
        OleDbCommand cmd;
        OleDbDataReader dr;
       
        #endregion
        #region DataAdapter
        public OleDbDataAdapter Adapter(string cmd)
        {
            con.Close();
            try
            {
                con.Open();
            }
            catch (Exception)
            {

                throw;
            }
          
                
            try
            {
                
                OleDbDataAdapter da = new OleDbDataAdapter(cmd, con);
                
                return da;
            }
            catch (Exception)
            {

                throw;
            }
                
          
            
        }
        #endregion
        #region Command
        public OleDbCommand Command(string sql)
        {   
           
            //Connection
            con.Close();
            try
            {
                con.Open();
               
            }
            catch (Exception E)
            {

                throw new ApplicationException("Datenbank konnte nicht geöffnet werden." + E);
            }
        
            //Connection END


            try
            {
                
                OleDbCommand cmd = new OleDbCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception E)
            {

                throw new ApplicationException("Write Error" + E);
            }

            return cmd;
        }
        #endregion
        #region DataReader
        public OleDbDataReader Reader(string sql)
        {

            //Connection
            con.Close();
            try
            {
                con.Open();
            }
            catch (Exception E)
            {

                throw new ApplicationException("Datenbank konnte nicht geöffnet werden." + E);
            }
            //Connection END

            try
            {
                cmd = new OleDbCommand(sql, con);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                dr = cmd.ExecuteReader();
                    
            }
            catch (Exception E)
            {

                throw new ApplicationException("Datenbank konnte nicht gelesen weden" + E);
            }
           
            return dr;
        }
        #endregion
     
    }
    }


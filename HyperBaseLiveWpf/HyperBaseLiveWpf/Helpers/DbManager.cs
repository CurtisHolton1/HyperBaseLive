using HyperBaseLiveWpf.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Helpers
{
    public class DbManager
    {
        private static readonly string _dbLocation = "Clients.Sqlite";
        public DbManager() {

        }

        public async Task<bool> CreateDB()
        {
            try
            {
                SQLiteConnection.CreateFile(_dbLocation);
                await Task.Run(() => CreateClientInstancesTable());
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<bool> CreateClientInstancesTable()
        {
            try
            {
                var SqlConnection = new SQLiteConnection("Data Source=" + _dbLocation  + "; Version=3");
                SqlConnection.Open();
                string sql = "CREATE TABLE ClientInstances(ID INTEGER PRIMARY KEY, Name varchar(50), InstanceId varchar(50), Location varchar(260), Version varchar(50), HBLAssetDir varchar(260))";
                SQLiteCommand cmd = new SQLiteCommand(sql, SqlConnection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating ClientInstances table");
                return false;
            }                  
        }

        public async Task<bool> AddOrUpdateClient(Client c)
        {
            try
            {           
                var clientToUpdate = await Task.Run(()=> GetClient(c.ID));
                var sqlConnection = new SQLiteConnection("Data Source=" + _dbLocation + "; Version=3");
                sqlConnection.Open();
                var sql = "";
                if (clientToUpdate != null)
                {
                    sql = "UPDATE ClientInstances SET ID = " + c.ID + ", Name = '" + c.Name + "' , InstanceID = '" + c.InstanceID + "', Location = '" + c.Location + "' , Version = '" + c.Version + "' , HBLAssetDir = '" + c.HBLAssetDir + "'";  
                    
                     
                }
                else
                {
                  sql = "INSERT INTO ClientInstances (Name, InstanceID, Location, Version, HBLAssetDir) VALUES ('" +c.Name + "' , '"  + c.InstanceID + "' , '" + c.Location +"' , '" + c.Version +"' , '" + c.HBLAssetDir + "')";                                    
                }
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlConnection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating ClientInstances table");
                return false;
            }
        }

        public async Task<Client> GetClient(int id)
        {
            try
            {
                var sqlConnection = new SQLiteConnection("Data Source=" + _dbLocation + "; Version=3");
                sqlConnection.Open();
                var sql = "SELECT * FROM ClientInstances WHERE Id = " + id;
                    SQLiteCommand cmd = new SQLiteCommand(sql, sqlConnection);
                cmd.ExecuteNonQuery();
                SQLiteDataReader reader = cmd.ExecuteReader();
                Client itemToReturn = null;
                while (reader.Read())
                {
                    itemToReturn = new Client { ID = Convert.ToInt32(reader["ID"].ToString()), Name = reader["Name"].ToString(), Location= reader["Location"].ToString(), Version = reader["Version"].ToString(), InstanceID = reader["InstanceID"].ToString(), HBLAssetDir = reader["HBLAssetDir"].ToString()};
                }
                return itemToReturn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating ClientInstances table");
                return null;
            }
        }

        public async Task<List<Client>> GetAllClients()
        {
            List<Client> listOfClients = new List<Client>();
            try
            {
                var sqlConnection = new SQLiteConnection("Data Source=" + _dbLocation + "; Version=3");
                sqlConnection.Open();
                var sql = "SELECT * FROM ClientInstances";
                SQLiteCommand cmd = new SQLiteCommand(sql, sqlConnection);
                cmd.ExecuteNonQuery();
                SQLiteDataReader reader = cmd.ExecuteReader();             
                while (reader.Read())
                {
                   var item = new Client { ID = Convert.ToInt32(reader["ID"].ToString()), Name = reader["Name"].ToString(), Location = reader["Location"].ToString(), Version = reader["Version"].ToString(), InstanceID = reader["InstanceID"].ToString(), HBLAssetDir = reader["HBLAssetDir"].ToString() };
                    listOfClients.Add(item);
                }
                return listOfClients;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating ClientInstances table");
                return null;
            }
        }


    }


}

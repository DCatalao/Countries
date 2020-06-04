using Countries.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Countries.Services
{
    public class DataService
    {
        private SQLiteConnection connection;

        private SQLiteCommand command;

        private DialogService dialogService;      

        public DataService()
        {
            dialogService = new DialogService();

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\Countries.sqlite";

            try
            {
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();

                string sqlcommand = "create table if not exists country" +
                                    "(name VARCHAR(50)," +
                                    "alpha2code VARCHAR(2)," +
                                    "alpha3code VARCHAR(3)," +
                                    "capital VARCHAR(50)," +
                                    "region VARCHAR(20)," +
                                    "subRegion VARCHAR(50)," +
                                    "population VARCHAR(50)," +
                                    "demonym VARCHAR(50)," +
                                    "area VARCHAR(50)," +
                                    "gini VARCHAR(50)," +
                                    "nativeName VARCHAR(50)," +
                                    "numericCode VARCHAR(3)," +
                                    "flag VARCHAR(500)," +
                                    "cioc VARCHAR(3));";


                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {

                dialogService.ShowMessage("Erro", e.Message);
            }

        }
        
        public void SaveCountries(List<Country> countries)
        {
            try
            {
                foreach (var country in countries)
                {
                    using (IDbConnection save = new SQLiteConnection(connection))
                    {
                        save.Execute("insert into country values(@name, @alpha2Code, @alpha3Code, @capital, " +
                            "@region, @subRegion, @population, @demonym, @area, @gini, @nativeName, @numericCode, @flag, @cioc);", country);                        

                    }                    
                }                

                connection.Close();
            }
            catch (Exception e)
            {

                dialogService.ShowMessage("Erro", e.Message);
            }
        }
        
        public List<Country> GetData()
        {
            List<Country> countries = new List<Country>();            

            try
            {
                string sql = "SELECT * FROM country";                

                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    countries.Add(new Country 
                    {
                        name = (string)reader["name"],
                        alpha2Code = (string)reader["alpha2code"],
                        alpha3Code = (string)reader["alpha3code"],
                        capital = (string)reader["capital"],
                        region = (string)reader["region"],
                        subRegion = (string)reader["subRegion"],
                        population = (string)reader["population"],
                        demonym = (string)reader["demonym"],
                        area = (string)reader["area"],
                        gini = (string)reader["gini"],
                        nativeName = (string)reader["nativeName"],
                        numericCode = (string)reader["numericCode"],
                        flag = (string)reader["flag"],                        
                        cioc = (string)reader["cioc"],                       

                    });
                }                

                connection.Close();

                return countries;
            }

            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
                return null;
            }
        }

    }
}

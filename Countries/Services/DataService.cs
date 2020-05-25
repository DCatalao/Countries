using Countries.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
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
                                    "(numericCode VARCHAR(3)," +
                                    "name VARCHAR(50)," +
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
                                    "flag VARCHAR(500)," +
                                    "ciocCode VARCHAR(3));" +
                                    
                                    "create table if not exists domain" +
                                    "(topLevelDomain VARCHAR(5));" +
                                    
                                    "create table if not exists topLevelDomain" +
                                    "(topLevelDomain VARCHAR(5) references domain(topLevelDomain)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));" +
                                    
                                    "create table if not exists callCode" +
                                    "(callingCode VARCHAR(5));" +
                                    
                                    "create table if not exists callingCodes" +
                                    "(callingCode VARCHAR(5) references callCode(callingCode)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));" +
                                    
                                    "create table if not exists spelling" +
                                    "(altSpelling VARCHAR(50));" +
                                    
                                    "create table if not exists altSpellings" +
                                    "(altSpelling VARCHAR(50) references spelling(altSpelling)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));" +
                                    
                                    "create table if not exists latLng" +
                                    "(lat VARCHAR(50)," +
                                    "lng VARCHAR(50)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));" +
                                    
                                    "create table if not exists timeZone" +
                                    "(timeZoneCode VARCHAR(50));" +
                                    
                                    "create table if not exists timeZones" +
                                    "(timeZoneCode VARCHAR(50) references timeZone(timeZoneCode)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));" +
                                    
                                    "create table if not exists borders" +
                                    "(ciocCode VARCHAR(3)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));" +
                                    
                                    "create table if not exists currency" +
                                    "(code VARCHAR(3)," +
                                    "name VARCHAR(50)," +
                                    "symbol VARCHAR(3));" +
                                    
                                    "create table if not exists currencies" +
                                    "(code VARCHAR(3) references currency(code)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));" +
                                    
                                    "create table if not exists translation" +
                                    "(de VARCHAR(50)," +
                                    "es VARCHAR(50)," +
                                    "fr VARCHAR(50)," +
                                    "ja VARCHAR(50)," +
                                    "it VARCHAR(50)," +
                                    "br VARCHAR(50)," +
                                    "pt VARCHAR(50)," +
                                    "nl VARCHAR(50)," +
                                    "hr VARCHAR(50)," +
                                    "fa VARCHAR(50)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));" +
                                    
                                    "create table if not exists regionalBlock" +
                                    "(acronym VARCHAR(50)," +
                                    "name VARCHAR(50)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));" +
                                    
                                    "create table if not exists otherAcronym" +
                                    "(acronym VARCHAR(50)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));" +
                                    
                                    "create table if not exists otherName" +
                                    "(name VARCHAR(50)," +
                                    "alpha3code VARCHAR(3) references country(alpha3code));";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception e)
            {

                dialogService.ShowMessage("Erro", e.Message);
            }

        }
        
        public void SaveData(List<Country> countries)
        {
            try
            {
                foreach (var country in countries)
                {                    
                    using (IDbConnection save = new SQLiteConnection(connection))
                    {
                        save.Execute("insert into country(numericCode, name, alpha2code, alpha3code, capital, region, subRegion, population, demonym, area, gini, nativeName, flag, ciocCode) " +
                            "values(@numericCode, @name, @alpha2Code, @alpha3Code, @capital, @region, @subRegion, @population, @demonym, @area, @gini, @nativeName, @flag, @cioc);" +
                            "insert into domain(topLevelDomain) values(@topLevelDomain)", country);                    
                        
                    }
                }
                connection.Close();
            }
            catch (Exception e)
            {

                dialogService.ShowMessage("Erro", e.Message);
            }
        }

    }
}

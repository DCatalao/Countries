using System;
using System.Collections.Generic;
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

            //try
            //{
            //    connection = new SQLiteConnection("Data Source" + path);
            //    connection.Open();

            //    string sqlcommand = "create table if not exists Country(numericCode VARCHAR(3) PRIMARY KEY," +
            //                                                           "name VARCHAR(50)," +
            //                                                           "alpha2code VARCHAR(2)," +
            //                                                           "alpha3code VARCHAR(3)," +
            //                                                           "capital VARCHAR(50)," +
            //                                                           "region VARCHAR(20)," +
            //                                                           "subRegion VARCHAR(50)," +
            //                                                           "population INT," +
            //                                                           "demonym VARCHAR(50)," +
            //                                                           "area DOUBLE," +
            //                                                           "gini DOUBLE," +
            //                                                           "nativeName VARCHAR(50)," +
            //                                                           "flag VARCHAR(500)," +
            //                                                           "ciocCode VARCHAR(3))";

            //    command = new SQLiteCommand(sqlcommand, connection);

            //    command.ExecuteNonQuery();
            //}
            //catch (Exception e)
            //{

            //    dialogService.ShowMessage("Erro", e.Message);
            //}

        }        
    }
}

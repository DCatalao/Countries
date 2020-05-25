using Countries.Models;
using Countries.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Countries
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>    
    public partial class MainWindow : Window
    {
        #region Atributos

        private List<Country> Countries;
        private NetworkService networkService;
        private DialogService dialogService;
        private DataService dataService;
        private ApiService apiService;

        #endregion        

        public MainWindow()
        {
            InitializeComponent();
            networkService = new NetworkService();
            dialogService = new DialogService();
            dataService = new DataService();
            apiService = new ApiService();
            LoadCountries();
        }

        private async void LoadCountries()
        {
            bool load;

            var connection = networkService.CheckConnection();

            if (!connection.IsSuccess)
            {
                LoadLocalBD();
                load = false;
            }
            else
            {
                await LoadApiCountries();
                load = true;
            }
            
            if(Countries.Count == 0)
            {
                //LabelResultado.Text = "Não há ligação à Internet e a Base de Dados não foi préviamente carregada! \n Tente novamente mais tarde!"
                return;
            }

            if (load)
            {
                //LabelStatus.Text = string.Format("Países carregados da internet em {0:F}", DateTime.Now);
            }
            else
            {
                //LabelStatus.Text = string.Format("Países carregados da Base de Dados.");
            }

        }

        private void LoadLocalBD()
        {
            dialogService.ShowMessage("WIP", "Ainda não implementado");
        }

        private async Task LoadApiCountries()
        {
            var response = await apiService.GetCountries("http://restcountries.eu", "/rest/v2/all");

            Countries = (List<Country>)response.Result;

            dataService.SaveData(Countries);
          
        }
    }
}

using Countries.Models;
using Countries.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            BDR_Progress.Visibility = Visibility.Visible;
            PRG_Progress.Value = 0;
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
                BindComboBox();
                
                load = false;
            }
            else
            {
                await LoadApiCountries();
                BindComboBox();
                
                load = true;
            }
            
            if(Countries.Count == 0)
            {
                TB_LabelInfo.Text = "Não há ligação à Internet e a Base de Dados não foi préviamente carregada! \n Tente novamente mais tarde!";
                return;
            }

            if (load)
            {
                TB_LabelInfo.Text = string.Format("Países carregados da internet em {0:F}", DateTime.Now);

                PRG_Progress.Value = 100;
            }
            else
            {
                TB_LabelInfo.Text = string.Format("Países carregados da Base de Dados.");

                PRG_Progress.Value = 100;
            }


        }        

        private void LoadLocalBD()
        {
            PRG_Progress.Value = 20;

            Countries = dataService.GetData();

            PRG_Progress.Value = 40;

            foreach (var country in Countries)
            {
                country.flagPNG = $"/Flags/{country.alpha3Code}.png";
            }

            PRG_Progress.Value = 80;
        }

        private async Task LoadApiCountries()
        {
            PRG_Progress.Value = 20;

            var response = await apiService.GetCountries("http://restcountries.eu", "/rest/v2/all");

            Countries = (List<Country>)response.Result;

            PRG_Progress.Value = 40;

            foreach (var country in Countries)
            {
                country.flagPNG = $"/Flags/{country.alpha3Code}.png";
            }

            PRG_Progress.Value = 60;

            dataService.SaveCountries(Countries);

            PRG_Progress.Value = 80;

        }

        private void BindComboBox()
        {
            CB_Countries.ItemsSource = Countries;

            PRG_Progress.Value = 90;
        }

        private void CB_Countries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Country country = new Country();

            country = (Country)CB_Countries.SelectedItem;

            DP_Country.Visibility = Visibility.Visible;
            TB_CapitalText.Visibility = Visibility.Visible;
            TB_RegionText.Visibility = Visibility.Visible;
            TB_SubRegionText.Visibility = Visibility.Visible;
            TB_PopulationText.Visibility = Visibility.Visible;
            TB_DemonymText.Visibility = Visibility.Visible;
            TB_NativeNameText.Visibility = Visibility.Visible;
            TB_CiocText.Visibility = Visibility.Visible;
            img_Flag.Source = new BitmapImage(new Uri("pack://application:,,,/Flags/" + country.alpha3Code+".png"));

            TB_name.Text = country.name;
            TB_Capital.Text = country.capital;
            TB_Region.Text = country.region;
            TB_SubRegion.Text = country.subRegion;
            TB_Population.Text = country.population;
            TB_Demonym.Text = country.demonym;
            TB_NativeName.Text = country.nativeName;
            TB_Cioc.Text = country.cioc;

            
        }

        private void BTN_close_Click(object sender, RoutedEventArgs e)
        {
            
            DP_Country.Visibility = Visibility.Hidden;
            TB_CapitalText.Visibility = Visibility.Hidden;
            TB_RegionText.Visibility = Visibility.Hidden;
            TB_SubRegionText.Visibility = Visibility.Hidden;
            TB_PopulationText.Visibility = Visibility.Hidden;
            TB_DemonymText.Visibility = Visibility.Hidden;
            TB_NativeNameText.Visibility = Visibility.Hidden;
            TB_CiocText.Visibility = Visibility.Hidden;
            img_Flag.Source = null;

            TB_name.Text = null;
            TB_Capital.Text = null;
            TB_Region.Text = null;
            TB_SubRegion.Text = null;
            TB_Population.Text = null;
            TB_Demonym.Text = null;
            TB_NativeName.Text = null;
            TB_Cioc.Text = null;
        }

        private void BTN_Info_Click(object sender, RoutedEventArgs e)
        {
            dialogService.ShowMessage("Info", "CET46 Países Software Version 1.0 by Daniel Catalão Cardozo");
        }
    }
}

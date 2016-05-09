#define DISABLE_XAML_GENERATED_BREAK_ON_UNHANDLED_EXCEPTION

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Net;
using System.Net.NetworkInformation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Currencies
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem) comboBox.SelectedItem;
            string selectedCurrency = selectedItem.Content.ToString();

            Currency result = await getCurrencyRates(selectedCurrency);
            if (result == null) return;

            DateTextBlock.Text = "Kursy walut dla " + selectedCurrency + " z dnia " + result.Date;

            if(selectedCurrency == "EUR")
            {
                CurrenciesTextBlock.Text = "USD: " + result.Rates.USD.ToString() + "\n"
                                         + "PLN: " + result.Rates.PLN.ToString() + "\n"
                                         + "GBP: " + result.Rates.GBP.ToString();
            }

            else if(selectedCurrency == "USD")
            {
                CurrenciesTextBlock.Text = "EUR: " + result.Rates.EUR.ToString() + "\n"
                                         + "PLN: " + result.Rates.PLN.ToString() + "\n"
                                         + "GBP: " + result.Rates.GBP.ToString();
            }

            else if(selectedCurrency == "PLN")
            {
                CurrenciesTextBlock.Text = "EUR: " + result.Rates.EUR.ToString() + "\n"
                                         + "USD: " + result.Rates.USD.ToString() + "\n"
                                         + "GBP: " + result.Rates.GBP.ToString();
            }

            else if(selectedCurrency == "GBP")
            {
                CurrenciesTextBlock.Text = "EUR: " + result.Rates.EUR.ToString() + "\n"
                                         + "USD: " + result.Rates.USD.ToString() + "\n"
                                         + "PLN: " + result.Rates.PLN.ToString();
            }
            
        }


        public async Task<Currency> getCurrencyRates(string currency)
        {
            string page = "http://api.fixer.io/latest?base=" + currency;

            if (CheckNet() == false)
            {
                MessageBox("Nie można połączyć się z serwerem.\nUpewnij się, że masz łączność z Internetem.");
                return null;
            }
            else
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(page))
                using (HttpContent content = response.Content)
                {
                    Currency result = await content.ReadAsAsync<Currency>().ConfigureAwait(false);
                    return result;
                }
            }

        }

        private async void MessageBox(string message)
        {
            MessageDialog MsgBox = new MessageDialog(message);
            await MsgBox.ShowAsync();
        }

        private bool CheckNet()
        {

            bool status;
            if (NetworkInterface.GetIsNetworkAvailable() == true)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }

    }
}

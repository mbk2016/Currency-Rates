using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Popups;
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

            DateTextBlock.Text = "Kursy walut z dnia " + result.Date;

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

            if (NetworkInterface.GetIsNetworkAvailable() == false)
            {
                MessageBox("Nie można połączyć się z serwerem.\nUpewnij się, że masz łączność z Internetem.");
                return null;
            }
            else
            {
                ProgressBar.Visibility = Visibility.Visible;
                ProgressBar.IsIndeterminate = true;
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(page))
                using (HttpContent content = response.Content)
                {
                    Currency result = await content.ReadAsAsync<Currency>().ConfigureAwait(false);
                    ProgressBar.Visibility = Visibility.Collapsed;
                    ProgressBar.IsIndeterminate = false;
                    return result;
                }
            }

        }

        public async void MessageBox(string message)
        {
            MessageDialog MsgBox = new MessageDialog(message);
            await MsgBox.ShowAsync();
        }
    }
}
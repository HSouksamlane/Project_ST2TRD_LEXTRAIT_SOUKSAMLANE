﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using LiveCharts;
using LiveCharts.Wpf;
using System.Text.RegularExpressions;

namespace UserInterface.Pages
{
    /// <summary>
    /// Logique d'interaction pour ETH.xaml
    /// </summary>
    public partial class CRO : Page
    {
        Crypto assetETH { get; set; }

        Crypto assetBTC { get; set; }

        Crypto assetBNB { get; set; }
        Crypto assetXRP { get; set; }

        Crypto assetSOL { get; set; }

        Crypto assetADA { get; set; }
        Crypto assetCRO { get; set; }

        Crypto assetDOT { get; set; }

        Crypto assetAVAX { get; set; }
        Crypto assetMATIC { get; set; }



        Boolean displayBTC { get; set; }
        Boolean displayBNB { get; set; }
        Boolean init { get; set; }

        public CRO()
        {
            init = true;
            int period = 365;
            assetETH = crypto_data.GetDailyPrice("ETH", period);
            assetBTC = crypto_data.GetDailyPrice("BTC", period);
            assetBNB = crypto_data.GetDailyPrice("BNB", period);
            assetXRP = crypto_data.GetDailyPrice("XRP", period);
            assetSOL = crypto_data.GetDailyPrice("SOL", period);
            assetADA = crypto_data.GetDailyPrice("ADA", period);
            assetCRO = crypto_data.GetDailyPrice("CRO", period);
            assetDOT = crypto_data.GetDailyPrice("DOT", period);
            assetAVAX = crypto_data.GetDailyPrice("AVAX", period);
            assetMATIC = crypto_data.GetDailyPrice("MATIC", period);



            InitializeComponent();
            init = false;


            setmarketCap();

            addToScatter(assetCRO, WpfPlot1, System.Drawing.Color.Purple);
            corr.Text = crypto_data.correlationCalculator(assetCRO, assetBTC, Int32.Parse(corrPeriod.Text));
        }

        private void backHomePage(object sender, RoutedEventArgs e)
        {

            Page homePage = new Landingpage();
            this.NavigationService.Navigate(homePage);

        }
        private void addToScatter(Crypto asset, ScottPlot.WpfPlot plot, System.Drawing.Color color)
        {
            IDictionary<DateTime, double> data = crypto_data.dictionnaryGeneratorDate(asset, "Close");
            DateTime[] dates = data.Keys.ToArray();
            double[] dataX = dates.Select(x => x.ToOADate()).ToArray();
            double[] dataY = data.Values.ToArray();
            plot.Plot.AddScatter(dataX, dataY, color, 1, 5);
            plot.Plot.XAxis.DateTimeFormat(true);
            plot.Refresh();

        }

        private void refreshGraph(Crypto asset1, Crypto asset2 = null, Crypto asset3 = null, Crypto asset4 = null)
        {
            WpfPlot1.Plot.Clear();
            addToScatter(asset1, WpfPlot1, System.Drawing.Color.Purple);

            if (displayBTC) // btc
            {
                addToScatter(asset2, WpfPlot1, System.Drawing.Color.Blue);
            }
            if (displayBNB) // bnb
            {
                addToScatter(asset3, WpfPlot1, System.Drawing.Color.BlueViolet);
            }
            if (asset4 != null)
            {
                addToScatter(asset4, WpfPlot1, System.Drawing.Color.Magenta);
            }


            WpfPlot1.Refresh();
        }

        private void showBnb(object sender, RoutedEventArgs e)
        {
            displayBNB = true;
            refreshGraph(assetETH, assetBTC, assetBNB);

        }

        private void unShowBnb(object sender, RoutedEventArgs e)
        {
            displayBNB = false;
            refreshGraph(assetETH, assetBTC, assetBNB);
        }

        private void showBtc(object sender, RoutedEventArgs e)
        {
            displayBTC = true;
            refreshGraph(assetETH, assetBTC, assetBNB);
        }

        private void unShowBtc(object sender, RoutedEventArgs e)
        {
            displayBTC = false;
            refreshGraph(assetETH, assetBTC, assetBNB);
        }

        private void setmarketCap()
        {
            IDictionary<string, string> marketCap = crypto_data.dictionnaryGenerator(assetXRP, "Market Cap", 10);

            string[] keys = marketCap.Keys.ToArray();
            string[] values = marketCap.Values.ToArray();

            setmarketCapRow(marketCapD9, marketCapV9, keys[0], values[0]);
            setmarketCapRow(marketCapD8, marketCapV8, keys[1], values[1]);
            setmarketCapRow(marketCapD7, marketCapV7, keys[2], values[2]);
            setmarketCapRow(marketCapD6, marketCapV6, keys[3], values[3]);
            setmarketCapRow(marketCapD5, marketCapV5, keys[4], values[4]);
            setmarketCapRow(marketCapD4, marketCapV4, keys[5], values[5]);
            setmarketCapRow(marketCapD3, marketCapV3, keys[6], values[6]);
            setmarketCapRow(marketCapD2, marketCapV2, keys[7], values[7]);
            setmarketCapRow(marketCapD1, marketCapV1, keys[8], values[8]);
            setmarketCapRow(marketCapD0, marketCapV0, keys[9], values[9]);

        }
        private void setmarketCapRow(TextBlock rowDate, TextBlock rowLabel, string keys, string values)
        {
            rowDate.Text = keys;
            rowLabel.Text = (values);
        }

        private void corrSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (init == false)
            {
                int period = 30;
                if (corrPeriod.Text != "")
                {
                    period = Int32.Parse(corrPeriod.Text);
                }
                ComboBoxItem ComboItem = (ComboBoxItem)corrComboBox.SelectedItem;
                string name = ComboItem.Name;
                switch (name)
                {
                    case "btc":
                        corr.Text = crypto_data.correlationCalculator(assetCRO, assetBTC, period);
                        break;
                    case "eth":
                        corr.Text = crypto_data.correlationCalculator(assetCRO, assetETH, period);
                        break;
                    case "xrp":
                        corr.Text = crypto_data.correlationCalculator(assetCRO, assetXRP, period);
                        break;
                    case "sol":
                        corr.Text = crypto_data.correlationCalculator(assetCRO, assetSOL, period);
                        break;
                    case "bnb":
                        corr.Text = crypto_data.correlationCalculator(assetCRO, assetBNB, period);
                        break;
                    case "cro":
                        corr.Text = crypto_data.correlationCalculator(assetCRO, assetCRO, period);
                        break;
                    case "ada":
                        corr.Text = crypto_data.correlationCalculator(assetCRO, assetADA, period);
                        break;
                    case "avax":
                        corr.Text = crypto_data.correlationCalculator(assetCRO, assetAVAX, period);
                        break;
                    case "dot":
                        corr.Text = crypto_data.correlationCalculator(assetCRO, assetDOT, period);
                        break;
                    case "matic":
                        corr.Text = crypto_data.correlationCalculator(assetCRO, assetMATIC, period);
                        break;
                    default:
                        corr.Text = "Not found";
                        break;
                }
            }
            
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            corrSelectionChanged(null, null);
        }
    }
}

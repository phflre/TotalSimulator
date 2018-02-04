using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using TotalSimulator;
using System.Linq;

namespace Total.Simulator.WPF
{
    public partial class MainWindow : Window
    {
        public TotalSimulator.TotalSimulator totalSimulator = new TotalSimulator.TotalSimulator();
        public MainWindow()
        {
            InitializeComponent();

            //Load
            this.BrowseButton.Click += BrowseButton_Click;
            this.ReadGeosn.Click += ReadGeosn_Click;
            this.LoadPrj.Click += LoadPrj_Click;
            //Save
            this.SaveDpiButton.Click += SaveDpiButton_Click;
            this.SaveKorButton.Click += SaveKorButton_Click;
            this.SavePrj.Click += SavePrj_Click;
        }

        private void SavePrj_Click(object sender, RoutedEventArgs e)
        {
            var filename = this.BrowseFile(".txt", "TXT Files (*.txt)|*.txt", false);
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
        }

        private void LoadPrj_Click(object sender, RoutedEventArgs e)
        {
            var filename = this.BrowseFile(".txt", "TXT Files (*.txt)|*.txt");
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
        }

        private void SaveKorButton_Click(object sender, RoutedEventArgs e)
        {
            var filename = this.BrowseFile(".kor", "kor Files (*.kor)|*.kor", false);
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
            File.WriteAllText(filename, this.totalSimulator.GetKorData(this.WGF.SelectedItems.OfType<TotalSimulator.Models.Point>()));
        }

        private void SaveDpiButton_Click(object sender, RoutedEventArgs e)
        {
            var filename = this.BrowseFile(".dpi", "dpi Files (*.dpi)|*.dpi", false);
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
            File.WriteAllText(filename, this.totalSimulator.GetDpiData());
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var filename = this.BrowseFile(".txt", "TXT Files (*.txt)|*.txt");
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
            this.totalSimulator.SetMeasurements(File.ReadAllText(filename));
            this.WGF.ItemsSource = this.totalSimulator.AllMeasuredWGFPoints;
            if (this.totalSimulator.Errors.Count > 0)
            {
                this.DataTextBox.Text = string.Join("\n", this.totalSimulator.Errors.ToArray());
            }
            else
            {
                this.DataTextBox.Text = this.totalSimulator.GetDpiData();
            }
            
        }

        private void ReadGeosn_Click(object sender, RoutedEventArgs e)
        {
            var filename = this.BrowseFile(".geosn", "geosn Files (*.geosn)|*.geosn");
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
        }

        //TODO implement another 4 button click events

        private string BrowseFile(string defaultExtension, string filter, bool browse = true)
        {
            FileDialog dlg;
            if (browse)
            {
                dlg = new OpenFileDialog();
            }
            else
            {
                dlg = new SaveFileDialog();
            }
            dlg.DefaultExt = defaultExtension;
            dlg.Filter = filter;
            Nullable<bool> result = dlg.ShowDialog();
            string filename = "";
            if (result == true)
            {
                filename = dlg.FileName;
            }
            return filename;
        }


    }
}

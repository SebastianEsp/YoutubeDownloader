using System;
using System.Collections.Generic;
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

namespace YoutubeDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Handles when text is changed
        private void urlBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
        //Exits the application
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Opens the preferences configuration window.
        private void MenuItem_Preferences_Click(object sender, RoutedEventArgs e)
        {
            PreferencesWindow prefWin = new PreferencesWindow();
            prefWin.Show();
        }

        private void DownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            Downloader dl = new Downloader();

            string videoUrl = urlBox.Text;

            dl.videoDownload(dl.GetVideoSource(dl.GetId(videoUrl)), videoUrl, dl.videoInfo("//title", videoUrl));
        }
    }
}

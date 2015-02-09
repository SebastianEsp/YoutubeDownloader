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
using System.Windows.Shapes;

namespace YoutubeDownloader
{
    /// <summary>
    /// Interaction logic for PreferencesWindow.xaml
    /// </summary>
    public partial class PreferencesWindow : Window
    {
        public PreferencesWindow()
        {
            InitializeComponent();
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FilePath_Box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FilePathEnter_Btn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultFilePath = FilePath_Box.Text;
        }

        private void SavePreferences_Btn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void DefaultFileName_ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DefaultFileName = true;
            if (DefaultFileName_ChkBox.IsChecked == false)
            {
                Properties.Settings.Default.DefaultFileName = false;
            }
        }
    }
}

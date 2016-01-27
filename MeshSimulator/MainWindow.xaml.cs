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
using MeshSimulator.Model;
using Environment = MeshSimulator.Model.Environment;
using System.Threading;
using MeshSimulator.View;

namespace MeshSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public VisualizationWindow visualizationWindow;

        public ViewPage viewPage;
        public SettingsPage settingsPage;
        public AboutPage aboutPage;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            viewPage = new ViewPage();
            settingsPage = new SettingsPage();
            aboutPage = new AboutPage();

            viewPage.IsUICheckBox.Checked += IsUICheckBox_Checked;
            viewPage.IsUICheckBox.Unchecked += IsUICheckBox_Checked;


            frame.Navigate(settingsPage);
        }

        void IsUICheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox)sender;
            visualizationWindow.SubscribeOnTurnEvent((bool)cb.IsChecked);
        }

        public void ShowVisualizationWindow()
        {
            if (visualizationWindow == null)
            {
                visualizationWindow = new VisualizationWindow();
                visualizationWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            }

            if (!visualizationWindow.IsVisible)
            {
                visualizationWindow.Show();
                visualizationWindow.Left = this.Left + Width;
                visualizationWindow.Top = this.Top;
            }
            else
                visualizationWindow.Hide();
        }

        public void DisposeVisualizationWindow()
        {
            visualizationWindow.Close();
            visualizationWindow = null;
        }

        private void MainItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(viewPage);
        }

        private void SettingsItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(settingsPage);
        }

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(aboutPage);
        }
    }
}

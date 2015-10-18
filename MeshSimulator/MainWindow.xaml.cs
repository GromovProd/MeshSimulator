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
        public class UIUpdateEventArgs : EventArgs
        {
            public bool IsUIUpdate { get; set; }
        }

        // Declare the delegate (if using non-generic pattern).
        public delegate void UIUpdateEventHandler(object sender, UIUpdateEventArgs e);

        // Declare the event.
        public UIUpdateEventHandler OnUIUpdateEvent;

        public VisualizationWindow visualizationWindow;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new ViewPage());
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
    }
}

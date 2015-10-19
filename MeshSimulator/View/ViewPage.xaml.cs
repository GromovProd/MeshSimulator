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

namespace MeshSimulator.View
{
    /// <summary>
    /// Interaction logic for ViewPage.xaml
    /// </summary>
    public partial class ViewPage : Page
    {
        public Environment Enviroment
        {
            get { return App.Enviroment; }
        }

        Thread t;

        public ViewPage()
        {
            InitializeComponent();

            this.Loaded += ViewPage_Loaded;
        }

        void ViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            t = new Thread(Enviroment.Emulate);

            ShowVisualizationWindow();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Enviroment.IsEmulate)
            {
                Enviroment.IsEmulate = false;
                t.Suspend();
            }
            else
            {
                Enviroment.IsEmulate = true;
                if (t.ThreadState == ThreadState.Unstarted)
                    t.Start();
                else
                {
                    t.Resume();
                }

            }

            DataContext = Enviroment;

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var chb = (CheckBox)sender;
            if (chb.IsChecked == true)
            {
                Enviroment.IsRealTime = true;

            }
            else
            {
                Enviroment.IsRealTime = false;
            }
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            var chb = (CheckBox)sender;
            if (chb.IsChecked == true)
            {
                ShowVisualizationWindow();
            }
            else
            {
                ShowVisualizationWindow();
            }
        }

        private void ShowVisualizationWindow()
        {
            var w = (MainWindow)App.Current.MainWindow;

            w.ShowVisualizationWindow();


        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MeshSimulator.Model;
using Environment = MeshSimulator.Model.Environment;
using System.Threading;
using System.Media;

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

        public Thread t;

        public ViewPage()
        {
            InitializeComponent();
            this.Loaded += ViewPage_Loaded;

        }

        void ViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Enviroment;

            App.Enviroment.OnFinish += Enviroment_OnFinish;
            //App.Enviroment.OnInfoExpanded += Enviroment_OnInfoExpanded;

            t = new Thread(Enviroment.Emulate);
            t.Priority = ThreadPriority.Highest;
            t.Name = "Prossessing";
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Enviroment.IsEmulate)
            {
                if (t.ThreadState != ThreadState.Stopped)
                {
                    t.Suspend();
                    Enviroment.IsEmulate = false;
                }
            }
            else
            {
                if (t.ThreadState == (ThreadState.Unstarted))
                {
                    t.Start();
                    Enviroment.IsEmulate = true;
                }
                else
                {
                    t.Resume();
                    Enviroment.IsEmulate = true;
                }

            }
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
                Enviroment.IsUIon = true;
                ShowVisualizationWindow();
            }
            else
            {
                Enviroment.IsUIon = false;
                ShowVisualizationWindow();
            }
        }

        void Enviroment_OnInfoExpanded(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Info expanded", "Create Report?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
            }
        }

        void Enviroment_OnFinish(object sender, EventArgs e)
        {
            SystemSounds.Beep.Play();
            var result = MessageBox.Show("That`s all folks!", "Create Report?", MessageBoxButton.YesNo);
            t.Abort();
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void ShowVisualizationWindow()
        {
            var w = (MainWindow)App.Current.MainWindow;
            w.ShowVisualizationWindow();
        }

        private void dgrid_Selected(object sender, RoutedEventArgs e)
        {
            var selectedStation = Enviroment.Stations.SingleOrDefault(i => i.IsSelected == true);
            if (selectedStation != null)
                selectedStation.IsSelected = false;
            var dg = (DataGrid)sender;
            var station = (IStation)dg.SelectedItem;
            station.IsSelected = true;
        }


    }
}

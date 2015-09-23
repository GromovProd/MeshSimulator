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

namespace MeshSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Environment enviroment = new Environment();

        public Environment Enviroment
        {
            get { return enviroment; }
            set { enviroment = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
            Enviroment.LoadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Enviroment.IsEmulate)
            {
                Enviroment.IsEmulate = false;

            }
            else
            {
                Enviroment.IsEmulate = true;
                //Dispatcher.BeginInvoke();
                
            }

        }
    }
}

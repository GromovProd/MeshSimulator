using MeshSimulator.Model;
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
using Environment = MeshSimulator.Model.Environment;

namespace MeshSimulator.View
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            ModelVariables variables = new ModelVariables();
            variables.CountOfStations = int.Parse(countOfStations.Text);
            variables.MaxSpeed = int.Parse(maxSpeed.Text);

            variables.Height = int.Parse(fHeight.Text);
            variables.Width = int.Parse(fWidth.Text);

            variables.EndTime = TimeSpan.Parse(endTime.Text);
            variables.EndTime = variables.EndTime.Add(new TimeSpan(int.Parse(endTimeDays.Text), 0, 0, 0));
            // Создать экземпляр модели

            App.CreateEnviroment(variables);

            //Перейти на страницу модуляции
            var a = (MainWindow)App.Current.MainWindow;
            NavigationService.Navigate(a.viewPage);
            //
        }
    }
}

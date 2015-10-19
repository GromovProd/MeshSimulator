using MeshSimulator.Model;
using MeshSimulator.Model.Station;
using MeshSimulator.View;
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
using Environment = MeshSimulator.Model.Environment;

namespace MeshSimulator
{
    /// <summary>
    /// Interaction logic for VisualizationWindow.xaml
    /// </summary>
    public partial class VisualizationWindow : Window
    {
        public Environment Enviroment
        {
            get { return App.Enviroment; }
        }
        public VisualizationWindow()
        {
            InitializeComponent();

            //доделать
            SubscribeOnTurnEvent(true);
        }

        public void SubscribeOnTurnEvent(bool IsUIUpdate)
        {
            if (IsUIUpdate)
            {
                Enviroment.OnTurn += Enviroment_OnTurn;
            }
            else
            {
                Enviroment.OnTurn -= Enviroment_OnTurn;
            }
        }

        void Enviroment_OnTurn(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                UpdateCanvas((Environment)sender);
            });

        }

        private void UpdateCanvas(Environment e)
        {
            wCanvas.Children.Clear();

            DrawStations(e.Stations);
            DrawConnectionRadius(e.Stations);
        }

        private void DrawStations(List<IStation> stations)
        {
            for (int i = 0; i < stations.Count; i++)
            {
                var s = stations[i];
                var elli = new Ellipse()
                {
                    Height = ViewConstants.STATIONPOINSIZE * 2,
                    Width = ViewConstants.STATIONPOINSIZE * 2,
                    Margin = new Thickness(s.Coordinate.X - ViewConstants.STATIONPOINSIZE, s.Coordinate.Y - ViewConstants.STATIONPOINSIZE, 0, 0),
                    Fill = ViewConstants.STATIONCOLORBRUSH
                };

                if (s.IsTransmit)
                {
                    elli.Fill = ViewConstants.STATIONTXCOLORBRUSH;
                }
                if (s.IsReceive)
                {
                    elli.Fill = ViewConstants.STATIONRXCOLORBRUSH;
                }

                wCanvas.Children.Add(elli);
            }
        }

        private void DrawConnectionRadius(List<IStation> stations)
        {
            for (int i = 0; i < stations.Count; i++)
            {
                var s = stations[i];
                if (s.IsTransmit)
                {
                    var elli = new Ellipse()
                    {
                        Height = s.ConnectionRadius * 2,
                        Width = s.ConnectionRadius * 2,
                        Margin = new Thickness(s.Coordinate.X - s.ConnectionRadius, s.Coordinate.Y - s.ConnectionRadius, 0, 0),
                        Fill = ViewConstants.CONNECTIONCOLORBRUSH,
                        Opacity = ViewConstants.CONNCTIONRADIUSTRANSPARENT
                    };

                    wCanvas.Children.Add(elli);
                }
            }
        }
    }
}

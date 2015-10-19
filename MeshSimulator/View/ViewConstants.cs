using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MeshSimulator.View
{
    public class ViewConstants
    {
        public const double STATIONPOINSIZE = 3;
        public static SolidColorBrush STATIONCOLORBRUSH = Brushes.Black;
        public static SolidColorBrush STATIONRXCOLORBRUSH = Brushes.Red;
        public static SolidColorBrush STATIONTXCOLORBRUSH = Brushes.Blue;

        public static SolidColorBrush CONNECTIONCOLORBRUSH = Brushes.Green;
        public const double CONNCTIONRADIUSTRANSPARENT = 0.3;

    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Environment = MeshSimulator.Model.Environment;

namespace MeshSimulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Environment enviroment = new Environment();

        public static Environment Enviroment
        {
            get { return enviroment; }
            set { enviroment = value; }
        }
    }
}

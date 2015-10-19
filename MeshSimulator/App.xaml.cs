using MeshSimulator.Model;
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
        private static Environment environment;

        public static Environment Enviroment
        {
            get { return environment; }
            private set { environment = value; }
        }

        public static Environment CreateEnviroment(ModelVariables variables)
        {
            Enviroment = new Environment(variables.CountOfStations, variables.MaxSpeed);
            return Enviroment;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model.PositionHelp
{
    public class RandomPositionHelper : IPositionHelper
    {
        public List<Coordinate> GetCoordinates(int countOfStations, int connectionRadius, int height, int width)
        {
            var list = new List<Coordinate>();

            var Rand = new Random();

            for (int i = 0; i < countOfStations; i++)
            {
                list.Add(new Coordinate() { X = Rand.Next(width - 20) + 10, Y = Rand.Next(height - 20) + 10 });
            }

            return list;
        }
    }
}

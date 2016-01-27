using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model.PositionHelp
{
    public class RoundPositionHelper : IPositionHelper
    {
        public List<Coordinate> GetCoordinates(int countOfStations, int connectionRadius, int height, int width)
        {
            var list = new List<Coordinate>();

            var centr = new Coordinate() { X = width / 2, Y = height / 2 };

            var L = 0.8 * connectionRadius * countOfStations;

            var r = L / (2 * Math.PI);

            var anglePosition = Math.Acos(1 - 0.8 * 0.8 * connectionRadius * connectionRadius / (2 * r * r));

            double tempAngle = 0;

            for (int i = 0; i < countOfStations; i++)
            {
                var x = centr.X + r * Math.Cos(tempAngle);
                var y = centr.Y + r * Math.Sin(tempAngle);

                var point = new Coordinate() { X = x, Y = y };

                tempAngle += anglePosition;

                list.Add(point);
            }

            //Сделать

            return list;

        }
    }
}

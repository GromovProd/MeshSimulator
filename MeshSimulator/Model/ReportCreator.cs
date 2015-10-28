using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class ReportCreator
    {
        private List<Report> reports = new List<Report>();

        public List<Report> Reports
        {
            get { return reports; }
            set { reports = value; }
        }

        private void Add(Environment env)
        {
            //добавить
        }
    }
}

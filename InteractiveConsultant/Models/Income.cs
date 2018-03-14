using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class Income
    {
        int FamilyPeopleCount { get; set; }
        public double PVSDD { get; set; }
        List<double> Revenue { get;}

        public Income(int familiPeopleCount)
        {
            Revenue = new List<double>();
            FamilyPeopleCount = familiPeopleCount;
        }

        public void SetIncome(IEnumerable<object> _income)
        {
            foreach(var i in _income)
            {
                Revenue.Add(Convert.ToDouble(i));
            }
        }

        public bool GetResultSDDFamily()
        {
            return ((((1.0 / 3.0) * Revenue.Sum()) / FamilyPeopleCount) < PVSDD);
        }
    }
}
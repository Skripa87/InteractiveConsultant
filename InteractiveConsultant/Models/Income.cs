using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InteractiveConsultant.Models
{
    public class Income
    {
        int FamilyPeopleCount { get; set; }
        double PVSDD { get; set; }
        List<double> income { get; set; }

        public Income(int familiPeopleCount)
        {
            income = new List<double>();
            FamilyPeopleCount = familiPeopleCount;
        }

        public bool GetResultSDDFamily()
        {
            return ((((1.0 / 3.0) * income.Sum()) / FamilyPeopleCount) < PVSDD);
        }
    }
}
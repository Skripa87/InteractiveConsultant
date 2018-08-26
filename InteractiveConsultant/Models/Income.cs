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
            if (_income != null)
            {
                foreach (var i in _income)
                {
                    try
                    {
                        Revenue.Add(Convert.ToDouble(i));
                    }
                    catch
                    {
                        Revenue.Add(0);
                    }
                }
            }
            else
            {
                Revenue.Add(0);
            }
        }

        public bool GetResultSDDFamily()
        {
            if (FamilyPeopleCount == 0) return false;
            else return (((Revenue.Sum()) / FamilyPeopleCount) < PVSDD);
        }
    }
}
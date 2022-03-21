using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab3Test
{
    class VersionInterval
    {
        private readonly List<int> mainVersionParts;

        private string StartPoint = "";
        private string EndPoint = "";
        string[] mass;

        public VersionInterval(string versionInterval)
        {
            if (versionInterval == "*")
            {
                StartPoint = "0.0.0";
                EndPoint = "1000.1000.1000";
            }
            else
            {
                if (!IsCorrect(versionInterval))
                {
                    throw new ArgumentException("Значение не корректно!");
                }

                mass = versionInterval.Split('.');
                string[] massNew = new string[mass.Length];

                while (massNew.Length < 3)
                {
                    massNew = new string[mass.Length + 1];

                    for (int i = 0; i < mass.Length; i++)
                    {
                        massNew[i] = mass[i];
                    }

                    massNew[mass.Length] = "x";
                    mass = massNew;
                }

                for (int i = 0; i < mass.Length; i++)
                {
                    string poin = ".";

                    if (i >= 2)
                    {
                        poin = "";
                    }

                    if (mass[i] == "x")
                    {
                        StartPoint += "0" + poin;
                        EndPoint += "1000" + poin;
                    }
                    else
                    {
                        StartPoint += mass[i] + poin;
                        EndPoint += mass[i] + poin;
                    }
                }
            }
        }
        public string GetStringInterval()
        {
            return " => " + StartPoint + " < " + EndPoint;
        }
        public bool FindInterval(Version v1)
        {
            string[] mass1 = v1.ToString().Split('.');
            string[] startPointMass = StartPoint.Split('.');
            string[] EndPointMass = EndPoint.Split('.');

            for (int i = 0; i < mass1.Length; i++)
            {
                if (Convert.ToInt32(mass1[i]) < Convert.ToInt32(startPointMass[i])) return false;
                if (Convert.ToInt32(mass1[i]) > Convert.ToInt32(EndPointMass[i])) return false;
            }

            return true;
        }
        public bool FindIntervalInInterval(VersionInterval v1)
        {
            string[] startPointMassCheck = v1.StartPoint.Split('.');
            string[] EndPointMassCheck = v1.EndPoint.Split('.');
            string[] startPointMass = StartPoint.Split('.');
            string[] EndPointMass = EndPoint.Split('.');

            for (int i = 0; i < startPointMassCheck.Length; i++)
            {
                if (Convert.ToInt32(startPointMassCheck[i]) < Convert.ToInt32(startPointMass[i])) return false;
                if (Convert.ToInt32(EndPointMassCheck[i]) > Convert.ToInt32(EndPointMass[i])) return false;
            }

            return true;
        }
        //(x|\d+(\.(x|\d+))*)|(>=(x|\d+(\.(x|\d+))*) <=(x|\d+(\.(x|\d+))*))
        private static bool IsCorrect(string version)
        {
            return Regex.IsMatch(version, @"x|\d+(\.(x|\d+))*");
        }
    }
}

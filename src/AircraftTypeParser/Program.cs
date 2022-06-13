using DmCommons;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace AircraftTypeParser
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            string str = Http.PostBody("https://www4.icao.int/doc8643/External/AircraftTypes");

            if (!string.IsNullOrEmpty(str))
            {
                Aircraft[] aircrafts = JsonConvert.DeserializeObject<Aircraft[]>(str);

                SortedDictionary<string, string> aircrafts_by_designator = new SortedDictionary<string, string>();

                foreach (Aircraft a in aircrafts)
                {
                    if (aircrafts_by_designator.ContainsKey(a.Designator))
                        aircrafts_by_designator[a.Designator] += $",{a.ModelFullName}";
                    else
                        aircrafts_by_designator.Add(a.Designator, $"{a.Designator}\t{a.WTC.Substring(a.WTC.Length - 1, 1)}{a.Description}\t{a.ManufacturerCode}\t{a.ModelFullName}");
                }

                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, string> i in aircrafts_by_designator)
                {
                    sb.AppendLine(i.Value);
                }

                File.WriteAllText("ICAO_Aircraft.txt", sb.ToString());
                sb = null;
            }
        }

        #endregion Private Methods

        #region Private Classes

        private class Aircraft
        {
            #region Public Fields

            public string AircraftDescription;
            public string Description;
            public string Designator;
            public string EngineCount;
            public string EngineType;
            public string ManufacturerCode;
            public string ModelFullName;
            public string WTC;
            public string WTG;

            #endregion Public Fields
        }

        #endregion Private Classes
    }
}
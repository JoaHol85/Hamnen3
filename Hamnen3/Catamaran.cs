using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Schema;

namespace Hamnen3
{
    class Catamaran : Boat
    {
        public int NumberOfBeds { get; set; }

        public Catamaran()
        {
            IDNumberOfBoat = SetID("K");
            MaxSpeed = MainWindow.RandomNumberBetweenInsertedParameters(1, 12);
            Weight = MainWindow.RandomNumberBetweenInsertedParameters(1200, 8000);
            DaysInHarbour = 3;
            NumberOfHarbourSpots = 3;
            NumberOfBeds = MainWindow.RandomNumberBetweenInsertedParameters(1, 4);
            BoatType = "Katamaran";
        }
        public Catamaran(string ID, double speed, int weight, int daysSpentInHarbour, int startSpot, string specialInfo)
        {
            DaysInHarbour = 3;
            NumberOfHarbourSpots = 3;
            IDNumberOfBoat = ID;
            MaxSpeed = speed;
            Weight = weight;
            DaysSpentInHarbour = daysSpentInHarbour;
            StartingHarbourSpot = startSpot;
            NumberOfBeds = int.Parse(specialInfo);
            BoatType = "Katamaran";

        }

        public override string PrintSpecialInfo()
        {
            return $"Antal bäddar: {NumberOfBeds} st";
        }

        public override string GetSpecialInfo()
        {
            return NumberOfBeds.ToString();
        }
        public override void SetSpecialInfo(string info)
        {
            NumberOfBeds = int.Parse(info);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Hamnen3
{
    class SailBoat : Boat
    {
        public int LengthOfBoat { get; set; }

        public SailBoat()
        {
            IDNumberOfBoat = SetID("S");
            MaxSpeed = MainWindow.RandomNumberBetweenInsertedParameters(1, 12);
            Weight = MainWindow.RandomNumberBetweenInsertedParameters(800, 6000);
            DaysInHarbour = 4;
            NumberOfHarbourSpots = 2;
            LengthOfBoat = MainWindow.RandomNumberBetweenInsertedParameters(10, 60);
            BoatType = "Segelbåt ";
        }
        public SailBoat(string ID, double speed, int weight, int daysSpentInHarbour, int startSpot, string specialInfo)
        {
            DaysInHarbour = 4;
            NumberOfHarbourSpots = 2;
            IDNumberOfBoat = ID;
            MaxSpeed = speed;
            Weight = weight;
            DaysSpentInHarbour = daysSpentInHarbour;
            StartingHarbourSpot = startSpot;
            LengthOfBoat = int.Parse(specialInfo);
            BoatType = "Segelbåt ";
        }

        public override string PrintSpecialInfo()
        {
            double length = MainWindow.CalculateLengthOfBoatMeterFromFeet(LengthOfBoat);
            return $"Antal meter lång: {length} m";
        }

        public override string GetSpecialInfo()
        {
            return LengthOfBoat.ToString();
        }

        public override void SetSpecialInfo(string info)
        {
            LengthOfBoat = int.Parse(info);
        }
    }
}

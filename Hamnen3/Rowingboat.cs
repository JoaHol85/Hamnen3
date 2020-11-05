using System;
using System.Collections.Generic;
using System.Text;

namespace Hamnen3
{
    class RowingBoat : Boat
    {
        public int NumberOfPassengers { get; set; }

        public RowingBoat()
        {
            IDNumberOfBoat = SetID("R");
            MaxSpeed = MainWindow.RandomNumberBetweenInsertedParameters(1, 3);
            Weight = MainWindow.RandomNumberBetweenInsertedParameters(100, 300);
            DaysInHarbour = 1;
            NumberOfHarbourSpots = 0.5;
            NumberOfPassengers = MainWindow.RandomNumberBetweenInsertedParameters(1, 6);
            BoatType = "Roddbåt  ";
        }

        public RowingBoat(string ID, double speed, int weight, int daysSpentInHarbour, int startSpot, string specialInfo)
        {
            DaysInHarbour = 1;
            NumberOfHarbourSpots = 0.5;
            IDNumberOfBoat = ID;
            MaxSpeed = speed;
            Weight = weight;
            BoatType = "Roddbåt ";
            DaysSpentInHarbour = daysSpentInHarbour;
            StartingHarbourSpot = startSpot;
            NumberOfPassengers = int.Parse(specialInfo);
        }

        public override string PrintSpecialInfo()
        {
            return $"Antal passagerare: {NumberOfPassengers} st";
        }

        public override string GetSpecialInfo()
        {
            return NumberOfPassengers.ToString();
        }

        public override void SetSpecialInfo(string info)
        {
            NumberOfPassengers = int.Parse(info);
        }


    }
}

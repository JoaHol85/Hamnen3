using System;
using System.Collections.Generic;
using System.Text;

namespace Hamnen3
{
    class CargoShip : Boat
    {
        public int NumberOfContainersOnShip { get; set; }

        public CargoShip()
        {
            IDNumberOfBoat = SetID("L");
            MaxSpeed = MainWindow.RandomNumberBetweenInsertedParameters(1, 20);
            Weight = MainWindow.RandomNumberBetweenInsertedParameters(3000, 20000);
            DaysInHarbour = 6;
            NumberOfHarbourSpots = 4;
            NumberOfContainersOnShip = MainWindow.RandomNumberBetweenInsertedParameters(0, 500);
            BoatType = "Lastfartyg";
        }

        public CargoShip(string ID, double speed, int weight, int daysSpentInHarbour, int startSpot, string specialInfo)
        {
            DaysInHarbour = 6;
            NumberOfHarbourSpots = 4;
            IDNumberOfBoat = ID;
            MaxSpeed = speed;
            Weight = weight;
            DaysSpentInHarbour = daysSpentInHarbour;
            StartingHarbourSpot = startSpot;
            NumberOfContainersOnShip = int.Parse(specialInfo);
            BoatType = "Lastfartyg";

        }

        public override string PrintSpecialInfo()
        {
            return $"Antal Kontainrar: {NumberOfContainersOnShip} st";
        }

        public override string GetSpecialInfo()
        {
            return NumberOfContainersOnShip.ToString();
        }
        public override void SetSpecialInfo(string info)
        {
            NumberOfContainersOnShip = int.Parse(info);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Hamnen3
{
    class MotorBoat : Boat
    {
        public int NumberOfHorsePowers { get; set; }

        public MotorBoat()
        {
            IDNumberOfBoat = SetID("M");
            MaxSpeed = MainWindow.RandomNumberBetweenInsertedParameters(1, 60);
            Weight = MainWindow.RandomNumberBetweenInsertedParameters(200, 3000);
            DaysInHarbour = 3;
            NumberOfHarbourSpots = 1;
            NumberOfHorsePowers = MainWindow.RandomNumberBetweenInsertedParameters(10, 1000);
            BoatType = "Motorbåt";
        }
        public MotorBoat(string ID, double speed, int weight, int daysSpentInHarbour, int startSpot, string specialInfo)
        {
            DaysInHarbour = 3;
            NumberOfHarbourSpots = 1;
            IDNumberOfBoat = ID;
            MaxSpeed = speed;
            Weight = weight;
            DaysSpentInHarbour = daysSpentInHarbour;
            StartingHarbourSpot = startSpot;
            NumberOfHorsePowers = int.Parse(specialInfo);
            BoatType = "Motorbåt";

        }

        public override string PrintSpecialInfo()
        {
            return $"Antal hästkrafter: {NumberOfHorsePowers}";
        }

        public override string GetSpecialInfo()
        {
            return NumberOfHorsePowers.ToString();
        }
        public override void SetSpecialInfo(string info)
        {
            NumberOfHorsePowers = int.Parse(info);
        }


    }
}

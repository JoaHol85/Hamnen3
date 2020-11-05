using System;
using System.Collections.Generic;
using System.Text;


namespace Hamnen3
{
    class Boat
    {
        public string IDNumberOfBoat { get; set; }
        public double MaxSpeed { get; set; }
        public int Weight { get; set; }
        public int DaysInHarbour { get; set; }
        public double NumberOfHarbourSpots { get; set; }
        public int DaysSpentInHarbour { get; set; } = 0;
        public int StartingHarbourSpot { get; set; }
        public string BoatType { get; set; }


        public static string SetID(string BoatIdentifier)
        {
            string name = "";
            for (int i = 0; i < 3; i++)
            {
                name += (Letter)MainWindow.RandomNumberBetweenInsertedParameters(0, 25);
            }
            return $"{BoatIdentifier}-{name}";
        }

        public virtual string PrintSpecialInfo()
        {
            return null;
        }

        public virtual string GetSpecialInfo()
        {
            return null;
        }

        public virtual void SetSpecialInfo(string info)
        {
            
        }

    }







}

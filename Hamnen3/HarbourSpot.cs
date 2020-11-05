using System;
using System.Collections.Generic;
using System.Text;

namespace Hamnen3
{
    class HarbourSpot
    {

        public int SpotNumber { get; set; }
        public Boat DockedBoat1 { get; set; }
        public Boat DockedBoat2 { get; set; }
        public bool BoatDocked { get; set; } = false;

        public HarbourSpot(int spotNr)
        {
            SpotNumber = spotNr;
        }
    
    }
}

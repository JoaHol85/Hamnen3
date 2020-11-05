using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;

namespace Hamnen3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static List<Boat> boatsInHarbour = new List<Boat>();
        static List<HarbourSpot> harbour1 = new List<HarbourSpot>();
        static List<HarbourSpot> harbour2 = new List<HarbourSpot>();
        static List<Boat> incomingBoats = new List<Boat>();
        static List<Boat> removeFromHarbour = new List<Boat>();
        static List<string> savedLines = new List<string>();
        static int totalNumberOfCargoshipsInHarbour = 0;
        static int totalNumberOfCatamaransInHarbour = 0;
        static int totalNumberOfMotorboatsInHarbour = 0;
        static int totalNumberOfSailboatsInHarbour = 0;
        static int totalNumberOfRowingboatsInHarbour = 0;
        static int numberOfShipsCreatedEachDay = 5;         // Changes the number of boats arriving to the harbour each day.

        int dissmissedShipsToday = 0;
        int dissmissedShips = 0;
        int freeSpotsInHarbour1 = 0;
        int freeSpotsInHarbour2 = 0;
        int totalWeightInHarbour = 0;
        double averageSpeedInHarbour = 0;



        public MainWindow()
        {
            InitializeComponent();

            FillHarbourSpotLists();

            if (File.Exists("SaveFile.txt"))
            {
                string[] text = File.ReadAllLines("SaveFile.txt");

                foreach (string line in text)
                {
                    string[] parts = line.Split(';');
                    if (parts[0].Substring(0, 1) == "R")
                        boatsInHarbour.Add(new RowingBoat(parts[0], double.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[5]), int.Parse(parts[6]), parts[8]));
                    if (parts[0].Substring(0, 1) == "M")
                        boatsInHarbour.Add(new MotorBoat(parts[0], double.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[5]), int.Parse(parts[6]), parts[8]));
                    if (parts[0].Substring(0, 1) == "S")
                        boatsInHarbour.Add(new SailBoat(parts[0], double.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[5]), int.Parse(parts[6]), parts[8]));
                    if (parts[0].Substring(0, 1) == "K")
                        boatsInHarbour.Add(new Catamaran(parts[0], double.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[5]), int.Parse(parts[6]), parts[8]));
                    if (parts[0].Substring(0, 1) == "L")
                        boatsInHarbour.Add(new CargoShip(parts[0], double.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[5]), int.Parse(parts[6]), parts[8]));
                }

                foreach (Boat boat in boatsInHarbour)
                {
                    int boatPosition = boat.StartingHarbourSpot;
                    if (boat is RowingBoat)
                    {
                        if (boatPosition <= 32)
                        {
                            boatPosition--;
                            if (harbour1[boatPosition].BoatDocked == true)
                            {
                                if (harbour1[boatPosition].DockedBoat1 == null && harbour1[boatPosition].DockedBoat2 != null)
                                    harbour1[boatPosition].DockedBoat1 = boat;
                                if (harbour1[boatPosition].DockedBoat1 != null && harbour1[boatPosition].DockedBoat2 == null)
                                    harbour1[boatPosition].DockedBoat2 = boat;
                            }
                            else
                            {
                                harbour1[boatPosition].DockedBoat1 = boat;
                                harbour1[boatPosition].BoatDocked = true;
                            }
                        }
                        if (boatPosition >= 33)
                        {
                            int tempBoatPosition = boatPosition - 32 - 1;
                            if (harbour2[tempBoatPosition].BoatDocked == true)
                            {
                                if (harbour2[tempBoatPosition].DockedBoat1 == null && harbour2[tempBoatPosition].DockedBoat2 != null)
                                    harbour2[tempBoatPosition].DockedBoat1 = boat;
                                if (harbour2[tempBoatPosition].DockedBoat1 != null && harbour2[tempBoatPosition].DockedBoat2 == null)
                                    harbour2[tempBoatPosition].DockedBoat2 = boat;
                            }
                            else
                            {
                                harbour2[tempBoatPosition].DockedBoat1 = boat;
                                harbour2[tempBoatPosition].BoatDocked = true;
                            }
                        }
                    }
                    else if (boat is MotorBoat)
                    {
                        if (boatPosition <= 32)
                        {
                            boatPosition--;
                            harbour1[boatPosition].BoatDocked = true;
                            harbour1[boatPosition].DockedBoat1 = boat;
                            harbour1[boatPosition].DockedBoat2 = boat;
                        }
                        if (boatPosition >= 33)
                        {
                            boatPosition--;
                            int tempBoatPosition = boatPosition - 32;
                            harbour2[tempBoatPosition].BoatDocked = true;
                            harbour2[tempBoatPosition].DockedBoat1 = boat;
                            harbour2[tempBoatPosition].DockedBoat2 = boat;
                        }
                    }
                    else
                    {
                        if (boatPosition <= 32)
                        {
                            boatPosition--;
                            for (int i = boatPosition; i < boatPosition + boat.NumberOfHarbourSpots; i++)
                            {
                                harbour1[i].BoatDocked = true;
                                harbour1[i].DockedBoat1 = boat;
                                harbour1[i].DockedBoat2 = boat;
                            }
                        }
                        if (boatPosition >= 33)
                        {
                            int tempBoatPosition = boatPosition - 32 - 1;
                            for (int i = tempBoatPosition; i < tempBoatPosition + boat.NumberOfHarbourSpots; i++)
                            {
                                harbour2[i].BoatDocked = true;
                                harbour2[i].DockedBoat1 = boat;
                                harbour2[i].DockedBoat2 = boat;
                            }
                        }
                    }
                }

                if (new FileInfo("SaveFile.txt").Length != 0)
                {
                    GetStatisticsInHarbour();
                }
            }

            PrintHarboursToListBoxes(test_Listbox1, 1, 32);
            PrintHarboursToListBoxes(test_Listbox2, 33, 64);

            WriteTxt();
        }


        private static void IncomingBoats()
        {
            incomingBoats.Clear();
            for (int i = 0; i < numberOfShipsCreatedEachDay; i++)
            {
                incomingBoats.Add(GetRandomBoat());
            }
        }


        private void GetStatisticsInHarbour()
        {
            freeSpotsInHarbour1 = CheckFreeSpotsInHarbour(harbour1);
            freeSpotsInHarbour2 = CheckFreeSpotsInHarbour(harbour2);
            totalWeightInHarbour = CalculateTotalWeightOfBoatsInHarbour();
            averageSpeedInHarbour = CalculateKnotsToKmPerHour(CalculateAverageMaxSpeedInHarbour());

            GetCountForEachBoatInHarbour();
            lbl_NumberOfCargoShips.Content = totalNumberOfCargoshipsInHarbour;
            lbl_NumberOfCatamarans.Content = totalNumberOfCatamaransInHarbour;
            lbl_NumberOfMotorboats.Content = totalNumberOfMotorboatsInHarbour;
            lbl_NumberOfRowingboats.Content = totalNumberOfRowingboatsInHarbour;
            lbl_NumberOfSailboats.Content = totalNumberOfSailboatsInHarbour;

            lbl_AverageSpeedInHarbour.Content = averageSpeedInHarbour;
            lbl_EmptySpotsHarbour1.Content = freeSpotsInHarbour1;
            lbl_EmptySpotsHarbour2.Content = freeSpotsInHarbour2;
            lbl_TotalWeightInHarbour.Content = totalWeightInHarbour;
            lbl_RejectedBoats.Content = dissmissedShips;
            lbl_RoomForRowingboat.Content = totalNumberOfRowingboatsInHarbour % 2 == 1 ? "Ja" : "Nej";

            lbl_RejectedBoatsMessage.Content = "";
            if (dissmissedShipsToday > 0)
            {
                lbl_RejectedBoatsMessage.Content = $"Båtar som ej fick plats i hamnen idag: {dissmissedShipsToday}";
            }
        }


        private static Boat GetRandomBoat()
        {
            int index = RandomNumberBetweenInsertedParameters(1, 5);
            switch (index)
            {
                case 1:
                    return new CargoShip();
                case 2:
                    return new Catamaran();
                case 3:
                    return new MotorBoat();
                case 4:
                    return new RowingBoat();
                case 5:
                    return new SailBoat();
                default:
                    return null;
            }
        }


        private static bool RowingBoatException(Boat boat, bool shipAdded, List<HarbourSpot> harbour)
        {
            if (boat is RowingBoat)
                for (int i = 0; i < harbour.Count; i++)
                {
                    if (harbour[i].DockedBoat2 == null && harbour[i].DockedBoat1 != null)
                    {
                        shipAdded = true;
                        harbour[i].BoatDocked = true;
                        harbour[i].DockedBoat2 = boat;
                    }
                    if (shipAdded)
                    {
                        boatsInHarbour.Add(boat);
                        boat.StartingHarbourSpot = harbour[i].SpotNumber;
                    }
                    if (shipAdded)
                        break;
                }
            if (!shipAdded && boat is RowingBoat)
            {
                for (int i = 0; i < harbour.Count; i++)
                {
                    if (harbour[i].BoatDocked == false)
                    {
                        shipAdded = true;
                        harbour[i].BoatDocked = true;
                        harbour[i].DockedBoat1 = boat;
                    }
                    if (shipAdded)
                    {
                        boatsInHarbour.Add(boat);
                        boat.StartingHarbourSpot = harbour[i].SpotNumber;
                    }
                    if (shipAdded)
                        break;
                }
            }

            return shipAdded;
        }


        static void WriteTxt() 
        {
            savedLines.Clear();
            foreach (Boat boat in boatsInHarbour)
            {
                savedLines.Add($"{boat.IDNumberOfBoat};{boat.MaxSpeed};{boat.Weight};{boat.DaysInHarbour};{boat.NumberOfHarbourSpots};{boat.DaysSpentInHarbour};{boat.StartingHarbourSpot};{boat.BoatType};{boat.GetSpecialInfo()}");
            }

            StreamWriter sw = new StreamWriter("SaveFile.txt");

            foreach (string line in savedLines)
            {
                sw.WriteLine(line);
            }
            sw.Close();
        }


        private static bool CheckForHolesAndAddBoatToHarbourDock(Boat boat, bool shipAdded, List<HarbourSpot> harbour)
        {
            for (int i = 0; i < harbour.Count; i++) 
            {
                if (i != 0 && shipAdded == false && harbour[i].BoatDocked == false && i + boat.NumberOfHarbourSpots <= harbour.Count() - 1 && harbour[i - 1].BoatDocked == true && harbour[i + (int)boat.NumberOfHarbourSpots].BoatDocked == true)
                {
                    for (int j = 0; j < boat.NumberOfHarbourSpots; j++)
                    {
                        if (harbour[i + j].BoatDocked == false)
                        {
                            if (j == boat.NumberOfHarbourSpots - 1)
                            {
                                for (int k = 0; k < boat.NumberOfHarbourSpots; k++)
                                {
                                    harbour[i + k].BoatDocked = true;
                                    harbour[i + k].DockedBoat1 = boat;
                                    harbour[i + k].DockedBoat2 = boat;
                                    shipAdded = true;
                                }
                                if (shipAdded)
                                {
                                    boatsInHarbour.Add(boat);
                                    boat.StartingHarbourSpot = harbour[i].SpotNumber;
                                }
                            }
                        }
                        else
                            break;
                        if (shipAdded)
                            break;
                    }
                }
                if (shipAdded)
                    break;
            }
            return shipAdded;
        }


        private static int CheckFreeSpotsInHarbour(List<HarbourSpot> harbourList)
        {
            int freeSpots = 0;
            foreach (HarbourSpot spot in harbourList)
            {
                if (spot.BoatDocked == false)
                    freeSpots++;
            }
            return freeSpots;
        }


        private static void RemoveBoatsFromBoatsInHarbourList()                
        {
            foreach (Boat boat in removeFromHarbour)
            {
                bool itemInList = removeFromHarbour
                    .Any(b => b.IDNumberOfBoat == boat.IDNumberOfBoat);
                if (itemInList)
                    boatsInHarbour.Remove(boat);
            }
            removeFromHarbour.Clear();
        }


        private static bool AddShipToNextFreeHarbourSpot(Boat boat, bool shipAdded, List<HarbourSpot> harbour)
        {
            for (int i = 0; i < harbour.Count; i++) 
            {
                if (harbour[i].BoatDocked == false && i + boat.NumberOfHarbourSpots <= harbour.Count())
                {
                    for (int j = 0; j < boat.NumberOfHarbourSpots; j++)
                    {
                        if (harbour[i + j].BoatDocked == false)
                        {
                            if (j == boat.NumberOfHarbourSpots - 1)
                            {
                                for (int k = 0; k < boat.NumberOfHarbourSpots; k++)
                                {
                                    harbour[i + k].BoatDocked = true;
                                    harbour[i + k].DockedBoat1 = boat;
                                    harbour[i + k].DockedBoat2 = boat;
                                    shipAdded = true;
                                }
                                if (shipAdded)
                                {
                                    boatsInHarbour.Add(boat);
                                    boat.StartingHarbourSpot = harbour[i].SpotNumber;
                                }
                            }
                        }
                        else
                            break;
                        if (shipAdded)
                            break;
                    }
                }
                if (shipAdded)
                    break;
            }
            return shipAdded;
        }


        private static bool CheckForBoatsLeavingHarbour(Boat boat, bool boatFound, List<HarbourSpot> harbour)
        {
            for (int i = 0; i < harbour.Count; i++)
            {
                if (harbour[i].BoatDocked == true)       
                {
                    if (boat is RowingBoat && harbour[i].DockedBoat1 != null)
                    {
                        if (boat.DaysInHarbour == harbour[i].DockedBoat1.DaysSpentInHarbour && boat.IDNumberOfBoat == harbour[i].DockedBoat1.IDNumberOfBoat)
                        {
                            harbour[i].DockedBoat1 = null;
                            boatFound = true;

                            bool itemInList = removeFromHarbour
                                .Any(b => b.IDNumberOfBoat == boat.IDNumberOfBoat);
                            if (!itemInList)
                                removeFromHarbour.Add(boat);
                        }
                    }
                    else if (boat is RowingBoat && harbour[i].DockedBoat2 != null)
                    {
                        if (boat.DaysInHarbour == harbour[i].DockedBoat2.DaysSpentInHarbour && boat.IDNumberOfBoat == harbour[i].DockedBoat2.IDNumberOfBoat)
                        {
                            harbour[i].DockedBoat2 = null;
                            boatFound = true;

                            bool itemInList = removeFromHarbour
                                .Any(b => b.IDNumberOfBoat == boat.IDNumberOfBoat);
                            if (!itemInList)
                                removeFromHarbour.Add(boat);
                        }
                    }
                    else if (boat.DaysInHarbour <= harbour[i].DockedBoat1.DaysSpentInHarbour && boat.IDNumberOfBoat == harbour[i].DockedBoat1.IDNumberOfBoat)
                    {
                        harbour[i].DockedBoat1 = null;
                        harbour[i].DockedBoat2 = null;
                        boatFound = true;

                        bool itemInList = removeFromHarbour
                            .Any(b => b.IDNumberOfBoat == boat.IDNumberOfBoat);
                        if (!itemInList)
                            removeFromHarbour.Add(boat);
                    }

                    if (harbour[i].DockedBoat1 == null && harbour[i].DockedBoat2 == null)
                        harbour[i].BoatDocked = false;
                }
            }
            return boatFound;
        }


        private static void FillHarbourSpotLists()
        {
            int i = 1;
            for (i = 1; i < 33; i++)
            {
                harbour1.Add(new HarbourSpot(i));
            }
            for (; i < 65; i++)
            {
                harbour2.Add(new HarbourSpot(i));
            }
        }


        private static void GetCountForEachBoatInHarbour() 
        {
            totalNumberOfCargoshipsInHarbour = 0;
            totalNumberOfCatamaransInHarbour = 0;
            totalNumberOfMotorboatsInHarbour = 0;
            totalNumberOfRowingboatsInHarbour = 0;
            totalNumberOfSailboatsInHarbour = 0;

            var q = boatsInHarbour
                .GroupBy(b => b.IDNumberOfBoat.Substring(0, 1));

            foreach (var group in q)
            {
                if (group.Key == "L")
                    totalNumberOfCargoshipsInHarbour = group.Count();
                else if (group.Key == "S")
                    totalNumberOfSailboatsInHarbour = group.Count();
                else if (group.Key == "K")
                    totalNumberOfCatamaransInHarbour = group.Count();
                else if (group.Key == "R")
                    totalNumberOfRowingboatsInHarbour = group.Count();
                else if (group.Key == "M")
                    totalNumberOfMotorboatsInHarbour = group.Count();
            }
        }


        public static double CalculateKnotsToKmPerHour(double knots)
        {
            return Math.Round((knots * 1.852), 1);
        }


        static double CalculateAverageMaxSpeedInHarbour()
        {
            var q = boatsInHarbour
                .Average(b => b.MaxSpeed);
            return q;
        }


        static int CalculateTotalWeightOfBoatsInHarbour()
        {
            var q = boatsInHarbour
                .Sum(b => b.Weight);
            return q;
        }


        public static int RandomNumberBetweenInsertedParameters(int lowestNumber, int HighestNumber)
        {
            Random random = new Random();
            return random.Next(lowestNumber, HighestNumber + 1);
        }


        public static double CalculateLengthOfBoatMeterFromFeet(int lengthInFeet)
        {
            double lengthInMeters = lengthInFeet * 0.3048;
            return Math.Round(lengthInMeters, 1);
        }


        private static void AddOneDayToBoatsInHarbour()
        {
            foreach (Boat boat in boatsInHarbour)
            {
                boat.DaysSpentInHarbour++;
            }
        }


        private void btn_NextDay_Click(object sender, RoutedEventArgs e)
        {
            AddOneDayToBoatsInHarbour();

            foreach (Boat boat in boatsInHarbour)
            {
                if (boat.DaysSpentInHarbour >= boat.DaysInHarbour)
                {
                    bool boatFound = false;
                    boatFound = CheckForBoatsLeavingHarbour(boat, boatFound, harbour1);
                    if (!boatFound)
                    {
                        CheckForBoatsLeavingHarbour(boat, boatFound, harbour2);
                    }
                }
            }

            RemoveBoatsFromBoatsInHarbourList();

            IncomingBoats();

            TryToAddBoatToHarbour();

            GetStatisticsInHarbour();

            PrintHarboursToListBoxes(test_Listbox1, 1, 32);
            PrintHarboursToListBoxes(test_Listbox2, 33, 64);

            WriteTxt();
        }

        private void TryToAddBoatToHarbour()
        {
            dissmissedShipsToday = 0;
            foreach (Boat boat in incomingBoats)
            {
                bool shipAdded = false;
                shipAdded = RowingBoatException(boat, shipAdded, harbour1);
                if (!shipAdded)
                {
                    shipAdded = RowingBoatException(boat, shipAdded, harbour2);
                }
                if (!shipAdded)
                {
                    shipAdded = CheckForHolesAndAddBoatToHarbourDock(boat, shipAdded, harbour1);
                }
                if (!shipAdded)
                {
                    shipAdded = CheckForHolesAndAddBoatToHarbourDock(boat, shipAdded, harbour2);
                }
                if (!shipAdded)
                {
                    shipAdded = AddShipToNextFreeHarbourSpot(boat, shipAdded, harbour1);
                }
                if (!shipAdded)
                {
                    shipAdded = AddShipToNextFreeHarbourSpot(boat, shipAdded, harbour2);
                }
                if (!shipAdded)
                {
                    dissmissedShips++;
                    dissmissedShipsToday++;
                }
            }
        }

        private void PrintHarboursToListBoxes(ListBox listBox, int startIndex, int endIndex)
        {
            int foundBoatsLength = 0;
            bool bigShipFound;
            bool smallShipFound;
            listBox.Items.Clear();
            listBox.Items.Add("Plats:\tBåttyp:\t\tID nr:\tVikt(kg)\tMax km/h:\tÖvrigt:");
            for (int i = startIndex; i < endIndex + 1; i++)
            {
                foundBoatsLength = 0;
                bigShipFound = false;
                smallShipFound = false;
                foreach (Boat boat in boatsInHarbour)
                {
                    if (boat.StartingHarbourSpot == i)
                    {
                        if (boat.NumberOfHarbourSpots > 1)
                        {
                            listBox.Items.Add($"{boat.StartingHarbourSpot}-{boat.StartingHarbourSpot + boat.NumberOfHarbourSpots - 1}\t{boat.BoatType}\t{boat.IDNumberOfBoat}\t {boat.Weight}\t     {CalculateKnotsToKmPerHour(boat.MaxSpeed)}\t\t{boat.PrintSpecialInfo()}");
                            foundBoatsLength = (int)boat.NumberOfHarbourSpots - 1;
                            bigShipFound = true;
                        }
                        else if (boat.NumberOfHarbourSpots < 2)
                        {
                            listBox.Items.Add($"{boat.StartingHarbourSpot}\t{boat.BoatType}\t{boat.IDNumberOfBoat}\t {boat.Weight}\t     {CalculateKnotsToKmPerHour(boat.MaxSpeed)}\t\t{boat.PrintSpecialInfo()}");
                            smallShipFound = true;
                        }
                    }
                    if (bigShipFound)
                    {
                        i += foundBoatsLength;
                        break;
                    }
                }
                if (!bigShipFound && !smallShipFound)
                    listBox.Items.Add($"{i}\tTom plats");
            }
        }

    }
}



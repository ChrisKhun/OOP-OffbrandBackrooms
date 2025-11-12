using System;
using System.Collections.Generic;
using System.Linq;

namespace OffBrandBackrooms
{
    public class RoomInfo
    {
        
        public List<string> defaultRoom { get; set; } 
        public List<string> abandonedUtilityHalls { get; set; } 
        public List<string> infiniteApartments { get; set; } 
        public List<string> poolRoom { get; set; } 
        public List<string> ikeaRoom { get; set; } 
        public List<string> arcadeRoom { get; set; } 
        public List<string> snackRoom { get; set; } 
        public List<string> carPark { get; set; } 
        public List<string> bosses { get; set; }
        public List<string> shops { get; set; }
        public List<string> roomNames { get; set; }
        public List<string> bossroomNames { get; set; }
        public List<string> stageNames { get; set; }
        

        public RoomInfo()
        {
            defaultRoom = new List<string> 
            { "DefaultRoom1", "DefaultRoom2", "DefaultRoom3", "DefaultRoom4",
            "DefaultRoom5", "DefaultRoom6", "DefaultRoom7", "DefaultRoom8",
            "DefaultRoom9", "DefaultRoom10", "DefaultRoom11", "DefaultRoom12",
            "DefaultRoom13", "DefaultRoom14", "DefaultBossRoom", "DefaultShop"
            };
            
            abandonedUtilityHalls = new List<string>
            {
                "AbandonedUtilityHalls1", "AbandonedUtilityHalls2", "AbandonedUtilityHalls3", "AbandonedUtilityHalls4",
                "AbandonedUtilityHalls5", "AbandonedUtilityHalls6", "AbandonedUtilityHalls7", "AbandonedUtilityHalls8",
                "AbandonedUtilityHalls9", "AbandonedUtilityHalls10", "AbandonedUtilityHalls11", "AbandonedUtilityHalls12",
                "AbandonedUtilityHalls13", "AbandonedUtilityHalls14", "AbandonedUtilityHallsBossRoom", "AbandonedUtilityHallsShop"
            };

            infiniteApartments = new List<string>
            {
                "InfiniteApartments1", "InfiniteApartments2", "InfiniteApartments3", "InfiniteApartments4",
                "InfiniteApartments5", "InfiniteApartments6", "InfiniteApartments7", "InfiniteApartments8",
                "InfiniteApartments9", "InfiniteApartments10", "InfiniteApartments11", "InfiniteApartments12",
                "InfiniteApartments13", "InfiniteApartments14", "InfiniteApartmentsBossRoom", "InfiniteApartmentsShop"
            };

            poolRoom = new List<string>
            {
                "Pool1", "Pool2", "Pool3", "Pool4",
                "Pool5", "Pool6", "Pool7", "Pool8",
                "Pool9", "Pool10", "Pool11", "Pool12",
                "Pool13", "Pool14", "PoolBossRoom", "PoolShop"
            };

            ikeaRoom = new List<string>
            {
                "Ikea1", "Ikea2", "Ikea3", "Ikea4",
                "Ikea5", "Ikea6", "Ikea7", "Ikea8",
                "Ikea9", "Ikea10", "Ikea11", "Ikea12",
                "Ikea13", "Ikea14", "IkeaBossRoom", "IkeaShop"
            };

            arcadeRoom = new List<string>
            {
                "Arcade1", "Arcade2", "Arcade3", "Arcade4",
                "Arcade5", "Arcade6", "Arcade7", "Arcade8",
                "Arcade9", "Arcade10", "Arcade11", "Arcade12",
                "Arcade13", "Arcade14", "ArcadeBossRoom", "ArcadeShop"
            };

            snackRoom = new List<string>
            {
                "SnackRoom1", "SnackRoom2", "SnackRoom3", "SnackRoom4",
                "SnackRoom5", "SnackRoom6", "SnackRoom7", "SnackRoom8",
                "SnackRoom9", "SnackRoom10", "SnackRoom11", "SnackRoom12",
                "SnackRoom13", "SnackRoom14", "SnackBossRoom", "SnackShop"
            };

            carPark = new List<string>
            {
                "CarPark1", "CarPark2", "CarPark3", "CarPark4",
                "CarPark5", "CarPark6", "CarPark7", "CarPark8",
                "CarPark9", "CarPark10", "CarPark11", "CarPark12",
                "CarPark13", "CarPark14", "CarParkBossRoom", "CarParkShop"
            };

            bosses = new List<string>
            {
                "Default Boss", "Abandoned Utility Halls Boss", "Infinite Apartments Boss", "Pool Boss", "Ikea Boss", "Arcade Boss", "Snack Boss",
                "Car Park Boss"
            };

            shops = new List<string>
            {
                "DefaultShop", "AbandonedUtilityHallsShop", "InfiniteApartmentsShop", "PoolShop", "IkeaShop", "ArcadeShop", "SnackShop", "CarParkShop"
            };

            roomNames = new List<string>
            { 
                "ignore", "DefaultRoom", "AbandonedUtilityHalls", "InfiniteApartments", "Pool", "Ikea", "Arcade", "SnackRoom", "CarPark" 
            };

            bossroomNames = new List<string>
            { 
                "DefaultBossRoom", "AbandonedUtilityHallsBossRoom", "InfiniteApartmentsBossRoom", "PoolBossRoom", "IkeaBossRoom", "ArcadeBossRoom", "SnackBossRoom", "CarParkBossRoom" 
            };

            stageNames = new List<string>
            {
                "Stage 1", "Stage 2", "Stage 3", "Stage 4", "Stage 5", "Stage 6", "Stage 7", "Stage 8"  
            };
        }
    }
}
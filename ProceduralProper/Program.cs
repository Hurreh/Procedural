using System;
using System.Collections.Generic;
using ProceduralProper.Generators;
namespace ProceduralProper
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            Dungeon dungeon = new Dungeon(100, 100, 20, 6, 6, 30, 6);
            RandomRoomsGenerator randomRoomsGenerator = new RandomRoomsGenerator();
            DrunkardWalk drunkardWalk = new DrunkardWalk();
            List<Room> rooms = new List<Room>();
            DLA dla = new DLA();
            BSP bsp = new BSP();
            List<char[,]> dungeons = randomRoomsGenerator.RandomGenerator(dungeon);
            //Room dungeons = dla.DLAProper(dungeon,5000);
            //rooms.Add(dungeons);
            List<Room> random = Utilities.RandomRoomPlacer(dungeons, dungeon);
            Utilities.ArrayPrinter(Utilities.RoomsPrinter(random, dungeon));
            //Utilities.ArrayPrinter(rooms);
            foreach (var item in random)
            {
                Console.WriteLine(item + " " + i);
                foreach (var tile in item.Tiles)
                {
                    Console.WriteLine(tile.TileType + " " + "X:" + tile.X + " " + "Y:" + tile.Y);
                    Console.WriteLine("Riddle: " + tile.HasRiddle + " Special wall: "+ tile.IsSpecialWall + " Door: " + tile.IsDoor);
                }
                i++;
            }
            Console.ReadKey();
            
        }
    }
}

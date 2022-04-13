using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralProper.Generators
{
    public class RandomRoomsGenerator
    {
        Random random = new Random();
        public List<char[,]> RandomGenerator(Dungeon dungeon)
        {
            List<char[,]> dungeons = new List<char[,]>();
            int numberOfRooms = CalculateNumberOfRooms(dungeon);

            //For each room we want to generate
            for (int i = 0; i < numberOfRooms; i++)
            {
                int roomSizeX = 0;
                int roomSizeY = 0;

                roomSizeX = random.Next(dungeon.minimalRoomSize, dungeon.maxRoomSizeX);
                roomSizeY = random.Next(dungeon.minimalRoomSize, dungeon.maxRoomSizeY);

                char[,] room = new char[roomSizeX, roomSizeY];

                dungeons.Add(room);
            }
            return dungeons;
        }
        public int CalculateNumberOfRooms(Dungeon dungeon)
        {
            int numberOfRooms;
            int dungeonSize = dungeon.DungeonSizeX * dungeon.DungeonSizeY;
            float averageRoomSize = (dungeon.maxRoomSizeX / 2) * (dungeon.maxRoomSizeY / 2);
            float howManyToFill = dungeonSize / averageRoomSize;
            numberOfRooms = (int)MathF.Ceiling(howManyToFill / ((1f / dungeon.RoomDensity) * 1000f));
            return numberOfRooms;
        }
    }
}

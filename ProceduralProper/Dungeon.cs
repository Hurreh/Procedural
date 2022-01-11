using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralProper
{
    public class Dungeon
    {
        public int DungeonSizeX { get; set; }
        public int DungeonSizeY { get; set; }
        public int RoomDensity { get; set; }
        public int maxRoomSizeX { get; set; }
        public int maxRoomSizeY { get; set; }
        //size bias determines whether a lot of smaller rooms or fewer bigger rooms will be generated.
        public int sizeBias { get; set; }
        public int minimalRoomSize { get; set; }

        public Dungeon(int dungeonSizeX, int dungeonSizeY, int roomDensity, int maxRoomSizeX, int maxRoomSizeY, int sizeBias, int minimalRoomSize)
        {
            this.DungeonSizeX = dungeonSizeX;
            this.DungeonSizeY = dungeonSizeY;

            this.maxRoomSizeX = maxRoomSizeX;
            this.maxRoomSizeY = maxRoomSizeY;
            this.sizeBias = sizeBias;
            this.RoomDensity = roomDensity;
            this.minimalRoomSize = minimalRoomSize;
        }
    }
}

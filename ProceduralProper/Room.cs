using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralProper
{
    public class Room
    {
        public List<Tile> Tiles { get; set; }
        public bool HasDoor { get; set; }
        public int RiddleCount { get; set; }
        public bool Solved { get; set; }
        public Tile RoomCenter { get; set; }
        public bool IsCorridor { get; set; }

        public Room()
        {
            Tiles = new List<Tile>();
            RoomCenter = new Tile(0,0);
            IsCorridor = false;
        }
        
    }
}

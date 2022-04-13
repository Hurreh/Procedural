using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProceduralProper
{
    public class Utilities
    {
        Random random = new Random();
        public static char[,] PopulateArray(char[,] map)
        {
            char[,] dungeonMap = map;
            for (int x = 0; x < dungeonMap.GetLength(0); x++)
            {
                for (int y = 0; y < dungeonMap.GetLength(1); y++)
                {
                    dungeonMap[x, y] = '\u2588';
                }
            }

            return dungeonMap;
        }
        public static void ArrayPrinter(char[,] array)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    Console.Write(array[y, x]);
                }
                Console.Write("\n");
            }
        }
        public static char[,] RoomsPrinter(List<Room> rooms, Dungeon dungeon)
        {
            char[,] map = new char[dungeon.DungeonSizeX, dungeon.DungeonSizeY];
            map = PopulateArray(map);
            //foreach room 
            for (int room = 0; room < rooms.Count(); room++)
            {
                for (int tile = 0; tile < rooms[room].Tiles.Count(); tile++)
                {
                    map[rooms[room].Tiles[tile].X, rooms[room].Tiles[tile].Y] = '\u2800';
                }
            }
            return map;
        }
        public static char[,] Thickener(char[,] map, int amount)
        {

            char[,] thickened = new char[map.GetLength(0), map.GetLength(1)];
            thickened = PopulateArray(thickened);
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == '\u2800')
                    {
                        for (int r = -amount; r <= amount; r++)
                        {
                            for (int c = -amount; c <= amount; c++)
                            {
                                if (!(x + c < 0 || x + c > map.GetLength(0) - 1 || y + r < 0 || y + r > map.GetLength(1) - 1)) //Check if it overflows
                                {
                                    thickened[x + c, y + r] = '\u2800';
                                    continue;

                                }
                                else
                                {
                                    continue;
                                }

                            }
                        }
                    }
                }
            }
            return thickened;

        }
        public static List<Room> RandomRoomPlacer(List<char[,]> dungeons, Dungeon dungeon)
        {
            Random random = new Random();
            char[,] map = new char[dungeon.DungeonSizeX, dungeon.DungeonSizeY];
            List<List<(int x, int y)>> rooms = new List<List<(int x, int y)>>();
            List<Room> roomsSorted = new List<Room>();
            List<List<(int x, int y)>> fullRooms = new List<List<(int x, int y)>>();
            map = Utilities.PopulateArray(map);

            for (int i = 0; i < dungeons.Count; i++)
            {
                int startingCoordX = 0;
                int startingCoordY = 0;
                //Coords can't begin in a place that would cause one of the sides of room to flow outside the boundaries.
                startingCoordX = random.Next(1, dungeon.DungeonSizeX - dungeon.maxRoomSizeX - 1);
                startingCoordY = random.Next(1, dungeon.DungeonSizeY - dungeon.maxRoomSizeY - 1);
                rooms.Add(new List<(int x, int y)>());
                fullRooms.Add(new List<(int x, int y)>());
                for (int x = 0; x < dungeon.DungeonSizeX; x++)
                {
                    for (int y = 0; y < dungeon.DungeonSizeY; y++)
                    {
                        if ((x >= startingCoordX && y >= startingCoordY) && (x <= startingCoordX + dungeons[i].GetLength(0) && y <= startingCoordY + dungeons[i].GetLength(1)))
                        {
                            map[x, y] = '\u2800';
                            fullRooms[i].Add((x, y));
                            //If point is on some corner of rectangle add it.
                            if ((x == startingCoordX || x == startingCoordX + dungeons[i].GetLength(0) - 1) && (y == startingCoordY || y == startingCoordY + dungeons[i].GetLength(1) - 1))
                            {
                                rooms[i].Add((x, y));
                            }
                        }
                    }
                }
            }
            roomsSorted = listRooms(rooms, fullRooms);
            //roomsSorted = addCorridors(roomsSorted);
            for (int i = 0; i < roomsSorted.Count(); i++)
            {
                roomsSorted[i] = DefineWalls(roomsSorted[i]);
                //roomsSorted[i] = DefineObjects(roomsSorted[i], 0 ,0 ,0);
            }

            return roomsSorted;
        }

        private static List<Room> addCorridors(List<Room> roomsSorted)
        {    
            List<Room> usedRoom = new List<Room>(); 
            List<Room> corridors = new List<Room>();
            Room currentRoom = roomsSorted[0];

            for (int i = 0; i < roomsSorted.Count(); i++)
            {
                Dictionary<Room, int> distances = new Dictionary<Room, int>();
                //calculate distance for how many rooms are there left to visit
                for (int z = 0; z < roomsSorted.Count() - usedRoom.Count(); z++)
                {
                    //can't calculate distance to itself or to already visited room
                    if (roomsSorted[z] != roomsSorted[i] & !usedRoom.Contains(roomsSorted[z]))
                    {
                        distances.Add(roomsSorted[z], CalculateDistance(roomsSorted[i], roomsSorted[z]));
                    }
                }             
                distances = distances.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                if (distances.Count() == 0)
                {
                    break;
                }

                int distX = currentRoom.RoomCenter.X - distances.Keys.First().RoomCenter.X;
                int distY = currentRoom.RoomCenter.Y - distances.Keys.First().RoomCenter.Y;

                Room corridor = new Room();
                corridor.IsCorridor = true;
                for (int x = 0; x < MathF.Abs(distX); x++)
                {
                    if (distX < 0)
                    {
                        corridor.Tiles.Add(new Tile(currentRoom.RoomCenter.X + x, currentRoom.RoomCenter.Y));
                    }
                    else
                    {
                        corridor.Tiles.Add(new Tile(currentRoom.RoomCenter.X - x, currentRoom.RoomCenter.Y));
                    }
                }
                for (int y = 0; y < MathF.Abs(distY); y++)
                {
                    if (distY < 0)
                    {
                        corridor.Tiles.Add(new Tile(distances.Keys.First().RoomCenter.X, currentRoom.RoomCenter.Y + y));
                    }
                    else
                    {
                        corridor.Tiles.Add(new Tile(distances.Keys.First().RoomCenter.X, currentRoom.RoomCenter.Y - y));
                    }
                }
                corridors.Add(corridor);
                usedRoom.Add(currentRoom);
                currentRoom = distances.Keys.First();
            }
            roomsSorted.AddRange(corridors);
            return roomsSorted;
        }
        public static int CalculateDistance(Room roomA, Room roomB)
        {
            int distance = 0;
            int distX = Math.Abs(roomA.RoomCenter.X - roomB.RoomCenter.X);
            int distY = Math.Abs(roomA.RoomCenter.Y - roomB.RoomCenter.Y);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            return 14 * distX + 10 * (distY - distX);
        }

        private static Room DefineObjects(Room room, int doorCount, int riddleCount, int specialTilesCount)
        {
            Random random = new Random();
            //Define doors
            foreach (var tile in room.Tiles)
            { 
                double prop = random.NextDouble();
                if (tile.TileType == TileType.L || tile.TileType == TileType.R || tile.TileType == TileType.TOP || tile.TileType == TileType.BOT)
                {
                   
                    if (doorCount != 2)
                    {
                        
                        //if this side already has doors skip
                        if (room.Tiles.Where(x => x.TileType == tile.TileType && x.IsDoor == true).Any())
                        {
                            goto SpecialCheck;
                        }
                        else
                        {

                            if (prop >= 0.9)
                            {
                                tile.IsDoor = true;
                                doorCount++;

                                if (doorCount == 2)
                                    continue;

                            }
                        }
                    }
                    SpecialCheck:
                    //special tile (window etc.) (30% chance)
                    if (tile.IsDoor == false && prop < 0.9 && prop >= 0.6)
                    {
                        tile.IsSpecialWall = true;
                        specialTilesCount++;
                    }
                }
                if (tile.TileType == TileType.MID)
                {
                    if (prop >= 0.7)
                    {
                        tile.HasRiddle = true;
                        riddleCount++;
                    }
                }


            }
            if (doorCount != 2 || riddleCount == 0)
            {
                DefineObjects(room, doorCount, riddleCount, specialTilesCount);
            }
            return room;
        }

        public static List<Room> listRooms(List<List<(int x, int y)>> rooms, List<List<(int x, int y)>> fullRooms)
        {
            //foreach room and...

            for (int room = 0; room < rooms.Count(); room++)
            {
                //...each it's vertice...
                for (int vertice = 0; vertice < rooms[room].Count(); vertice++)
                {

                    //we have to check each room
                    for (int nestedRoom = 0; nestedRoom < rooms.Count(); nestedRoom++)
                    {
                        //but we cannot compare a room to itself
                        if (room != nestedRoom)
                        {
                            int i = 0;
                            //universal logic for all configurations. We are checking if current room is in any rectangle inside the bigger shape (if it's a bigger shape if not then it's just rectangle inside the rectangle).
                            for (int nestedVertice = 0; nestedVertice < rooms[nestedRoom].Count() / 4; nestedVertice++)
                            {
                                if (rooms[room][vertice].x >= rooms[nestedRoom][i * 4].x && rooms[room][vertice].y >= rooms[nestedRoom][i * 4].y
                                    && rooms[room][vertice].x <= rooms[nestedRoom][3 + i * 4].x && rooms[room][vertice].y <= rooms[nestedRoom][3 + i * 4].y)
                                {
                                    //rooms are being used for calculating merge whereas fullRooms are actual rooms.
                                    List<(int x, int y)> mergedRooms = new List<(int x, int y)>(rooms[room].Concat(rooms[nestedRoom]));
                                    List<(int x, int y)> mergedFullRooms = new List<(int x, int y)>(fullRooms[room].Concat(fullRooms[nestedRoom]));
                                    rooms.Add(mergedRooms); fullRooms.Add(mergedFullRooms);
                                    rooms.RemoveAt(room); fullRooms.RemoveAt(room);
                                    if (room < nestedRoom)
                                        nestedRoom--;
                                    rooms.RemoveAt(nestedRoom); fullRooms.RemoveAt(nestedRoom);
                                    goto LoopStart;
                                }
                                i++;
                            }
                        }
                    }
                }
            LoopStart:
                continue;
            }
            List<Room> listOfRooms = new List<Room>();
            foreach (var singleRoom in fullRooms)
            {
                int maxX = singleRoom.Max(room => room.x) + 1; 
                int maxY = singleRoom.Max(room => room.y) + 1;
                char[,] drawMap = new char[maxX, maxY];
                Room room = new Room();
                foreach (var tile in singleRoom)
                {
                    //dodać tworzenie na nowo mapy.
                    drawMap[tile.x, tile.y] = '.';

                    room.Tiles.Add(new Tile(tile.x, tile.y));
                }
                room.drawMap = drawMap;
                listOfRooms.Add(room);
            }
            for (int i = 0; i < rooms.Count(); i++)
            {
                Tile centerTile = new Tile(0, 0);
                int distanceToCenterX = (int)MathF.Round((rooms[i][2].x - rooms[i][0].x) / 2);
                int distanceToCenterY = (int)MathF.Round((rooms[i][1].y - rooms[i][0].y) / 2);
                centerTile.X = (rooms[i][2].x - distanceToCenterX);
                centerTile.Y = (rooms[i][1].y - distanceToCenterY);
                listOfRooms[i].RoomCenter = centerTile;
            }
            return listOfRooms;
        }

        public static Room DefineWalls(Room room)
        {
            foreach (var tile in room.Tiles) //FROM MOST COMMON TO LEAST COMMON
            {
                //MID 
                if (room.Tiles.Exists(x => x.X == tile.X + 1 && x.Y == tile.Y)
                    && room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                    && room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                    && room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y))
                {
                    tile.TileType = TileType.MID;
                    continue;
                }

                //LEFT
                if (room.Tiles.Exists(x => (x.X == tile.X + 1 && x.Y == tile.Y)
                               && room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                               && room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y)))
                {
                    tile.TileType = TileType.L;
                    continue;
                }
                //LEFT DOWN
                if (room.Tiles.Exists(x => x.X == tile.X + 1 && x.Y == tile.Y)
                               && !room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                               && room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                               && !room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y))
                {
                    tile.TileType = TileType.LD;
                    continue;
                }
                //Blocked left and right
                if (!room.Tiles.Exists(x => x.X == tile.X + 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y))
                {
                    tile.TileType = TileType.LR;
                    continue;
                }
                //Blocked down, left and right
                if (!room.Tiles.Exists(x => x.X == tile.X + 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y))
                {
                    tile.TileType = TileType.DLR;
                    continue;

                }
                //LEFT UPPER
                if (room.Tiles.Exists(x => (x.X == tile.X + 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y)))
                {
                    tile.TileType = TileType.LU;
                    continue;
                }

                //Blocked up, down and left
                if (room.Tiles.Exists(x => (x.X == tile.X + 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y)))
                {
                    tile.TileType = TileType.UDL;
                    continue;
                }
                //Blocked up, left and right
                if (!room.Tiles.Exists(x => x.X == tile.X + 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y))
                {
                    tile.TileType = TileType.ULR;
                    continue;
                }

                //TOP
                if (room.Tiles.Exists(x => (x.X == tile.X - 1 && x.Y == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X + 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y)))
                {
                    tile.TileType = TileType.TOP;
                    continue;
                }
                //RIGHT UPPER
                if (!room.Tiles.Exists(x => x.X == tile.X + 1 && x.Y == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y))
                {
                    tile.TileType = TileType.RU;
                    continue;
                }
                //Blocked up and down
                if (room.Tiles.Exists(x => (x.X == tile.X + 1 && x.Y == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y)))
                {
                    tile.TileType = TileType.UD;
                    continue;
                }
                //Blocked up, down and right
                if (!room.Tiles.Exists(x => x.X == tile.X + 1 && x.Y == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y))
                {
                    tile.TileType = TileType.UDR;
                    continue;
                }

                //BOT
                if (room.Tiles.Exists(x => (x.X == tile.X - 1 && x.Y == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X + 1 && x.Y == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)))
                {
                    tile.TileType = TileType.BOT;
                    continue;
                }
                //Right
                if (room.Tiles.Exists(x => (x.X == tile.X - 1 && x.Y == tile.Y)
                               && room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                               && room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y)))
                {
                    tile.TileType = TileType.R;
                    continue;
                }
                //RIGHT DOWN
                if (!room.Tiles.Exists(x => x.X == tile.X + 1 && x.Y == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X - 1 && x.Y == tile.Y)
                                       && room.Tiles.Exists(x => x.X == tile.X && x.Y + 1 == tile.Y)
                                       && !room.Tiles.Exists(x => x.X == tile.X && x.Y - 1 == tile.Y))
                {
                    tile.TileType = TileType.RD;
                    continue;
                }

            }
            return room;
        }
    }
}

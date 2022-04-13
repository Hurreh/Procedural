using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralProper.Generators
{
    public class BSP
    {

        Random random = new Random();
        public List<char[,]> BSPGenerator(Dungeon dungeon, int iterations)
        {
            List<char[,]> rooms = new List<char[,]>();
            char[,] dungeonMap = new char[(int)MathF.Ceiling(dungeon.DungeonSizeX / 3), (int)MathF.Ceiling(dungeon.DungeonSizeY / 3)];
            dungeonMap = Utilities.PopulateArray(dungeonMap);
            (char[,], char[,]) CutMap = CutInHalf(dungeonMap, false);

            char[,] BSPRight = CutMap.Item1;
            char[,] BSPLeft = CutMap.Item2;

            rooms.AddRange(Cut(BSPLeft, iterations, dungeon.minimalRoomSize));
            rooms.AddRange(Cut(BSPRight, iterations, dungeon.minimalRoomSize));

            return rooms;
        }
        public (char[,], char[,]) CutInHalf(char[,] map, bool axis)
        {
            //Add check if proportions aren't too botched (too wide or too tall)
            double coinFlip = random.NextDouble();
            char[,] newMap;
            //cut Horizontally
            //true - cut horizontally, false - cut vertically
            if (axis)
            {
                double leftRight = random.NextDouble();
                //y stays the same
                int y = map.GetLength(1);
                //take a half of map
                int halfOfMap = (int)MathF.Ceiling(map.GetLength(0) / 2);
                //cut it more either left or right 
                int tenPercent = leftRight >= 0.5 ? halfOfMap - random.Next((int)MathF.Ceiling(halfOfMap / 7)) : halfOfMap + random.Next((int)MathF.Ceiling(halfOfMap / 7));
                newMap = new char[tenPercent, y];
                map = new char[map.GetLength(0) - newMap.GetLength(0), map.GetLength(1)];
                return (newMap, map);
            }
            //cut vertically
            else
            {
                double upDown = random.NextDouble();
                //x stays the same
                int x = map.GetLength(0);
                //take a half of map
                int halfOfMap = (int)MathF.Ceiling(map.GetLength(1) / 2);
                //cut it more either up or down 
                int tenPercent = upDown >= 0.5 ? halfOfMap - random.Next((int)MathF.Ceiling(halfOfMap / 10)) : halfOfMap + random.Next((int)MathF.Ceiling(halfOfMap / 10));
                newMap = new char[x, tenPercent];
                map = new char[map.GetLength(0), map.GetLength(1) - newMap.GetLength(1)];
                return (newMap, map);
            }

        }
        public List<char[,]> Cut(char[,] map, int iterations, int minimalDungeonSize)
        {
            map = Utilities.PopulateArray(map);
            List<char[,]> dungeons = new List<char[,]>();
            dungeons.Add(map);
            //Check if the left list has any rooms big enough to cut
            bool allCut = !dungeons.Exists(x => x.GetLength(0) > minimalDungeonSize || x.GetLength(1) > minimalDungeonSize);
            int z = 0;
            bool cut = false;
            for (int i = 0; i < iterations; i++)
            {
                cut = !cut;
                bool itemBigEnough = dungeons[z].GetLength(0) > minimalDungeonSize && dungeons[z].GetLength(1) > minimalDungeonSize;
                if (itemBigEnough)
                {   //chop
                    (char[,], char[,]) cutMap = CutInHalf(dungeons[z], cut);
                    //add
                    dungeons[i] = cutMap.Item2;
                    dungeons.Add(cutMap.Item1);
                    z++;
                }
            }

            return dungeons;
        }
    }
}

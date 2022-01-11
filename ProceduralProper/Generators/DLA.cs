using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProceduralProper.Generators
{
    public class DLA
    {
        Random random = new Random();
        public Room ThinDLA(Dungeon dungeon, int iterations, bool thicc)
        {
            Room room = new Room();
            double diceRoll = 0;
            int randomArrayPlace = 0;
            char[,] map = new char[dungeon.DungeonSizeX, dungeon.DungeonSizeY];
            map = Utilities.PopulateArray(map);

            int startingPointY = (int)Math.Ceiling(dungeon.DungeonSizeY / 2f);
            int startingPointX = (int)Math.Ceiling(dungeon.DungeonSizeX / 2f);

            (int x, int y) firstStep = (startingPointX, startingPointY);
            List<(int x, int y)> takenSpaces = new List<(int x, int y)>();
            takenSpaces.Add(firstStep);
            map[startingPointX, startingPointY] = '\u2800';
            room.Tiles.Add(new Tile(startingPointX, startingPointY));
            //Variant A -- wrong
            for (int i = 0; i < iterations; i++)
            {
                (int x, int y) particle = PlacePartile(dungeon);
                while (true)
                {
                    diceRoll = random.NextDouble();
                    if (diceRoll >= 0.75f)
                        if (particle.y + 1 > dungeon.DungeonSizeY - 1)
                            break;
                        else
                        {
                            particle.y++;
                            if (HasNeighbour(particle, map, thicc))
                            {
                                map[particle.x, particle.y] = '\u2800';
                                room.Tiles.Add(new Tile(particle.x, particle.y));
                                break;
                            }
                            continue;
                        }
                    //move down
                    if (diceRoll < 0.75f && diceRoll >= 0.5f)
                        if (particle.y - 1 < 0)
                            break;
                        else
                        {
                            particle.y--;
                            if (HasNeighbour(particle, map, thicc))
                            {
                                map[particle.x, particle.y] = '\u2800';
                                room.Tiles.Add(new Tile(particle.x, particle.y));
                                break;
                            }
                            continue;
                        }
                    //move left
                    if (diceRoll < 0.5f && diceRoll >= 0.25f)
                        if (particle.x - 1 < 0)
                            break;
                        else
                        {
                            particle.x--;
                            if (HasNeighbour(particle, map, thicc))
                            {
                                map[particle.x, particle.y] = '\u2800';
                                room.Tiles.Add(new Tile(particle.x, particle.y));
                                break;
                            }
                            continue;
                        }
                    //move right
                    if (diceRoll < 0.25f)
                        if (particle.x + 1 > dungeon.DungeonSizeX - 1)
                            break;
                        else
                        {
                            particle.x++;
                            if (HasNeighbour(particle, map, thicc))
                            {
                                map[particle.x, particle.y] = '\u2800';
                                room.Tiles.Add(new Tile(particle.x, particle.y));
                                break;
                            }
                            continue;

                        }
                }

            }
            return room;
        }
        //Diffusion Limited Aggregation - good implementation
        public Room DLAProper(Dungeon dungeon, int iterations)
        {
            double diceRoll = 0;
            Room room = new Room();
            char[,] map = new char[dungeon.DungeonSizeX, dungeon.DungeonSizeY];
            map = Utilities.PopulateArray(map);

            int startingPointY = (int)Math.Ceiling(dungeon.DungeonSizeY / 2f);
            int startingPointX = (int)Math.Ceiling(dungeon.DungeonSizeX / 2f);

            (int x, int y) firstStep = (startingPointX, startingPointY);
            List<(int x, int y)> takenSpaces = new List<(int x, int y)>();
            takenSpaces.Add(firstStep);
            map[startingPointX, startingPointY] = '\u2800';
            room.Tiles.Add(new Tile(startingPointX, startingPointY));
            //Variant A -- wrong
            for (int i = 0; i < iterations; i++)
            {

                (int x, int y) particle = PlacePartile(dungeon);
                while (true)
                {
                    diceRoll = random.NextDouble();
                    if (diceRoll >= 0.75f)
                        if (particle.y + 1 > dungeon.DungeonSizeY - 1)
                            break;
                        else
                        {
                            particle.y++;
                            if (map[particle.x, particle.y] == '\u2800')
                            {
                                map[particle.x, particle.y - 1] = '\u2800';
                                room.Tiles.Add(new Tile(particle.x, particle.y - 1));
                                break;
                            }
                            continue;
                        }
                    //move down
                    if (diceRoll < 0.75f && diceRoll >= 0.5f)
                        if (particle.y - 1 < 0)
                            break;
                        else
                        {
                            particle.y--;
                            if (map[particle.x, particle.y] == '\u2800')
                            {
                                map[particle.x, particle.y + 1] = '\u2800';
                                room.Tiles.Add(new Tile(particle.x, particle.y + 1));
                                break;
                            }
                            continue;
                        }
                    //move left
                    if (diceRoll < 0.5f && diceRoll >= 0.25f)
                        if (particle.x - 1 < 0)
                            break;
                        else
                        {
                            particle.x--;
                            if (map[particle.x, particle.y] == '\u2800')
                            {
                                map[particle.x + 1, particle.y] = '\u2800';
                                room.Tiles.Add(new Tile(particle.x + 1, particle.y));
                                break;
                            }
                            continue;
                        }
                    //move right
                    if (diceRoll < 0.25f)
                        if (particle.x + 1 > dungeon.DungeonSizeX - 1)
                            break;
                        else
                        {
                            particle.x++;
                            if (map[particle.x, particle.y] == '\u2800')
                            {
                                map[particle.x - 1, particle.y] = '\u2800';
                                room.Tiles.Add(new Tile(particle.x - 1, particle.y));
                                break;
                            }
                            continue;

                        }
                }

            }
            return room;
        }
        //DLA WITH CENTRAL ATTRACTOR
        public Room DLAwCA(Dungeon dungeon, int iterations)
        {
            Room room = new Room();
            double diceRoll = 0;
            char[,] map = new char[dungeon.DungeonSizeX, dungeon.DungeonSizeY];
            map = Utilities.PopulateArray(map);

            int startingPointY = (int)Math.Ceiling(dungeon.DungeonSizeY / 2f);
            int startingPointX = (int)Math.Ceiling(dungeon.DungeonSizeX / 2f);

            (int x, int y) firstStep = (startingPointX, startingPointY);
            List<(int x, int y)> takenSpaces = new List<(int x, int y)>();
            takenSpaces.Add(firstStep);
            map[startingPointX, startingPointY] = '\u2800';
            room.Tiles.Add(new Tile(startingPointX, startingPointY));
            for (int i = 0; i < iterations; i++)
            {
                (int x, int y) particle = PlacePartile(dungeon);
                List<Node> pathway = AStar(map, particle, (startingPointX, startingPointY));
                for (int z = 0; z < pathway.Count(); z++)
                {
                    if (map[pathway[z].X, pathway[z].Y] == '\u2800')
                    {
                        if (z - 1 < 0)
                            continue;
                        else
                            map[pathway[z - 1].X, pathway[z - 1].Y] = '\u2800';
                        room.Tiles.Add(new Tile(pathway[z - 1].X, pathway[z - 1].Y));
                    }
                }
            }
            return room;
        }
        //Place particle somewhere on the edge
        private (int x, int y) PlacePartile(Dungeon dungeon)
        {
            double xy = random.NextDouble();
            (int x, int y) coords = (0, 0);
            if (xy > 0.50f)
            {
                coords.x = random.Next(0, dungeon.DungeonSizeX - 1);
                coords.y = random.NextDouble() > 0.5f ? coords.y = 0 : coords.y = dungeon.DungeonSizeY - 1;
            }
            if (xy < 0.50f)
            {
                coords.y = random.Next(0, dungeon.DungeonSizeY - 1);
                coords.x = random.NextDouble() > 0.5f ? coords.x = 0 : coords.x = dungeon.DungeonSizeX - 1;
            }
            return coords;
        }
        public List<Node> AStar(char[,] map, (int x, int y) startPoint, (int x, int y) endPoint)
        {
            char[,] path = new char[map.GetLength(0), map.GetLength(1)];
            List<Node> openNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();
            List<Node> pathway = new List<Node>();
            Node currentNode = new Node(startPoint.x, startPoint.y, 0, CalculateDistance(startPoint, endPoint));
            openNodes.Add(currentNode);
            while (openNodes.Count > 0)
            {
                //We are at destination
                if (currentNode.X == endPoint.x && currentNode.Y == endPoint.y)
                {
                    pathway = RetracePath(closedNodes);
                    break;
                }
                for (int r = -1; r <= 1; r++)
                {
                    for (int c = -1; c <= 1; c++)
                    {
                        if (!(currentNode.X + r < 0 || currentNode.X + r > map.GetLength(0) - 1 || currentNode.Y + c < 0 || currentNode.Y + c > map.GetLength(1) - 1)) //Check if it overflows
                        {
                            if (r == 0 && c == 0)
                                continue; // Skip center tile
                            //ignore if it's already closed
                            if (!closedNodes.Exists(node => node.X == currentNode.X + r && node.Y == currentNode.Y + c))
                            {
                                //node that we are checking
                                Node checkedNode = new Node(currentNode.X + r, currentNode.Y + c, CalculateDistance(startPoint, (currentNode.X + r, currentNode.Y + c)), CalculateDistance((currentNode.X + r, currentNode.Y + c), endPoint));
                                checkedNode.ParentNode = currentNode;
                                //if there is already that node in open nodes with higher FCost, update it's cost.
                                if (openNodes.Exists(node => node.X == checkedNode.X && node.Y == checkedNode.Y && node.FCost > checkedNode.FCost))
                                {
                                    int f = openNodes.FindIndex(node => node.X == checkedNode.X && node.Y == checkedNode.Y);
                                    openNodes[f] = checkedNode;
                                }
                                else
                                {
                                    if (!openNodes.Exists(node => node.X == checkedNode.X && node.Y == checkedNode.Y))
                                    {
                                        //if it doesn't exist, add it.
                                        openNodes.Add(checkedNode);
                                    }
                                }

                            }
                            else
                                continue;

                        }
                        else
                            continue;
                    }
                }

                //currentNode = openNodes.OrderByDescending(node => node.FCost).First();
                foreach (var item in openNodes)
                {
                    if (item.FCost < currentNode.FCost || (item.FCost == currentNode.FCost && item.HCost < currentNode.HCost))
                    {
                        currentNode = item;
                    }
                }
                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);
            }

            return pathway;
        }
        public int CalculateDistance((int x, int y) startPoint, (int x, int y) endPoint)
        {
            int distance = 0;
            int distX = Math.Abs(endPoint.x - startPoint.x);
            int distY = Math.Abs(endPoint.y - startPoint.y);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            return 14 * distX + 10 * (distY - distX);
        }
        List<Node> RetracePath(List<Node> closedNodes)
        {
            List<Node> path = new List<Node>();
            closedNodes.Reverse();
            Node currentNode = closedNodes[0];
            path.Add(currentNode);
            foreach (var item in closedNodes)
            {
                path.Add(item.ParentNode);
            }
            path.Reverse();
            return path;
        }
        bool HasNeighbour((int x, int y) point, char[,] map, bool thicc)
        {
            int x = point.x;
            int y = point.y;
            if (thicc)
            {
                for (int r = -1; r <= 1; r++)
                {
                    for (int c = -1; c <= 1; c++)
                    {
                        if ((c == -1 && r == -1) || (c == -1 && r == 1) || (c == 1 && r == -1) || (c == 1 && r == 1))
                            continue;
                        if (!(x + c < 0 || x + c > map.GetLength(0) - 1 || y + r < 0 || y + r > map.GetLength(1) - 1)) //Check if it overflows
                        {
                            if (r == 0 && c == 0) continue; // Skip center tile
                            if (map[x + c, y + r] == '\u2800')
                            {
                                return true;
                            }
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }
            else
            {
                for (int r = -1; r <= 1; r++)
                {
                    for (int c = -1; c <= 1; c++)
                    {
                        if (!(x + c < 0 || x + c > map.GetLength(0) - 1 || y + r < 0 || y + r > map.GetLength(1) - 1)) //Check if it overflows
                        {
                            if (r == 0 && c == 0) continue; // Skip center tile
                            if (map[x + c, y + r] == '\u2800')
                            {
                                return true;
                            }
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }
            return false;
        }

    }
}

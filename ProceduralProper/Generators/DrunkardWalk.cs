using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralProper.Generators
{
    public class DrunkardWalk
    {
        Random random = new Random();
        public char[,] DrunkardWalkGenerator(Dungeon dungeon, int steps, bool improved)
        {
            double diceRoll = 0;
            char[,] walkpath = new char[dungeon.DungeonSizeX, dungeon.DungeonSizeY];
            walkpath = Utilities.PopulateArray(walkpath);


            int startingPointX = random.Next(0, dungeon.DungeonSizeX);
            int startingPointY = random.Next(0, dungeon.DungeonSizeY);
            walkpath[startingPointX, startingPointY] = '\u2800';
            (int x, int y) previousStep = (startingPointX, startingPointY);
            for (int i = 0; i < steps; i++)
            {
                diceRoll = random.NextDouble();
                //move up if there's still place. Otherwise roll again and add one more step. Drunkard never gives up!.
                if (diceRoll >= 0.75f)
                    if (previousStep.y + 1 >= dungeon.DungeonSizeY)
                    {
                        steps++;
                        continue;
                    }
                    else
                    {
                        walkpath[previousStep.x, previousStep.y + 1] = '\u2800';
                        previousStep = (previousStep.x, previousStep.y + 1);
                        continue;
                    }
                //move down
                if (diceRoll < 0.75f && diceRoll >= 0.5f)
                    if (previousStep.y - 1 > 0)
                    {
                        walkpath[previousStep.x, previousStep.y - 1] = '\u2800';
                        previousStep = (previousStep.x, previousStep.y - 1);
                        continue;
                    }
                    else
                    {

                        steps++;
                        continue;
                    }
                //move left
                if (diceRoll < 0.5f && diceRoll >= 0.25f)
                    if (previousStep.x - 1 > 0)
                    {
                        walkpath[previousStep.x - 1, previousStep.y] = '\u2800';
                        previousStep = (previousStep.x - 1, previousStep.y);
                        continue;
                    }
                    else
                    {
                        steps++;
                        continue;
                    }
                //move right
                if (diceRoll < 0.25f)
                    if (previousStep.x + 1 >= dungeon.DungeonSizeX)
                    {
                        steps++;
                        continue;
                    }
                    else
                    {
                        walkpath[previousStep.x + 1, previousStep.y] = '\u2800';
                        previousStep = (previousStep.x + 1, previousStep.y);
                        continue;

                    }

            }
            return walkpath;
        }
    }
}

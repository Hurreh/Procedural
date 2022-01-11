using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralProper
{
    public class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TileType TileType { get; set; }
        public bool HasRiddle { get; set; }
        public bool IsDoor { get; set; }
        public bool IsSpecialWall { get; set; }

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public enum TileType
    {   //                                                     ULR
        //ROOM TILES                                           LR
        L, //LEFT                                           LU TOP RU
        LU, //LEFT UPPER                            UDL UD  L  MID  R  UD UDR
        LD, //LEFT DOWN                                     LD BOT RD
        R, //RIGHT                                             LR 
        RU,//RIGHT UPPER                                       DLR
        RD,//RIGHT DOWN
        TOP,//TOP
        BOT,//BOTTOM
        MID, //MIDDLE

        //Corridors and tightspots
        UD, //Blocked up and down
        LR, //Blocked left and right
        UDL, //Blocked up, down and left
        UDR, //Blocked up, down and right
        DLR, //Blocked down, left and right
        ULR //Blocked up, left and right

        //IF SURROUNDED
        //MID


        //IF NO TILE ON LEFT
        //L, LU, LD, LR, UDL, ULR, DLR, - all options
        //AND
        //IF NO UP
        //L, LD, LR, DLR
        //AND IF UP
        //LU, UDL, ULR
        
            
        
        //IF TILE ON LEFT
        //TOP, BOT, RU,R, RD, UD, UDR - all options
        //AND
        //IF NO UP
        //BOT, R, RD, 
        //AND
        //IF UP
        //TOP, RU, UD, UDR
    }
}

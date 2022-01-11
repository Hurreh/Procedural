using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralProper
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; set; }
        public Node ParentNode { get; set; }

        public Node(int x, int y, int gCost, int hCost)
        {
            X = x;
            Y = y;
            GCost = gCost;
            HCost = hCost;
            FCost = gCost + hCost;
        }
    }
}

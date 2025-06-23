using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace prototype.enemies.enemy_treversal_algorithm
{
    internal class Node
    {
        public Vector2 position;
        public bool iswalkable;
        public int gcost;
        int hcost;
        public int fcost;
        public Node parent;
        public int nodexindex_x;
        public int nodeyindex_y;
        
        public Node(Vector2 position, bool iswalkable,int x, int y)
        {
            this.position = position;
            this.iswalkable = iswalkable;
            setnodeindex(x, y);
        }
        public void addhcost(int hcost)
        {
            this.hcost = hcost;
        }
        public void addgcost(int x)
        {
            gcost = x+1; 
        }
        public void setgcost(int gcost)
        {
            this.gcost = gcost;
        }
        public void sumcost()
        {
            fcost  = gcost + hcost;
        }
        public void setparent(Node n)
        {
            parent = n;
        }
        public void setnodeindex(int x, int y)
        {
            nodexindex_x = x;
            nodeyindex_y = y;
        }
    }
}

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace prototype.enemies.enemy_treversal_algorithm
{
    internal class hashsetfornodes
    {
        List<Node>[] hashtable;
        int size;
        public hashsetfornodes(int size) 
        {
            this.size = size;
            hashtable = new List<Node>[size];
            for (int i = 0; i < size; i++)
            {
                hashtable[i] = new List<Node>();
            }
        }
        int gethashidex(Node node)//calculate the index of this node using its grid index * a prime number
        {
            return (node.nodexindex_x * 23 + node.nodeyindex_y) % size;
        }
        public void insertnode(Node n)
        {
            int index = gethashidex(n);
            hashtable[index].Add(n);
        }
        public bool has(Node n)//check whether the hashset contians this node
        {
            if (hashtable[gethashidex(n)].Contains(n))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        
    }
}

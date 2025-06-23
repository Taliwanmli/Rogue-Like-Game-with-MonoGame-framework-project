using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace prototype.enemies.enemy_treversal_algorithm
{
    internal class minheap
    { 
        public List<Node> nodes = new List<Node>();
        Node output;
        public void swap(int a, int b)//swap 2 item in the list
        {
            Node tempnode = nodes[a];
            nodes[a] = nodes[b];
            nodes[b] = tempnode;
        }
        public void upheap(int index)//when add item at the end, put it at right position int he list
        {
            while(index > 0)
            {
                if (nodes[index].fcost < nodes[(index - 1) / 2].fcost)
                {
                    swap(index, (index - 1) / 2);
                    index = (index - 1) / 2;
                }
                else
                {
                    break;
                }
            }
        }
        public void downheap(int index)//when remove item, restructure the bianry tree to correct order
        {
            int length = nodes.Count;
            while(index < length)
            {
                int currentindex = index;
                int leftchildindex = index * 2 + 1;
                int rightchildindex = index * 2 + 2;
                if (leftchildindex < length && nodes[leftchildindex].fcost < nodes[currentindex].fcost)
                {
                    currentindex = leftchildindex;
                }
                if (rightchildindex < length && nodes[rightchildindex].fcost < nodes[currentindex].fcost)
                {
                    currentindex = rightchildindex;
                }
                if(currentindex != index)
                {
                    swap(currentindex, index);
                    index = currentindex;
                }
                else
                {
                    break;
                }  
            } 
        }
        public Node removenode()//put swap first and last node, remove the last node, and restructure the tree
        {
            output = nodes[0];
            swap(0,nodes.Count()-1);
            nodes.RemoveAt(nodes.Count()-1);
            downheap(0);
            return output;
        }
        public Node recordthisnode()
        {
            removenode();
            return output;
        }
        public void addnode(Node n)//inserting node
        {
            nodes.Add(n);
            upheap(nodes.Count - 1);
        }
    }
}

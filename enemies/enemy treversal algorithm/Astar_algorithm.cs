using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using prototype.Map;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace prototype.enemies.enemy_treversal_algorithm
{
    internal class Astar_algorithm
    {
        //153x86 grid
        public bool spawnfail;  
        
        public Stack<Vector2> traversal(Vector2 current_pos, Vector2 Target_pos, Node[,] emptyGrid)
        {
            Node[,] Grid = emptyGrid;
            minheap openset = new minheap();
            hashsetfornodes closeset = new hashsetfornodes(400);
            int numofiteration = 0;
            spawnfail = false;  
            // find the node position of current node and target node
            int currentnode_x = (int)(current_pos.X / 50);
            //if(currentnode_x > 0) { currentnode_x -= 1; }//to keep it the same index as the 2d array
            int currentnode_y = (int)(current_pos.Y / 50);
            //if(currentnode_y > 0) { currentnode_y -= 1; }   
            int targetnode_x = (int)(Target_pos.X/ 50);
            //if(targetnode_x > 0) {  targetnode_x -= 1; }   
            int targetnode_y = (int)(Target_pos.Y / 50);
            //if(targetnode_y > 0) { targetnode_y -= 1; } 

            Node startnode = Grid[currentnode_x, currentnode_y];
            startnode.setgcost(0);
            Node endnode = Grid[targetnode_x, targetnode_y];
            closeset.insertnode(startnode);
                                                                                                                                                      
            //this is intialization of the openset, and after I can begin the algorithm to find the target
            foreach (Node n in findneibours(currentnode_x, currentnode_y,targetnode_x,targetnode_y,true,Grid))
            {
                if(n == endnode)
                {
                    Stack<Vector2> output = new Stack<Vector2>();
                    output.Push(endnode.position);
                    return output;
                }
                if (n.iswalkable)    //walkable openset, otherwise closeset
                {
                    n.setparent(startnode);
                    openset.addnode(n);
                }
                else
                {
                    closeset.insertnode(n);
                }
            }
            if(openset.nodes.Count == 0) //if the open set is empty, return a empty list for deleting enemy
            {
                spawnfail = true;
                return null;
            }
            //the algorithm begins here
            while (openset.nodes.Count > 0 && numofiteration < 6500)
            {//if the closest node found is the target node, out put the position of it
                numofiteration++;
                Node tempnode = openset.nodes[0];
                //this is a loop to test if it gointo a deadend
                List<Node> listtoaddtoopenset = new List<Node>();
                foreach (Node n in findneibours(tempnode.nodexindex_x, tempnode.nodeyindex_y, targetnode_x, targetnode_y, false,Grid))
                {
                    if (n == endnode) //if the neibour is the target
                    {
                        endnode.setparent(tempnode);
                        Stack<Vector2> output = new Stack<Vector2>();
                        Node temp = endnode;
                        CreatePath(temp, startnode, output);
                        return output;
                    }
                    if(n.iswalkable && !closeset.has(n)) //if it's walkable and not in close set
                    {
                        if (openset.nodes.Contains(n)) //if the node already in open set
                        {
                            //if the gcost to walk from this neibour to a node in the open set is shorter than calculated before, use this gcost and set 
                            //this node's(already in openset) parent the node we currently at
                                                
                            Node nodetocheck = n; //this is the neiboured found (the one already in open set)
                            nodetocheck.addgcost(tempnode.gcost); //calculate the new gcost to walk from node we currently at
                            int thenalreadytheregcost = n.gcost; //this node's original g cost
                            int thenewpathgcosttoadd = nodetocheck.gcost; //the new calculated gcost
                            if (thenewpathgcosttoadd < thenalreadytheregcost)
                            {
                                n.setgcost(thenewpathgcosttoadd); //set g cost for this node the new g cost calculated
                                n.sumcost();
                                n.setparent(tempnode); //sets the parent for this node(already there) the node we currently add
                            }
                        }
                        else //walkable, and not in openset
                        {
                            n.setparent(tempnode);
                            n.addgcost(tempnode.gcost);
                            listtoaddtoopenset.Add(n);
                        } 
                    }
                    else if (!n.iswalkable) //if not walkable add to close set
                    {
                        closeset.insertnode(n);
                    }
                }
                if (listtoaddtoopenset == null)//that means all of neibours is not walkable or already in close set, this is a deadend
                {
                    closeset.insertnode(openset.removenode());//remove this node we are not going to this one
                }
                else
                {
                    closeset.insertnode(openset.removenode());//remove this node, it's fully explored, and put this node in close set
                    foreach (Node n in listtoaddtoopenset)
                    {
                        openset.addnode(n);
                    } 
                }
            }
            return null;
        }
        List<Node> findneibours(int x, int y,int targetx,int targety,bool addgcost, Node[,] Grid) //this will return the valid neibours of a node it is currently on
        {
            List<Node> temp = new List<Node>();
            if(x-1 > 0) 
            { 
                if(y >= 0 && y < 86) 
                {
                    temp.Add(Grid[x - 1, y]);
                    Grid[x - 1, y].addhcost(Manhhatandistance(x - 1, targetx, y, targety));
                    if (addgcost)
                    {
                        Grid[x - 1, y].addgcost(Grid[x,y].gcost);
                    }
                    Grid[x - 1, y].sumcost();
                }  
            }
            if(x+1 < 153) 
            {
                if (y >= 0 && y < 86)
                {
                    temp.Add(Grid[x + 1, y]);
                    Grid[x + 1, y].addhcost(Manhhatandistance(x + 1, targetx, y, targety));
                    if (addgcost)
                    {
                        Grid[x + 1, y].addgcost(Grid[x, y].gcost);
                    }
                    Grid[x + 1, y].sumcost();
                }
            }
            if(y-1 > 0) 
            { 
                if(x >= 0 && x < 153)
                {
                    temp.Add(Grid[x, y - 1]);
                    Grid[x, y - 1].addhcost(Manhhatandistance(x, targetx, y - 1, targety));
                    if (addgcost)
                    {
                        Grid[x, y - 1].addgcost(Grid[x, y].gcost);
                    }
                    Grid[x, y - 1].sumcost();
                }
            }
            if (y+1 <86) 
            {
                if (x >= 0 && x < 153)
                {
                    temp.Add(Grid[x, y + 1]);
                    Grid[x, y + 1].addhcost(Manhhatandistance(x, targetx, y + 1, targety));
                    if (addgcost)
                    {
                        Grid[x, y + 1].addgcost(Grid[x, y].gcost);
                    }
                    Grid[x, y + 1].sumcost();
                }
            }
            return temp;
        }

        int Manhhatandistance(int x1, int x2, int y1, int y2) //this is the method to estimate the h cost between node
        {
            return(Math.Abs(x1-x2) + Math.Abs(y1-y2));
        }
        
        void CreatePath(Node n,Node startnode, Stack<Vector2> output)
        {
            output.Push(n.position);
            n = n.parent;
            if (n != startnode && n != null)
            {
                CreatePath(n, startnode, output); //recurstion
            }
        }
    }
}
                
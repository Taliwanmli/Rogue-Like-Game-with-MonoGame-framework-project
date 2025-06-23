using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using prototype.Map;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace prototype.enemies.enemy_treversal_algorithm
{
    internal class Gridsystem
    {
        Node[,] Grid = new Node[153, 86];//153x86 grid
        public Gridsystem(Mapgeneration map)
        {
            for (int i = 0; i < 153; i++)//put node into grid
            {
                for (int j = 0; j < 86; j++)
                {
                    Rectangle tempnode = new Rectangle(i * 50, j * 50, 50, 50);
                    foreach (Rooms r in map.rooms)//if intersects any walls mark as non walkable
                    {
                        Rectangle temproom = new Rectangle((int)r.roompostition.X - 50, (int)r.roompostition.Y - 50, r.roomspace.Width + 100, r.roomspace.Height - 100);

                            if(tempnode.Intersects(temproom))
                            {
                                Grid[i, j] = new Node(new Vector2(i * 50, j * 50), false,i,j);
                                break;
                            }
                            else
                            {
                                Grid[i, j] = new Node(new Vector2(i * 50, j * 50), true,i,j);
                            }

                    }
                }
            }
        }
        public Node[,] Getemptygrid()
        {
            return Grid;
        }
    }
}

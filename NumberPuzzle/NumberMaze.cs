using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NumberPuzzle
{
    public class NumberMaze
    {
        int[,] maze = new int[,]
             {{0,0,1,6,7,5,4,3,2,1,7,5,7,9,9},
              {3,7,5,1,3,6,9,3,6,9,7,9,4,2,9},
              {6,2,3,4,5,6,2,1,7,6,3,4,2,6,9},
              {7,4,2,8,2,3,4,8,8,8,7,8,7,6,5},
              {9,4,3,3,7,5,1,8,8,9,7,9,4,5,5},
              {7,7,7,3,3,5,2,3,4,3,2,3,4,7,3},
              {2,6,5,6,7,4,5,8,5,5,5,8,8,4,6},
              {4,4,9,5,6,3,4,2,9,6,8,3,7,6,9},
              {3,2,1,6,9,6,6,2,8,9,2,6,3,6,1},
              {2,6,2,2,1,2,3,3,4,4,8,2,3,6,1},
              {2,5,5,5,2,2,6,3,2,6,8,4,7,7,2},
              {5,3,2,2,3,2,2,2,2,3,4,2,2,2,3},
              {2,1,4,4,1,1,5,1,5,5,6,4,2,5,3},
              {1,4,2,2,1,2,2,2,7,1,6,2,8,4,7},
              {5,4,2,6,1,7,2,2,3,3,3,3,3,3,1} };

        public int[,] Maze {  get => maze;   private set => maze = value;  }

        int goal = 29;

        public int StartRow { get; set; }
        public int StartCol { get; set; }
        public int rmax;
        public int cmax;
        public bool solved = false;

        public int[][] solution;

        enum Direction { NORTH=1, EAST, SOUTH, WEST};
        Direction[] DIRNEW = { Direction.NORTH, Direction.EAST, Direction.WEST };
        Direction[] DIRNSW = { Direction.NORTH, Direction.SOUTH, Direction.WEST };
        Direction[] DIRESW = { Direction.EAST, Direction.SOUTH, Direction.WEST };
        Direction[] DIRNES = { Direction.NORTH, Direction.EAST, Direction.SOUTH };

        int[,] occupied;
        public int[,] solidx;

        public int Goal { get => goal; set => goal = value; }

        public NumberMaze()
        {
            rmax = maze.GetLength(0);
            cmax = maze.GetLength(1);
            StartRow = 15;
            StartCol = 5;
        }

        public void Solve()
        {
            solved = false;
            solution = new int[Goal+1][];

            occupied = new int[rmax, cmax];
            solidx   = new int[rmax, cmax];
            
            bool res =  findSum(1, StartRow - 1, StartCol - 1, 0);
        }

        void copy2DMatrix(int[,] src, int[,] dest)
        {
            if (src.GetLength(0) != dest.GetLength(0) || src.GetLength(1) != dest.GetLength(1))
            {
                Console.WriteLine("error in copy2DMatrix().\n");
                return;
            }
                
            for (int ii = 0; ii < src.GetLength(0); ii++)
                for (int jj = 0; jj < src.GetLength(1); jj++)
                    dest[ii, jj] = src[ii, jj];
         
        }
        bool findSum(int n, int i, int j, Direction dir)
        {
            if (solved == true) return false;
            
            int csum = 0;
            int i2 = i;
            int j2 = j;
            bool found = false;
            switch (dir)
            {
                case Direction.NORTH:
                    for (int ii = i; ii >= 0; ii--)
                    {
                        csum += maze[ii, j];
                        if (csum > n)
                            return false;
                        if (csum == n)
                        {
                            i2 = ii;
                            found = true;
                            break;
                        }
                    }
                    break;
                case Direction.EAST:
                    for (int jj = j; jj < cmax; jj++)
                    {
                        csum += maze[i, jj];
                        if (csum > n)
                            return false;
                        if (csum == n)
                        {
                            j2 = jj;
                            found = true;
                            break;
                        }
                    }
                    break;
                case Direction.SOUTH:
                    for (int ii = i; ii < rmax; ii++)
                    {
                        csum += maze[ii, j];
                        if (csum > n)
                            return false;
                        if (csum == n)
                        {
                            i2 = ii;
                            found = true;
                            break;
                        }
                    }
                    break;
                case Direction.WEST:
                    for (int jj = j; jj >= 0; jj--)
                    {
                        csum += maze[i, jj];
                        if (csum > n)
                            return false;
                        if (csum == n)
                        {
                            j2 = jj;
                            found = true;
                            break;
                        }
                    }
                    break;
                default:
                    if (maze[i, j] == n)
                        found = true;
                    break;
            }

            if (!found) return false;

            switch (dir)
            {
                case Direction.NORTH:
                    for (int ii = i; ii >= i2; ii--) {
                        if (occupied[ii, j] != 0)
                            return false;
                    }
                    for (int ii = i; ii >= i2; ii--)
                        occupied[ii, j] = n;
                    break;
                case Direction.EAST:
                    for (int jj = j; jj <= j2; jj++) {
                        if (occupied[i, jj] != 0)
                            return false;
                    }
                    for (int jj = j; jj <= j2; jj++)
                        occupied[i, jj] = n;
                    break;
                case Direction.SOUTH:
                    for (int ii = i; ii <= i2; ii++) {
                        if (occupied[ii, j] != 0)
                            return false;
                    }
                    for (int ii = i; ii <= i2; ii++)
                        occupied[ii, j] = n;
                    break;
                case Direction.WEST:
                    for (int jj = j; jj >= j2; jj--) {
                        if (occupied[i, jj] != 0)
                            return false;
                    }
                    for (int jj = j; jj >= j2; jj--)
                        occupied[i, jj] = n;
                    break;                    
                default:
                    occupied[i2, j2] = n;
                    break;
            }

            int i0, j0, istep, jstep;
            
            istep = ((i2 > i) ? 1 : ((i2 < i) ? -1 : 0));
            jstep = ((j2 > j) ? 1 : ((j2 < j) ? -1 : 0));

            solution[n] = new int[2];
            i0 = i; j0 = j;
            //          bool isFirst = true;
            solution[n][0] = i + j * cmax;
            solution[n][1] = i2 + j2 * cmax;
            

            if (n == Goal)
            {
                copy2DMatrix(occupied, solidx);
                solved = true;
                return true;
            }


            if (i2 > 0 & dir != Direction.SOUTH)
            {
                foreach (var k in DIRNEW)
                {
                    findSum(n + 1, i2 - 1, j2, k);   // #North
                }
            }

            if (j2 > 0 & dir != Direction.EAST)
            {
                foreach (var k in DIRNSW)
                {
                    findSum(n + 1, i2, j2-1, k);   // # West
                }
            }
            
    
            if (i2 < rmax-1 & dir != Direction.NORTH)
            {
                foreach (var k in DIRESW)
                {
                    findSum(n + 1, i2 + 1, j2, k); //# South
                }
            }

            if (j2 < cmax-1 & dir != Direction.WEST)
            {
                foreach (var k in DIRNES)
                {
                    findSum(n + 1, i2, j2 + 1, k); //# East
                }
            }

            i0 = i; j0 = j;
            while (true)
            {
                occupied[i0, j0] = 0;  // update occupied
                if ((i0 == i2) && (j0 == j2))
                    break;
                i0 += istep; j0 += jstep;
            } 

            return false;            
        }


    }
    
}

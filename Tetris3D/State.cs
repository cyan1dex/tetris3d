using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tetris3D
{
   public class State : IComparable
   {
      HashSet<Point> coordHash = new HashSet<Point>();
      HashSet<Point> holesHash = new HashSet<Point>();
      HashSet<Point> cellHash = new HashSet<Point>();
      public TetrisBlock[,] gameBoard;
      public Point[] points;
      public Point[] currentLocation;
      int utility, holeDepth, roofHeight, wallHeight;
      public Orientations currentOrientation;

      public int CompareTo(object obj)
      {
         State temp = (State)obj;
         if (this.utility > temp.utility)
            return 1;
         if (this.utility < temp.utility)
            return -1;
         else
            return 0;
      }

      public State(TetrisBlock[,] gameBoard, Point[] points, Orientations currentOrientation, Point[] currentLocation)
      {
         this.currentLocation = currentLocation;
         this.currentOrientation = currentOrientation;
         this.points = points;
         this.gameBoard = gameBoard;
         this.utility = 0;
      }


      public void calculateUtility() //TODO: need to leave cliffs for hanging pieces on
      {                              //TODO: Need Row & column transition count from filled to empty
         int blanks = 0;             //TODO: Row Holes: # of rows with at least one hole
         int holes = 0;
         int lineBonus = 0;
         int completionCount = 0;
         holeDepth = 0;
         roofHeight = 0;
         wallHeight = 0;

         for (int y = this.gameBoard.GetLength(1) - 1; y >= 0; y--) //Height function, change to check only current pieces new height
         {
            for (int x = 0; x < this.gameBoard.GetLength(0); x++)
            {
               if (x == 0)
                  completionCount = 0;

               if (this.gameBoard[x, y] != null)
               {
                  completionCount++;
                  Point cur = new Point(x, y);
                  cellHash.Add(cur);
                  if (cur != this.currentLocation[0] || cur != this.currentLocation[1] || cur != this.currentLocation[2] || cur != this.currentLocation[3])
                  {
                      coordHash.Add(new Point(x, y - 1));//TODO: current piece location b4 drop is affecting gameboard

                      if (y < 20 && y > wallHeight)
                          wallHeight = y;
                  }
                  blanks += ((y+1) * (y+1));
               }

               else if (this.gameBoard[x, y] == null) //Holes functions
               {
                  foreach (Point coord in coordHash)
                  {
                     if (coord.X == x && coord.Y == y)
                     { holes++; holesHash.Add(coord); }
                  }
               }
            }

            foreach (Point hole in holesHash) //Roof size covering hole function
            {
                checkHoleRoof(hole, 1);
            }

            if (completionCount == 10)//Eroded pieces
               lineBonus -= 550;//TODO: needs to multiply times number of cells that were eroded by the used piece
         }

         this.utility = holes * 50 + blanks + lineBonus + roofHeight * 4;

      }

      public void checkHoleRoof(Point CurrentCell, int height)
      {
         Point cellCoord = new Point(CurrentCell.X, CurrentCell.Y + height);
         if (cellHash.Contains(cellCoord))
         {
            roofHeight += 1;
            height +=1;
            checkHoleRoof(cellCoord, height);
         }
      }

      public void checkHoleDepth(Point currentHole, int depth)
      {
         Point holeCoord = new Point(currentHole.X, currentHole.Y + depth);
         if (holesHash.Contains(holeCoord))
         {
            holeDepth += 1;
            depth = depth*depth;
            checkHoleDepth(holeCoord, depth);
         }
      }

   }
}

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tetris3D
{
   class Board
   {
      TetrisSession currentSession;
      TetrisPiece currentPiece;
      TetrisPiece nextPiece;
      TetrisBlock[,] gameBoard;
      Orientations tempOrientation;
      IPriorityQueue priorityQ = new BinaryPriorityQueue();
      List<State> stateList = new List<State>();

      public Board(TetrisSession currentSession)
      {
         this.currentSession = currentSession;
         this.currentPiece = currentSession.CurrentTetrisPiece();
         this.nextPiece = currentSession.NextTetrisPiece();
         this.gameBoard = currentSession.GameBoard;
      }

      public bool moveDown(Point[] tempLocations, TetrisBlock[,] temp)
      {
         foreach (Point point in tempLocations)
         {
            if ((point.Y - 1) < 0 || (!this.isCurrentPieceAtLocation(new Point(point.X, point.Y - 1), tempLocations) && temp[point.X, (point.Y - 1)] != null))
               return false;
         }
         return true;
      }

      public bool isCurrentPieceAtLocation(Point point, Point[] tempLocations) //Check if the point below is not part of the current piece
      {
         foreach (Point currentPoint in tempLocations)
         {
            if (point == currentPoint)
               return true;
         }
         return false;
      }

      public Point minRangeValue(int x, TetrisPiece current)
      {
         if ((x == 0 || x == 1) && current.Type == TetrisPieces.IBlock)
            return new Point(5, 5);
         else if ((x == 0 || x == 1) && current.Type == TetrisPieces.JBlock)
            return new Point(4, 5);
         else if ((x == 2 || x == 3) && current.Type == TetrisPieces.JBlock)
            return new Point(3, 5);
         else if ((x == 2 || x == 3) && current.Type == TetrisPieces.LBlock)
            return new Point(5, 3);
         else if ((x == 0 || x == 1) && current.Type == TetrisPieces.LBlock)
            return new Point(5, 4);
         else if ((x == 2) && current.Type == TetrisPieces.TBlock)
            return new Point(4, 5);
         else if ((x == 3) && current.Type == TetrisPieces.TBlock)
            return new Point(5, 4);
         else if ((x == 2 || x == 3) && current.Type == TetrisPieces.SBlock)
            return new Point(4, 5);
         else if ((x == 2 || x == 3) && current.Type == TetrisPieces.ZBlock)
            return new Point(5, 4);
         else if ((x == 2 || x == 3) && current.Type == TetrisPieces.IBlock)
            return new Point(4, 3);
         else if (current.Type == TetrisPieces.OBlock)
            return new Point(5, 4);
         else
            return new Point(4, 4);
      }

      public void boardAddition(int j, int x, Point[] tempLocations, TetrisBlock[,] temp)
      {
         if (x == 0)
         {
            tempOrientation = Orientations.North;
            tempLocations = currentPiece.getNorthOrientation;
         }
         else if (x == 1)
         {
            tempOrientation = Orientations.South;
            tempLocations = currentPiece.getSouthOrientation;
         }
         else if (x == 2)
         {
            tempOrientation = Orientations.West;
            tempLocations = currentPiece.getWestOrientation;
         }
         else if (x == 3)
         {
            tempOrientation = Orientations.East;
            tempLocations = currentPiece.getEastOrientation;
         }

         for (int i = 0; i < 4; i++)
         {
            tempLocations[i].X = tempLocations[i].X + j;
         }

         while (moveDown(tempLocations, temp) == true)
         {
            for (int i = 0; i < 4; i++)
            {
               tempLocations[i].Y = tempLocations[i].Y - 1;
            }
         }

         foreach (Point p in tempLocations)
         {
            temp[p.X, p.Y] = new TetrisBlock(TetrisColors.Gray);
         }
      }

      public void cumlativeBoard(State curState)
      {
         int xInit = 0;
         int xMax = 4;
         if (nextPiece.Type == TetrisPieces.OBlock) //Limits orientation iterations if one direction is the same as another
            xInit = 3;
         else if (nextPiece.Type == TetrisPieces.ZBlock || nextPiece.Type == TetrisPieces.SBlock || nextPiece.Type == TetrisPieces.IBlock)
         { xInit = 1; xMax = 3; }

         for (int x = xInit; x < xMax; x++)
         {
            Point range = minRangeValue(x, nextPiece);

            for (int j = -range.X; j < range.Y; j++)
            {
               TetrisBlock[,] temp = new TetrisBlock[10, 24];
               Point[] tempLocations = new Point[4];
               temp = (TetrisBlock[,])curState.gameBoard.Clone();
               tempLocations = (Point[])nextPiece.PieceLocations.Clone();

               if (x == 0)
               {
                  tempLocations = nextPiece.getNorthOrientation;
               }
               else if (x == 1)
               {
                  tempLocations = nextPiece.getSouthOrientation;
               }
               else if (x == 2)
               {
                  tempLocations = nextPiece.getWestOrientation;
               }
               else if (x == 3)
               {
                  tempLocations = nextPiece.getEastOrientation;
               }

               for (int i = 0; i < 4; i++)
               {
                  tempLocations[i].X = tempLocations[i].X + j;
               }

               while (moveDown(tempLocations, temp) == true)
               {
                  for (int i = 0; i < 4; i++)
                  {
                     tempLocations[i].Y = tempLocations[i].Y - 1;
                  }
               }

               foreach (Point p in tempLocations)
               {
                  temp[p.X, p.Y] = new TetrisBlock(TetrisColors.Gray);
               }

               State cumlativeState = new State(temp, curState.points, curState.currentOrientation, this.nextPiece.PieceLocations);
               cumlativeState.calculateUtility(); //calculate for only cumlative
               priorityQ.Push(cumlativeState); //need to push cumlative state
            }
         }
      }

      public void boardVariants()
      {
         TetrisBlock[,] temp = new TetrisBlock[10, 24];

         int xInit = 0;
         int xMax = 4;
         if (currentPiece.Type == TetrisPieces.OBlock) //Limits orientation iterations if one direction is the same as another
            xInit = 3;
         else if (currentPiece.Type == TetrisPieces.ZBlock || currentPiece.Type == TetrisPieces.SBlock || currentPiece.Type == TetrisPieces.IBlock)
         { xInit = 1; xMax = 3; }

         for (int x = xInit; x < xMax; x++)
         {
            Point range = minRangeValue(x, currentPiece);

            for (int j = -range.X; j < range.Y; j++)
            {
               
               Point[] tempLocations = new Point[4];
               temp = (TetrisBlock[,])gameBoard.Clone();
               tempLocations = (Point[])currentPiece.PieceLocations.Clone();

               if (x == 0)
               {
                  tempOrientation = Orientations.North;
                  tempLocations = currentPiece.getNorthOrientation;
               }
               else if (x == 1)
               {
                  tempOrientation = Orientations.South;
                  tempLocations = currentPiece.getSouthOrientation;
               }
               else if (x == 2)
               {
                  tempOrientation = Orientations.West;
                  tempLocations = currentPiece.getWestOrientation;
               }
               else if (x == 3)
               {
                  tempOrientation = Orientations.East;
                  tempLocations = currentPiece.getEastOrientation;
               }

               for (int i = 0; i < 4; i++)
               {
                  tempLocations[i].X = tempLocations[i].X + j;
                  tempLocations[i].Y = tempLocations[i].Y - 1;
                  tempLocations[i].Y = tempLocations[i].Y - 1;
               }

               while (moveDown(tempLocations, temp) == true)
               {
                  for (int i = 0; i < 4; i++)
                  {
                     tempLocations[i].Y = tempLocations[i].Y - 1;
                  }
               }

               foreach (Point p in tempLocations)
               {
                  temp[p.X, p.Y] = new TetrisBlock(TetrisColors.Gray);
               }
               foreach (Point p in currentPiece.pieceLocations)
               {
                   temp[p.X, p.Y] = null;
               }
               
               State current = new State(temp, tempLocations, tempOrientation, this.currentPiece.PieceLocations); 
               stateList.Add(current);
            }
         }

         foreach (State curState in stateList)
         {
            cumlativeBoard(curState);
         }
         
         State best = (State)priorityQ.Pop();

         this.currentSession.removeBlocksFromGameboard(this.currentPiece.PieceLocations);

         if (currentPiece.Type == TetrisPieces.LBlock && best.currentOrientation == Orientations.South)
            this.currentPiece.referanceLocation = new Point(best.points[0].X - 1, best.points[0].Y);
         else if (currentPiece.Type == TetrisPieces.JBlock && best.currentOrientation == Orientations.West)
            this.currentPiece.referanceLocation = new Point(best.points[0].X + 1, best.points[0].Y - 1);
         else if (currentPiece.Type == TetrisPieces.JBlock && best.currentOrientation == Orientations.South)
            this.currentPiece.referanceLocation = new Point(best.points[0].X + 1, best.points[0].Y);
         else if (currentPiece.Type == TetrisPieces.IBlock && (best.currentOrientation == Orientations.West || best.currentOrientation == Orientations.East))
            this.currentPiece.referanceLocation = new Point(best.points[0].X + 1, best.points[0].Y);
         else
            this.currentPiece.referanceLocation = new Point(best.points[0].X, best.points[0].Y);

         this.currentPiece.orientation = best.currentOrientation;
         this.currentPiece.updatePieceLocations();
         this.currentSession.addBlocksToGameBoard(this.currentPiece.PieceLocations, this.currentPiece.Color);

      }

   }
}

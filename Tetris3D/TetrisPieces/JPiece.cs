﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Tetris3D
{
    /// <summary>
    /// This class represents the J Tetris piece
    /// </summary>
    class JPiece : TetrisPiece
    {
        /// <summary>
        /// Returns the color of the Tetris piece
        /// </summary>
        public override TetrisColors Color
        {
            get
            {
                return TetrisColors.Blue;
            }
        }

        /// <summary>
        /// Returns the type of Tetris piece
        /// </summary>
        public override TetrisPieces Type
        {
            get
            {
                return TetrisPieces.JBlock;
            }
        }

        /// <summary>
        /// Returns the vertical height of the Tetris piece as it is orientated
        /// </summary>
        public override int VerticalHeight
        {
            get
            {
                switch (this.Orientation)
                {
                    case Orientations.East: return 2;
                    case Orientations.North: return 3;
                    case Orientations.South: return 3;
                    case Orientations.West: return 2;
                    default: throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Constructs a new J Tetris piece
        /// </summary>
        /// <param name="referanceLocation">The referance point used to position the Tetris piece</param>
        public JPiece(Point referanceLocation)
            : base(referanceLocation)
        {
        }

        /// <summary>
        /// Constructs a new J Tetris piece
        /// </summary>
        /// <param name="referanceLocation">The referance point used to position the Tetris piece</param>
        /// <param name="orientation">The orientation of the Tetris piece</param>
        public JPiece(Point referanceLocation, Orientations orientation)
            : base(referanceLocation, orientation)
        {
        }

        /// <summary>
        /// The location the Tetris piece occupies when orientated north
        /// </summary>
        /// <returns>An array of the points the Tetris piece occupies when orientated north</returns>
        protected override Point[] pointsForNorthOrientation()
        {
            Point[] newLocation = new Point[4];
            newLocation[0] = new Point(this.referanceLocation.X, this.referanceLocation.Y);
            newLocation[1] = new Point(this.referanceLocation.X - 1, this.referanceLocation.Y);
            newLocation[2] = new Point(this.referanceLocation.X, this.referanceLocation.Y + 1);
            newLocation[3] = new Point(this.referanceLocation.X, this.referanceLocation.Y + 2);

            return newLocation;
        }

        /// <summary>
        /// The location the Tetris piece occupies when orientated east
        /// </summary>
        /// <returns>An array of the points the Tetris piece occupies when orientated east</returns>
        protected override Point[] pointsForEastOrientation()
        {
            Point[] newLocation = new Point[4];
            newLocation[0] = new Point(this.referanceLocation.X, this.referanceLocation.Y);
            newLocation[1] = new Point(this.referanceLocation.X - 2, this.referanceLocation.Y);
            newLocation[2] = new Point(this.referanceLocation.X - 2, this.referanceLocation.Y + 1);
            newLocation[3] = new Point(this.referanceLocation.X - 1, this.referanceLocation.Y);

            return newLocation;
        }

        /// <summary>
        /// The location the Tetris piece occupies when orientated south
        /// </summary>
        /// <returns>An array of the points the Tetris piece occupies when orientated south</returns>
        protected override Point[] pointsForSouthOrientation()
        {
            Point[] newLocation = new Point[4];
            newLocation[0] = new Point(this.referanceLocation.X - 1, this.referanceLocation.Y);
            newLocation[1] = new Point(this.referanceLocation.X - 1, this.referanceLocation.Y + 1);
            newLocation[2] = new Point(this.referanceLocation.X - 1, this.referanceLocation.Y + 2);
            newLocation[3] = new Point(this.referanceLocation.X, this.referanceLocation.Y + 2);

            return newLocation;
        }

        /// <summary>
        /// The location the Tetris piece occupies when orientated west
        /// </summary>
        /// <returns>An array of the points the Tetris piece occupies when orientated west</returns>
        protected override Point[] pointsForWestOrientation()
        {
            Point[] newLocation = new Point[4];
            newLocation[0] = new Point(this.referanceLocation.X - 1, this.referanceLocation.Y + 1);
            newLocation[1] = new Point(this.referanceLocation.X, this.referanceLocation.Y + 1);
            newLocation[2] = new Point(this.referanceLocation.X - 2, this.referanceLocation.Y + 1);
            newLocation[3] = new Point(this.referanceLocation.X, this.referanceLocation.Y);

            return newLocation;
        }
    }
}
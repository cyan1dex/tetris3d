﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Tetris3D
{
    /// <summary>
    /// This class represents a 3 dimensional cube
    /// </summary>
    /// A 3D shape is composed of a collection of triangles
    class BasicShape
    {
        /// <summary>
        /// The relative size of the shape
        /// </summary>
        private Vector3 shapeSize;

        /// <summary>
        /// The relative position of the shape
        /// </summary>
        private Vector3 shapePosition;

        /// <summary>
        /// An array of shape vertices used to map textures to the block
        /// </summary>
        private VertexPositionNormalTexture[] shapeVertices = new VertexPositionNormalTexture[36];

        /// <summary>
        /// The number of triangles used to construct the cube
        /// </summary>
        private const int shapeTriangles = 12;

        private VertexBuffer shapeBuffer;

        /// <summary>
        /// The color of the cube
        /// </summary>
        private TetrisColors tetrisColor;

        /// <summary>
        /// Construcs a new cube shape
        /// </summary>
        /// <param name="size">The relative size of the cube</param>
        /// <param name="position">The relative position of the cube</param>
        /// <param name="tetrisColor">The color of the cube</param>
        public BasicShape(Vector3 size, Vector3 position, TetrisColors tetrisColor)
        {
            this.shapeSize = size;
            this.shapePosition = position;
            this.tetrisColor = tetrisColor;
        }

        /// <summary>
        /// Updates shapeVertices with with vertice positions based on position and size
        /// </summary>
        private void BuildShape()
        {
            shapeVertices = new VertexPositionNormalTexture[this.shapeVertices.Length];

            Vector3 topLeftFront = shapePosition +
                new Vector3(-0.5f, 0.5f, -0.5f) * shapeSize;
            Vector3 bottomLeftFront = shapePosition +
                new Vector3(-0.5f, -0.5f, -0.5f) * shapeSize;
            Vector3 topRightFront = shapePosition +
                new Vector3(0.5f, 0.5f, -0.5f) * shapeSize;
            Vector3 bottomRightFront = shapePosition +
                new Vector3(0.5f, -0.5f, -0.5f) * shapeSize;
            Vector3 topLeftBack = shapePosition +
                new Vector3(-0.5f, 0.5f, 0.5f) * shapeSize;
            Vector3 topRightBack = shapePosition +
                new Vector3(0.5f, 0.5f, 0.5f) * shapeSize;
            Vector3 bottomLeftBack = shapePosition +
                new Vector3(-0.5f, -0.5f, 0.5f) * shapeSize;
            Vector3 bottomRightBack = shapePosition +
                new Vector3(0.5f, -0.5f, 0.5f) * shapeSize;

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 0.5f) * shapeSize;
            Vector3 backNormal = new Vector3(0.0f, 0.0f, -0.5f) * shapeSize;
            Vector3 topNormal = new Vector3(0.0f, 0.5f, 0.0f) * shapeSize;
            Vector3 bottomNormal = new Vector3(0.0f, -0.5f, 0.0f) * shapeSize;
            Vector3 leftNormal = new Vector3(-0.5f, 0.0f, 0.0f) * shapeSize;
            Vector3 rightNormal = new Vector3(0.5f, 0.0f, 0.0f) * shapeSize;

            Vector2 textureTopLeft = new Vector2(0.125f * (float)this.tetrisColor, 0.0f);
            Vector2 textureTopRight = new Vector2(0.125f * ((float)this.tetrisColor + 1.0f), 0.0f);
            Vector2 textureBottomLeft = new Vector2(0.125f * (float)this.tetrisColor, 1.0f);
            Vector2 textureBottomRight = new Vector2(0.125f * ((float)this.tetrisColor + 1.0f), 1.0f);

            // Front face.
            shapeVertices[0] = new VertexPositionNormalTexture(
                topLeftFront, frontNormal, textureTopLeft);
            shapeVertices[1] = new VertexPositionNormalTexture(
                bottomLeftFront, frontNormal, textureBottomLeft);
            shapeVertices[2] = new VertexPositionNormalTexture(
                topRightFront, frontNormal, textureTopRight);
            shapeVertices[3] = new VertexPositionNormalTexture(
                bottomLeftFront, frontNormal, textureBottomLeft);
            shapeVertices[4] = new VertexPositionNormalTexture(
                bottomRightFront, frontNormal, textureBottomRight);
            shapeVertices[5] = new VertexPositionNormalTexture(
                topRightFront, frontNormal, textureTopRight);

            // Back face.
            shapeVertices[6] = new VertexPositionNormalTexture(
                topLeftBack, backNormal, textureTopRight);
            shapeVertices[7] = new VertexPositionNormalTexture(
                topRightBack, backNormal, textureTopLeft);
            shapeVertices[8] = new VertexPositionNormalTexture(
                bottomLeftBack, backNormal, textureBottomRight);
            shapeVertices[9] = new VertexPositionNormalTexture(
                bottomLeftBack, backNormal, textureBottomRight);
            shapeVertices[10] = new VertexPositionNormalTexture(
                topRightBack, backNormal, textureTopLeft);
            shapeVertices[11] = new VertexPositionNormalTexture(
                bottomRightBack, backNormal, textureBottomLeft);

            // Top face.
            shapeVertices[12] = new VertexPositionNormalTexture(
                topLeftFront, topNormal, textureBottomLeft);
            shapeVertices[13] = new VertexPositionNormalTexture(
                topRightBack, topNormal, textureTopRight);
            shapeVertices[14] = new VertexPositionNormalTexture(
                topLeftBack, topNormal, textureTopLeft);
            shapeVertices[15] = new VertexPositionNormalTexture(
                topLeftFront, topNormal, textureBottomLeft);
            shapeVertices[16] = new VertexPositionNormalTexture(
                topRightFront, topNormal, textureBottomRight);
            shapeVertices[17] = new VertexPositionNormalTexture(
                topRightBack, topNormal, textureTopRight);

            // Bottom face.
            shapeVertices[18] = new VertexPositionNormalTexture(
                bottomLeftFront, bottomNormal, textureTopLeft);
            shapeVertices[19] = new VertexPositionNormalTexture(
                bottomLeftBack, bottomNormal, textureBottomLeft);
            shapeVertices[20] = new VertexPositionNormalTexture(
                bottomRightBack, bottomNormal, textureBottomRight);
            shapeVertices[21] = new VertexPositionNormalTexture(
                bottomLeftFront, bottomNormal, textureTopLeft);
            shapeVertices[22] = new VertexPositionNormalTexture(
                bottomRightBack, bottomNormal, textureBottomRight);
            shapeVertices[23] = new VertexPositionNormalTexture(
                bottomRightFront, bottomNormal, textureTopRight);

            // Left face.
            shapeVertices[24] = new VertexPositionNormalTexture(
                topLeftFront, leftNormal, textureTopRight);
            shapeVertices[25] = new VertexPositionNormalTexture(
                bottomLeftBack, leftNormal, textureBottomLeft);
            shapeVertices[26] = new VertexPositionNormalTexture(
                bottomLeftFront, leftNormal, textureBottomRight);
            shapeVertices[27] = new VertexPositionNormalTexture(
                topLeftBack, leftNormal, textureTopLeft);
            shapeVertices[28] = new VertexPositionNormalTexture(
                bottomLeftBack, leftNormal, textureBottomLeft);
            shapeVertices[29] = new VertexPositionNormalTexture(
                topLeftFront, leftNormal, textureTopRight);

            // Right face.
            shapeVertices[30] = new VertexPositionNormalTexture(
                topRightFront, rightNormal, textureTopLeft);
            shapeVertices[31] = new VertexPositionNormalTexture(
                bottomRightFront, rightNormal, textureBottomLeft);
            shapeVertices[32] = new VertexPositionNormalTexture(
                bottomRightBack, rightNormal, textureBottomRight);
            shapeVertices[33] = new VertexPositionNormalTexture(
                topRightBack, rightNormal, textureTopRight);
            shapeVertices[34] = new VertexPositionNormalTexture(
                topRightFront, rightNormal, textureTopLeft);
            shapeVertices[35] = new VertexPositionNormalTexture(
                bottomRightBack, rightNormal, textureBottomRight);
        }

        /// <summary>
        /// Draws the shape onto a graphics device
        /// </summary>
        /// <param name="device">The graphics device to draw the shape onto</param>
        public void RenderShape(GraphicsDevice device)
        {
            BuildShape();

            shapeBuffer = new VertexBuffer(device,
                VertexPositionNormalTexture.SizeInBytes * shapeVertices.Length,
                BufferUsage.WriteOnly);
            shapeBuffer.SetData(shapeVertices);

            device.Vertices[0].SetSource(shapeBuffer, 0,
                VertexPositionNormalTexture.SizeInBytes);
            device.VertexDeclaration = new VertexDeclaration(
                device, VertexPositionNormalTexture.VertexElements);
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, shapeTriangles);
        }

    }
}
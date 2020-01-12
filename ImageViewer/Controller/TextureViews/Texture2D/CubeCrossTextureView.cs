﻿using ImageFramework.DirectX;
using ImageFramework.Model.Shader;
using ImageFramework.Utility;
using ImageViewer.Controller.TextureViews.Shared;
using ImageViewer.Models;
using SharpDX;

namespace ImageViewer.Controller.TextureViews.Texture2D
{
    class CubeCrossTextureView : PlainTextureView
    {
        public CubeCrossTextureView(ModelsEx models, TextureViewData data)
        : base(models, data, ShaderBuilder.Builder2D)
        {}

        public override void Draw(int id, ITexture texture)
        {
            if (texture == null) return;

            var mip = models.Display.ActiveMipmap;
            // -x
            DrawLayer(Matrix.Translation(-2.0f, 0.0f, 0.0f), 1, texture.GetSrView(1, mip));
            // -y
            DrawLayer(Matrix.Translation(0.0f, -2.0f, 0.0f), 3, texture.GetSrView(3, mip));
            // +y
            DrawLayer(Matrix.Translation(0.0f, 2.0f, 0.0f), 2, texture.GetSrView(2, mip));
            // +z
            DrawLayer(Matrix.Translation(0.0f, 0.0f, 0.0f), 4, texture.GetSrView(4, mip));
            // +x
            DrawLayer(Matrix.Translation(2.0f, 0.0f, 0.0f), 0, texture.GetSrView(0, mip));
            // -z
            DrawLayer(Matrix.Translation(4.0f, 0.0f, 0.0f), 5, texture.GetSrView(5, mip));
        }

        public override Size3 GetTexelPosition(Vector2 mouse)
        {
            var transMouse = GetDirectXMouseCoordinates(mouse);

            // on which layer are the mouse coordinates?
            // layer 4 is between [-1, 1]

            // clamp mouse coordinates between -1 and 1 + set the layer
            if (transMouse.X < -1.0f)
            {
                // layer 1
                models.Display.ActiveLayer = 1;
                transMouse.X += 2.0f;
            }
            else if (transMouse.X > 1.0f)
            {
                // layer 0 or 5
                if (transMouse.X > 3.0f)
                {
                    // layer 5
                    models.Display.ActiveLayer = 5;
                    transMouse.X -= 4.0f;
                }
                else
                {
                    models.Display.ActiveLayer = 0;
                    transMouse.X -= 2.0f;
                }
            }
            else
            {
                // layer 2, 3 or 4
                if (transMouse.Y > 1.0f)
                {
                    // layer 3
                    models.Display.ActiveLayer = 3;
                    transMouse.Y -= 2.0f;
                }
                else if (transMouse.Y < -1.0f)
                {
                    // layer 2
                    models.Display.ActiveLayer = 2;
                    transMouse.Y += 2.0f;
                }
                else
                {
                    models.Display.ActiveLayer = 4;
                }
            }

            var pt = Utility.CanonicalToTexelCoordinates(transMouse.X, transMouse.Y,
                models.Images.GetWidth(models.Display.ActiveMipmap),
                models.Images.GetHeight(models.Display.ActiveMipmap));

            return new Size3(pt.X, pt.Y, 0);
        }
    }
}

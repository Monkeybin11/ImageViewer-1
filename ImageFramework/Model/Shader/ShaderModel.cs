﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageFramework.DirectX;
using ImageFramework.ImageLoader;

namespace ImageFramework.Model.Shader
{
    public class ShaderModel : IDisposable
    {
        private QuadShader quad = new QuadShader();
        private ConvertFormatShader convert = new ConvertFormatShader();

        public ShaderModel()
        {

        }

        /// <summary>
        /// creates a new resource with the given format
        /// </summary>
        /// <param name="texture">src texture data</param>
        /// <param name="dstFormat">dst format</param>
        /// <returns></returns>
        public TextureArray2D Convert(TextureArray2D texture, SharpDX.DXGI.Format dstFormat)
        {
            Debug.Assert(ImageFormat.IsSupported(dstFormat));
            Debug.Assert(ImageFormat.IsSupported(texture.Format));

            if (texture.Format == dstFormat) return texture.Clone();

            var res = new TextureArray2D(
                texture.NumLayers, texture.NumMipmaps, 
                texture.Width, texture.Height,
                dstFormat
            );

            var dev = Device.Get();
            dev.Vertex.Set(quad.Vertex);
            dev.Pixel.Set(convert.Pixel);

            for (int curLayer = 0; curLayer < texture.NumLayers; ++curLayer)
            {
                for (int curMipmap = 0; curMipmap < texture.NumMipmaps; ++curMipmap)
                {
                    dev.Pixel.SetShaderResource(ConvertFormatShader.InputSrvBinding, texture.GetSrView(curLayer, curMipmap));
                    dev.OutputMerger.SetRenderTargets(res.GetRtView(curLayer, curMipmap));
                    dev.SetViewScissors(texture.GetWidth(curMipmap), texture.GetHeight(curMipmap));
                    dev.DrawQuad();
                }
            }

            return res;
        }

        public void Dispose()
        {
            quad?.Dispose();
        }
    }
}
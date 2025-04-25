using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Utility;

public static class Texture2DExtensions
{
    public static IntPtr GetOpenGlTextureHandle(this Texture2D texture)
    {
        if (texture == null) return IntPtr.Zero;

        var internalTexture = texture.GetType()
            .GetField("_texture", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(texture);

        if (internalTexture == null) return IntPtr.Zero;

        var textureHandle = (IntPtr)internalTexture.GetType()
            .GetField("TextureHandle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(internalTexture)!;

        return textureHandle;
    }
}
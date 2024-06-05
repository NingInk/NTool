using UnityEngine;
using UnityEngine.UI;

namespace NTool.Extensions
{
    public static class TextureExtension
    {
        #region Image

        public static void LoadSpriteByPath(this Image image, string path)
        {
            var tex = new Texture2D(0, 0);
            if (System.IO.File.Exists(path))
            {
                tex.LoadImage(System.IO.File.ReadAllBytes(path));
                image.sprite = tex.ToSprite();
            }
            else
            {
                Debug.Log($"路径({path})不存在");
                image.sprite = null;
            }
        }

        #endregion

        /// <summary>
        /// Texture2D 转成 Spite
        /// </summary>
        /// <param name="texture">用于获得精灵图形的纹理。</param>
        /// <param name="pivot">精灵相对于其图形矩形的枢轴点。</param>
        /// <returns></returns>
        public static Sprite ToSprite(this Texture2D texture, Vector2 pivot = default(Vector2))
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);
        }

        /// <summary>
        /// 将Texture解压到Texture2D
        /// 解决部分Texture2D转换错误
        /// </summary>
        /// <param name="source"></param>
        /// <returns>byte[]</returns>
        public static Texture2D DeCompress(Texture source)
        {
            if (source == null)
            {
                return null;
            }

            var renderTex = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear
            );
            Graphics.Blit(source, renderTex);
            var previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            var readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }

        /// <summary>
        /// 裁剪纹理--待测试
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Texture2D SplitTexture(this Texture2D source, Rect rect)
        {
            var croppedTexture = new Texture2D((int)rect.width, (int)rect.height);
            croppedTexture.SetPixels(
                source.GetPixels((int)rect.xMin, (int)rect.yMin, (int)rect.width, (int)rect.height)
            );
            croppedTexture.Apply();
            return croppedTexture;
        }

        public static Vector2 Size(this Texture texture)
        {
            return new Vector2(texture.width, texture.height);
        }

        public static Texture2D CaptureCamera(this Camera camera, Rect rect)
        {
            var renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;

            var screenShot = new Texture2D(
                (int)rect.width,
                (int)rect.height,
                TextureFormat.RGB24,
                false
            );
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.Destroy(renderTexture);

            return screenShot;
        }

        public static Color HtmlStringToColor(this string htmlString)
        {
            var parseSucceed = ColorUtility.TryParseHtmlString(htmlString, out var retColor);
            return parseSucceed ? retColor : Color.black;
        }
    }
}

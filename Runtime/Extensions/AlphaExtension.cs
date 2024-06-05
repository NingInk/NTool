using UnityEngine;
using UnityEngine.UI;

namespace NTool.Extensions
{
    public static class AlphaExtension
    {
        /// <summary>
        /// 修改Graphic的颜色透明度
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="alpha">透明度</param>
        public static Graphic ToAlpha(this Graphic target, float alpha)
        {
            target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);
            return target;
        }

        /// <summary>
        /// 设置颜色透明度
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="alpha">透明度</param>
        /// <returns>修改透明度后的颜色</returns>
        public static Color ToAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static Image FillAmount(this Image selfImage, float fillAmount)
        {
            selfImage.fillAmount = fillAmount;
            return selfImage;
        }

        public static CanvasGroup Show(this CanvasGroup cg) => cg.Set(1, true, true);

        public static CanvasGroup Hide(this CanvasGroup cg) => cg.Set(0, false, false);

        public static CanvasGroup Set(
            this CanvasGroup cg,
            float alpha,
            bool interactable,
            bool blocksRaycasts
        )
        {
            cg.alpha = alpha;
            cg.interactable = interactable;
            cg.blocksRaycasts = blocksRaycasts;
            return cg;
        }

        public static SpriteRenderer Alpha(this SpriteRenderer self, float alpha)
        {
            var color = self.color;
            color.a = alpha;
            self.color = color;
            return self;
        }
    }
}

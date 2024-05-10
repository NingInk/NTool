using UnityEngine;
using UnityEngine.UI;

namespace NTool.Extensions
{
    public static class AlphaMethods
    {
        /// <summary>
        /// 修改Graphic的颜色透明度
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="alpha">透明度</param>
        public static void ToAlpha(this Graphic target, float alpha) =>
            target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);

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
    }
}
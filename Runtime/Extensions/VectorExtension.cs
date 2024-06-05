using UnityEngine;

namespace NTool.Extensions
{
    public static class VectorExtension
    {
        #region 旋转

        public static Vector3 RotateAngleXY(Vector3 point, Vector3 center, float angle, float z = 0)
        {
            return RotateAngle(point.XY(), center.XY(), angle).AddZ(z);
        }

        public static Vector3 RotateAngleXZ(Vector3 point, Vector3 center, float angle, float y = 0)
        {
            return RotateAngle(point.XZ(), center.XZ(), angle).AddY(y);
        }

        public static Vector3 RotateAngleYZ(Vector3 point, Vector3 center, float angle, float x = 0)
        {
            return RotateAngle(point.YZ(), center.YZ(), angle).AddX(x);
        }

        public static Vector2 RotateAngle(Vector2 point, Vector2 center, float angle)
        {
            angle *= Mathf.Deg2Rad;
            var x =
                (point.x - center.x) * Mathf.Cos(angle)
                + (point.y - center.y) * Mathf.Sin(angle)
                + center.x;
            var y =
                -(point.x - center.x) * Mathf.Sin(angle)
                + (point.y - center.y) * Mathf.Cos(angle)
                + center.y;
            return new Vector2(x, y);
        }

        #endregion

        #region Area

        /// <summary>
        /// 获取中心点
        /// </summary>
        /// <param name="rect"></param>
        public static Vector2 GetCenter(Vector2[] rect)
        {
            return GetCenter(rect, out var _, out var _);
        }

        public static Rect GetRect(Vector2[] rect)
        {
            GetCenter(rect, out var max, out var min);
            //return new Rect(min, max - min);
            return new Rect(min.x, min.y, max.x, max.y);
        }

        public static Vector2 GetCenter(Vector2[] rect, out Vector2 max, out Vector2 min)
        {
            if (rect.Length < 1)
            {
                max = min = Vector2.zero;
                return Vector3.zero;
            }

            min.x = max.x = rect[0].x;
            min.y = max.y = rect[0].y;
            for (var aaa = 1; aaa < rect.Length; aaa++)
            {
                if (rect[aaa].x < min.x)
                    min.x = rect[aaa].x;
                if (rect[aaa].x > max.x)
                    max.x = rect[aaa].x;
                if (rect[aaa].y < min.y)
                    min.y = rect[aaa].y;
                if (rect[aaa].y > max.y)
                    max.y = rect[aaa].y;
            }

            return new Vector2((min.x + max.x) / 2, (min.y + max.y) / 2);
        }

        /// <summary>
        /// 判断点pos在区域rect里吗
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool Polygon(Vector2 pos, Vector2[] rect)
        {
            int i,
                j;
            var c = false;
            for (i = 0, j = rect.Length - 1; i < rect.Length; j = i++)
            {
                if (
                    ((rect[i].y > pos.y) != (rect[j].y > pos.y))
                    && (
                        pos.x
                        < (rect[j].x - rect[i].x) * (pos.y - rect[i].y) / (rect[j].y - rect[i].y)
                            + rect[i].x
                    )
                )
                {
                    c = !c;
                }
            }

            return c;
        }

        /// <summary>
        /// 将target按照比例限制在Limit的范围内
        /// </summary>
        /// <param name="limit">限制的最大值</param>
        /// <param name="target">被限制的目标</param>
        /// <returns>限制后的值</returns>
        public static Vector2 LimitSize(Vector2 limit, Vector2 target)
        {
            var size = target;
            if (target.x > limit.x)
            {
                size.x = limit.x;
                size.y = limit.x / target.x * target.y;
            }

            if (size.y > limit.y)
            {
                size.y = limit.y;
                size.x = limit.y / size.y * size.x;
            }

            return size;
        }

        public static Vector3 LimitPosition(Vector3 pos, Vector3 min, Vector3 max)
        {
            pos.x = Mathf.Clamp(pos.x, min.x, max.x);
            pos.y = Mathf.Clamp(pos.y, min.y, max.y);
            pos.z = Mathf.Clamp(pos.z, min.z, max.z);
            return pos;
        }

        #endregion

        /// <summary> 降维 </summary>
        public static Vector2 XZ(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }

        /// <summary> 降维 </summary>
        public static Vector2 XY(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }

        /// <summary> 降维 </summary>
        public static Vector2 YZ(this Vector3 vector3)
        {
            return new Vector2(vector3.y, vector3.z);
        }

        /// <summary> 增维 </summary>
        public static Vector3 AddX(this Vector2 vector2, float x = 0)
        {
            return new Vector3(x, vector2.x, vector2.y);
        }

        /// <summary> 增维 </summary>
        public static Vector3 AddY(this Vector2 vector2, float y = 0)
        {
            return new Vector3(vector2.x, y, vector2.y);
        }

        /// <summary> 增维 </summary>
        public static Vector3 AddZ(this Vector2 vector2, float z = 0)
        {
            return new Vector3(vector2.x, vector2.y, z);
        }

        #region QFramework

        /// <summary> 朝向目标的矢量 </summary>
        public static Vector3 DirectionTo(this Component self, Component to) =>
            to.transform.position - self.transform.position;

        /// <summary> 朝向目标的矢量 </summary>
        public static Vector3 DirectionTo(this GameObject self, GameObject to) =>
            to.transform.position - self.transform.position;

        /// <summary> 朝向目标的矢量 </summary>
        public static Vector3 DirectionTo(this Component self, GameObject to) =>
            to.transform.position - self.transform.position;

        /// <summary> 朝向目标的矢量 </summary>
        public static Vector3 DirectionTo(this GameObject self, Component to) =>
            to.transform.position - self.transform.position;

        /// <summary> 朝向源的矢量 </summary>
        public static Vector3 DirectionFrom(this Component self, Component from) =>
            self.transform.position - from.transform.position;

        /// <summary> 朝向源的矢量 </summary>
        public static Vector3 DirectionFrom(this GameObject self, GameObject from) =>
            self.transform.position - from.transform.position;

        /// <summary> 朝向源的矢量 </summary>
        public static Vector3 DirectionFrom(this GameObject self, Component from) =>
            self.transform.position - from.transform.position;

        /// <summary> 朝向源的矢量 </summary>
        public static Vector3 DirectionFrom(this Component self, GameObject from) =>
            self.transform.position - from.transform.position;

        /// <summary> 朝向目标的单位矢量 </summary>
        public static Vector3 NormalizedDirectionTo(this Component self, Component to) =>
            self.DirectionTo(to).normalized;

        /// <summary> 朝向目标的单位矢量 </summary>
        public static Vector3 NormalizedDirectionTo(this GameObject self, GameObject to) =>
            self.DirectionTo(to).normalized;

        /// <summary> 朝向目标的单位矢量 </summary>
        public static Vector3 NormalizedDirectionTo(this Component self, GameObject to) =>
            self.DirectionTo(to).normalized;

        /// <summary> 朝向目标的单位矢量 </summary>
        public static Vector3 NormalizedDirectionTo(this GameObject self, Component to) =>
            self.DirectionTo(to).normalized;

        /// <summary> 朝向源的单位矢量 </summary>
        public static Vector3 NormalizedDirectionFrom(this Component self, Component from) =>
            self.DirectionFrom(from).normalized;

        /// <summary> 朝向源的单位矢量 </summary>
        public static Vector3 NormalizedDirectionFrom(this GameObject self, GameObject from) =>
            self.DirectionFrom(from).normalized;

        /// <summary> 朝向源的单位矢量 </summary>
        public static Vector3 NormalizedDirectionFrom(this GameObject self, Component from) =>
            self.DirectionFrom(from).normalized;

        /// <summary> 朝向源的单位矢量 </summary>
        public static Vector3 NormalizedDirectionFrom(this Component self, GameObject from) =>
            self.DirectionFrom(from).normalized;

        /// <summary> 设置维度值 </summary>
        public static Vector3 X(this Vector3 self, float x)
        {
            self.x = x;
            return self;
        }

        /// <summary> 设置维度值 </summary>
        public static Vector3 Y(this Vector3 self, float y)
        {
            self.y = y;
            return self;
        }

        /// <summary> 设置维度值 </summary>
        public static Vector3 Z(this Vector3 self, float z)
        {
            self.z = z;
            return self;
        }

        /// <summary> 设置维度值 </summary>
        public static Vector2 X(this Vector2 self, float x)
        {
            self.x = x;
            return self;
        }

        /// <summary> 设置维度值 </summary>
        public static Vector2 Y(this Vector2 self, float y)
        {
            self.y = y;
            return self;
        }

        /// <summary> 世界坐标之间的距离 </summary>
        public static float Distance(this GameObject self, GameObject other)
        {
            return Vector3.Distance(self.Position(), other.Position());
        }

        /// <summary> 世界坐标之间的距离 </summary>
        public static float Distance(this Component self, GameObject other)
        {
            return Vector3.Distance(self.Position(), other.Position());
        }

        /// <summary> 世界坐标之间的距离 </summary>
        public static float Distance(this GameObject self, Component other)
        {
            return Vector3.Distance(self.Position(), other.Position());
        }

        /// <summary> 世界坐标之间的距离 </summary>
        public static float Distance(this Component self, Component other)
        {
            return Vector3.Distance(self.Position(), other.Position());
        }

        /// <summary> 世界坐标之间的2D距离 </summary>
        public static float Distance2D(this GameObject self, GameObject other)
        {
            return Vector2.Distance(self.Position2D(), other.Position2D());
        }

        /// <summary> 世界坐标之间的2D距离 </summary>
        public static float Distance2D(this Component self, GameObject other)
        {
            return Vector2.Distance(self.Position2D(), other.Position2D());
        }

        /// <summary> 世界坐标之间的2D距离 </summary>
        public static float Distance2D(this GameObject self, Component other)
        {
            return Vector2.Distance(self.Position2D(), other.Position2D());
        }

        /// <summary> 世界坐标之间的2D距离 </summary>
        public static float Distance2D(this Component self, Component other)
        {
            return Vector2.Distance(self.Position2D(), other.Position2D());
        }

        /// <summary> 局部坐标之间的距离 </summary>
        public static float LocalDistance(this GameObject self, GameObject other)
        {
            return Vector3.Distance(self.LocalPosition(), other.LocalPosition());
        }

        /// <summary> 局部坐标之间的距离 </summary>
        public static float LocalDistance(this Component self, GameObject other)
        {
            return Vector3.Distance(self.LocalPosition(), other.LocalPosition());
        }

        /// <summary> 局部坐标之间的距离 </summary>
        public static float LocalDistance(this GameObject self, Component other)
        {
            return Vector3.Distance(self.LocalPosition(), other.LocalPosition());
        }

        /// <summary> 局部坐标之间的距离 </summary>
        public static float LocalDistance(this Component self, Component other)
        {
            return Vector3.Distance(self.LocalPosition(), other.LocalPosition());
        }

        /// <summary> 局部坐标之间的2D距离 </summary>
        public static float LocalDistance2D(this GameObject self, GameObject other)
        {
            return Vector2.Distance(self.LocalPosition2D(), other.LocalPosition2D());
        }

        /// <summary> 局部坐标之间的2D距离 </summary>
        public static float LocalDistance2D(this Component self, GameObject other)
        {
            return Vector2.Distance(self.LocalPosition2D(), other.LocalPosition2D());
        }

        /// <summary> 局部坐标之间的2D距离 </summary>
        public static float LocalDistance2D(this GameObject self, Component other)
        {
            return Vector2.Distance(self.LocalPosition2D(), other.LocalPosition2D());
        }

        /// <summary> 局部坐标之间的2D距离 </summary>
        public static float LocalDistance2D(this Component self, Component other)
        {
            return Vector2.Distance(self.LocalPosition2D(), other.LocalPosition2D());
        }

        #endregion
    }
}

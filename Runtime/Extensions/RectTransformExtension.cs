using Cysharp.Threading.Tasks;
using System.Linq;
using Unity.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NTool.Extensions
{
    public static class RectTransformExtension
    {
        /// <summary>
        /// 获取鼠标在世界坐标的位置
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static Vector3 GetMousePOSOnWorld(this RectTransform rect, Camera cam = null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                rect,
                Input.mousePosition,
                cam,
                out var pos
            );
            return pos;
        }

        /// <summary>
        /// 获取鼠标在在UI上的位置
        /// </summary>
        /// <param name="rect">UI</param>
        /// <param name="cam">关联摄像头</param>
        /// <returns></returns>
        public static Vector2 GetMousePosOnUI(this RectTransform rect, Camera cam = null)
        {
            return GetScreenPosOnUI(rect, Input.mousePosition, cam);
        }

        /// <summary>
        /// 获取屏幕坐标在UI上的位置
        /// </summary>
        /// <param name="rect">坐标相对的父物体</param>
        /// <param name="screenPoint">屏幕坐标</param>
        /// <param name="cam">Rect所在Canvas的Camera</param>
        /// <returns></returns>
        public static Vector2 GetScreenPosOnUI(
            this RectTransform rect,
            Vector2 screenPoint,
            Camera cam = null
        )
        {
            if (cam == null)
            {
                Canvas c = rect.GetComponentInParent<Canvas>();
                if (c.renderMode == RenderMode.ScreenSpaceCamera)
                    cam = c.worldCamera;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect,
                screenPoint,
                cam,
                out Vector2 pos
            );
            return pos;
        }

        /// <summary>
        /// 跟随鼠标屏幕坐标
        /// </summary>
        /// <param name="self">跟随鼠标的UI</param>
        /// <param name="offset">UI中心点到鼠标位置的偏移值</param>
        /// <param name="limit">限制范围在屏幕坐标内</param>
        public static void FollowMousePosition(
            this RectTransform self,
            Vector2 offset = default,
            bool limit = true
        )
        {
            self.FollowScreenPosition(Input.mousePosition, offset, limit: limit);
        }

        /// <summary>
        /// 跟随鼠标屏幕坐标
        /// </summary>
        /// <param name="self">跟随鼠标的UI</param>
        /// <param name="offset">UI中心点到鼠标位置的偏移值</param>
        /// <param name="camera">指定摄像机</param>
        /// <param name="limit">限制范围在屏幕坐标内</param>
        public static void FollowMousePosition(
            this RectTransform self,
            Vector2 offset,
            Camera camera,
            bool limit = true
        )
        {
            self.FollowScreenPosition(Input.mousePosition, offset, camera, limit);
        }

        /// <summary>
        /// UI跟随屏幕位置
        /// </summary>
        /// <param name="self">指定UI</param>
        /// <param name="screenPoint">屏幕坐标</param>
        /// <param name="offset">偏移值</param>
        /// <param name="camera">摄像机</param>
        /// <param name="limit">限制范围在屏幕坐标内</param>
        public static void FollowScreenPosition(
            this RectTransform self,
            Vector2 screenPoint,
            Vector2 offset,
            Camera camera = null,
            bool limit = true
        )
        {
            RectTransform parent = ((RectTransform)self.transform.parent);
            // self.anchorMax = self.anchorMin = parent.pivot; //修改自身锚点到父对象的中心点
            // // 更新自身坐标    鼠标位置=》相对于父对象的本地坐标+偏移
            // self.anchoredPosition = UtilityTool.GetMousePosOnUI(parent, cam) + offset;
            // 鼠标位置=》相对于父对象的本地坐标=》世界坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                screenPoint,
                camera,
                out Vector2 pos
            );
            self.position = parent.TransformPoint(pos);
            // 当前坐标+偏移
            self.anchoredPosition += offset;
            if (limit)
            {
                var minX = self.pivot.x * self.sizeDelta.x;
                var maxX = Screen.width - (self.sizeDelta.x - minX);
                var minY = self.pivot.y * self.sizeDelta.y;
                var maxY = Screen.height - (self.sizeDelta.y - minY);
                self.position = new Vector3(
                    Mathf.Clamp(self.position.x, minX, maxX),
                    Mathf.Clamp(self.position.y, minY, maxY),
                    self.position.z
                );
            }
        }

        /// <summary>
        /// 获取继承UIBehaviour对象的RectTransform
        /// ！！！确定当前的对象存在RectTransform的情况下调用
        /// </summary>
        /// <param name="ub"></param>
        /// <returns></returns>
        public static RectTransform RectTransform(this UIBehaviour ub)
        {
            try
            {
                return ub.transform as RectTransform;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
                Debug.Log(ub);
                Debug.Log(ub.name);
                throw;
            }
        }

        /// <summary>
        /// 获取当前transform的RectTransform
        /// ！！！确定当前的对象存在RectTransform的情况下调用
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static RectTransform RectTransform(this Transform trans)
        {
            return trans as RectTransform;
        }

        /// <summary>
        /// 如果rt的四个顶点都包含在这个rectTransform中，则返回true
        /// </summary>
        /// <param name="container">主体</param>
        /// <param name="rt">被计算的Rect</param>
        /// <param name="drawLine">划线</param>
        /// <returns></returns>
        public static bool Contains(
            this RectTransform container,
            RectTransform rt,
            bool drawLine = false
        )
        {
            // 获取容器的四个顶点
            var containerCorners = new Vector3[4];
            container.GetWorldCorners(containerCorners);
            // 获取容器宽高
            var width = Mathf.Abs(containerCorners[2].x - containerCorners[0].x);
            var height = Mathf.Abs(containerCorners[2].y - containerCorners[0].y);
            var rect = new Rect(containerCorners[0].x, containerCorners[0].y, width, height);
            // 获取要判断UI的四个顶点
            var rtCorners = new Vector3[4];
            rt.GetWorldCorners(rtCorners);
            if (drawLine)
            {
                for (var i = 0; i < 4; i++)
                {
                    Debug.DrawLine(
                        containerCorners[i],
                        containerCorners[i + 1 >= 4 ? 0 : i + 1],
                        Color.red,
                        100
                    );
                    Debug.DrawLine(
                        rtCorners[i],
                        rtCorners[i + 1 >= 4 ? 0 : i + 1],
                        Color.green,
                        100
                    );
                }
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPaused = true;
#endif
            }

            // 依次判断四个顶点是否都在矩形中
            foreach (var corner in rtCorners)
            {
                if (!rect.Contains(corner))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 如果point包含在这个rectTransform中，则返回true
        /// </summary>
        /// <param name="container">主体</param>
        /// <param name="point">被计算的Rect</param>
        /// <param name="drawLine">划线</param>
        /// <returns></returns>
        public static bool Contains(
            this RectTransform container,
            Vector2 point,
            bool drawLine = false
        )
        {
            // 获取容器的四个顶点
            var containerCorners = new Vector3[4];
            container.GetWorldCorners(containerCorners);
            // 获取容器宽高
            var width = Mathf.Abs(containerCorners[2].x - containerCorners[0].x);
            var height = Mathf.Abs(containerCorners[2].y - containerCorners[0].y);
            var rect = new Rect(containerCorners[0].x, containerCorners[0].y, width, height);
            // 获取要判断UI的四个顶点
            if (!drawLine)
                return rect.Contains(point);
            for (var i = 0; i < 4; i++)
            {
                Debug.DrawLine(
                    containerCorners[i],
                    containerCorners[i + 1 >= 4 ? 0 : i + 1],
                    Color.red,
                    100
                );
                Debug.DrawLine(point, containerCorners[i], Color.green, 100);
            }
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif

            return rect.Contains(point);
        }

        #region 重构UI、延迟

        /// <summary>
        /// 强制执行UI自适应
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static async UniTask ReBuild(this RectTransform source)
        {
            if (source == null)
                return;
            var layoutGroups = source.gameObject.AncestorsAndSelf().OfComponent<LayoutGroup>();
            foreach (var lg in layoutGroups)
            {
                await UniTask.Yield();
                LayoutRebuilder.ForceRebuildLayoutImmediate(lg.RectTransform());
            }
        }

        /// <summary>
        /// 强制执行UI自适应
        /// 从指定位置开始向下深度遍历所有子对象向上回收
        /// </summary>
        /// <param name="source"></param>
        public static async UniTask ReBuildDepth(this RectTransform source)
        {
            var layoutGroups = source
                .gameObject.DescendantsAndSelf()
                .OfComponent<LayoutGroup>()
                .Reverse();
            foreach (var lg in layoutGroups)
            {
                if (!lg.gameObject.activeInHierarchy)
                    continue;
                await UniTask.Yield();
                LayoutRebuilder.ForceRebuildLayoutImmediate(lg.RectTransform());
            }
        }

        /// <summary>
        /// 强制执行UI自适应
        /// 从指定位置开始向下广度遍历所有子对象向上回收布局
        /// </summary>
        /// <param name="source">开始节点</param>
        /// <param name="maximumDepth">最大深度，负值为无限深</param>
        public static async UniTask ReBuildBreadth(this RectTransform source, int maximumDepth = -1)
        {
            if (maximumDepth > 0)
            {
                maximumDepth--;
            }
            else if (maximumDepth == 0)
            {
                return;
            }

            var children = source.gameObject.Children();
            if (children.Any())
            {
                var tasks = children
                    .OfComponent<RectTransform>()
                    .Select(r => ReBuildBreadth(r, maximumDepth));
                await UniTask.WhenAll(tasks);
            }

            var layoutGroups = children
                .OfComponent<LayoutGroup>()
                .Where(lg => lg.gameObject.activeInHierarchy)
                .ToArray();
            if (layoutGroups.Any())
            {
                foreach (var lg in layoutGroups)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(lg.RectTransform());
                }

                await UniTask.Yield();
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace NTool.Utility
{
    public class UnityUtils
    {
        public static void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        /// <summary>
        /// 获取当前点击的UI
        /// </summary>
        /// <returns></returns>
        public static List<RaycastResult> OnClickUI()
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results;
        }

        /// <summary>
        /// 交换子对象的父物体
        /// </summary>
        /// <param name="target1"></param>
        /// <param name="target2"></param>
        public static void ExChangeParent(Transform target1, Transform target2)
        {
            Debug.Log(target1.parent + " " + target2.parent);
            var temp = target1.parent;
            target1.SetParent(target2.parent);
            target2.SetParent(temp);
        }

        #region 射线检测

        /// <summary>
        /// 获取鼠标射线检测到的所有对象
        /// </summary>
        /// <returns></returns>
        public static RaycastHit[] GetMouseAllHit(int layerMask = -5)
        {
            var results = Array.Empty<RaycastHit>();
            if (Camera.main != null)
            {
                Physics.RaycastNonAlloc(
                    Camera.main.ScreenPointToRay(Input.mousePosition),
                    results,
                    Mathf.Infinity,
                    layerMask
                );
            }

            return results;
        }

        /// <summary>
        /// 获取鼠标射线检测到的对象
        /// </summary>
        /// <returns></returns>
        public static RaycastHit GetMouseHit(int layerMask = -5) =>
            Camera.main != null
                ? GetRayHit(Camera.main.ScreenPointToRay(Input.mousePosition), layerMask)
                : default;

        /// <summary>
        /// 获取指定射线检测到的对象
        /// </summary>
        /// <param name="ray">指定射线</param>
        /// <param name="layerMask">遮罩层级</param>
        /// <returns></returns>
        public static RaycastHit GetRayHit(Ray ray, int layerMask = -5) =>
            Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask) ? hit : default;

        #endregion

        #region 道路

        public static Vector3 GetPointInTerrain(float x, float y)
        {
            var hit = GetRayHit(new Ray(new Vector3(x, 1000, y), Vector3.down));
            var pos = Vector3.zero;
            var vv = hit.point;
            var terrainData = hit.transform.GetComponent<Terrain>().terrainData;
            if (hit.transform.GetComponent<Terrain>())
            {
                var yyy = terrainData.GetHeight(
                    (int)(
                        (vv.x - hit.transform.position.x)
                        / terrainData.size.x
                        * terrainData.heightmapResolution
                    ),
                    (int)(
                        (vv.z - hit.transform.position.z)
                        / terrainData.size.z
                        * terrainData.heightmapResolution
                    )
                );
                pos = new Vector3(vv.x, yyy, vv.z);
                //Debug.DrawLine(pos, Vector3.zero, Color.gray);
            }

            return pos;
        }

        #endregion

        #region 图片

        /// <summary>
        /// 对相机截图。
        /// </summary>
        /// <returns>The screenshot2.</returns>
        /// <param name="camera">Camera.要被截屏的相机</param>
        /// <param name="rect">Rect.截屏的区域</param>
        /// <param name="canvas">所需画布</param>
        public static Texture2D CaptureCamera(Camera camera, Rect rect, Canvas canvas = null)
        {
            // 创建一个RenderTexture对象
            var rt = new RenderTexture( /*(int)rect.width*/
                Screen.width,
                Screen.height /*(int)rect.height*/
                ,
                0
            );
            // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
            camera.targetTexture = rt;
            if (canvas)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = camera;
            }

            camera.Render();
            //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。
            //ps: camera2.targetTexture = rt;
            //ps: camera2.Render();
            //ps: -------------------------------------------------------------------
            // 激活这个rt, 并从中中读取像素。
            RenderTexture.active = rt;
            var screenShot = new Texture2D(
                (int)rect.width,
                (int)rect.height,
                TextureFormat.ARGB32,
                false
            );
            screenShot.ReadPixels(rect, 0, 0); // 注：这个时候，它是从RenderTexture.active中读取像素
            screenShot.Apply();
            // 重置相关参数，以使用camera继续在屏幕上显示
            camera.targetTexture = null;
            //ps: camera2.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Object.Destroy(rt);
            screenShot.name = "Screenshot_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Debug.Log(string.Format($"截屏了一张照片: {screenShot.name}"));
            if (canvas)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.worldCamera = null;
            }

            return screenShot;
        }

        public static async UniTask CaptureCamera(
            Camera camera,
            Rect rect,
            Canvas canvas,
            UnityAction<Texture2D> act,
            bool inScreen = false
        )
        {
            await UniTask.Yield();
            // 创建一个RenderTexture对象
            var rt = inScreen
                ? new RenderTexture(Screen.width, Screen.height, 0)
                : new RenderTexture((int)rect.width, (int)rect.height, 0);
            // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
            camera.targetTexture = rt;
            if (canvas)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = camera;
            }

            camera.Render();
            //   UnityEditor.EditorApplication.isPaused = true;
            await UniTask.Yield();

            RenderTexture.active = rt;
            var screenShot = new Texture2D(
                (int)rect.width,
                (int)rect.height,
                TextureFormat.ARGB32,
                false
            );
            screenShot.ReadPixels(rect, 0, 0); // 注：这个时候，它是从RenderTexture.active中读取像素
            screenShot.Apply();
            Debug.Log(screenShot.width + ":" + screenShot.height);
            // 重置相关参数，以使用camera继续在屏幕上显示
            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(rt);
            screenShot.name = "Screenshot_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Debug.Log(string.Format($"截屏了一张照片: {screenShot.name}"));
            if (canvas)
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.worldCamera = null;
            }

            act?.Invoke(screenShot);
        }

        #endregion

        #region 重建

        public static void ResetList<T>(
            IEnumerable<T> list,
            Transform parent,
            UnityAction<GameObject, T> act,
            bool isActive = true
        )
        {
            var array = list.ToArray();
            ResetList(
                array.Length,
                parent,
                parent.GetChild(0).gameObject,
                (gg, i) => act?.Invoke(gg, array[i]),
                isActive
            );
        }

        public static void ResetList<T1, T2>(
            IEnumerable<T1> list,
            Transform parent,
            UnityAction<T2, T1> act,
            bool isActive = true
        )
            where T2 : MonoBehaviour
        {
            var array = list.ToArray();
            ResetList<T2>(
                array.Length,
                parent,
                parent.GetChild(0).gameObject,
                (gg, i) => act?.Invoke(gg, array[i]),
                isActive
            );
        }

        public static void ResetList<T1, T2>(
            IEnumerable<T1> list,
            Transform parent,
            GameObject ex,
            UnityAction<T2, T1> act,
            bool isActive = true
        )
            where T2 : MonoBehaviour
        {
            var array = list.ToArray();
            ResetList<T2>(array.Length, parent, ex, (gg, i) => act?.Invoke(gg, array[i]), isActive);
        }

        public static void ResetList<T>(int length, Transform parent, UnityAction<T, int> act)
            where T : MonoBehaviour
        {
            ResetList(0, length, parent, parent.GetChild(0).gameObject, act, true);
        }

        public static void ResetList<T>(
            int length,
            Transform parent,
            GameObject ex,
            UnityAction<T, int> act
        )
            where T : MonoBehaviour
        {
            ResetList(0, length, parent, ex, act, true);
        }

        public static void ResetList<T>(
            int length,
            Transform parent,
            GameObject ex,
            UnityAction<T, int> act,
            bool isActive
        )
            where T : MonoBehaviour
        {
            ResetList(0, length, parent, ex, act, isActive);
        }

        public static void ResetList<T>(
            int startIndex,
            int length,
            Transform parent,
            UnityAction<T, int> act
        )
            where T : MonoBehaviour
        {
            ResetList(
                startIndex,
                length,
                parent,
                parent.GetChild(startIndex).gameObject,
                act,
                true
            );
        }

        public static void ResetList<T>(
            int startIndex,
            int length,
            Transform parent,
            GameObject ex,
            UnityAction<T, int> act
        )
            where T : MonoBehaviour
        {
            ResetList(startIndex, length, parent, ex, act, true);
        }

        /// <summary>
        /// 根据列表重建
        /// </summary>
        /// <param name="startIndex">开始下标</param>
        /// <param name="length">列表长度</param>
        /// <param name="parent">重建的根节点</param>
        /// <param name="ex">重建对象的示例</param>
        /// <param name="act">事件</param>
        /// <param name="isActive">是否默认激活</param>
        public static void ResetList<T>(
            int startIndex,
            int length,
            Transform parent,
            GameObject ex,
            UnityAction<T, int> act,
            bool isActive
        )
            where T : MonoBehaviour
        {
            var endIndex = startIndex + length;
            for (var i = parent.childCount; i < startIndex; i++)
            {
                new GameObject("child_" + i).transform.SetParent(parent);
            }

            for (var i = startIndex; i < endIndex; i++)
            {
                var gg =
                    i < parent.childCount
                        ? parent.GetChild(i).gameObject
                        : Object.Instantiate(ex, parent);
                var t = gg.GetComponentInChildren<T>();
                act?.Invoke(t, i - startIndex);
                gg.SetActive(isActive);
            }

            for (var i = endIndex; i < parent.childCount; i++)
            {
                parent.GetChild(i)?.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 根据列表重建
        /// </summary>
        /// <param name="length">列表长度</param>
        /// <param name="parent">重建的根节点</param>
        /// <param name="ex">重建对象的示例</param>
        /// <param name="act">事件</param>
        /// <param name="isActive">是否默认激活</param>
        /// <param name="pool">池子</param>
        /// <param name="poolAct">池子事件</param>
        public static void ResetList(
            int length,
            Transform parent,
            GameObject ex,
            UnityAction<GameObject, int> act,
            bool isActive = true,
            Queue<GameObject> pool = null,
            UnityAction<GameObject> poolAct = null
        )
        {
            for (var i = 0; i < length; i++)
            {
                GameObject gg;
                if (i < parent.childCount)
                    gg = parent.GetChild(i).gameObject;
                else if (pool is { Count: > 0 })
                {
                    gg = pool.Dequeue();
                    gg.transform.SetParent(parent);
                }
                else
                    gg = Object.Instantiate(ex, parent);

                act?.Invoke(gg, i);
                gg.SetActive(isActive);
            }

            for (var i = length; i < parent.childCount; i++)
            {
                var gg = parent.GetChild(i).gameObject;
                gg.SetActive(false);
                pool?.Enqueue(gg);
            }

            if (pool is { Count: > 0 })
                foreach (var item in pool)
                {
                    poolAct?.Invoke(item);
                }
        }

        public static void ResetList(
            int length,
            Transform parent,
            UnityAction<GameObject, int> act,
            bool isActive = true
        )
        {
            ResetList(length, parent, parent.GetChild(0).gameObject, act, isActive);
        }

        /// <summary>
        /// 根据字典重建
        /// </summary>
        /// <typeparam name="T1">key</typeparam>
        /// <typeparam name="T2">value</typeparam>
        /// <typeparam name="T3">子对象身上必须的组件</typeparam>
        /// <param name="dic">字典</param>
        /// <param name="parent">重建的根节点</param>
        /// <param name="ex">重建对象的示例</param>
        /// <param name="act">事件</param>
        public static void ResetDictionary<T1, T2, T3>(
            Dictionary<T1, T2> dic,
            Transform parent,
            GameObject ex,
            UnityAction<T3, KeyValuePair<T1, T2>> act
        )
            where T3 : MonoBehaviour
        {
            var index = 0;
            foreach (var item in dic)
            {
                var gg =
                    parent.childCount > index
                        ? parent.GetChild(index).gameObject
                        : Object.Instantiate(ex, parent);
                act?.Invoke(gg.GetComponent<T3>(), item);
                gg.SetActive(true);
                index++;
            }

            for (var i = dic.Count; i < parent.childCount; i++)
            {
                parent.GetChild(i).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 根据字典重建
        /// </summary>
        /// <typeparam name="T1">key</typeparam>
        /// <typeparam name="T2">value</typeparam>
        /// <param name="dic">字典</param>
        /// <param name="parent">重建的根节点</param>
        /// <param name="ex">重建对象的示例</param>
        /// <param name="act">事件</param>
        public static void ResetDictionary<T1, T2>(
            Dictionary<T1, T2> dic,
            Transform parent,
            GameObject ex,
            UnityAction<GameObject, KeyValuePair<T1, T2>> act
        )
        {
            ResetDictionary(0, dic, parent, ex, act);
        }

        public static void ResetDictionary<T1, T2>(
            Dictionary<T1, T2> dic,
            Transform parent,
            UnityAction<GameObject, KeyValuePair<T1, T2>> act
        )
        {
            ResetDictionary(0, dic, parent, parent.GetChild(0).gameObject, act);
        }

        public static void ResetDictionary<T1, T2>(
            int startIndex,
            Dictionary<T1, T2> dic,
            Transform parent,
            GameObject ex,
            UnityAction<GameObject, KeyValuePair<T1, T2>> act
        )
        {
            for (int i = parent.childCount; i < startIndex; i++)
            {
                new GameObject("child_" + i).transform.SetParent(parent);
            }

            var index = startIndex;
            foreach (var item in dic)
            {
                var gg =
                    parent.childCount > index
                        ? parent.GetChild(index).gameObject
                        : Object.Instantiate(ex, parent);
                act?.Invoke(gg, item);
                gg.SetActive(true);
                index++;
            }

            for (var i = dic.Count + startIndex; i < parent.childCount; i++)
            {
                parent.GetChild(i).gameObject.SetActive(false);
            }
        }

        #endregion
    }
}

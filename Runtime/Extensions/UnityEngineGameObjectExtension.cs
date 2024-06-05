using UnityEngine;

namespace NTool.Extensions
{
    public static class UnityEngineGameObjectExtension
    {
        public static GameObject Show(this GameObject selfObj)
        {
            selfObj.SetActive(true);
            return selfObj;
        }

        public static T Show<T>(this T selfComponent)
            where T : Component
        {
            selfComponent.gameObject.Show();
            return selfComponent;
        }

        public static GameObject Hide(this GameObject selfObj)
        {
            selfObj.SetActive(false);
            return selfObj;
        }

        public static T Hide<T>(this T selfComponent)
            where T : Component
        {
            selfComponent.gameObject.Hide();
            return selfComponent;
        }

        public static void DestroyGameObj<T>(this T selfBehaviour)
            where T : Component
        {
            selfBehaviour.gameObject.DestroySelf();
        }

        public static void DestroyGameObjGracefully<T>(this T selfBehaviour)
            where T : Component
        {
            if (selfBehaviour && selfBehaviour.gameObject)
            {
                selfBehaviour.gameObject.DestroySelfGracefully();
            }
        }

        public static T DestroyGameObjAfterDelay<T>(this T selfBehaviour, float delay)
            where T : Component
        {
            selfBehaviour.gameObject.DestroySelfAfterDelay(delay);
            return selfBehaviour;
        }

        public static T DestroyGameObjAfterDelayGracefully<T>(this T selfBehaviour, float delay)
            where T : Component
        {
            if (selfBehaviour && selfBehaviour.gameObject)
            {
                selfBehaviour.gameObject.DestroySelfAfterDelay(delay);
            }

            return selfBehaviour;
        }

        public static GameObject Layer(this GameObject selfObj, int layer)
        {
            selfObj.layer = layer;
            return selfObj;
        }

        public static T Layer<T>(this T selfComponent, int layer)
            where T : Component
        {
            selfComponent.gameObject.layer = layer;
            return selfComponent;
        }

        public static GameObject Layer(this GameObject selfObj, string layerName)
        {
            selfObj.layer = LayerMask.NameToLayer(layerName);
            return selfObj;
        }

        public static T Layer<T>(this T selfComponent, string layerName)
            where T : Component
        {
            selfComponent.gameObject.layer = LayerMask.NameToLayer(layerName);
            return selfComponent;
        }

        public static bool IsInLayerMask(this GameObject selfObj, LayerMask layerMask)
        {
            return LayerMaskUtility.IsInLayerMask(selfObj, layerMask);
        }

        public static bool IsInLayerMask<T>(this T selfComponent, LayerMask layerMask)
            where T : Component
        {
            return LayerMaskUtility.IsInLayerMask(selfComponent.gameObject, layerMask);
        }
    }

    public static class LayerMaskUtility
    {
        public static bool IsInLayerMask(int layer, LayerMask layerMask)
        {
            var objLayerMask = 1 << layer;
            return (layerMask.value & objLayerMask) == objLayerMask;
        }

        public static bool IsInLayerMask(GameObject gameObj, LayerMask layerMask)
        {
            // 根据Layer数值进行移位获得用于运算的Mask值
            var objLayerMask = 1 << gameObj.layer;
            return (layerMask.value & objLayerMask) == objLayerMask;
        }
    }
}

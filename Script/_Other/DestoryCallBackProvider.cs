using Sirenix.OdinInspector;

namespace _Other_.Runtime.Code
{
    public abstract class DeletableScriptableObject : SerializedScriptableObject
    {
#if UNITY_EDITOR
        protected virtual void OnDestroy()
        {
        }
#endif
#if UNITY_EDITOR
        public sealed class DestoryCallBackProvider : UnityEditor.AssetModificationProcessor
        {
            private static readonly System.Type _targetType = typeof(DeletableScriptableObject);

            private const string _fileEnding = ".asset";

            public static UnityEditor.AssetDeleteResult OnWillDeleteAsset(string path, UnityEditor.RemoveAssetOptions _)
            {
                if (!path.EndsWith(_fileEnding))
                    return UnityEditor.AssetDeleteResult.DidNotDelete;

                var assetType = UnityEditor.AssetDatabase.GetMainAssetTypeAtPath(path);
                if (assetType == null || (assetType != _targetType && !assetType.IsSubclassOf(_targetType)))
                    return UnityEditor.AssetDeleteResult.DidNotDelete;
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<DeletableScriptableObject>(path);
                asset.OnDestroy();

                return UnityEditor.AssetDeleteResult.DidNotDelete;
            }
        }
#endif
    }
}
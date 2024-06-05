using System.IO;

namespace NTool.Extensions
{
    public static class SystemIOExtension
    {
        /// <summary>
        /// 文件夹不存在就创建
        /// </summary>
        /// <param name="dirFullPath">指定路径</param>
        /// <returns>文件夹路径</returns>
        public static string CreateDirIfNotExists(this string dirFullPath)
        {
            if (!Directory.Exists(dirFullPath))
            {
                Directory.CreateDirectory(dirFullPath);
            }

            return dirFullPath;
        }

        /// <summary>
        /// 文件夹存在就删除
        /// </summary>
        /// <param name="dirFullPath">指定路径</param>
        public static void DeleteDirIfExists(this string dirFullPath)
        {
            if (Directory.Exists(dirFullPath))
            {
                Directory.Delete(dirFullPath, true);
            }
        }

        /// <summary>
        /// 清空指定文件夹
        /// </summary>
        /// <param name="dirFullPath">制定路径</param>
        public static void EmptyDirIfExists(this string dirFullPath)
        {
            if (Directory.Exists(dirFullPath))
            {
                Directory.Delete(dirFullPath, true);
            }

            Directory.CreateDirectory(dirFullPath);
        }

        /// <summary>
        /// 文件存在就删除
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns>文件存在并删除为True，否者为FALSE</returns>
        public static bool DeleteFileIfExists(this string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
                return true;
            }

            return false;
        }

        public static string CombinePath(this string selfPath, string toCombinePath)
        {
            return Path.Combine(selfPath, toCombinePath);
        }

        /// <summary>
        /// 根据路径获取文件名称
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件名</returns>
        public static string GetFileName(this string filePath)
        {
            return Path.GetFileName(filePath);
        }

        /// <summary>
        /// 获取没有扩展名的文件名
        /// </summary>
        /// <param name="filePath">指定路径</param>
        /// <returns>文件名</returns>
        public static string GetFileNameWithoutExtend(this string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// 获取扩展名
        /// </summary>
        /// <param name="filePath">指定路径</param>
        /// <returns>扩展名</returns>
        public static string GetFileExtendName(this string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// 获取文件文件夹
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件夹名称</returns>
        public static string GetFolderPath(this string path)
        {
            return string.IsNullOrEmpty(path) ? string.Empty : Path.GetDirectoryName(path);
        }
    }
}

using System.IO;
using System.Linq;

namespace NTool.Extensions
{
    public struct PathData
    {
        private readonly string _path;

        public PathData(string path)
        {
            this._path = path;
        }

        public bool IsValid => !string.IsNullOrEmpty(_path);

        public bool IsDirectory => Directory.Exists(_path);
        public bool IsFile      => File.Exists(_path);

        public FileInfo      AsFile()      => new(_path);
        public DirectoryInfo AsDirectory() => new(_path);

        public static implicit operator string(PathData data) => data._path;

        public PathData Combine(params string[] paths)
        {
            var ps = new[] { _path }.Concat(paths);
            return Path.Combine(ps.ToArray()).AsPath();
        }

        public string NameNoExtension => Path.GetFileNameWithoutExtension(_path);
        public string Extension       => Path.GetExtension(_path);
        public string Name            => Path.GetFileName(_path);
    }

    public static class SystemIOExtension
    {
        public static PathData AsPath(this string path)
        {
            return new PathData(path);
        }

        public static FileInfo      AsFile(this      string path) => new(path);
        public static DirectoryInfo AsDirectory(this string path) => new(path);

        /// <summary>
        /// 确保文件夹存在
        /// </summary>
        /// <param name="info">指定路径</param>
        /// <returns>文件夹信息</returns>
        public static DirectoryInfo MakeExists(this DirectoryInfo info)
        {
            if (!info.Exists)
            {
                info.Create();
            }

            return info;
        }

        /// <summary>
        /// 文件夹存在就删除
        /// </summary>
        /// <param name="info">指定路径</param>
        public static DirectoryInfo MakeDelete(this DirectoryInfo info)
        {
            if (info.Exists)
            {
                info.Delete(true);
            }

            return info;
        }

        /// <summary>
        /// 清空指定文件夹
        /// </summary>
        /// <param name="info">制定路径</param>
        public static DirectoryInfo Clear(this DirectoryInfo info)
        {
            return info.MakeDelete().MakeExists();
        }

        /// <summary>
        /// 文件存在就删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns>文件存在并删除为True，否者为FALSE</returns>
        public static FileInfo MakeDelete(this FileInfo info)
        {
            if (info.Exists)
            {
                info.Delete();
            }

            return info;
        }
    }
}
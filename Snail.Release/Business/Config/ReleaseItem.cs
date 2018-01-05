using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Snail.Release.Business.Config
{
    /// <summary>
    /// 模板配置项
    /// </summary>
    public class ReleaseItem
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 映射地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 模板文件路径
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// 配置项每部分的规则
        /// </summary>
        public string Part { get; set; }

        public bool IsMatch(string path)
        {
            Init();
            return this._reg.IsMatch(path);
        }

        /// <summary>
        /// 从当前请求中解析出物理路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns>返回模板,物理路径</returns>
        public string PhysicalPath(string path)
        {
            Init();
            if (this._parts.Count <= 0)
            {
                return null;
            }
            var pathParts = SplitPath(path);
            if (pathParts.Length != this._parts.Count)
            {
                return null;
            }            
            StringBuilder pathBuilder = new StringBuilder();
            bool isAppendParam = false;
            for (var i = 0; i < pathParts.Length; i++)
            {
                var part = this._parts[i];                 
                if (part.PartType == TempItemPartTypes.Path)
                {
                    pathBuilder.Append(pathParts[i]).Append("\\");
                }
                else if (part.PartType == TempItemPartTypes.Parameter)
                {
                    pathBuilder.Append(pathParts[i]);
                    if (isAppendParam)
                    {
                        pathBuilder.Append("_");
                    }
                    else
                    {
                        isAppendParam = true;
                    }
                }                
            }
            return pathBuilder.ToString(); 
        }

        private List<TempItemPart> _parts;
       
        private bool _isInit = false;

        private Regex _reg;
        
        private void Init()
        {
            if (this._isInit)
            {
                return;
            }
            lock (this)
            {
                if (this._isInit)
                {
                    return;
                }
                this._isInit = true;
            }           
            this._parts = new List<TempItemPart>();
            if (string.IsNullOrEmpty(this.Path) || string.IsNullOrEmpty(this.Part))
            {
                return;
            }
            this._reg = new Regex(this.Path);
            this.Template = System.IO.Path.Combine(SystemConfig.Instance.TemplateFilePath, this.Template);
            var pathItems = this.Path.Split(new string[] { "/" }, System.StringSplitOptions.RemoveEmptyEntries);
            var partItems = this.Part.Split(new string[] { "/" }, System.StringSplitOptions.RemoveEmptyEntries);
            if (pathItems.Length != partItems.Length)
            {
                return;
            }
            for (var i = 0; i < pathItems.Length; i++)
            {
                this._parts.Add(new TempItemPart()
                {
                    PartType = partItems[i],
                    Path = pathItems[i]
                });                 
            }
        }

        private string[] SplitPath(string path)
        {
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return  path.Split('/');
        }
    }
}

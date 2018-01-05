using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Snail.Release.Business.Config
{
    /// <summary>
    /// 发布项配置
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

        /// <summary>
        /// 该发布项是否匹配指定的路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsMatch(string path)
        {
            Init();
            return this._reg.IsMatch(path);
        }       

        public List<TempItemPart> Parts;
       
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
            Parts = new List<TempItemPart>();
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
                Parts.Add(new TempItemPart()
                {
                    PartType = partItems[i],
                    Path = pathItems[i]
                });                 
            }
        }

       
    }
}

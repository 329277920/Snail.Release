using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Snail.Release.Business.Config
{
    /// <summary>
    /// 发布项配置
    /// </summary>
    public class ReleaseConfigItem
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
        /// 缓存提供者
        /// </summary>
        public string Providers { get; set; }

        /// <summary>
        /// 是否启用客户端缓存
        /// </summary>
        public bool ClientCached { get; set; }

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

        private bool _isInit = false;

        private Regex _reg;

        private string[] _providers;

        public string[] TryGetProviders
        {
            get
            {
                if (_providers == null)
                {
                    if (string.IsNullOrEmpty(Providers))
                    {
                        _providers = new string[0];
                    }
                    _providers = Providers.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                }
                return _providers;
            }
        }

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
            if (string.IsNullOrEmpty(this.Path))
            {
                return;
            }
            this._reg = new Regex(this.Path);
            this.Template = System.IO.Path.Combine(SystemConfig.Instance.TemplateFilePath, this.Template);
        }
    }
}

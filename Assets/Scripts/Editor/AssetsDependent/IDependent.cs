using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Dependent
{
    /// <summary>
    /// 收集依赖关系接口
    /// </summary>
    public interface IDependent
    {
        /// <summary>
        /// 开始收集依赖
        /// </summary>
        void Collect(bool excludeSelf, bool recursive);
    }
}

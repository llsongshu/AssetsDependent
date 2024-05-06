using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Dependent
{
    static public class AssetUtil
    {
        /// <summary>
        /// 是否为有效资源
        /// </summary>
        public static bool IsVaild(string assetPath) {
            if (assetPath.EndsWith(".meta")) {
                return false;
            }
            return true;
        }
    }
}

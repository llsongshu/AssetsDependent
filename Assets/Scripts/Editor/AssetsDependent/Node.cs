using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Dependent
{
    public abstract class Node:IDisposable
    {
        protected AssetDirector mDirector {
            get {
                return AssetDirector.Director;
            }
        }
        public int depth;
        public abstract void Dispose();
    }
}

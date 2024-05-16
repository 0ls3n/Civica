using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVMR
{
    public interface IViewModelChild
    {
        public void Init(ObservableObject o);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace SystemExpert
{
    public class KMeanParam : BindableBase
    {
        private int k;

        public int K
        {
            get => k;
            set
            {
                k = value; 
                this.RaisePropertyChanged(nameof(K));
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace SystemExpert
{
    public class BilateralParam:BindableBase
    {
        private int d;
        public int D
        {
            get => d;
            set
            {
                d = value;
                this.RaisePropertyChanged(nameof(D));
            }
        }

        private double sigmaColor;
        public double SigmaColor
        {
            get => sigmaColor;
            set
            {
                sigmaColor = value; 
                this.RaisePropertyChanged(nameof(SigmaColor));
            }
        }

        private double sigmaSpace;

        public double SigmaSpace
        {
            get => sigmaSpace;
            set
            {
                sigmaSpace = value; 
                this.RaisePropertyChanged(nameof(SigmaSpace));
            }
        }

    }
}

using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class HoughCircleParam : BindableBase
    {
        public HoughCircleParam()
        {
            dp = 1;
            minDist = 20;
            param1 = 200;
            param2 = 10;
            minRadius = 10;
            maxRadius = 50;
        }

        private double dp;
        public double Dp
        {
            get => dp;
            set
            {
                dp = value;
                RaisePropertyChanged(nameof(Dp));
            }
        }

        private double minDist;
        public double MinDist
        {
            get => minDist;
            set
            {
                minDist = value;
                RaisePropertyChanged(nameof(MinDist));
            }
        }

        private double param1;
        public double Param1
        {
            get => param1;
            set
            {
                param1 = value;
                RaisePropertyChanged(nameof(Param1));
            }
        }

        private double param2;
        public double Param2
        {
            get => param2;
            set
            {
                param2 = value;
                RaisePropertyChanged(nameof(Param2));
            }
        }

        private int minRadius;
        public int MinRadius
        {
            get => minRadius;
            set
            {
                minRadius = value;
                RaisePropertyChanged(nameof(MinRadius));
            }
        }

        private int maxRadius;
        public int MaxRadius
        {
            get => maxRadius;
            set
            {
                maxRadius = value;
                RaisePropertyChanged("MaxRadius");
            }
        }

    }
}

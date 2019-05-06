using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace SystemExpert
{
    public class HSV : BindableBase
    {
        private int _h;

        public int H
        {
            get => this._h;
            set
            {
                this._h = value;
                this.RaisePropertyChanged(nameof(this.H));
            }
        }

        private int _s;

        public int S
        {
            get => this._s;
            set
            {
                this._s = value;
                this.RaisePropertyChanged(nameof(this.S));
            }
        }

        private int _v;

        public int V
        {
            get => this._v;
            set
            {
                this._v = value;
                this.RaisePropertyChanged(nameof(this.V));
            }
        }
    }

    public class InRangeParam : BindableBase
    {
        private HSV _low;

        public HSV Low
        {
            get => this._low;
            set
            {
                this._low = value;
                this.RaisePropertyChanged(nameof(this.Low));
            }
        }

        private HSV _high;

        public HSV High
        {
            get => this._high;
            set
            {
                this._high = value;
                this.RaisePropertyChanged(nameof(this.High));
            }
        }

        #region Constructor

        public InRangeParam()
        {
            this.Low = new HSV();
            this.High = new HSV();
        }

        #endregion Constructor

    }
}

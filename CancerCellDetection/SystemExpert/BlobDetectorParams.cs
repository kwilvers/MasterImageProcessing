using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class BlobDetectorParams : BindableBase
    {
        public BlobDetectorParams()
        {
            param = new SimpleBlobDetector.Params();
            param.BlobColor = 255;
            param.MinCircularity = 0.1f;
            param.MinConvexity = 0.87f;
            param.MinInertiaRatio = 0.01f;
            param.MinThreshold = 10;
            param.MaxThreshold = 200;
            param.MinArea = 1500;
        }

        SimpleBlobDetector.Params param;

        public SimpleBlobDetector.Params Param { get => param; set => param = value; }

        public bool FilterByColor
        {
            get => Param.FilterByColor  ;
            set
            {
                Param.FilterByColor = value;
                RaisePropertyChanged(nameof(FilterByColor));
            }
        }
        
        public bool FilterByConvexity
        {
            get { return param.FilterByConvexity; }
            set
            {
                param.FilterByConvexity = value;
                RaisePropertyChanged(nameof(FilterByConvexity));
            }
        }
        
        public float MaxInertiaRatio
        {
            get { return param.MaxInertiaRatio; }
            set
            {
                param.MaxInertiaRatio = value;
                RaisePropertyChanged(nameof(MaxInertiaRatio));
            }
        }
        
        public float MinInertiaRatio
        {
            get { return param.MinInertiaRatio; }
            set
            {
                param.MinInertiaRatio = value;
                RaisePropertyChanged(nameof(MinInertiaRatio));
            }
        }

        public bool FilterByInertia
        {
            get { return param.FilterByInertia; }
            set
            {
                param.FilterByInertia = value;
                RaisePropertyChanged(nameof(FilterByInertia));
            }
        }

        public float MaxCircularity
        {
            get { return param.MaxCircularity; }
            set
            {
                param.MaxCircularity = value;
                RaisePropertyChanged(nameof(MaxCircularity));
            }
        }

        public float MinCircularity
        {
            get { return param.MinCircularity; }
            set
            {
                param.MinCircularity = value;
                RaisePropertyChanged(nameof(MinCircularity));
            }
        }

        public bool FilterByArea
        {
            get { return param.FilterByArea; }
            set
            {
                param.FilterByArea = value;
                RaisePropertyChanged(nameof(FilterByArea));
            }
        }

        public bool FilterByCircularity
        {
            get { return param.FilterByCircularity; }
            set
            {
                param.FilterByCircularity = value;
                RaisePropertyChanged(nameof(FilterByCircularity));
            }
        }

        public float MaxArea
        {
            get { return param.MaxArea; }
            set
            {
                param.MaxArea = value;
                RaisePropertyChanged(nameof(MaxArea));
            }
        }

        public float MinArea
        {
            get { return param.MinArea; }
            set
            {
                param.MinArea = value;
                RaisePropertyChanged(nameof(MinArea));
            }
        }

        public float MinDistBetweenBlobs
        {
            get { return param.MinDistBetweenBlobs; }
            set
            {
                param.MinDistBetweenBlobs = value;
                RaisePropertyChanged(nameof(MinDistBetweenBlobs));
            }
        }

        public float MaxThreshold
        {
            get { return param.MaxThreshold; }
            set
            {
                param.MaxThreshold = value;
                RaisePropertyChanged(nameof(MaxThreshold));
            }
        }
        
        public float MinThreshold
        {
            get { return param.MinThreshold; }
            set
            {
                param.MinThreshold = value;
                RaisePropertyChanged(nameof(MinThreshold));
            }
        }

        public float ThresholdStep
        {
            get { return param.ThresholdStep; }
            set
            {
                param.ThresholdStep = value;
                RaisePropertyChanged(nameof(ThresholdStep));
            }
        }

        public float MinConvexity
        {
            get { return param.MinConvexity; }
            set
            {
                param.MinConvexity = value;
                RaisePropertyChanged(nameof(MinConvexity));
            }
        }

        public float MaxConvexity
        {
            get { return param.MaxConvexity; }
            set
            {
                param.MaxConvexity = value;
                RaisePropertyChanged(nameof(MaxConvexity));
            }
        }

        public byte BlobColor
        {
            get { return param.BlobColor; }
            set
            {
                param.BlobColor = value;
                RaisePropertyChanged(nameof(BlobColor));
            }
        }

        public uint MinRepeatability
        {
            get { return param.MinRepeatability; }
            set
            {
                param.MinRepeatability = value;
                RaisePropertyChanged(nameof(MinRepeatability));
            }
        }
    }
}

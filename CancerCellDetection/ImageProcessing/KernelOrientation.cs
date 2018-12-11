using System;

namespace ImageProcessing
{
    public enum KernelOrientation
    {
        None = 0,
        West = 1,
        WesternNorth = 2,
        North = 3,
        EasternNorth = 4,
        East = 5,
        EasternSouth = 6,
        South = 7,
        WesternSouth = 8
    }

    public static class KernelOrientationExtension
    {
        public static byte ToValue(this KernelOrientation k)
        {
            switch (k)
            {
                case KernelOrientation.None:
                    return 255;
                case KernelOrientation.West:
                    return 180;
                case KernelOrientation.WesternNorth:
                    return 135; 
                case KernelOrientation.North:
                    return 90;
                case KernelOrientation.EasternNorth:
                    return 45;
                case KernelOrientation.East:
                    return 0;
                case KernelOrientation.EasternSouth:
                    return 135;
                case KernelOrientation.South:
                    return 90;
                case KernelOrientation.WesternSouth:
                    return 45;
            }

            return 255;
        }
    }
}
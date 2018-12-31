using AR.Common.FrameWork.MathLib.Utilities;
using AR.Vision.FrameWork.TMap.ArMMT.Shared;
using OpenCvSharp;
using System;

namespace AR.Vision.FrameWork.TMap.ArMMT
{
    public enum KernelType
    {
        Simple,
        Weighted,
        TwoPoints
    }

    public class CvHelper
    {
        public static JaggedArray<float> CreateKernel(
            KernelType kernelType,
            int derivativeWindowSize,
            bool xKernel)
        {
            switch (kernelType)
            {
                case KernelType.Simple: return CreateKernelSimple(derivativeWindowSize + 1, xKernel);
                case KernelType.Weighted: return CreateKernelPonderated(derivativeWindowSize + 1, xKernel);
                case KernelType.TwoPoints: return CreateKernelTwoPoints(xKernel);

                default:
                    throw new ArgumentOutOfRangeException(nameof(kernelType), kernelType, null);
            }
        }

        public static JaggedArray<float> CreateKernelTwoPoints(bool xKernel)
        {
            int kernelWidth, kernelHeight;

            if (xKernel)
            {
                kernelWidth = 2;
                kernelHeight = 1;
            }
            else
            {
                kernelWidth = 1;
                kernelHeight = 2;
            }

            return new JaggedArray<float>(kernelWidth, kernelHeight, new float[] {1, -1});
        }

        public static JaggedArray<float> CreateKernelPonderated(int kernelSize, bool xKernel)
        {
            var kernel = new JaggedArray<float>(kernelSize, kernelSize);

            int step = (kernelSize - 1) / 4;

            int param_dec = (int) (Math.Round(step / 2.0) - 1);

            int largeur = 1;
            int longeur = 2;

            int[] yy = {0, 1, 0};

            double x1a = -(largeur * step - param_dec) + step + 1;
            double x1b = step + 1;
            double x1c = (largeur * step - param_dec) + step + 1;

            double x2a = -(longeur * step - param_dec) + 2 * step + 1;
            double x2b = 2 * step + 1;
            double x2c = (longeur * step - param_dec) + 2 * step + 1;

            double a2 = yy[1] / ((x2b - x2a) * (x2b - x2c));
            double a1 = yy[1] / ((x1b - x1a) * (x1b - x1c));

            float sum = 0;

            for (int i = 1; i <= 4 * step + 1; i++)
            {
                for (int j = 1; j <= 2 * step + 1; j++)
                {
                    double i2 = a2 * (i - x2a) * (i - x2c);
                    double j1 = a1 * (j - x1a) * (j - x1c);

                    int idx, idx2;

                    if (xKernel)
                    {
                        int x = j - 1;
                        int x2 = kernelSize - 1 - x;
                        int y = i - 1;

                        idx = y * kernelSize + x;
                        idx2 = y * kernelSize + x2;
                    }
                    else
                    {
                        int x = i - 1;
                        int y = j - 1;
                        int y2 = kernelSize - 1 - y;

                        idx = y * kernelSize + x;
                        idx2 = y2 * kernelSize + x;
                    }

                    var v = (float) (Math.Max(j1, (double) 0) * Math.Max(i2, 0));

                    kernel[idx] = v;
                    kernel[idx2] = -v;

                    sum += kernel[idx];
                }
            }

            var ecartsCentres = 2f * step;

            for (var i = 0; i < kernelSize * kernelSize; i++)
            {
                kernel[i] = kernel[i] / (sum * ecartsCentres);
            }

            return kernel;
        }

        public static JaggedArray<float> CreateKernelSimple(int kernelSize, bool xKernel)
        {
            var kernel = new JaggedArray<float>(kernelSize, kernelSize);

            var derivativeWindowSize2 = (kernelSize - 1) / 2;

            for (var y = 0; y < kernelSize; y++)
            {
                for (var x = 0; x < kernelSize; x++)
                {
                    if (xKernel)
                    {
                        // [  1  1  0 -1 -1 ]
                        // [  1  1  0 -1 -1 ]
                        // [  1  1  0 -1 -1 ]
                        // [  1  1  0 -1 -1 ]
                        // [  1  1  0 -1 -1 ]

                        if (x < derivativeWindowSize2)
                            kernel.SetElement(x, y, 1);
                        else if (x == derivativeWindowSize2)
                            kernel.SetElement(x, y, 0);
                        else
                            kernel.SetElement(x, y, -1);
                    }
                    else
                    {
                        // [  1  1  1  1  1]
                        // [  1  1  1  1  1]
                        // [  0  0  0  0  0]
                        // [ -1 -1 -1 -1 -1]
                        // [ -1 -1 -1 -1 -1]

                        if (y < derivativeWindowSize2)
                            kernel.SetElement(x, y, 1);
                        else if (y == derivativeWindowSize2)
                            kernel.SetElement(x, y, 0);
                        else
                            kernel.SetElement(x, y, -1);
                    }
                }
            }

            return kernel;
        }

        public static JaggedArray<float> GetMeasurabilityMatrix(
            JaggedArray<byte> fiability,
            int derivativeWindowSize,
            KernelType kernelType)
        {
            return GetMeasurabilityMatrix(fiability, derivativeWindowSize, kernelType, false, out _);
        }

        public static JaggedArray<float> GetMeasurabilityMatrix(
            JaggedArray<byte> fiability,
            int derivativeWindowSize,
            KernelType kernelType,
            bool debug,
            out MeasurabilityMatrixDebug debugData)
        {
            // argument checks
            if (derivativeWindowSize % 2 != 0)
                throw new ArgumentException("derivativeWindowSize must be divisible by 2");

            using (var filterX = CreateKernel(kernelType, derivativeWindowSize, true).ToMatOfFloat())
            using (var filterY = CreateKernel(kernelType, derivativeWindowSize, false).ToMatOfFloat())
            using (var ones = new MatOfFloat(derivativeWindowSize + 1, derivativeWindowSize + 1, 1))
            using (var fiabilityMatrix = fiability.ToMatOfByte())
            using (var fiabilityMatrixBinByte = new MatOfByte(fiabilityMatrix.Threshold(1, 1, ThresholdTypes.Binary)))
            using (var fiabilityMatrixBin = fiabilityMatrixBinByte.ToMatOfFloat())
            using (var meanFilterNotNormalized = new MatOfFloat(filterX.Abs() + filterY.Abs()))
            using (var normalisationMatrix = new MatOfFloat(ones.Conv2(meanFilterNotNormalized)))
            {
                var normalisationConst =
                    normalisationMatrix.Get<float>(derivativeWindowSize / 2, derivativeWindowSize / 2);

                using (var measurabilityNotNormalized =
                    new MatOfFloat(fiabilityMatrixBin.Conv2(meanFilterNotNormalized)))
                using (var measurability = new MatOfFloat(measurabilityNotNormalized / normalisationConst))
                {
                    debugData = debug
                        ? new MeasurabilityMatrixDebug
                        {
                            FiabilityMatrixBin = fiabilityMatrixBin.ToJaggedArray(),
                            MeanFilterNotNormalized = meanFilterNotNormalized.ToJaggedArray(),
                            NormalisationMatrix = normalisationMatrix.ToJaggedArray(),
                            NormalisationConst = normalisationConst,
                            MeasurabilityNotNormalized = measurabilityNotNormalized.ToJaggedArray(),
                            Measurability = measurability.ToJaggedArray()
                        }
                        : null;

                    return measurability.ToJaggedArray();
                }
            }
        }

        public static JaggedArray<float> GetSmoothedPhase(
            JaggedArray<float> phase,
            int derivativeWindowSize,
            KernelType kernelType)
        {
            var smoothedPhase = GetSmoothedPhase(phase, derivativeWindowSize, kernelType,
                out var meanFilterNotNormalized, out _, out var normalisationMatrix);

            meanFilterNotNormalized?.Dispose();
            normalisationMatrix?.Dispose();

            return smoothedPhase;
        }

        public static JaggedArray<float> GetSmoothedPhase(
            JaggedArray<float> phase,
            int derivativeWindowSize,
            KernelType kernelType,
            out MatOfFloat meanFilterNotNormalized,
            out double normalisationConst,
            out MatOfFloat normalisationMatrix)
        {
            // argument checks
            if (derivativeWindowSize % 2 != 0)
                throw new ArgumentException("derivativeWindowSize must be divisible by 2");

            using (var filterX = CreateKernel(kernelType, derivativeWindowSize, true).ToMatOfFloat())
            using (var filterY = CreateKernel(kernelType, derivativeWindowSize, false).ToMatOfFloat())
            using (var ones = new MatOfFloat(derivativeWindowSize + 1, derivativeWindowSize + 1, 1))
            using (var phaseMatrix = phase.ToMatOfFloat())
            {
                meanFilterNotNormalized = new MatOfFloat(filterX.Abs() + filterY.Abs());
                normalisationMatrix = new MatOfFloat(ones.Conv2(meanFilterNotNormalized));
                normalisationConst = normalisationMatrix.Get<float>(derivativeWindowSize / 2, derivativeWindowSize / 2);

                using (var smoothedPhaseNotNormalized = new MatOfFloat(phaseMatrix.Conv2(meanFilterNotNormalized)))
                using (var smoothedPhase = new MatOfFloat(smoothedPhaseNotNormalized / normalisationConst))
                {
                    return smoothedPhase.ToJaggedArray();
                }
            }
        }

        public static JaggedArray<float> GetMeanDerivative(
            JaggedArray<float> input,
            int derivativeWindowSize,
            bool xKernel,
            KernelType kernelType)
        {
            // argument checks
            if (derivativeWindowSize % 2 != 0)
                throw new ArgumentException("derivativeWindowSize must be divisible by 2");

            var kernel = CreateKernel(kernelType, derivativeWindowSize, xKernel);

            using (var inputMat = input.ToMatOfFloat())
            using (var kernelMat = kernel.ToMatOfFloat())
            using (var outputMat = new MatOfFloat(inputMat.Conv2(kernelMat)))
            {
                if (kernelType != KernelType.Simple)
                    return outputMat.ToJaggedArray();

                var step = derivativeWindowSize / 2f;
                var div = (2 * step) * (4 * step + 1) * (2 * step + 1);

                using (var outputMatDiv = new MatOfFloat(outputMat / div))
                {
                    return outputMatDiv.ToJaggedArray();
                }
            }
        }

        public static void Eigen(float cxx, float cyx, float cxy, float cyy,
            out float λx, out float λy,
            out float xxy1, out float xxy0,
            out float Lx, out float Ly, out float θ)
        {
            // https://docs.opencv.org/3.0-beta/modules/core/doc/operations_on_arrays.html#eigen

            using (var mat = new MatOfFloat(new[] {2, 2}, new[] {cxx, cyx, cxy, cyy}))
            using (var eigenValues = new MatOfFloat())
            using (var eigenVectors = new MatOfFloat())
            {
                Cv2.Eigen(mat, eigenValues, eigenVectors);

                λx = eigenValues.At<float>(0, 0);
                λy = eigenValues.At<float>(1, 0);

                xxy1 = eigenVectors.At<float>(0, 1);
                xxy0 = eigenVectors.At<float>(1, 1);

                Lx = (float) (2 * Math.PI / λx);
                Ly = (float) (2 * Math.PI / λy);

                θ = (float) (Math.Atan2(xxy1, xxy0) * 180 / Math.PI);
            }
        }

        public static void DeltaPhase(
            System.Drawing.Point p1, System.Drawing.Point p2,
            float cxx, float cyx, float cxy, float cyy,
            float φ1x, float φ1y,
            float φ2x, float φ2y,
            out float deltaPhX, out float deltaPhY)
        {
            var deltaX = p2.X - p1.X;
            var deltaY = p2.Y - p1.Y;

            using (var mat = new MatOfFloat(new[] {2, 2}, new[] {cxx, cyx, cxy, cyy}))
            {
                var φ1Mat = new MatOfFloat(new[] {2, 1}, new float[] {φ1x, φ1y});
                var φ2Mat = new MatOfFloat(new[] {2, 1}, new float[] {φ2x, φ2y});

                var deltaMat = new MatOfFloat(new[] {2, 1}, new float[] {deltaX, deltaY});

                var φEstim = new MatOfFloat(φ1Mat + new MatOfFloat(mat * deltaMat));

                var deltaPhMat = new MatOfFloat(φEstim - φ2Mat);

                deltaPhX = Math.Abs(deltaPhMat.At<float>(0, 0));
                deltaPhY = Math.Abs(deltaPhMat.At<float>(1, 0));
            }
        }
    }

    public class MeasurabilityMatrixDebug
    {
        public JaggedArray<float> FiabilityMatrixBin { get; set; }

        public JaggedArray<float> MeanFilterNotNormalized { get; set; }

        public JaggedArray<float> NormalisationMatrix { get; set; }

        public float NormalisationConst { get; set; }

        public JaggedArray<float> MeasurabilityNotNormalized { get; set; }

        public JaggedArray<float> Measurability { get; set; }
    }
}

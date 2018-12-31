using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using MathNet.Numerics.LinearAlgebra;


namespace AR.Common.FrameWork.MathLib.Utilities
{
    [Serializable()]
    public class JaggedArray<T>
    {
        #region Ctors

        public JaggedArray()
        { }

        public JaggedArray(int width, int height)
        {
            ContainerAllocate(width, height);
        }

        public JaggedArray(int width, int height, T initialValue)
        {
            ContainerAllocate(width, height, initialValue);
        }

        public JaggedArray(int width, int height, T[] array1D)
        {
            ContainerAllocate(width, height);
            ContainerFill(array1D);
        }

        #endregion

        #region Public properties

        public T[][] Container { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Size Size
        {
            get { return new Size(Width, Height); }
        }

        public int Length
        {
            get { return Width * Height; }
        }

        public int Count
        {
            get { return Length; }
        }

        #endregion

        #region Access container

        void ContainerAllocate(int width, int height)
        {
            Width = width;
            Height = height;

            Container = new T[Height][];

            for (int y = 0; y < Height; y++)
            {
                Container[y] = new T[Width];
            }
        }

        void ContainerAllocate(int width, int height, T initialValue)
        {
            Width = width;
            Height = height;

            Container = new T[Height][];

            for (var y = 0; y < Height; y++)
            {
                Container[y] = new T[Width];

                for (var x = 0; x < Width; x++)
                {
                    Container[y][x] = initialValue;
                }
            }
        }

        void ContainerFill(T[] array1D)
        {
            for (int y = 0; y < Height; y++)
            {
                Array.Copy(array1D, y * Width, Container[y], 0, Width);
            }
        }

        T[] ContainerDump()
        {
            var ret = new T[Length];

            for (var y = 0; y < Height; y++)
            {
                Array.Copy(Container[y], 0, ret, y * Width, Width);
            }

            return ret;
        }

        public T this[int index]
        {
            get
            {
                var y = index / Width;
                var x = index % Width;

                return Container[y][x];
            }
            set
            {
                var y = index / Width;
                var x = index % Width;

                Container[y][x] = value;
            }
        }

        public T GetElement(int x, int y)
        {
            return Container[y][x];
        }

        public void SetElement(int x, int y, T value)
        {
            Container[y][x] = value;
        }

        public T[] ToArray()
        {
            return ContainerDump();
        }

        public JaggedArray<T2> Cast<T2>()
        {
            var ret = new JaggedArray<T2>(Width, Height);

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    T2 casted = (T2)Convert.ChangeType(GetElement(x, y), typeof(T2));

                    ret.SetElement(x, y, casted);
                }
            }

            return ret;
        }

        #endregion

        #region ROI handling

        public JaggedArray<T> ExtractRoi(Rectangle roi)
        {
            // à tester & à améliorer (copie ligne par ligne)
            //var output = new T[roi.Width * roi.Height];
            var output = new JaggedArray<T>(roi.Width, roi.Height);

            for (var yRoi = 0; yRoi < roi.Height; yRoi++)
            {
                for (var xRoi = 0; xRoi < roi.Width; xRoi++)
                {
                    //var indexRoi = yRoi * roi.Width + xRoi;
                    // int indexOrig = (yRoi + roi.Y) * m_Width + (xRoi + roi.X);

                    //output[indexRoi] = this[indexOrig];
                    //output[indexRoi] = GetElement(xRoi + roi.X, yRoi + roi.Y);

                    output.SetElement(xRoi, yRoi, GetElement(xRoi + roi.X, yRoi + roi.Y));
                }
            }

            return output;
        }

        public void InsertRoi(JaggedArray<T> values, Rectangle roi)
        {
            if (values.Length != roi.Width * roi.Height)
                throw new Exception("array length do not match roi");

            for (var yRoi = 0; yRoi < roi.Height; yRoi++)
            {
                for (var xRoi = 0; xRoi < roi.Width; xRoi++)
                {
                    //int indexRoi = yRoi * roi.Width + xRoi;
                    int indexOrig = (yRoi + roi.Y) * Width + (xRoi + roi.X);

                    this[indexOrig] = values.GetElement(xRoi, yRoi);
                }
            }
        }

        #endregion

        public void ToCsv(string filename)
        {
            ToCsv(filename, CultureInfo.InvariantCulture, ';');
        }

        public void ToCsv(string filename,CultureInfo cultureInfo,char csvSeparator)
        {
            using (var sw = File.CreateText(filename))
            {
                for (var y = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        var vdouble = (double)Convert.ChangeType(GetElement(x, y), typeof(double));

                        sw.Write(vdouble.ToString(cultureInfo) + csvSeparator);
                    }
                    sw.Write(Environment.NewLine);
                }
            }
        }

        public static JaggedArray<double> FromCsv(string filename)
        {
            return FromCsv(filename, CultureInfo.InvariantCulture, ';');
        }

        public static JaggedArray<double> FromCsv(string filename, CultureInfo cultureInfo, char csvSeparator)
        {
            int colCount = 0, rowCount = 0;

            var rawValues = new List<double[]>();

            using (var mainSr = File.OpenText(filename))
            {
                while (!mainSr.EndOfStream)
                {
                    var line = mainSr.ReadLine();

                    var values = new List<double>();

                    foreach(var valStr in line.Split(csvSeparator))
                    {
                        double result;

                        if (double.TryParse(valStr, NumberStyles.Any, cultureInfo, out result))
                            values.Add(result);
                    }

                    if (values.Count > 0)
                    {
                        if (colCount == 0)
                            colCount = values.Count;
                        else if (colCount != values.Count)
                            throw new Exception("Invalid csv");

                        rawValues.Add(values.ToArray());

                        rowCount++;
                    }
                }
            }
            return new JaggedArray<double>(colCount, rowCount) { Container = rawValues.ToArray() };
        }

        public bool Equals(JaggedArray<T> input)
        {
            if (Size != input.Size) return false;

            for(int i=0;i<Length;i++)
            {
                if (!this[i].Equals( input[i]))
                    return false;
            }
                
            return true;
        }

        #region Math

        public double Mean()
        {
            double sum = 0;
            int cpt = 0;

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    double el = (double)Convert.ChangeType(GetElement(x, y), typeof(double));
                    if (!double.IsNaN(el))
                    {
                        sum += el;
                        cpt++;
                    }
                }
            }
            return sum / cpt;
        }

        public float FMean()
        {
            double sum = 0;
            int cpt = 0;

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    float el = (float)Convert.ChangeType(GetElement(x, y), typeof(float));
                    if (!float.IsNaN(el))
                    {
                        sum += el;
                        cpt++;
                    }
                }
            }

            return (float)(sum / cpt);
        }
        #endregion

        #region Transformations

        public JaggedArray<T> FlipX()
        {
            var flipped = new JaggedArray<T>(Width, Height);

            var w = Width;
            var h = Height;

            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    flipped.SetElement(w-x-1, y, GetElement(x, y));
                }
            }

            return flipped;
        }

        public JaggedArray<T> FlipY()
        {
            var flipped = new JaggedArray<T>(Width, Height);

            var w = Width;
            var h = Height;

            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    flipped.SetElement(x, h - y - 1, GetElement(x, y));
                }
            }

            return flipped;
        }



        #endregion

    }
}
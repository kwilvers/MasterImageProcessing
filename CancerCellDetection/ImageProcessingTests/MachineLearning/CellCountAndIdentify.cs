using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessingTests.MachineLearning
{
    [TestClass]
    public class CellCountAndIdentify
    {
        [TestMethod]
        public void GetMetadata()
        {
            Image image = new Bitmap(@".\metadata.png");
            // Get the PropertyItems property from image.
            var propItems = image.PropertyItems;
            int count = 0;

            foreach (var propItem in propItems)
            {
                Console.WriteLine("Property Item " + count.ToString());
                Console.WriteLine("   iD: 0x" + propItem.Id.ToString("x"));
                Console.WriteLine("   type: " + propItem.Type.ToString());
                Console.WriteLine("   length: " + propItem.Len.ToString() + " bytes");

                count++;
            }
        }
    }
}

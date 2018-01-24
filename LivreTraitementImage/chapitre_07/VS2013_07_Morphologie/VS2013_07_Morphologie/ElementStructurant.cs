using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VS2013_07_Morphologie
{
    public class ElementStructurant
    {
        //champs
        public enum TypeStructurant
        {
            croix,
            carre
        };

        private TypeStructurant v_structurant;

        private byte[,] v_pixel;

        //constructeur
        public ElementStructurant(TypeStructurant type_de_structure)
        {
            v_structurant = type_de_structure;
            switch (v_structurant)
            {
                case TypeStructurant.croix:
                    v_pixel = new byte[3, 3];
                    v_pixel[0, 0] = 0;
                    v_pixel[0, 1] = 1;
                    v_pixel[0, 2] = 0;
                    v_pixel[1, 0] = 1;
                    v_pixel[1, 1] = 0;
                    v_pixel[1, 2] = 1;
                    v_pixel[2, 0] = 0;
                    v_pixel[2, 1] = 1;
                    v_pixel[2, 2] = 0;
                    break;
                case TypeStructurant.carre:
                    v_pixel = new byte[3, 3];
                    v_pixel[0, 0] = 1;
                    v_pixel[0, 1] = 1;
                    v_pixel[0, 2] = 1;
                    v_pixel[1, 0] = 1;
                    v_pixel[1, 1] = 0;
                    v_pixel[1, 2] = 1;
                    v_pixel[2, 0] = 1;
                    v_pixel[2, 1] = 1;
                    v_pixel[2, 2] = 1;
                    break;
            }
        }

        //appliquer une dilatation
        public byte AppliquerDilatation(byte[,] tab_3x3)
        {
            byte niveau = 127;
            byte point_chaud = tab_3x3[1, 1];
            if (point_chaud == 0)
            {
                niveau = point_chaud;
            }
            if (point_chaud == 255)
            {
                byte pch1 = v_pixel[0, 0];
                byte pixel1 = tab_3x3[0, 0];
                byte pch2 = v_pixel[0, 1];
                byte pixel2 = tab_3x3[0, 1];
                byte pch3 = v_pixel[0, 2];
                byte pixel3 = tab_3x3[0, 2];
                byte pch4 = v_pixel[1, 2];
                byte pixel4 = tab_3x3[1, 2];
                byte pch5 = v_pixel[2, 2];
                byte pixel5 = tab_3x3[2, 2];
                byte pch6 = v_pixel[2, 1];
                byte pixel6 = tab_3x3[2, 1];
                byte pch7 = v_pixel[2, 0];
                byte pixel7 = tab_3x3[2, 0];
                byte pch8 = v_pixel[1, 0];
                byte pixel8 = tab_3x3[1, 0];
                int contact = 0;
                if (pch1 != 0 && pixel1 == 0)
                {
                    contact++;
                }
                if (pch2 != 0 && pixel2 == 0)
                {
                    contact++;
                }
                if (pch3 != 0 && pixel3 == 0)
                {
                    contact++;
                }
                if (pch4 != 0 && pixel4 == 0)
                {
                    contact++;
                }
                if (pch5 != 0 && pixel5 == 0)
                {
                    contact++;
                }
                if (pch6 != 0 && pixel6 == 0)
                {
                    contact++;
                }
                if (pch7 != 0 && pixel7 == 0)
                {
                    contact++;
                }
                if (pch8 != 0 && pixel8 == 0)
                {
                    contact++;
                }
                if (contact != 0)
                {
                    niveau = 0;
                }
                else
                {
                    niveau = point_chaud;
                }
            }
            return niveau;
        }

        //appliquer une erosion
        public byte AppliquerErosion(byte[,] tab_3x3)
        {
            byte niveau = 127;
            byte point_chaud = tab_3x3[1, 1];
            if (point_chaud == 0)
            {
                byte pch1 = v_pixel[0, 0];
                byte pixel1 = tab_3x3[0, 0];
                byte pch2 = v_pixel[0, 1];
                byte pixel2 = tab_3x3[0, 1];
                byte pch3 = v_pixel[0, 2];
                byte pixel3 = tab_3x3[0, 2];
                byte pch4 = v_pixel[1, 2];
                byte pixel4 = tab_3x3[1, 2];
                byte pch5 = v_pixel[2, 2];
                byte pixel5 = tab_3x3[2, 2];
                byte pch6 = v_pixel[2, 1];
                byte pixel6 = tab_3x3[2, 1];
                byte pch7 = v_pixel[2, 0];
                byte pixel7 = tab_3x3[2, 0];
                byte pch8 = v_pixel[1, 0];
                byte pixel8 = tab_3x3[1, 0];
                int nb_marqueur = 0;
                if (pch1 != 0)
                {
                    nb_marqueur++;
                }
                if (pch2 != 0)
                {
                    nb_marqueur++;
                }
                if (pch3 != 0)
                {
                    nb_marqueur++;
                }
                if (pch4 != 0)
                {
                    nb_marqueur++;
                }
                if (pch5 != 0)
                {
                    nb_marqueur++;
                }
                if (pch6 != 0)
                {
                    nb_marqueur++;
                }
                if (pch7 != 0)
                {
                    nb_marqueur++;
                }
                if (pch8 != 0)
                {
                    nb_marqueur++;
                }
                int contact = 0;
                if (pch1 != 0 && pixel1 == 0)
                {
                    contact++;
                }
                if (pch2 != 0 && pixel2 == 0)
                {
                    contact++;
                }
                if (pch3 != 0 && pixel3 == 0)
                {
                    contact++;
                }
                if (pch4 != 0 && pixel4 == 0)
                {
                    contact++;
                }
                if (pch5 != 0 && pixel5 == 0)
                {
                    contact++;
                }
                if (pch6 != 0 && pixel6 == 0)
                {
                    contact++;
                }
                if (pch7 != 0 && pixel7 == 0)
                {
                    contact++;
                }
                if (pch8 != 0 && pixel8 == 0)
                {
                    contact++;
                }
                if (contact == nb_marqueur)
                {
                    niveau = point_chaud;
                }
                else
                {
                    niveau = 255;
                }
            }
            else
            {
                niveau = 255;
            }
            return niveau;
        }
    } //end class
}
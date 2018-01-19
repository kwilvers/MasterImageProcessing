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

namespace VS2013_07_SeuillageOptimal {
  public class BruitImage8bit {
    //champs
    private int v_largeur = 0;
    private int v_hauteur = 0;
    private byte[] tab_pixels = null;
    //proprietes
    public byte[,] P_Pixels_LH {
      get {
        return TransposerTableauPixelEnLH_8bit(tab_pixels, v_largeur, v_hauteur);
      }
    }
    //constructeur
    public BruitImage8bit(int largeur, int hauteur) {
      v_largeur = largeur;
      v_hauteur = hauteur;
      tab_pixels = new byte[v_largeur * v_hauteur];
    }
    //modeliser un bruit uniforme
    public BitmapSource BitmapModeleUniforme(int niv_mini, int niv_maxi) {
      Random generateur = new Random();
      byte niv_gris = 0;
      for (int xx = 0; xx < tab_pixels.Length; xx++) {
        niv_gris = (byte)generateur.Next(niv_mini, niv_maxi + 1);
        tab_pixels[xx] = niv_gris;
      }
      BitmapSource bti = BitmapSource.Create(v_largeur, v_hauteur, 96.0, 96.0,
        PixelFormats.Gray8, null, tab_pixels, v_largeur);
      return bti;
    }
    //modeliser un bruit uniforme avec taux de couverture
    public BitmapSource BitmapModeleUniforme(int niv_mini, int niv_maxi, double taux_couverture) {
      Random generateur = new Random();
      byte niv_gris = 0;
      for (int xx = 0; xx < tab_pixels.Length; xx++) {
        niv_gris = (byte)generateur.Next(niv_mini, niv_maxi + 1);
        tab_pixels[xx] = niv_gris;
      }
      double borne_inf = 127 - (127 - niv_mini) * taux_couverture;
      double borne_sup = 127 + (niv_maxi - 127) * taux_couverture;
      for (int xx = 0; xx < tab_pixels.Length; xx++) {
        if (tab_pixels[xx] >= borne_inf && tab_pixels[xx] <= borne_sup) {
          tab_pixels[xx] = 127;
        }
      }
      BitmapSource bti = BitmapSource.Create(v_largeur, v_hauteur, 96.0, 96.0,
        PixelFormats.Gray8, null, tab_pixels, v_largeur);
      return bti;
    }
    //modeliser un bruit poivre et sel
    public BitmapSource BitmapModelePoivreEtSel(int niv_1, int niv_2) {
      Random generateur = new Random();
      for (int xx = 0; xx < tab_pixels.Length; xx++) {
        tab_pixels[xx] = 127;
      }
      for (int qte = 0; qte < tab_pixels.Length / 20; qte++) {
        int pos_alea = generateur.Next(0, tab_pixels.Length);
        tab_pixels[pos_alea] = (byte)niv_1;
      }
      for (int qte = 0; qte < tab_pixels.Length / 20; qte++) {
        int pos_alea = generateur.Next(0, tab_pixels.Length);
        tab_pixels[pos_alea] = (byte)niv_2;
      }
      BitmapSource bti = BitmapSource.Create(v_largeur, v_hauteur, 96.0, 96.0,
        PixelFormats.Gray8, null, tab_pixels, v_largeur);
      return bti;
    }
    //transposition tableau pixel dimension 1 vers 2 avec codage 8 bits
    private byte[,] TransposerTableauPixelEnLH_8bit(byte[] tab_pixel, int pixel_larg, int pixel_haut) {
      byte[,] tab_LH = new byte[pixel_haut, pixel_larg];
      int lig = 0;
      int col = 0;
      for (int xx = 0; xx < tab_pixel.Length; xx++) {
        byte niveau_gris = tab_pixel[xx];
        tab_LH[lig, col] = niveau_gris;
        col++;
        if (col == pixel_larg) {
          col = 0;
          lig++;
        }
      }
      return tab_LH;
    }
    //transposition tableau pixel dimension 2 vers 1 avec codage 8 bits
    private byte[] ConvertirTableauPixelEnUnique_8bit(int[,] tab_pixel_int_LH_modif, int pixel_largeur, int pixel_hauteur) {
      byte[] tab = new byte[pixel_largeur * pixel_hauteur];
      int cpt = 0;
      for (int lig = 0; lig < pixel_hauteur; lig++) {
        for (int col = 0; col < pixel_largeur; col++) {
          tab[cpt] = (byte)tab_pixel_int_LH_modif[lig, col];
          cpt++;
        }
      }
      return tab;
    }
    ////
    //public WriteableBitmap WabModeleUniforme(int niv_mini, int niv_maxi) {
    //  WriteableBitmap wab = new WriteableBitmap(v_largeur, v_hauteur, 96.0, 96.0, PixelFormats.Gray8, null);
    //  //Random generateur = new Random();
    //  //for (int lig = 0; lig < v_hauteur; lig++) {
    //  //  for (int col = 0; col < v_largeur; col++) {
    //  //    int niv_gris = generateur.Next(niv_mini, niv_maxi + 1);
    //  //    EcrirePixel(wab, lig, col, niv_gris);
    //  //  }
    //  //}
    //  Random generateur = new Random();
    //  int[] tab_pixels = new int[v_largeur * v_hauteur];
    //  int niv_gris = 0;
    //  for (int xx = 0; xx < tab_pixels.Length; xx++) {
    //    niv_gris = generateur.Next(niv_mini, niv_maxi + 1);
    //    tab_pixels[xx] = 255;
    //  }
    //  wab.Lock();
    //  wab.WritePixels(new Int32Rect(0, 0, v_largeur - 1, v_hauteur - 1), tab_pixels, v_largeur, 0);
    //  //wab.AddDirtyRect(new Int32Rect(0, 0, v_largeur - 1, v_hauteur - 1));
    //  wab.Unlock();
    //  return wab;
    //}

    //private void EcrirePixel(WriteableBitmap wab, int lig, int col, int niveau_gris) {
    //  // Reserve the back buffer for updates.
    //  wab.Lock();
    //  unsafe {
    //    // Get a pointer to the back buffer.
    //    int pBackBuffer = (int)wab.BackBuffer;
    //    // Find the address of the pixel to draw.
    //    pBackBuffer += lig * wab.BackBufferStride;
    //    pBackBuffer += col;
    //    // Compute the pixel's color.
    //    //int color_data = 255 << 16; // R
    //    //color_data |= 128 << 8;   // G
    //    //color_data |= 255 << 0;   // B
    //    // Assign the color data to the pixel.
    //    *((int*)pBackBuffer) = niveau_gris;
    //  }
    //  // Specify the area of the bitmap that changed.
    //  wab.AddDirtyRect(new Int32Rect(col, lig, 1, 1));
    //  // Release the back buffer and make it available for display.
    //  wab.Unlock();
    //}
  }//end class
}

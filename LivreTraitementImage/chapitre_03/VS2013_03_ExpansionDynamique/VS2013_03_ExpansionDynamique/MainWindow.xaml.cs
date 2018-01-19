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

namespace VS2013_03_ExpansionDynamique {
  /// <summary>
  /// Logique d'interaction pour MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    //donnees
    private string RC = Environment.NewLine;
    private string doss_exe = Environment.CurrentDirectory;
    private bool v_fen_charge = false;
    //constructeur
    public MainWindow() {
      InitializeComponent();
    }
    //menu fichier -> quitter
    private void x_men_fichier_quitter_Click(object sender, RoutedEventArgs e) {
      this.Close();
    }
    //fenetre evenement Loaded
    private void Window_Loaded(object sender, RoutedEventArgs e) {
      v_fen_charge = true;
      Uri uri = new Uri("pack://application:,,,/VS2013_03_ExpansionDynamique;component/collection_images/gendarme_newyork_8bit_800x583_96dpi.jpg", UriKind.Absolute);
      GenererHistogrammeNormalise(uri, x_page.x_border_histo_initial);
      ExpansionDynamique();
    }
    //
    private void GenererHistogrammeNormalise(Uri uri, Border conteneur) {
      BitmapImage bti = new BitmapImage();
      bti.BeginInit();
      bti.UriSource = uri;
      bti.EndInit();
      x_page.x_img.Width = bti.PixelWidth;
      x_page.x_img.Height = bti.PixelHeight;
      x_page.x_img.Source = bti;
      //image 8 bits contenant des niveaux 256 gris 
      WriteableBitmap wb = new WriteableBitmap(bti);
      int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
      byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
      wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
      int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
      byte[,] tab_pixel_gris_LH = new byte[wb.PixelHeight, wb.PixelWidth];
      for (int lig = 0; lig < wb.PixelHeight; lig++) {
        for (int col = 0; col < wb.PixelWidth; col++) {
          int gris_int = tab_pixel_int_LH[lig, col];
          tab_pixel_gris_LH[lig, col] = (byte)gris_int;
        }
      }
      HistoNormaliseNg visuel_histo = new HistoNormaliseNg();
      visuel_histo.Titre = "Histogramme normalisé pour les niveaux de gris";
      visuel_histo.PixelImage_LH = tab_pixel_gris_LH;
      visuel_histo.PixelLargeur = wb.PixelWidth;
      visuel_histo.PixelHauteur = wb.PixelHeight;
      visuel_histo.AfficherCourbeCumul = true;
      conteneur.Child = visuel_histo;
    }
    //
    private void ExpansionDynamique() {
      Uri uri = new Uri("pack://application:,,,/VS2013_03_ExpansionDynamique;component/collection_images/gendarme_newyork_8bit_800x583_96dpi.jpg", UriKind.Absolute);
      BitmapImage bti = new BitmapImage();
      bti.BeginInit();
      bti.UriSource = uri;
      bti.EndInit();
      //image 8 bits contenant des niveaux 256 gris 
      WriteableBitmap wb = new WriteableBitmap(bti);
      int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
      byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
      wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
      int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
      byte niv_mini = 255;
      byte niv_maxi = 0;
      for (int lig = 0; lig < wb.PixelHeight; lig++) {
        for (int col = 0; col < wb.PixelWidth; col++) {
          int gris_int = tab_pixel_int_LH[lig, col];
          if (gris_int < niv_mini) {
            niv_mini = (byte)gris_int;
          }
          if (gris_int > niv_maxi) {
            niv_maxi = (byte)gris_int;
          }
        }
      }
      double alpha = (0 * niv_maxi - 255 * niv_mini) / (niv_maxi - niv_mini);
      double beta = (255 - 0) / (niv_maxi - niv_mini);
      x_page.x_tbl_equation.Text = "équation: I2 = " + alpha.ToString(".00") + " + " + beta.ToString(".00") + " * I1";
      for (int lig = 0; lig < wb.PixelHeight; lig++) {
        for (int col = 0; col < wb.PixelWidth; col++) {
          int gris_int = tab_pixel_int_LH[lig, col];
          int nouvelle_intensite = (int)(alpha + beta * gris_int);
          if (nouvelle_intensite > 255) {
            nouvelle_intensite = 255;
          }
          if (nouvelle_intensite < 0) {
            nouvelle_intensite = 0;
          }
          tab_pixel_int_LH[lig, col] = nouvelle_intensite;
        }
      }
      byte[] tab_pixel_modif = new byte[largeur_numerisation * wb.PixelHeight];
      tab_pixel_modif = ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH, wb.PixelWidth, wb.PixelHeight);
      BitmapSource bti_modif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0, PixelFormats.Gray8,
        null, tab_pixel_modif, largeur_numerisation);
      x_page.x_img_traitee.Width = bti_modif.PixelWidth;
      x_page.x_img_traitee.Height = bti_modif.PixelHeight;
      x_page.x_img_traitee.Source = bti_modif;
      HistoNormaliseNg visuel_histo = new HistoNormaliseNg();
      visuel_histo.Titre = "Histogramme normalisé pour les niveaux de gris";
      byte[,] tab_pixel_byte_LH = new byte[wb.PixelHeight, wb.PixelWidth];
      for (int lig = 0; lig < wb.PixelHeight; lig++) {
        for (int col = 0; col < wb.PixelWidth; col++) {
          tab_pixel_byte_LH[lig, col] = (byte)tab_pixel_int_LH[lig, col];
        }
      }
      visuel_histo.PixelImage_LH = tab_pixel_byte_LH;
      visuel_histo.PixelLargeur = wb.PixelWidth;
      visuel_histo.PixelHauteur = wb.PixelHeight;
      visuel_histo.AfficherCourbeCumul = true;
      x_page.x_border_histo_final.Child = visuel_histo;
    }
    //transposition tableau pixel dimension 1 vers 2 avec codage 32 bits
    private int[,] ConvertirTableauPixelEnLH_32bit(byte[] tab_pixel, int pixel_larg, int pixel_haut) {
      int[,] tab_LH = new int[pixel_haut, pixel_larg];
      int lig = 0;
      int col = 0;
      for (int xx = 0; xx < tab_pixel.Length; xx += 4) {
        byte comp_b = tab_pixel[xx];
        byte comp_g = tab_pixel[xx + 1];
        byte comp_r = tab_pixel[xx + 2];
        byte comp_a = tab_pixel[xx + 3];
        int couleur_int = comp_a << 24 | comp_r << 16 | comp_g << 8 | comp_b;
        tab_LH[lig, col] = couleur_int;
        col++;
        if (col == pixel_larg) {
          col = 0;
          lig++;
        }
      }
      return tab_LH;
    }
    //transposition tableau pixel dimension 2 vers 1 avec codage 32 bits
    private byte[] ConvertirTableauPixelEnUnique_32bit(int[,] tab_pixel_int_LH_modif, int pixel_largeur, int pixel_hauteur) {
      byte[] tab = new byte[pixel_largeur * 4 * pixel_hauteur];
      int cpt = 0;
      for (int lig = 0; lig < pixel_hauteur; lig++) {
        for (int col = 0; col < pixel_largeur; col++) {
          int couleur_int = tab_pixel_int_LH_modif[lig, col];//code en argb
          Color couleur = new Color();
          couleur.A = (byte)(couleur_int >> 24);
          couleur.R = (byte)(couleur_int >> 16);
          couleur.G = (byte)(couleur_int >> 8);
          couleur.B = (byte)(couleur_int);
          //code bgra pour tableau unique
          tab[cpt] = couleur.B;
          cpt++;
          tab[cpt] = couleur.G;
          cpt++;
          tab[cpt] = couleur.R;
          cpt++;
          tab[cpt] = couleur.A;
          cpt++;
        }
      }
      return tab;
    }
    ////transposition tableau pixel dimension 1 vers 2 avec codage 8 bits
    private int[,] ConvertirTableauPixelEnLH_8bit(byte[] tab_pixel, int pixel_larg, int pixel_haut) {
      int[,] tab_LH = new int[pixel_haut, pixel_larg];
      int lig = 0;
      int col = 0;
      for (int xx = 0; xx < tab_pixel.Length; xx++) {
        byte comp = tab_pixel[xx];
        int couleur_int = (byte)comp;
        tab_LH[lig, col] = couleur_int;
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
  }//end class
}


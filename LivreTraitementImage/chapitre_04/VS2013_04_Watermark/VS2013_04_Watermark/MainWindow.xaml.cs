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

namespace VS2013_04_Watermark {
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
      Uri uri_1 = new Uri("pack://application:,,,/VS2013_04_Watermark;component/collection_images/jo_1971_8bit_388x418_96dpi.jpg", UriKind.Absolute);
      BitmapImage bti_1 = new BitmapImage();
      bti_1.BeginInit();
      bti_1.UriSource = uri_1;
      bti_1.EndInit();
      x_img_origine.Width = bti_1.PixelWidth;
      x_img_origine.Height = bti_1.PixelHeight;
      x_img_origine.Source = bti_1;
      Uri uri_2 = new Uri("pack://application:,,,/VS2013_04_Watermark;component/collection_images/marqueur_8bit_388x418_96dpi.bmp", UriKind.Absolute);
      BitmapImage bti_2 = new BitmapImage();
      bti_2.BeginInit();
      bti_2.UriSource = uri_2;
      bti_2.EndInit();
      x_img_marqueur.Width = bti_2.PixelWidth;
      x_img_marqueur.Height = bti_2.PixelHeight;
      x_img_marqueur.Source = bti_2;
      AdditionnerImage();
    }
    //additionner les deux images
    private void AdditionnerImage() {
      //image 1 avec 8 bits 256 gris
      Uri uri_1 = new Uri("pack://application:,,,/VS2013_04_Watermark;component/collection_images/jo_1971_8bit_388x418_96dpi.jpg", UriKind.Absolute);
      BitmapImage bti_1 = new BitmapImage();
      bti_1.BeginInit();
      bti_1.UriSource = uri_1;
      bti_1.EndInit();
      WriteableBitmap wb_1 = new WriteableBitmap(bti_1);
      int largeur_numerisation_1 = (wb_1.Format.BitsPerPixel / 8) * wb_1.PixelWidth;
      byte[] tab_pixel_1 = new byte[largeur_numerisation_1 * wb_1.PixelHeight];
      wb_1.CopyPixels(tab_pixel_1, largeur_numerisation_1, 0);
      int[,] tab_pixel_int_LH_1 = ConvertirTableauPixelEnLH_8bit(tab_pixel_1, wb_1.PixelWidth, wb_1.PixelHeight);
      //image 2 avec 8 bits 256 gris
      Uri uri_2 = new Uri("pack://application:,,,/VS2013_04_Watermark;component/collection_images/marqueur_8bit_388x418_96dpi.bmp", UriKind.Absolute);
      BitmapImage bti_2 = new BitmapImage();
      bti_2.BeginInit();
      bti_2.UriSource = uri_2;
      bti_2.EndInit();
      WriteableBitmap wb_2 = new WriteableBitmap(bti_2);
      int largeur_numerisation_2 = (wb_2.Format.BitsPerPixel / 8) * wb_2.PixelWidth;
      byte[] tab_pixel_2 = new byte[largeur_numerisation_2 * wb_2.PixelHeight];
      wb_2.CopyPixels(tab_pixel_2, largeur_numerisation_2, 0);
      int[,] tab_pixel_int_LH_2 = ConvertirTableauPixelEnLH_8bit(tab_pixel_2, wb_2.PixelWidth, wb_2.PixelHeight);
      int[,] tab_pixel_int_LH_add = new int[wb_1.PixelHeight, wb_1.PixelWidth];
      for (int lig = 0; lig < wb_1.PixelHeight; lig++) {
        for (int col = 0; col < wb_1.PixelWidth; col++) {
          int niveau_gris_int_1 = tab_pixel_int_LH_1[lig, col];
          int niveau_gris_int_2 = tab_pixel_int_LH_2[lig, col];
          int niveau_gris_int_add = 0;
          if (niveau_gris_int_2 != 255) {
            niveau_gris_int_add = Math.Min(niveau_gris_int_1 + niveau_gris_int_2, 255);
          }
          else {
            niveau_gris_int_add = niveau_gris_int_1;
          }
          tab_pixel_int_LH_add[lig, col] = niveau_gris_int_add;
        }
      }
      byte[] tab_pixel_add = ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_add, wb_1.PixelWidth, wb_1.PixelHeight);
      BitmapSource bti_add = BitmapSource.Create(wb_1.PixelWidth, wb_1.PixelHeight, 96.0, 96.0,
        PixelFormats.Gray8, null, tab_pixel_add, largeur_numerisation_1);
      x_img_add.Width = bti_add.PixelWidth;
      x_img_add.Height = bti_add.PixelHeight;
      x_img_add.Source = bti_add;
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

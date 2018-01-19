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

namespace VS2013_02_TransPuissance {
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
      x_table_lut.ModeliserCourbe(FonctionLutPuissance);
    }
    //vider les controles
    private void ViderControleImage(Uri uri, Image controle_image, int largeur, int hauteur) {
      BitmapImage bti = new BitmapImage();
      bti.BeginInit();
      bti.UriSource = uri;
      bti.EndInit();
      controle_image.Width = largeur;
      controle_image.Height = hauteur;
      controle_image.Source = bti;
    }
    //selection methode conversion
    private void x_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (v_fen_charge == true) {
        Uri uri_vide = new Uri("pack://application:,,,/VS2013_02_TransPuissance;component/contenu/image/fond_damier.png", UriKind.Absolute);
        ViderControleImage(uri_vide, x_img, 770, 399);
        ViderControleImage(uri_vide, x_img_transform, 456, 438);
        if (x_cbx_select.SelectedIndex == 1) {
          Uri uri = new Uri("pack://application:,,,/VS2013_02_TransPuissance;component/collection_images/radio_genou_32bit_800x800_96dpi.png", UriKind.Absolute);
          BitmapImage bti = new BitmapImage();
          bti.BeginInit();
          bti.UriSource = uri;
          bti.EndInit();
          x_img.Width = bti.PixelWidth;
          x_img.Height = bti.PixelHeight;
          x_img.Source = bti;
          AppliquerTransformationImageCouleur32bits(bti);
        }
        if (x_cbx_select.SelectedIndex == 2) {
          Uri uri = new Uri("pack://application:,,,/VS2013_02_TransPuissance;component/collection_images/louis_de_funes_3_32bit_1000x1000_96dpi.jpg", UriKind.Absolute);
          BitmapImage bti = new BitmapImage();
          bti.BeginInit();
          bti.UriSource = uri;
          bti.EndInit();
          x_img.Width = bti.PixelWidth;
          x_img.Height = bti.PixelHeight;
          x_img.Source = bti;
          AppliquerTransformationImageCouleur32bits(bti);
        }
      }
    }
    //fonction LUT utilisée: transformation logarithme
    private double FonctionLutPuissance(double x) {
      double puissance = 4;
      double y = Math.Pow(x, puissance);
      y = y * 255 / Math.Pow(255, puissance);
      if (y < 0) {
        y = 0;
      }
      if (y > 255) {
        y = 255;
      }
      return y;
    }
    //appliquer la transformation sur une image 8 bits
    private void AppliquerTransformationImage256gris(BitmapImage bti) {
      WriteableBitmap wb = new WriteableBitmap(bti);
      int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
      byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
      wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
      int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
      int[,] tab_pixel_int_LH_modif = new int[wb.PixelHeight, wb.PixelWidth];
      for (int lig = 0; lig < wb.PixelHeight; lig++) {
        for (int col = 0; col < wb.PixelWidth; col++) {
          int niveau_gris_int = tab_pixel_int_LH[lig, col];
          int niveau_gris_int_tranf = (int)FonctionLutPuissance(niveau_gris_int);
          tab_pixel_int_LH_modif[lig, col] = niveau_gris_int_tranf;
        }
      }
      byte[] tab_pixel_modif = ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_modif, wb.PixelWidth, wb.PixelHeight);
      BitmapSource bti_modif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
        PixelFormats.Gray8, null, tab_pixel_modif, largeur_numerisation);
      x_img_transform.Width = bti_modif.PixelWidth;
      x_img_transform.Height = bti_modif.PixelHeight;
      x_img_transform.Source = bti_modif;
    }
    //appliquer la transformation sur une image 32 bits
    private void AppliquerTransformationImageCouleur32bits(BitmapImage bti) {
      WriteableBitmap wb = new WriteableBitmap(bti);
      int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
      byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
      wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
      int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_32bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
      int[,] tab_pixel_int_LH_modif = new int[wb.PixelHeight, wb.PixelWidth];
      for (int lig = 0; lig < wb.PixelHeight; lig++) {
        for (int col = 0; col < wb.PixelWidth; col++) {
          int couleur_int = tab_pixel_int_LH[lig, col];
          Color couleur = new Color();
          couleur.A = (byte)(couleur_int >> 24);
          couleur.R = (byte)(couleur_int >> 16);
          couleur.G = (byte)(couleur_int >> 8);
          couleur.B = (byte)(couleur_int);
          Color couleur_tranf = new Color();
          couleur_tranf.A = couleur.A;
          couleur_tranf.R = (byte)FonctionLutPuissance(couleur.R);
          couleur_tranf.G = (byte)FonctionLutPuissance(couleur.G);
          couleur_tranf.B = (byte)FonctionLutPuissance(couleur.B);
          int couleur_transf_int = couleur_tranf.A << 24 | couleur_tranf.R << 16 | couleur_tranf.G << 8 | couleur_tranf.B;
          tab_pixel_int_LH_modif[lig, col] = couleur_transf_int;
        }
      }
      byte[] tab_pixel_modif = ConvertirTableauPixelEnUnique_32bit(tab_pixel_int_LH_modif, wb.PixelWidth, wb.PixelHeight);
      BitmapSource bti_modif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
        PixelFormats.Bgra32, null, tab_pixel_modif, largeur_numerisation);
      x_img_transform.Width = bti_modif.PixelWidth;
      x_img_transform.Height = bti_modif.PixelHeight;
      x_img_transform.Source = bti_modif;
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


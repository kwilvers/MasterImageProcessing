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

namespace VS2013_01_LirePixel
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //donnees
        private string RC = Environment.NewLine;

        private bool v_fen_chargee = false;
        private string doss_exe = Environment.CurrentDirectory;

        private BitmapImage v_bti_choisi = null;

        //constructeur
        public MainWindow()
        {
            InitializeComponent();
        }

        //menu fichier -> quitter
        private void x_men_fichier_quitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //fenetre evenement Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            v_fen_chargee = true;
            x_btn_lecture.IsEnabled = false;
        }

        //
        private void x_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (v_fen_chargee == true)
                {
                    //aucune selection
                    if (x_cbx_select.SelectedIndex == 0)
                    {
                        x_img.Width = 775;
                        x_img.Height = 481;
                        x_img.Source = new BitmapImage(new Uri("contenu/image/fond_damier.png", UriKind.Relative));
                        x_btn_lecture.IsEnabled = false;
                        x_btn_infos.IsEnabled = false;
                        x_img_modif.Width = 775;
                        x_img_modif.Height = 396;
                        x_img_modif.Source =
                            new BitmapImage(new Uri("contenu/image/fond_damier.png", UriKind.Relative));
                    }
                    //image couleur
                    if (x_cbx_select.SelectedIndex == 1)
                    {
                        BitmapImage bti = new BitmapImage();
                        bti.BeginInit();
                        bti.UriSource =
                            new Uri(
                                "pack://application:,,,/VS2013_01_LirePixel;component/collection_images/coul_louis_de_funes_1.jpg",
                                UriKind.Absolute);
                        bti.EndInit();
                        x_img.Width = bti.PixelWidth;
                        x_img.Height = bti.PixelHeight;
                        x_img.Source = bti;
                        x_btn_lecture.IsEnabled = true;
                        x_btn_infos.IsEnabled = true;
                        v_bti_choisi = bti;
                        x_img_modif.Width = 775;
                        x_img_modif.Height = 396;
                        x_img_modif.Source =
                            new BitmapImage(new Uri("contenu/image/fond_damier.png", UriKind.Relative));
                    }
                    //image niveau de gris
                    if (x_cbx_select.SelectedIndex == 2)
                    {
                        BitmapImage bti = new BitmapImage();
                        bti.BeginInit();
                        bti.UriSource =
                            new Uri(
                                "pack://application:,,,/VS2013_01_LirePixel;component/collection_images/ng_louis_de_funes_1.jpg",
                                UriKind.Absolute);
                        bti.EndInit();
                        x_img.Width = bti.PixelWidth;
                        x_img.Height = bti.PixelHeight;
                        x_img.Source = bti;
                        x_btn_lecture.IsEnabled = true;
                        x_btn_infos.IsEnabled = true;
                        v_bti_choisi = bti;
                        x_img_modif.Width = 775;
                        x_img_modif.Height = 396;
                        x_img_modif.Source =
                            new BitmapImage(new Uri("contenu/image/fond_damier.png", UriKind.Relative));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //btn lecture des pixels
        private void x_btn_lecture_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bti = new BitmapImage();
            //image couleur
            if (x_cbx_select.SelectedIndex == 1)
            {
                bti.BeginInit();
                bti.UriSource =
                    new Uri(
                        "pack://application:,,,/VS2013_01_LirePixel;component/collection_images/coul_louis_de_funes_1.jpg",
                        UriKind.Absolute);
                bti.EndInit();
                WriteableBitmap wb = new WriteableBitmap(bti);
                int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight]; //codage bgra
                wb.CopyPixels(tab_pixel, largeur_numerisation, 0);

                
                int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_32bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                int[,] tab_pixel_int_LH_modif = new int[wb.PixelHeight, wb.PixelWidth];
                for (int lig = 0; lig < wb.PixelHeight; lig++)
                {
                    for (int col = 0; col < wb.PixelWidth; col++)
                    {
                        int couleur_int = tab_pixel_int_LH[lig, col];
                        Color couleur = new Color();
                        if (col % 20 == 0 || lig % 20 == 0)
                        {
                            couleur.A = 255;
                            couleur.R = 255;
                            couleur.G = 255;
                            couleur.B = 255;
                        }
                        else
                        {
                            couleur.A = (byte) (couleur_int >> 24);
                            couleur.R = (byte) (couleur_int >> 16);
                            couleur.G = (byte) (couleur_int >> 8);
                            couleur.B = (byte) (couleur_int);
                        }
                        int couleur_int_modif = couleur.A << 24 | couleur.R << 16 | couleur.G << 8 | couleur.B;
                        tab_pixel_int_LH_modif[lig, col] = couleur_int_modif;
                    }
                }
                byte[] tab_pixel_modif =
                    ConvertirTableauPixelEnUnique_32bit(tab_pixel_int_LH_modif, wb.PixelWidth, wb.PixelHeight);
                BitmapSource bti_modif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                    PixelFormats.Bgra32, null, tab_pixel_modif, largeur_numerisation);
                x_img_modif.Width = bti_modif.PixelWidth;
                x_img_modif.Height = bti_modif.PixelHeight;
                x_img_modif.Source = bti_modif;
            }
            //image niveau de gris
            if (x_cbx_select.SelectedIndex == 2)
            {
                bti.BeginInit();
                bti.UriSource =
                    new Uri(
                        "pack://application:,,,/VS2013_01_LirePixel;component/collection_images/ng_louis_de_funes_1.jpg",
                        UriKind.Absolute);
                bti.EndInit();
                WriteableBitmap wb = new WriteableBitmap(bti);
                int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight]; //codage bgra
                wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
                int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                int[,] tab_pixel_int_LH_modif = new int[wb.PixelHeight, wb.PixelWidth];
                for (int lig = 0; lig < wb.PixelHeight; lig++)
                {
                    for (int col = 0; col < wb.PixelWidth; col++)
                    {
                        int comp_int = tab_pixel_int_LH[lig, col];
                        int comp_int_modif;
                        if (col % 20 == 0 || lig % 20 == 0)
                        {
                            comp_int_modif = 255;
                        }
                        else
                        {
                            comp_int_modif = comp_int;
                        }
                        tab_pixel_int_LH_modif[lig, col] = comp_int_modif;
                    }
                }
                byte[] tab_pixel_modif =
                    ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_modif, wb.PixelWidth, wb.PixelHeight);
                BitmapSource bti_modif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                    PixelFormats.Gray8, null, tab_pixel_modif, largeur_numerisation);
                x_img_modif.Width = bti_modif.PixelWidth;
                x_img_modif.Height = bti_modif.PixelHeight;
                x_img_modif.Source = bti_modif;
            }
        }

        //
        private int[,] ConvertirTableauPixelEnLH_32bit(byte[] tab_pixel, int pixel_larg, int pixel_haut)
        {
            int[,] tab_LH = new int[pixel_haut, pixel_larg];
            int lig = 0;
            int col = 0;
            for (int xx = 0; xx < tab_pixel.Length; xx += 4)
            {
                byte comp_b = tab_pixel[xx];
                byte comp_g = tab_pixel[xx + 1];
                byte comp_r = tab_pixel[xx + 2];
                byte comp_a = tab_pixel[xx + 3];
                int couleur_int = comp_a << 24 | comp_r << 16 | comp_g << 8 | comp_b;
                tab_LH[lig, col] = couleur_int;
                col++;
                if (col == pixel_larg)
                {
                    col = 0;
                    lig++;
                }
            }
            return tab_LH;
        }

        //
        private byte[] ConvertirTableauPixelEnUnique_32bit(int[,] tab_pixel_int_LH_modif, int pixel_largeur,
            int pixel_hauteur)
        {
            byte[] tab = new byte[pixel_largeur * 4 * pixel_hauteur];
            int cpt = 0;
            for (int lig = 0; lig < pixel_hauteur; lig++)
            {
                for (int col = 0; col < pixel_largeur; col++)
                {
                    int couleur_int = tab_pixel_int_LH_modif[lig, col]; //code en argb
                    Color couleur = new Color();
                    couleur.A = (byte) (couleur_int >> 24);
                    couleur.R = (byte) (couleur_int >> 16);
                    couleur.G = (byte) (couleur_int >> 8);
                    couleur.B = (byte) (couleur_int);
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

        //
        private int[,] ConvertirTableauPixelEnLH_8bit(byte[] tab_pixel, int pixel_larg, int pixel_haut)
        {
            int[,] tab_LH = new int[pixel_haut, pixel_larg];
            int lig = 0;
            int col = 0;
            for (int xx = 0; xx < tab_pixel.Length; xx++)
            {
                byte comp = tab_pixel[xx];
                int couleur_int = (byte) comp;
                tab_LH[lig, col] = couleur_int;
                col++;
                if (col == pixel_larg)
                {
                    col = 0;
                    lig++;
                }
            }
            return tab_LH;
        }

        //
        private byte[] ConvertirTableauPixelEnUnique_8bit(int[,] tab_pixel_int_LH_modif, int pixel_largeur,
            int pixel_hauteur)
        {
            byte[] tab = new byte[pixel_largeur * pixel_hauteur];
            int cpt = 0;
            for (int lig = 0; lig < pixel_hauteur; lig++)
            {
                for (int col = 0; col < pixel_largeur; col++)
                {
                    tab[cpt] = (byte) tab_pixel_int_LH_modif[lig, col];
                    cpt++;
                }
            }
            return tab;
        }

        //
        private string AfficherInfos(BitmapImage wb)
        {
            string aff = "";
            aff += "PixelWidth PixelHeight: " + wb.PixelWidth.ToString() + "x" + wb.PixelHeight.ToString() + " pixels" +
                   RC;
            aff += "résolution DpiX DpiY: " + wb.DpiX.ToString() + "x" + wb.DpiY.ToString() + RC;
            aff += "Width Height: " + wb.Width.ToString() + "x" + wb.Height.ToString() + " pixels" + RC;
            aff += "format: " + wb.Format.BitsPerPixel.ToString() + " bits par pixel" + RC;
            return aff;
        }

        //
        private void x_btn_infos_Click(object sender, RoutedEventArgs e)
        {
            x_cnv_couverture.Visibility = Visibility.Visible;
            x_tbl_infos.Text = AfficherInfos(v_bti_choisi);
        }

        //
        private void x_btn_fermer_infos_Click(object sender, RoutedEventArgs e)
        {
            x_tbl_infos.Text = "";
            x_cnv_couverture.Visibility = Visibility.Collapsed;
        }
    } //end class
}
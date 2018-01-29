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

namespace VS2013_01_ConvertirNiveauGris
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //donnees
        private string RC = Environment.NewLine;

        private string doss_exe = Environment.CurrentDirectory;

        private bool v_fen_charge = false;

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
            v_fen_charge = true;
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource =
                new Uri(
                    "pack://application:,,,/VS2013_01_ConvertirNiveauGris;component/collection_images/aff_rabbi_jacob_800x1022_96dpi.jpg",
                    UriKind.Absolute);
            bti.EndInit();
            x_img.Width = bti.PixelWidth;
            x_img.Height = bti.PixelHeight;
            x_img.Source = bti;
        }

        //selection methode conversion
        private void x_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (v_fen_charge == true)
            {
                if (x_cbx_select.SelectedIndex == 0)
                {
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource =
                        new Uri(
                            "pack://application:,,,/VS2013_01_ConvertirNiveauGris;component/contenu/image/fond_damier.png",
                            UriKind.Absolute);
                    bti.EndInit();
                    x_img_conversion.Width = 770;
                    x_img_conversion.Height = 401;
                    x_img_conversion.Source = bti;
                }
                if (x_cbx_select.SelectedIndex == 1)
                {
                    Convertir256NiveauxGrisMoyenne();
                }
                if (x_cbx_select.SelectedIndex == 2)
                {
                    Convertir256NiveauxGrisRec709();
                }
            }
        }

        //convertir selon moyenne composante
        private void Convertir256NiveauxGrisMoyenne()
        {
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource =
                new Uri(
                    "pack://application:,,,/VS2013_01_ConvertirNiveauGris;component/collection_images/aff_rabbi_jacob_800x1022_96dpi.jpg",
                    UriKind.Absolute);
            bti.EndInit();
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeurNumerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tabPixel = new byte[largeurNumerisation * wb.PixelHeight]; //codage bgra
            wb.CopyPixels(tabPixel, largeurNumerisation, 0);
            int[,] tabPixelIntLh = ConvertirTableauPixelEnLH_32bit(tabPixel, wb.PixelWidth, wb.PixelHeight);
            int[,] tabPixelIntLhModif = new int[wb.PixelHeight, wb.PixelWidth];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    int couleurInt = tabPixelIntLh[lig, col];
                    Color couleur = new Color
                    {
                        A = (byte) (couleurInt >> 24),
                        R = (byte) (couleurInt >> 16),
                        G = (byte) (couleurInt >> 8),
                        B = (byte) (couleurInt)
                    };
                    double moyenne = (couleur.R + couleur.G + couleur.B) / 3;
                    couleur.R = (byte) moyenne;
                    couleur.G = (byte) moyenne;
                    couleur.B = (byte) moyenne;
                    int couleurIntModif = couleur.A << 24 | couleur.R << 16 | couleur.G << 8 | couleur.B;
                    tabPixelIntLhModif[lig, col] = couleurIntModif;
                }
            }
            byte[] tabPixelModif =
                ConvertirTableauPixelEnUnique_32bit(tabPixelIntLhModif, wb.PixelWidth, wb.PixelHeight);
            BitmapSource btiModif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                PixelFormats.Bgra32, null, tabPixelModif, largeurNumerisation);
            x_img_conversion.Width = btiModif.PixelWidth;
            x_img_conversion.Height = btiModif.PixelHeight;
            x_img_conversion.Source = btiModif;
        }

        //methode recommandation 709
        private void Convertir256NiveauxGrisRec709()
        {
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource =
                new Uri(
                    "pack://application:,,,/VS2013_01_ConvertirNiveauGris;component/collection_images/aff_rabbi_jacob_800x1022_96dpi.jpg",
                    UriKind.Absolute);
            bti.EndInit();
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeurNumerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tabPixel = new byte[largeurNumerisation * wb.PixelHeight]; //codage bgra
            wb.CopyPixels(tabPixel, largeurNumerisation, 0);
            int[,] tabPixelIntLh = ConvertirTableauPixelEnLH_32bit(tabPixel, wb.PixelWidth, wb.PixelHeight);
            int[,] tabPixelIntLhModif = new int[wb.PixelHeight, wb.PixelWidth];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    int couleurInt = tabPixelIntLh[lig, col];
                    Color couleur = new Color
                    {
                        A = (byte) (couleurInt >> 24),
                        R = (byte) (couleurInt >> 16),
                        G = (byte) (couleurInt >> 8),
                        B = (byte) (couleurInt)
                    };
                    double rec709 = 0.2125d * couleur.R + 0.7154d * couleur.G + 0.0721d * couleur.G;
                    couleur.R = (byte) rec709;
                    couleur.G = (byte) rec709;
                    couleur.B = (byte) rec709;
                    int couleurIntModif = couleur.A << 24 | couleur.R << 16 | couleur.G << 8 | couleur.B;
                    tabPixelIntLhModif[lig, col] = couleurIntModif;
                }
            }
            byte[] tabPixelModif =
                ConvertirTableauPixelEnUnique_32bit(tabPixelIntLhModif, wb.PixelWidth, wb.PixelHeight);
            BitmapSource btiModif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                PixelFormats.Bgra32, null, tabPixelModif, largeurNumerisation);
            x_img_conversion.Width = btiModif.PixelWidth;
            x_img_conversion.Height = btiModif.PixelHeight;
            x_img_conversion.Source = btiModif;
        }

        //
        private int[,] ConvertirTableauPixelEnLH_32bit(byte[] tabPixel, int pixelLarg, int pixelHaut)
        {
            int[,] tabLh = new int[pixelHaut, pixelLarg];
            int lig = 0;
            int col = 0;
            for (int xx = 0; xx < tabPixel.Length; xx += 4)
            {
                byte compB = tabPixel[xx];
                byte compG = tabPixel[xx + 1];
                byte compR = tabPixel[xx + 2];
                byte compA = tabPixel[xx + 3];
                int couleurInt = compA << 24 | compR << 16 | compG << 8 | compB;
                tabLh[lig, col] = couleurInt;
                col++;
                if (col == pixelLarg)
                {
                    col = 0;
                    lig++;
                }
            }
            return tabLh;
        }

        //
        private byte[] ConvertirTableauPixelEnUnique_32bit(int[,] tabPixelIntLhModif, int pixelLargeur,
            int pixelHauteur)
        {
            byte[] tab = new byte[pixelLargeur * 4 * pixelHauteur];
            int cpt = 0;
            for (int lig = 0; lig < pixelHauteur; lig++)
            {
                for (int col = 0; col < pixelLargeur; col++)
                {
                    int couleurInt = tabPixelIntLhModif[lig, col]; //code en argb
                    Color couleur = new Color
                    {
                        A = (byte) (couleurInt >> 24),
                        R = (byte) (couleurInt >> 16),
                        G = (byte) (couleurInt >> 8),
                        B = (byte) (couleurInt)
                    };
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
    } //end class
}
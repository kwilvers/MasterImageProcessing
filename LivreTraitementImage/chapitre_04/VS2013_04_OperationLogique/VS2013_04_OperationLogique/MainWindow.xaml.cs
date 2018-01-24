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

namespace VS2013_04_OperationLogique
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
            Uri uri_1 = new Uri(
                "pack://application:,,,/VS2013_04_OperationLogique;component/collection_images/operation_logique_8bit_ref1.bmp",
                UriKind.Absolute);
            BitmapImage bti_1 = new BitmapImage();
            bti_1.BeginInit();
            bti_1.UriSource = uri_1;
            bti_1.EndInit();
            x_img_origine_1.Width = bti_1.PixelWidth;
            x_img_origine_1.Height = bti_1.PixelHeight;
            x_img_origine_1.Source = bti_1;
            Uri uri_2 = new Uri(
                "pack://application:,,,/VS2013_04_OperationLogique;component/collection_images/operation_logique_8bit_ref2.bmp",
                UriKind.Absolute);
            BitmapImage bti_2 = new BitmapImage();
            bti_2.BeginInit();
            bti_2.UriSource = uri_2;
            bti_2.EndInit();
            x_img_origine_2.Width = bti_2.PixelWidth;
            x_img_origine_2.Height = bti_2.PixelHeight;
            x_img_origine_2.Source = bti_2;
        }

        //
        private void x_cbx_operation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (v_fen_charge == true)
            {
                Uri uri_vide =
                    new Uri("pack://application:,,,/VS2013_04_OperationLogique;component/contenu/image/fond_damier.png",
                        UriKind.Absolute);
                ViderControleImage(uri_vide, x_img_operation_res, 388, 418);
                ViderControleImage(uri_vide, x_img_operation_intensite, 388, 418);
                //A AND B
                if (x_cbx_operation.SelectedIndex == 1)
                {
                    //image 1 avec 8 bits 256 gris
                    Uri uri_1 = new Uri(
                        "pack://application:,,,/VS2013_04_OperationLogique;component/collection_images/operation_logique_8bit_ref1.bmp",
                        UriKind.Absolute);
                    BitmapImage bti_1 = new BitmapImage();
                    bti_1.BeginInit();
                    bti_1.UriSource = uri_1;
                    bti_1.EndInit();
                    WriteableBitmap wb_1 = new WriteableBitmap(bti_1);
                    int largeur_numerisation_1 = (wb_1.Format.BitsPerPixel / 8) * wb_1.PixelWidth;
                    byte[] tab_pixel_1 = new byte[largeur_numerisation_1 * wb_1.PixelHeight];
                    wb_1.CopyPixels(tab_pixel_1, largeur_numerisation_1, 0);
                    int[,] tab_pixel_int_LH_1 =
                        ConvertirTableauPixelEnLH_8bit(tab_pixel_1, wb_1.PixelWidth, wb_1.PixelHeight);
                    //image 2 avec 8 bits 256 gris
                    Uri uri_2 = new Uri(
                        "pack://application:,,,/VS2013_04_OperationLogique;component/collection_images/operation_logique_8bit_ref2.bmp",
                        UriKind.Absolute);
                    BitmapImage bti_2 = new BitmapImage();
                    bti_2.BeginInit();
                    bti_2.UriSource = uri_2;
                    bti_2.EndInit();
                    WriteableBitmap wb_2 = new WriteableBitmap(bti_2);
                    int largeur_numerisation_2 = (wb_2.Format.BitsPerPixel / 8) * wb_2.PixelWidth;
                    byte[] tab_pixel_2 = new byte[largeur_numerisation_2 * wb_2.PixelHeight];
                    wb_2.CopyPixels(tab_pixel_2, largeur_numerisation_2, 0);
                    int[,] tab_pixel_int_LH_2 =
                        ConvertirTableauPixelEnLH_8bit(tab_pixel_2, wb_2.PixelWidth, wb_2.PixelHeight);
                    //comparaison pour mettre en evidence la surface de l'operation logique
                    int[,] tab_pixel_int_LH_oper = new int[wb_1.PixelHeight, wb_1.PixelWidth];
                    for (int lig = 0; lig < wb_1.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb_1.PixelWidth; col++)
                        {
                            int niveau_gris_int_1 = tab_pixel_int_LH_1[lig, col];
                            int niveau_gris_int_2 = tab_pixel_int_LH_2[lig, col];
                            int niveau_gris_int_oper = 255;
                            if (niveau_gris_int_1 != 255)
                            {
                                niveau_gris_int_oper = 127;
                            }
                            if (niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = 127;
                            }
                            if (niveau_gris_int_1 != 255 && niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = 0;
                            }
                            tab_pixel_int_LH_oper[lig, col] = niveau_gris_int_oper;
                        }
                    }
                    byte[] tab_pixel_oper =
                        ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_oper, wb_1.PixelWidth, wb_1.PixelHeight);
                    BitmapSource bti_oper_1 = BitmapSource.Create(wb_1.PixelWidth, wb_1.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_oper, largeur_numerisation_1);
                    x_img_operation_res.Width = bti_oper_1.PixelWidth;
                    x_img_operation_res.Height = bti_oper_1.PixelHeight;
                    x_img_operation_res.Source = bti_oper_1;
                    //comparaison pour mettre en evidence la surface de l'operation logique
                    int[,] tab_pixel_int_LH_inten = new int[wb_1.PixelHeight, wb_1.PixelWidth];
                    for (int lig = 0; lig < wb_1.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb_1.PixelWidth; col++)
                        {
                            int niveau_gris_int_1 = tab_pixel_int_LH_1[lig, col];
                            int niveau_gris_int_2 = tab_pixel_int_LH_2[lig, col];
                            int niveau_gris_int_oper = 255;
                            if (niveau_gris_int_1 != 255)
                            {
                                niveau_gris_int_oper = niveau_gris_int_1;
                            }
                            if (niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = niveau_gris_int_2;
                            }
                            if (niveau_gris_int_1 != 255 && niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = (niveau_gris_int_1 + niveau_gris_int_2) / 2;
                            }
                            tab_pixel_int_LH_inten[lig, col] = niveau_gris_int_oper;
                        }
                    }
                    byte[] tab_pixel_inten =
                        ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_inten, wb_1.PixelWidth, wb_1.PixelHeight);
                    BitmapSource bti_inten_1 = BitmapSource.Create(wb_1.PixelWidth, wb_1.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_inten, largeur_numerisation_1);
                    x_img_operation_intensite.Width = bti_inten_1.PixelWidth;
                    x_img_operation_intensite.Height = bti_inten_1.PixelHeight;
                    x_img_operation_intensite.Source = bti_inten_1;
                }
                //A OR B
                if (x_cbx_operation.SelectedIndex == 2)
                {
                    //image 1 avec 8 bits 256 gris
                    Uri uri_1 = new Uri(
                        "pack://application:,,,/VS2013_04_OperationLogique;component/collection_images/operation_logique_8bit_ref1.bmp",
                        UriKind.Absolute);
                    BitmapImage bti_1 = new BitmapImage();
                    bti_1.BeginInit();
                    bti_1.UriSource = uri_1;
                    bti_1.EndInit();
                    WriteableBitmap wb_1 = new WriteableBitmap(bti_1);
                    int largeur_numerisation_1 = (wb_1.Format.BitsPerPixel / 8) * wb_1.PixelWidth;
                    byte[] tab_pixel_1 = new byte[largeur_numerisation_1 * wb_1.PixelHeight];
                    wb_1.CopyPixels(tab_pixel_1, largeur_numerisation_1, 0);
                    int[,] tab_pixel_int_LH_1 =
                        ConvertirTableauPixelEnLH_8bit(tab_pixel_1, wb_1.PixelWidth, wb_1.PixelHeight);
                    //image 2 avec 8 bits 256 gris
                    Uri uri_2 = new Uri(
                        "pack://application:,,,/VS2013_04_OperationLogique;component/collection_images/operation_logique_8bit_ref2.bmp",
                        UriKind.Absolute);
                    BitmapImage bti_2 = new BitmapImage();
                    bti_2.BeginInit();
                    bti_2.UriSource = uri_2;
                    bti_2.EndInit();
                    WriteableBitmap wb_2 = new WriteableBitmap(bti_2);
                    int largeur_numerisation_2 = (wb_2.Format.BitsPerPixel / 8) * wb_2.PixelWidth;
                    byte[] tab_pixel_2 = new byte[largeur_numerisation_2 * wb_2.PixelHeight];
                    wb_2.CopyPixels(tab_pixel_2, largeur_numerisation_2, 0);
                    int[,] tab_pixel_int_LH_2 =
                        ConvertirTableauPixelEnLH_8bit(tab_pixel_2, wb_2.PixelWidth, wb_2.PixelHeight);
                    //comparaison pour mettre en evidence la surface de l'operation logique
                    int[,] tab_pixel_int_LH_oper = new int[wb_1.PixelHeight, wb_1.PixelWidth];
                    for (int lig = 0; lig < wb_1.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb_1.PixelWidth; col++)
                        {
                            int niveau_gris_int_1 = tab_pixel_int_LH_1[lig, col];
                            int niveau_gris_int_2 = tab_pixel_int_LH_2[lig, col];
                            int niveau_gris_int_oper = 255;
                            if (niveau_gris_int_1 != 255)
                            {
                                niveau_gris_int_oper = 127;
                            }
                            if (niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = 127;
                            }
                            if (niveau_gris_int_1 != 255 && niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = 0;
                            }
                            if (niveau_gris_int_1 != 255 && niveau_gris_int_2 == 255)
                            {
                                niveau_gris_int_oper = 0;
                            }
                            if (niveau_gris_int_1 == 255 && niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = 0;
                            }
                            tab_pixel_int_LH_oper[lig, col] = niveau_gris_int_oper;
                        }
                    }
                    byte[] tab_pixel_oper =
                        ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_oper, wb_1.PixelWidth, wb_1.PixelHeight);
                    BitmapSource bti_oper_1 = BitmapSource.Create(wb_1.PixelWidth, wb_1.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_oper, largeur_numerisation_1);
                    x_img_operation_res.Width = bti_oper_1.PixelWidth;
                    x_img_operation_res.Height = bti_oper_1.PixelHeight;
                    x_img_operation_res.Source = bti_oper_1;
                    //comparaison pour mettre en evidence la surface de l'operation logique
                    int[,] tab_pixel_int_LH_inten = new int[wb_1.PixelHeight, wb_1.PixelWidth];
                    for (int lig = 0; lig < wb_1.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb_1.PixelWidth; col++)
                        {
                            int niveau_gris_int_1 = tab_pixel_int_LH_1[lig, col];
                            int niveau_gris_int_2 = tab_pixel_int_LH_2[lig, col];
                            int niveau_gris_int_oper = 255;
                            if (niveau_gris_int_1 != 255)
                            {
                                niveau_gris_int_oper = niveau_gris_int_1;
                            }
                            if (niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = niveau_gris_int_2;
                            }
                            if (niveau_gris_int_1 != 255 && niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = (niveau_gris_int_1 + niveau_gris_int_2) / 2;
                            }
                            if (niveau_gris_int_1 != 255 && niveau_gris_int_2 == 255)
                            {
                                niveau_gris_int_oper = (niveau_gris_int_1 + niveau_gris_int_2) / 2;
                            }
                            if (niveau_gris_int_1 == 255 && niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = (niveau_gris_int_1 + niveau_gris_int_2) / 2;
                            }
                            tab_pixel_int_LH_inten[lig, col] = niveau_gris_int_oper;
                        }
                    }
                    byte[] tab_pixel_inten =
                        ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_inten, wb_1.PixelWidth, wb_1.PixelHeight);
                    BitmapSource bti_inten_1 = BitmapSource.Create(wb_1.PixelWidth, wb_1.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_inten, largeur_numerisation_1);
                    x_img_operation_intensite.Width = bti_inten_1.PixelWidth;
                    x_img_operation_intensite.Height = bti_inten_1.PixelHeight;
                    x_img_operation_intensite.Source = bti_inten_1;
                }
                //A XOR B
                if (x_cbx_operation.SelectedIndex == 3)
                {
                    //image 1 avec 8 bits 256 gris
                    Uri uri_1 = new Uri(
                        "pack://application:,,,/VS2013_04_OperationLogique;component/collection_images/operation_logique_8bit_ref1.bmp",
                        UriKind.Absolute);
                    BitmapImage bti_1 = new BitmapImage();
                    bti_1.BeginInit();
                    bti_1.UriSource = uri_1;
                    bti_1.EndInit();
                    WriteableBitmap wb_1 = new WriteableBitmap(bti_1);
                    int largeur_numerisation_1 = (wb_1.Format.BitsPerPixel / 8) * wb_1.PixelWidth;
                    byte[] tab_pixel_1 = new byte[largeur_numerisation_1 * wb_1.PixelHeight];
                    wb_1.CopyPixels(tab_pixel_1, largeur_numerisation_1, 0);
                    int[,] tab_pixel_int_LH_1 =
                        ConvertirTableauPixelEnLH_8bit(tab_pixel_1, wb_1.PixelWidth, wb_1.PixelHeight);
                    //image 2 avec 8 bits 256 gris
                    Uri uri_2 = new Uri(
                        "pack://application:,,,/VS2013_04_OperationLogique;component/collection_images/operation_logique_8bit_ref2.bmp",
                        UriKind.Absolute);
                    BitmapImage bti_2 = new BitmapImage();
                    bti_2.BeginInit();
                    bti_2.UriSource = uri_2;
                    bti_2.EndInit();
                    WriteableBitmap wb_2 = new WriteableBitmap(bti_2);
                    int largeur_numerisation_2 = (wb_2.Format.BitsPerPixel / 8) * wb_2.PixelWidth;
                    byte[] tab_pixel_2 = new byte[largeur_numerisation_2 * wb_2.PixelHeight];
                    wb_2.CopyPixels(tab_pixel_2, largeur_numerisation_2, 0);
                    int[,] tab_pixel_int_LH_2 =
                        ConvertirTableauPixelEnLH_8bit(tab_pixel_2, wb_2.PixelWidth, wb_2.PixelHeight);
                    //comparaison pour mettre en evidence la surface de l'operation logique
                    int[,] tab_pixel_int_LH_oper = new int[wb_1.PixelHeight, wb_1.PixelWidth];
                    for (int lig = 0; lig < wb_1.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb_1.PixelWidth; col++)
                        {
                            int niveau_gris_int_1 = tab_pixel_int_LH_1[lig, col];
                            int niveau_gris_int_2 = tab_pixel_int_LH_2[lig, col];
                            int niveau_gris_int_oper = 255;
                            if (niveau_gris_int_1 != 255)
                            {
                                niveau_gris_int_oper = 127;
                            }
                            if (niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = 127;
                            }
                            if (niveau_gris_int_1 != 255 && niveau_gris_int_2 == 255)
                            {
                                niveau_gris_int_oper = 0;
                            }
                            if (niveau_gris_int_1 == 255 && niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = 0;
                            }
                            tab_pixel_int_LH_oper[lig, col] = niveau_gris_int_oper;
                        }
                    }
                    byte[] tab_pixel_oper =
                        ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_oper, wb_1.PixelWidth, wb_1.PixelHeight);
                    BitmapSource bti_oper_1 = BitmapSource.Create(wb_1.PixelWidth, wb_1.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_oper, largeur_numerisation_1);
                    x_img_operation_res.Width = bti_oper_1.PixelWidth;
                    x_img_operation_res.Height = bti_oper_1.PixelHeight;
                    x_img_operation_res.Source = bti_oper_1;
                    //comparaison pour mettre en evidence la surface de l'operation logique
                    int[,] tab_pixel_int_LH_inten = new int[wb_1.PixelHeight, wb_1.PixelWidth];
                    for (int lig = 0; lig < wb_1.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb_1.PixelWidth; col++)
                        {
                            int niveau_gris_int_1 = tab_pixel_int_LH_1[lig, col];
                            int niveau_gris_int_2 = tab_pixel_int_LH_2[lig, col];
                            int niveau_gris_int_oper = 255;
                            if (niveau_gris_int_1 != 255)
                            {
                                niveau_gris_int_oper = niveau_gris_int_1;
                            }
                            if (niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = niveau_gris_int_2;
                            }
                            if (niveau_gris_int_1 != 255 && niveau_gris_int_2 == 255)
                            {
                                niveau_gris_int_oper = (niveau_gris_int_1 + niveau_gris_int_2) / 2;
                            }
                            if (niveau_gris_int_1 == 255 && niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = (niveau_gris_int_1 + niveau_gris_int_2) / 2;
                            }
                            tab_pixel_int_LH_inten[lig, col] = niveau_gris_int_oper;
                        }
                    }
                    byte[] tab_pixel_inten =
                        ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_inten, wb_1.PixelWidth, wb_1.PixelHeight);
                    BitmapSource bti_inten_1 = BitmapSource.Create(wb_1.PixelWidth, wb_1.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_inten, largeur_numerisation_1);
                    x_img_operation_intensite.Width = bti_inten_1.PixelWidth;
                    x_img_operation_intensite.Height = bti_inten_1.PixelHeight;
                    x_img_operation_intensite.Source = bti_inten_1;
                }
                //NOT A AND B
                if (x_cbx_operation.SelectedIndex == 4)
                {
                    //image 1 avec 8 bits 256 gris
                    Uri uri_1 = new Uri(
                        "pack://application:,,,/VS2013_04_OperationLogique;component/collection_images/operation_logique_8bit_ref1.bmp",
                        UriKind.Absolute);
                    BitmapImage bti_1 = new BitmapImage();
                    bti_1.BeginInit();
                    bti_1.UriSource = uri_1;
                    bti_1.EndInit();
                    WriteableBitmap wb_1 = new WriteableBitmap(bti_1);
                    int largeur_numerisation_1 = (wb_1.Format.BitsPerPixel / 8) * wb_1.PixelWidth;
                    byte[] tab_pixel_1 = new byte[largeur_numerisation_1 * wb_1.PixelHeight];
                    wb_1.CopyPixels(tab_pixel_1, largeur_numerisation_1, 0);
                    int[,] tab_pixel_int_LH_1 =
                        ConvertirTableauPixelEnLH_8bit(tab_pixel_1, wb_1.PixelWidth, wb_1.PixelHeight);
                    //image 2 avec 8 bits 256 gris
                    Uri uri_2 = new Uri(
                        "pack://application:,,,/VS2013_04_OperationLogique;component/collection_images/operation_logique_8bit_ref2.bmp",
                        UriKind.Absolute);
                    BitmapImage bti_2 = new BitmapImage();
                    bti_2.BeginInit();
                    bti_2.UriSource = uri_2;
                    bti_2.EndInit();
                    WriteableBitmap wb_2 = new WriteableBitmap(bti_2);
                    int largeur_numerisation_2 = (wb_2.Format.BitsPerPixel / 8) * wb_2.PixelWidth;
                    byte[] tab_pixel_2 = new byte[largeur_numerisation_2 * wb_2.PixelHeight];
                    wb_2.CopyPixels(tab_pixel_2, largeur_numerisation_2, 0);
                    int[,] tab_pixel_int_LH_2 =
                        ConvertirTableauPixelEnLH_8bit(tab_pixel_2, wb_2.PixelWidth, wb_2.PixelHeight);
                    //comparaison pour mettre en evidence la surface de l'operation logique
                    int[,] tab_pixel_int_LH_oper = new int[wb_1.PixelHeight, wb_1.PixelWidth];
                    for (int lig = 0; lig < wb_1.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb_1.PixelWidth; col++)
                        {
                            int niveau_gris_int_1 = tab_pixel_int_LH_1[lig, col];
                            int niveau_gris_int_2 = tab_pixel_int_LH_2[lig, col];
                            int niveau_gris_int_oper = 255;
                            if (niveau_gris_int_1 != 255)
                            {
                                niveau_gris_int_oper = 127;
                            }
                            if (niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = 127;
                            }
                            if (niveau_gris_int_1 == 255 && niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = 0;
                            }
                            tab_pixel_int_LH_oper[lig, col] = niveau_gris_int_oper;
                        }
                    }
                    byte[] tab_pixel_oper =
                        ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_oper, wb_1.PixelWidth, wb_1.PixelHeight);
                    BitmapSource bti_oper_1 = BitmapSource.Create(wb_1.PixelWidth, wb_1.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_oper, largeur_numerisation_1);
                    x_img_operation_res.Width = bti_oper_1.PixelWidth;
                    x_img_operation_res.Height = bti_oper_1.PixelHeight;
                    x_img_operation_res.Source = bti_oper_1;
                    //comparaison pour mettre en evidence la surface de l'operation logique
                    int[,] tab_pixel_int_LH_inten = new int[wb_1.PixelHeight, wb_1.PixelWidth];
                    for (int lig = 0; lig < wb_1.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb_1.PixelWidth; col++)
                        {
                            int niveau_gris_int_1 = tab_pixel_int_LH_1[lig, col];
                            int niveau_gris_int_2 = tab_pixel_int_LH_2[lig, col];
                            int niveau_gris_int_oper = 255;
                            if (niveau_gris_int_1 != 255)
                            {
                                niveau_gris_int_oper = niveau_gris_int_1;
                            }
                            if (niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = niveau_gris_int_2;
                            }
                            if (niveau_gris_int_1 == 255 && niveau_gris_int_2 != 255)
                            {
                                niveau_gris_int_oper = (niveau_gris_int_1 + niveau_gris_int_2) / 2;
                            }
                            tab_pixel_int_LH_inten[lig, col] = niveau_gris_int_oper;
                        }
                    }
                    byte[] tab_pixel_inten =
                        ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_inten, wb_1.PixelWidth, wb_1.PixelHeight);
                    BitmapSource bti_inten_1 = BitmapSource.Create(wb_1.PixelWidth, wb_1.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_inten, largeur_numerisation_1);
                    x_img_operation_intensite.Width = bti_inten_1.PixelWidth;
                    x_img_operation_intensite.Height = bti_inten_1.PixelHeight;
                    x_img_operation_intensite.Source = bti_inten_1;
                }
            }
        }

        //vider les controles
        private void ViderControleImage(Uri uri, Image controle_image, int largeur, int hauteur)
        {
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            controle_image.Width = largeur;
            controle_image.Height = hauteur;
            controle_image.Source = bti;
        }

        //appliquer la transformation sur une image 8 bits
        private void AppliquerTransformationImage256gris(BitmapImage bti)
        {
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
            wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
            int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            int[,] tab_pixel_int_LH_modif = new int[wb.PixelHeight, wb.PixelWidth];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    int niveau_gris_int = tab_pixel_int_LH[lig, col];
                    //int niveau_gris_int_tranf = (int)FonctionLutNegative(niveau_gris_int);
                    //tab_pixel_int_LH_modif[lig, col] = niveau_gris_int_tranf;
                }
            }
            byte[] tab_pixel_modif =
                ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_modif, wb.PixelWidth, wb.PixelHeight);
            BitmapSource bti_modif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_modif, largeur_numerisation);
            //x_img_transform.Width = bti_modif.PixelWidth;
            //x_img_transform.Height = bti_modif.PixelHeight;
            //x_img_transform.Source = bti_modif;
        }

        //appliquer la transformation sur une image 32 bits
        private void AppliquerTransformationImageCouleur32bits(BitmapImage bti)
        {
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
            wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
            int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_32bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            int[,] tab_pixel_int_LH_modif = new int[wb.PixelHeight, wb.PixelWidth];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    int couleur_int = tab_pixel_int_LH[lig, col];
                    Color couleur = new Color();
                    couleur.A = (byte) (couleur_int >> 24);
                    couleur.R = (byte) (couleur_int >> 16);
                    couleur.G = (byte) (couleur_int >> 8);
                    couleur.B = (byte) (couleur_int);
                    Color couleur_tranf = new Color();
                    couleur_tranf.A = couleur.A;
                    //couleur_tranf.R = (byte)FonctionLutNegative(couleur.R);
                    //couleur_tranf.G = (byte)FonctionLutNegative(couleur.G);
                    //couleur_tranf.B = (byte)FonctionLutNegative(couleur.B);
                    int couleur_transf_int = couleur_tranf.A << 24 | couleur_tranf.R << 16 | couleur_tranf.G << 8 |
                                             couleur_tranf.B;
                    tab_pixel_int_LH_modif[lig, col] = couleur_transf_int;
                }
            }
            byte[] tab_pixel_modif =
                ConvertirTableauPixelEnUnique_32bit(tab_pixel_int_LH_modif, wb.PixelWidth, wb.PixelHeight);
            BitmapSource bti_modif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                PixelFormats.Bgra32, null, tab_pixel_modif, largeur_numerisation);
            //x_img_transform.Width = bti_modif.PixelWidth;
            //x_img_transform.Height = bti_modif.PixelHeight;
            //x_img_transform.Source = bti_modif;
        }

        //transposition tableau pixel dimension 1 vers 2 avec codage 32 bits
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

        //transposition tableau pixel dimension 2 vers 1 avec codage 32 bits
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

        ////transposition tableau pixel dimension 1 vers 2 avec codage 8 bits
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

        //transposition tableau pixel dimension 2 vers 1 avec codage 8 bits
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
    } //end class
}
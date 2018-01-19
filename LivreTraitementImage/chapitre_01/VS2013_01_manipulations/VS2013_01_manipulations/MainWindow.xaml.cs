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
using swf = System.Windows.Forms;
using System.Net;
using System.IO;
using System.Windows.Resources;
using System.IO.Compression;
using System.Reflection;

namespace VS2013_01_manipulations {
  /// <summary>
  /// Logique d'interaction pour MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    //donnees
    private string RC = Environment.NewLine;
    private bool v_fen_chargee = false;
    private string doss_exe = Environment.CurrentDirectory;
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
      v_fen_chargee = true;
      x_tbl_infos.Text = "";
      x_tbl_infos.Height = 1000;
      x_tbl_uri.Text = "";
    }
    //selection d'une action dans le combobox
    private void x_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      try {
        if (v_fen_chargee == true) {
          //aucune selection
          if (x_cbx_select.SelectedIndex == 0) {
            x_img.Width = 775;
            x_img.Height = 559;
            x_img.Source = new BitmapImage(new Uri("contenu/image/fond_damier.png", UriKind.Relative));
            //x_img.Source = new BitmapImage(new Uri("pack://application:,,,/contenu/image/fond_damier.png", UriKind.Absolute));
            x_tbl_infos.Text = "";
            x_tbl_uri.Text = "";
          }
          //charger une image embarquée en ressource incorporée dans le package
          if (x_cbx_select.SelectedIndex == 1) {
            x_tbl_infos.Text = "";
            Assembly assembly = Assembly.GetExecutingAssembly();
            //string[] noms = assembly.GetManifestResourceNames();
            //string infos = "";
            //for (int xx = 0; xx < noms.Length; xx++) {
            //  infos += noms[xx] + RC;
            //}
            //MessageBox.Show(infos);
            Stream fichier_image = assembly.GetManifestResourceStream("VS2013_01_manipulations.collection_images.louis_de_funes_bourvil.jpg");
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.StreamSource = fichier_image;
            bti.EndInit();
            x_img.Width = bti.PixelWidth;
            x_img.Height = bti.PixelHeight;
            x_img.Source = bti;
            AfficherInfos(bti);
            x_tbl_uri.Text = "VS2013_01_manipulations.collection_images.louis_de_funes_bourvil.jpg";
          }
          //charger une image embarquée en ressource dans une dll (ressource typée)
          if (x_cbx_select.SelectedIndex == 2) {
            x_tbl_infos.Text = "";
            //BitmapImage bti = new BitmapImage(new Uri("pack://application:,,,/VS2013_CollectionImages;component/images/004_louis_de_funes.jpg", UriKind.Absolute));
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = new Uri("pack://application:,,,/VS2013_CollectionImages;component/images/004_louis_de_funes.jpg", UriKind.Absolute);
            bti.EndInit();
            x_img.Width = bti.PixelWidth;
            x_img.Height = bti.PixelHeight;
            x_img.Source = bti;
            AfficherInfos(bti);
            x_tbl_uri.Text = "pack://application:,,,/VS2013_CollectionImages;component/images/004_louis_de_funes.jpg";
          }
          //charger une image embarquée en ressource incorporée dans une dll (ressource non typée)
          if (x_cbx_select.SelectedIndex == 3) {
            x_tbl_infos.Text = "";
            Assembly assembly = Assembly.Load("VS2013_CollectionImages");
            Stream fichier_image = assembly.GetManifestResourceStream("VS2013_CollectionImages.images.funes_2.jpg");
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.StreamSource = fichier_image;
            bti.EndInit();
            x_img.Width = bti.PixelWidth;
            x_img.Height = bti.PixelHeight;
            x_img.Source = bti;
            AfficherInfos(bti);
            x_tbl_uri.Text = "VS2013_CollectionImages.images.funes_2.jpg";
          }
          //charger une image stockée sur l'ordinateur
          if (x_cbx_select.SelectedIndex == 4) {
            x_tbl_infos.Text = "";
            x_img.Width = 775;
            x_img.Height = 559;
            x_img.Source = new BitmapImage(new Uri("contenu/image/fond_damier.png", UriKind.Relative));
            swf.OpenFileDialog boite_fichier = new swf.OpenFileDialog();
            boite_fichier.InitialDirectory = "c:\\";
            boite_fichier.Filter = "fichier PNG (*.png)|*.png|fichier JPEG (*.jpg)|*.jpg|fichier GIF (*.gif)|*.gif";
            boite_fichier.FilterIndex = 2;
            boite_fichier.RestoreDirectory = true;
            boite_fichier.Multiselect = false;
            boite_fichier.Title = "Rechercher un fichier graphique";
            swf.DialogResult res = boite_fichier.ShowDialog();
            if (res == swf.DialogResult.OK) {
              //MessageBox.Show(boite_fichier.FileName);
              BitmapImage bti = new BitmapImage();
              bti.BeginInit();
              bti.UriSource = new Uri(boite_fichier.FileName, UriKind.Absolute);
              bti.CacheOption = BitmapCacheOption.None;
              bti.EndInit();
              int largeur = bti.PixelWidth;
              int hauteur = bti.PixelHeight;
              x_img.Width = largeur;
              x_img.Height = hauteur;
              x_img.Stretch = Stretch.None;
              x_img.Source = bti;
              x_img.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
              x_img.VerticalAlignment = System.Windows.VerticalAlignment.Top;
              AfficherInfos(bti);
              x_tbl_uri.Text = boite_fichier.FileName;
            }
          }
          //charger une image à 300 dpi
          if (x_cbx_select.SelectedIndex == 5) {
            x_tbl_infos.Text = "";
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = new Uri("pack://application:,,,/collection_images/05_funes_300dpi.png", UriKind.Absolute);
            bti.EndInit();
            x_img.Width = bti.Width;
            x_img.Height = bti.Height;
            x_img.Source = bti;
            AfficherInfos(bti);
            x_tbl_uri.Text = "pack://application:,,,/collection_images/05_funes_300dpi.png";
          }
          //charger une image empaquetée dans un zip depuis un serveur web
          if (x_cbx_select.SelectedIndex == 6) {
            x_img.Width = 775;
            x_img.Height = 559;
            x_img.Source = new BitmapImage(new Uri("contenu/image/fond_damier.png", UriKind.Relative));
            x_tbl_infos.Text = "-> téléchargement demandé: http://www.reypatrice.fr/images_zippees.zip" + RC;
            WebClient wbc = new WebClient();
            wbc.DownloadProgressChanged += wbc_DownloadProgressChanged;
            wbc.DownloadFileCompleted += wbc_DownloadFileCompleted;
            wbc.DownloadFileAsync(new Uri("http://www.reypatrice.fr/images_zippees.zip"), doss_exe + "/images_zippees.zip");
          }
        }
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }
    private void wbc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
      x_tbl_infos.Text += "-> progression = " + e.ProgressPercentage.ToString() + " => ";
      x_tbl_infos.Text += e.BytesReceived.ToString() + " bytes reçus sur " + e.TotalBytesToReceive.ToString() + " bytes total" + RC;
    }
    private void wbc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
      try {
        x_tbl_infos.Text += "-> téléchargement terminé" + RC;
        if (Directory.Exists(doss_exe + "/images_zippees") == true) {
          Directory.Delete(doss_exe + "/images_zippees", true);
        }
        ZipFile.ExtractToDirectory(doss_exe + "/images_zippees.zip", doss_exe);
        BitmapImage bti = new BitmapImage();
        bti.BeginInit();
        bti.UriSource = new Uri(doss_exe + "/images_zippees/funes_3.jpg", UriKind.Absolute);
        bti.EndInit();
        x_img.Source = bti;
        x_img.Width = bti.Width;
        x_img.Height = bti.Height;
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }
    //
    private void AfficherInfos(BitmapImage bti) {
      int largeur = bti.PixelWidth;
      int hauteur = bti.PixelHeight;
      x_tbl_infos.Text += "LxH (PixelWidth et PixelHeight) : " + largeur.ToString() + " par " + hauteur.ToString() + " pixels" + RC;
      double dpix = bti.DpiX;
      double dpiy = bti.DpiY;
      x_tbl_infos.Text += "résolution: " + dpix.ToString() + " DPI suivant X et " + dpiy.ToString() + " DPI suivant Y" + RC;
      double ind_largeur = bti.Width;
      double ind_hauteur = bti.Height;
      x_tbl_infos.Text += "LxH (Width et Height) en unité indépendante du périphérique: " + ind_largeur.ToString() + " par " + ind_hauteur.ToString() + " pixels" + RC;
      PixelFormat format_natif = bti.Format;
      x_tbl_infos.Text += "nombre de bits par pixel: " + format_natif.BitsPerPixel.ToString() + RC;
    }


  }//end class
}

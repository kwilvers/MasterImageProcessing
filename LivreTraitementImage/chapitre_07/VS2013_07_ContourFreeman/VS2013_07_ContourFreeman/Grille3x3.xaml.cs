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

namespace VS2013_07_ContourFreeman {
  /// <summary>
  /// Logique d'interaction pour Grille3x3.xaml
  /// </summary>
  public partial class Grille3x3 : UserControl {
    public Grille3x3() {
      InitializeComponent();
      //positionnement du lignage
      int deplac_h = 0;
      int deplac_v = 0;
      for (int lig = 1; lig <= 4; lig++) {
        Line ligne = new Line();
        ligne.Width = 1;
        ligne.Height = 90;
        ligne.X1 = 0;
        ligne.Y1 = 0;
        ligne.X2 = 0;
        ligne.Y2 = 90;
        ligne.Stroke = new SolidColorBrush(Colors.Black);
        ligne.StrokeThickness = 1;
        ligne.StrokeDashArray = new DoubleCollection() { 2, 2 };
        Canvas.SetLeft(ligne, deplac_h);
        Canvas.SetTop(ligne, deplac_v);
        x_cnv_lignage.Children.Add(ligne);
        deplac_h += 30;
      }
      deplac_h = 0;
      deplac_v = 0;
      for (int col = 1; col <= 4; col++) {
        Line ligne = new Line();
        ligne.Width = 90;
        ligne.Height = 1;
        ligne.X1 = 0;
        ligne.Y1 = 0;
        ligne.X2 = 90;
        ligne.Y2 = 0;
        ligne.Stroke = new SolidColorBrush(Colors.Black);
        ligne.StrokeThickness = 1;
        ligne.StrokeDashArray = new DoubleCollection() { 2, 2 };
        Canvas.SetLeft(ligne, deplac_h);
        Canvas.SetTop(ligne, deplac_v);
        x_cnv_lignage.Children.Add(ligne);
        deplac_v += 30;
      }
    }
    //
    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      
    }
    //
    public void AfficherVoisinage(int lig, int col, int[,] tab_pixels_LH, string[,] tab_etiq_LH) {
      AfficherVoisins(0, 0, tab_pixels_LH[lig - 1, col - 1], tab_etiq_LH[lig - 1, col - 1]);
      AfficherVoisins(0, 1, tab_pixels_LH[lig - 1, col], tab_etiq_LH[lig - 1, col]);
      AfficherVoisins(0, 2, tab_pixels_LH[lig - 1, col + 1], tab_etiq_LH[lig - 1, col + 1]);
      AfficherVoisins(1, 0, tab_pixels_LH[lig, col - 1], tab_etiq_LH[lig, col - 1]);
      AfficherVoisins(1, 1, tab_pixels_LH[lig, col], tab_etiq_LH[lig, col]);
      AfficherVoisins(1, 2, tab_pixels_LH[lig, col + 1], tab_etiq_LH[lig, col + 1]);
      AfficherVoisins(2, 0, tab_pixels_LH[lig + 1, col - 1], tab_etiq_LH[lig + 1, col - 1]);
      AfficherVoisins(2, 1, tab_pixels_LH[lig + 1, col], tab_etiq_LH[lig + 1, col]);
      AfficherVoisins(2, 2, tab_pixels_LH[lig + 1, col + 1], tab_etiq_LH[lig + 1, col + 1]);
    }
    //
    private void AfficherVoisins(int lig, int col, int niv_pixel, string etiquette) {
      Rectangle rect = new Rectangle();
      rect.Width = 30;
      rect.Height = 30;
      rect.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)niv_pixel, (byte)niv_pixel, (byte)niv_pixel));
      rect.Stroke = new SolidColorBrush(Colors.Transparent);
      rect.StrokeThickness = 0;
      Canvas.SetLeft(rect, 30 * col);
      Canvas.SetTop(rect, 30 * lig);
      x_cnv_grille.Children.Add(rect);
      if (etiquette != "-") {
        TextBlock tb = new TextBlock();
        tb.Text = etiquette;
        tb.Foreground = new SolidColorBrush(Colors.White);
        tb.FontFamily = new FontFamily("Verdana");
        tb.FontSize = 14;
        tb.TextAlignment = TextAlignment.Center;
        tb.Width = 30;
        tb.Height = 23;
        Canvas.SetLeft(tb, 30 * col);
        Canvas.SetTop(tb, 30 * lig + 7);
        x_cnv_grille.Children.Add(tb);
      }
    }
    //vider la grille
    public void ViderLaGrille() {
      x_cnv_grille.Children.Clear();
    }
  }//end class
}
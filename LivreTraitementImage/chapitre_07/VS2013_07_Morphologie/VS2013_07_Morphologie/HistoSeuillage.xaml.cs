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

namespace VS2013_07_Morphologie {
  /// <summary>
  /// Logique d'interaction pour HistoSeuillage.xaml
  /// </summary>
  public partial class HistoSeuillage : UserControl {
    //constructeur
    public HistoSeuillage() {
      InitializeComponent();
    }
    //usercontrol evenement Loaded
    private void UserControl_Loaded(object sender, RoutedEventArgs e) {

    }
    //afficher l'histogramme de l'image initiale a partir
    //d'un tableau contenant juste la valeur de gris (0-255)
    public void VisualiserHistoGrisImgInitiale(byte[] tab_pixels_gris, int nb_largeur, int nb_hauteur) {
      //repartition des niveaux de gris de 0 à 255
      byte[] tab_repartition = new byte[256];
      for (int lig = 0; lig <= 255; lig++) {
        tab_repartition[lig] = 0;
      }
      for (int xx = 0; xx < nb_largeur * nb_hauteur; xx++) {
        byte niv_gris = tab_pixels_gris[xx];
        tab_repartition[niv_gris] += 1;
      }
      //calcul des probabilités de la répartition
      double[] tab_proba = new double[256];
      double proba_maxi = 0;
      for (int xx = 0; xx < tab_repartition.Length; xx++) {
        tab_proba[xx] = (double)tab_repartition[xx] / (double)(nb_largeur * nb_hauteur);
        if (tab_proba[xx] > proba_maxi) {
          proba_maxi = tab_proba[xx];
        }
      }
      //ajouter la courbe des repartitions
      x_cnv_courbe.Children.Clear();
      Polyline courbe = new Polyline();
      courbe.Stroke = new SolidColorBrush(Colors.Black);
      courbe.StrokeThickness = 2;
      courbe.Fill = new SolidColorBrush(Colors.Transparent);
      courbe.StrokeLineJoin = PenLineJoin.Round;
      PointCollection collect = new PointCollection();
      double hauteur_cnv = x_cnv_courbe.ActualHeight;
      double decalage_x = 0;
      for (int lig = 0; lig <= 255; lig++) {
        double pos_x = 0 + decalage_x;
        double pos_y = tab_proba[lig] * hauteur_cnv / (proba_maxi * 1.5);
        Point point_calcul = new Point(pos_x, pos_y);
        decalage_x += 2;
        collect.Add(point_calcul);
      }
      courbe.Points = collect;
      Canvas.SetLeft(courbe, 0);
      Canvas.SetTop(courbe, 0);
      x_cnv_courbe.Children.Add(courbe);
      //ajouter le fond de la courbe des repartitions
      Polyline courbe_fond = new Polyline();
      courbe_fond.Stroke = new SolidColorBrush(Colors.Transparent);
      courbe_fond.StrokeThickness = 0;
      courbe_fond.Fill = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
      courbe_fond.StrokeLineJoin = PenLineJoin.Round;
      PointCollection collect_fond = new PointCollection();
      collect_fond.Add(new Point(0, 0));
      decalage_x = 0;
      for (int lig = 0; lig <= 255; lig++) {
        double pos_x = 0 + decalage_x;
        double pos_y = tab_proba[lig] * hauteur_cnv / (proba_maxi * 1.5);
        Point point_calcul = new Point(pos_x, pos_y);
        decalage_x += 2;
        collect_fond.Add(point_calcul);
      }
      collect_fond.Add(new Point(512, 0));
      courbe_fond.Points = collect_fond;
      Canvas.SetLeft(courbe_fond, 0);
      Canvas.SetTop(courbe_fond, 0);
      x_cnv_courbe.Children.Add(courbe_fond);
    }
    //modifier position de la ligne verticale indicatrice de seuillage
    public void PositionnerLigneVertiSeuillage(int seuil) {
      Canvas.SetLeft(x_line_indic, 19 + seuil * 2);
      x_text_indic.Text = seuil.ToString("000");
      Canvas.SetLeft(x_text_indic, 7 + seuil * 2);
    }
    //modifier position de la ligne verticale indicatrice de seuillage
    public void PositionnerLigne2VertiSeuillage(int seuil) {
      x_line_indic2.Visibility = Visibility.Visible;
      x_text_indic2.Visibility = Visibility.Visible;
      Canvas.SetLeft(x_line_indic2, 19 + seuil * 2);
      x_text_indic2.Text = seuil.ToString("000");
      Canvas.SetLeft(x_text_indic2, 7 + seuil * 2);
    }
  }//end class
}

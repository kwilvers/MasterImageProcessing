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

namespace VS2013_03_HistoCumule {
  /// <summary>
  /// Logique d'interaction pour HistoCumule.xaml
  /// </summary>
  public partial class HistoCumule : UserControl {
   //enumeration
    public enum TypeComposante { niveau_de_gris, composante_R, composante_G, composante_B };
    //champs
    private int v_repart_maxi = 0;
    //proprietes
    public int[] RepartitionPixel { set; private get; }
    public string Titre { set; private get; }
    public TypeComposante Composante { set; private get; }
    //constructeur
    public HistoCumule() {
      InitializeComponent();
    }
    //usercontrol evenement Loaded
    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      this.UpdateLayout();
      x_text_titre.Text = this.Titre;
      InscrireEtiquetteNbPixel();
      AjouterLeFondDeLaCourbe();
      AjouterLaCourbe();
      EtalonnageBarreCouleur();
    }
    //
    private void InscrireEtiquetteNbPixel() {
      v_repart_maxi = this.RepartitionPixel[255];
      x_text_nb_pix_max.Text = v_repart_maxi.ToString("### ### ###");
      x_text_nb_pix_1tiers.Text = (v_repart_maxi / 3).ToString("### ### ###");
      x_text_nb_pix_2tiers.Text = (2 * v_repart_maxi / 3).ToString("### ### ###");
    }
    //
    private void AjouterLaCourbe() {
      Polyline courbe = new Polyline();
      courbe.Stroke = new SolidColorBrush(Colors.Black);
      courbe.StrokeThickness = 3;
      courbe.Fill = new SolidColorBrush(Colors.Transparent);
      courbe.StrokeLineJoin = PenLineJoin.Round;
      PointCollection collect = new PointCollection();
      double hauteur_cnv = x_cnv_courbe.ActualHeight;
      double decalage_x = 0;
      for (int lig = 0; lig <= 255; lig++) {
        double pos_x = 0 + decalage_x;
        double pos_y = (this.RepartitionPixel[lig] * hauteur_cnv) / v_repart_maxi;
        Point point_calcul = new Point(pos_x, pos_y);
        decalage_x += 2;
        collect.Add(point_calcul);
      }
      courbe.Points = collect;
      Canvas.SetLeft(courbe, 0);
      Canvas.SetTop(courbe, 0);
      x_cnv_courbe.Children.Add(courbe);
    }
    //
    private void AjouterLeFondDeLaCourbe() {
      Polyline courbe_fond = new Polyline();
      courbe_fond.Stroke = new SolidColorBrush(Colors.Black);
      courbe_fond.StrokeThickness = 0;
      switch (this.Composante) {
        case TypeComposante.niveau_de_gris:
          courbe_fond.Fill = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));//gainsboro
          break;
        case TypeComposante.composante_R:
          courbe_fond.Fill = new SolidColorBrush(Color.FromArgb(49,220,20,60));//rouge clair
          break;
        case TypeComposante.composante_G:
          courbe_fond.Fill = new SolidColorBrush(Color.FromArgb(49,0,128,0));//vert clair
          break;
        case TypeComposante.composante_B:
          courbe_fond.Fill = new SolidColorBrush(Color.FromArgb(41,7,152,255));//bleu clair
          break;
      }
      courbe_fond.StrokeLineJoin = PenLineJoin.Round;
      PointCollection collect = new PointCollection();
      collect.Add(new Point(0, 0));
      double hauteur_cnv = x_cnv_courbe.ActualHeight;
      double decalage_x = 0;
      for (int lig = 0; lig <= 255; lig++) {
        double pos_x = 0 + decalage_x;
        double pos_y = (this.RepartitionPixel[lig] * hauteur_cnv) / v_repart_maxi;
        Point point_calcul = new Point(pos_x, pos_y);
        decalage_x += 2;
        collect.Add(point_calcul);
      }
      collect.Add(new Point(512, 0));
      courbe_fond.Points = collect;
      Canvas.SetLeft(courbe_fond, 0);
      Canvas.SetTop(courbe_fond, 0);
      x_cnv_courbe.Children.Add(courbe_fond);
    }
    //
    private void EtalonnageBarreCouleur() {
      LinearGradientBrush degrade = new LinearGradientBrush();
      degrade.StartPoint = new Point(0, 0.5);
      degrade.EndPoint = new Point(1, 0.5);
      GradientStop couleur_debut = new GradientStop();
      couleur_debut.Color = Colors.Black;
      couleur_debut.Offset = 0;
      degrade.GradientStops.Add(couleur_debut);
      GradientStop couleur_fin = new GradientStop();
      couleur_fin.Offset = 1;
      if (this.Composante == TypeComposante.niveau_de_gris) {
        couleur_fin.Color = Colors.White;
      }
      if (this.Composante == TypeComposante.composante_R) {
        couleur_fin.Color = Color.FromArgb(255, 255, 0, 0);
      }
      if (this.Composante == TypeComposante.composante_G) {
        couleur_fin.Color = Color.FromArgb(255, 0, 255, 0);
      }
      if (this.Composante == TypeComposante.composante_B) {
        couleur_fin.Color = Color.FromArgb(255, 0, 0, 255);
      }
      degrade.GradientStops.Add(couleur_fin);
      x_rect_etalonnage.Fill = degrade;
    }
  }//end class
}


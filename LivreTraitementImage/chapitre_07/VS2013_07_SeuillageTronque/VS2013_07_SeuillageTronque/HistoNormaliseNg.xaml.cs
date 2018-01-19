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

namespace VS2013_07_SeuillageTronque {
  /// <summary>
  /// Logique d'interaction pour HistoNormaliseNg.xaml
  /// </summary>
  public partial class HistoNormaliseNg : UserControl {
    //champs
    private double v_proba_maxi = 0d;
    private string RC = Environment.NewLine;
    private int v_gris_mini_pres = 255;
    private int v_gris_maxi_pres = 0;
    //proprietes
    public string Titre { set; private get; }
    public byte[,] PixelImage_LH { set; private get; }
    public int PixelLargeur { set; private get; }
    public int PixelHauteur { set; private get; }
    public bool AfficherCourbeCumul { set; private get; }
    //constructeur
    public HistoNormaliseNg() {
      InitializeComponent();
    }
    //usercontrol evenement Loaded
    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      this.UpdateLayout();
      x_text_titre.Text = this.Titre;
      //repartition des niveaux de gris de 0 à 255
      int[] tab_repartition = new int[256];
      for (int lig = 0; lig <= 255; lig++) {
        tab_repartition[lig] = 0;
      }
      for (int lig = 0; lig < this.PixelHauteur; lig++) {
        for (int col = 0; col < this.PixelLargeur; col++) {
          byte niveau_gris = this.PixelImage_LH[lig, col];
          tab_repartition[niveau_gris] += 1;
        }
      }
      //calcul des probabilités de la répartition
      double[] tab_proba = new double[256];
      for (int xx = 0; xx < tab_repartition.Length; xx++) {
        tab_proba[xx] = (double)tab_repartition[xx] / (double)(this.PixelLargeur * this.PixelHauteur);
        if (tab_repartition[xx] != 0 && xx < v_gris_mini_pres) {
          v_gris_mini_pres = xx;
        }
        if (tab_repartition[xx] != 0 && xx > v_gris_maxi_pres) {
          v_gris_maxi_pres = xx;
        }
      }
      //affichage des etiquettes sur axe des ordonnées
      for (int xx = 0; xx <= 255; xx++) {
        if (tab_proba[xx] > v_proba_maxi) {
          v_proba_maxi = tab_proba[xx];
        }
      }
      x_text_prob_pix_max.Text = v_proba_maxi.ToString("0.000");
      x_text_prob_pix_1tiers.Text = (v_proba_maxi / 3).ToString("0.000");
      x_text_prob_pix_2tiers.Text = (2 * v_proba_maxi / 3).ToString("0.000");
      //ajouter les barres de l'histogramme
      double hauteur_stack = x_stack_barre.ActualHeight;
      for (int xx = 0; xx <= 255; xx++) {
        Rectangle barre = new Rectangle();
        barre.Width = 2;
        barre.Height = (tab_proba[xx] * hauteur_stack) / v_proba_maxi;
        barre.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        barre.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
        barre.StrokeThickness = 0;
        barre.Fill = new SolidColorBrush(Colors.Black);
        x_stack_barre.Children.Add(barre);
      }
      //
      EtalonnageBarreCouleur();
      //generer la courbe des cumuls
      double[] tab_proba_cumul = new double[256];
      double cumul_proba = 0d;
      for (int xx = 0; xx < tab_proba.Length; xx++) {
        cumul_proba += tab_proba[xx];
        tab_proba_cumul[xx] = cumul_proba;
      }
      if (this.AfficherCourbeCumul == true) {
        Polyline courbe = new Polyline();
        courbe.Stroke = new SolidColorBrush(Colors.Red);
        courbe.StrokeThickness = 3;
        courbe.Fill = new SolidColorBrush(Colors.Transparent);
        courbe.StrokeLineJoin = PenLineJoin.Round;
        PointCollection collect = new PointCollection();
        double hauteur_cnv = x_cnv_courbe.ActualHeight;
        double decalage_x = 0;
        for (int lig = 0; lig <= 255; lig++) {
          double pos_x = 0 + decalage_x;
          double pos_y = tab_proba_cumul[lig] * hauteur_cnv;
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
      string infos = "";
      infos += (this.PixelLargeur * this.PixelHauteur).ToString() + " pixels" + RC;
      infos += "largeur = " + this.PixelLargeur.ToString() + " px" + RC;
      infos += "hauteur = " + this.PixelHauteur.ToString() + " px" + RC;
      infos += "plage de gris:" + RC;
      infos += v_gris_mini_pres.ToString("000") + " à " + v_gris_maxi_pres.ToString("000") + RC;
      x_text_infos.Text = infos;
      double pos_x_etendue = 55 + v_gris_mini_pres * 2;
      double larg_etendue = (v_gris_maxi_pres - v_gris_mini_pres) * 2;
      x_rect_etendue.Width = larg_etendue;
      Canvas.SetLeft(x_rect_etendue, pos_x_etendue);
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
      couleur_fin.Color = Colors.White;
      degrade.GradientStops.Add(couleur_fin);
      x_rect_etalonnage.Fill = degrade;
    }
  }//end class
}



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
  /// Logique d'interaction pour Grille12x12.xaml
  /// </summary>
  public partial class Grille12x12 : UserControl {
    //donnees
    private List<Rectangle> v_liste_rectangle = null;
    private List<TextBlock> v_liste_texte = null;
    //constructeur
    public Grille12x12() {
      InitializeComponent();
      v_liste_rectangle = new List<Rectangle>();
      //positionnement des rectangles
      double deplac_h = 0;
      double deplac_v = 0;
      for (int lig = 0; lig < 12; lig++) {
        for (int col = 0; col < 12; col++) {
          Rectangle rect = new Rectangle();
          rect.Name = "x_rect_" + lig.ToString("00") + "_" + col.ToString("00");//x_rect_00_00
          rect.Width = 30;
          rect.Height = 30;
          rect.Fill = new SolidColorBrush(Colors.White);
          rect.Stroke = new SolidColorBrush(Colors.Transparent);
          rect.StrokeThickness = 0;
          Canvas.SetLeft(rect, deplac_h);
          Canvas.SetTop(rect, deplac_v);
          x_cnv_grille.Children.Add(rect);
          v_liste_rectangle.Add(rect);
          deplac_h += 30;
        }
        deplac_h = 0;
        deplac_v += 30;
      }
      //positionnement du lignage
      deplac_h = 0;
      deplac_v = 0;
      for (int lig = 1; lig <= 13; lig++) {
        Line ligne = new Line();
        ligne.Width = 1;
        ligne.Height = 360;
        ligne.X1 = 0;
        ligne.Y1 = 0;
        ligne.X2 = 0;
        ligne.Y2 = 360;
        ligne.Stroke = new SolidColorBrush(Colors.Black);
        ligne.StrokeThickness = 1;
        ligne.StrokeDashArray = new DoubleCollection() { 2, 2 };
        Canvas.SetLeft(ligne, deplac_h);
        Canvas.SetTop(ligne, deplac_v);
        x_cnv_grille.Children.Add(ligne);
        deplac_h += 30;
      }
      deplac_h = 0;
      deplac_v = 0;
      for (int col = 1; col <= 13; col++) {
        Line ligne = new Line();
        ligne.Width = 360;
        ligne.Height = 1;
        ligne.X1 = 0;
        ligne.Y1 = 0;
        ligne.X2 = 360;
        ligne.Y2 = 0;
        ligne.Stroke = new SolidColorBrush(Colors.Black);
        ligne.StrokeThickness = 1;
        ligne.StrokeDashArray = new DoubleCollection() { 2, 2 };
        Canvas.SetLeft(ligne, deplac_h);
        Canvas.SetTop(ligne, deplac_v);
        x_cnv_grille.Children.Add(ligne);
        deplac_v += 30;
      }
      v_liste_texte = new List<TextBlock>();
    }
    //usercontrol evenement Loaded
    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      
    }
    //fixer le motif a partir d'un tableau de pixel codés en int
    public void FixerLeMotif(int[,] tab_pixels_essai_LH) {
      for (int lig = 0; lig < 12; lig++) {
        for (int col = 0; col < 12; col++) {
          string nom = "x_rect_" + lig.ToString("00") + "_" + col.ToString("00");
          Color couleur = Color.FromArgb(255, (byte)tab_pixels_essai_LH[lig, col],
            (byte)tab_pixels_essai_LH[lig, col], (byte)tab_pixels_essai_LH[lig, col]);
          for (int xx = 0; xx < v_liste_rectangle.Count; xx++) {
            Rectangle rect = v_liste_rectangle[xx];
            if (rect.Name == nom) {
              rect.Fill = new SolidColorBrush(couleur);
            }
          }
        }
      }
    }
    //convertir un int en objet color
    private Color CorrespondanceIntVersColor(int couleur_int) {
      Color couleur = new Color();
      couleur.A = (byte)(couleur_int >> 24);
      couleur.R = (byte)(couleur_int >> 16);
      couleur.G = (byte)(couleur_int >> 8);
      couleur.B = (byte)(couleur_int);
      return couleur;
    }
    //convertir un color en int
    private int CorrespondanceColorVersInt(Color couleur) {
      int couleur_int = 0;
      couleur_int = (couleur.A << 24) | (couleur.R << 16) | (couleur.G << 8) | couleur.B;
      return couleur_int;
    }
    //
    public void AfficherEtiquette(string etiquette, int lig, int col) {
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
      tb.Name = "x_etiq_" + etiquette;
      x_cnv_grille.Children.Add(tb);
      v_liste_texte.Add(tb);
    }
    //vider pour remise a zero
    public void ViderPourRemiseAZero() {
      for (int xx = 0; xx < v_liste_rectangle.Count; xx++) {
        v_liste_rectangle[xx].Fill = new SolidColorBrush(Colors.White);
      }
      for (int xx = 0; xx < v_liste_texte.Count; xx++) {
        string nom = v_liste_texte[xx].Name;
        TextBlock tb = (TextBlock)x_cnv_grille.FindName(nom);
        x_cnv_grille.Children.Remove(tb);
      }
      v_liste_texte.Clear();
    }
  }//end class
}


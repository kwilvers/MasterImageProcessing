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

namespace VS2013_01_CodageDecodage
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
            RemplirRectangleCouleur();
            CodageDecodage();
        }

        //remplir le rectangle avec la couleur en fonction des glissieres
        private void RemplirRectangleCouleur()
        {
            Color couleur = new Color();
            couleur.R = (byte) x_slider_r.Value;
            couleur.G = (byte) x_slider_g.Value;
            couleur.B = (byte) x_slider_b.Value;
            couleur.A = (byte) x_slider_a.Value;
            SolidColorBrush pinceau = new SolidColorBrush(couleur);
            x_rect_couleur.Fill = pinceau;
        }

        //quand on modifie la valeur d'une glissiere
        private void x_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (v_fen_chargee == true)
            {
                RemplirRectangleCouleur();
            }
        }

        //bouton pour lancer le codage et decodage de la couleur
        private void x_btn_codage_Click(object sender, RoutedEventArgs e)
        {
            CodageDecodage();
        }

        //
        private void CodageDecodage()
        {
            Color couleur = new Color();
            couleur.R = (byte) x_slider_r.Value;
            couleur.G = (byte) x_slider_g.Value;
            couleur.B = (byte) x_slider_b.Value;
            couleur.A = (byte) x_slider_a.Value;
            x_tbl_byte_a.Text = "byte = " + couleur.A.ToString();
            x_tbl_binaire_a.Text = RepresentationBinaireByte(couleur.A);
            x_tbl_byte_r.Text = "byte = " + couleur.R.ToString();
            x_tbl_binaire_r.Text = RepresentationBinaireByte(couleur.R);
            x_tbl_byte_g.Text = "byte = " + couleur.G.ToString();
            x_tbl_binaire_g.Text = RepresentationBinaireByte(couleur.G);
            x_tbl_byte_b.Text = "byte = " + couleur.B.ToString();
            x_tbl_binaire_b.Text = RepresentationBinaireByte(couleur.B);
            int couleur_int = 0;
            couleur_int = couleur.A << 24 | couleur.R << 16 | couleur.G << 8 | couleur.B << 0;
            x_tbl_int.Text = couleur_int.ToString();
            x_tbl_binaire_int.Text = RepresentationBinaireInt(couleur_int);
            byte couleur_decode_a = (byte) (couleur_int >> 24);
            x_tbl_byte_a_decode.Text = "byte = " + couleur_decode_a.ToString();
            byte couleur_decode_r = (byte) (couleur_int >> 16);
            x_tbl_byte_r_decode.Text = "byte = " + couleur_decode_r.ToString();
            byte couleur_decode_g = (byte) (couleur_int >> 8);
            x_tbl_byte_g_decode.Text = "byte = " + couleur_decode_g.ToString();
            byte couleur_decode_b = (byte) (couleur_int >> 0);
            x_tbl_byte_b_decode.Text = "byte = " + couleur_decode_b.ToString();
            Color couleur_decode =
                Color.FromArgb(couleur_decode_a, couleur_decode_r, couleur_decode_g, couleur_decode_b);
            x_rect_couleur_decode.Fill = new SolidColorBrush(couleur_decode);
        }

        //representation binaire d'un byte
        private string RepresentationBinaireByte(byte valeur_byte)
        {
            char[] b = new char[8];
            int pos = 7;
            int i = 0;
            while (i < 8)
            {
                if ((valeur_byte & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }

        //represntation binaire d'un int
        private string RepresentationBinaireInt(int valeur_int)
        {
            char[] b = new char[32];
            int pos = 31;
            int i = 0;
            while (i < 32)
            {
                if ((valeur_int & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }
    } //end class
}
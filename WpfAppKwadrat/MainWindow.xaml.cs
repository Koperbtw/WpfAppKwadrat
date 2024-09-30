using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfAppKwadrat
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        double trapez = 0;
        double rotationAngle = 0;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double bok;
            if (double.TryParse(txtBok.Text, out bok) && bok >= 0)
            {
                if (!string.IsNullOrEmpty(txtBok.Text))
                {
                    lblKomunikat.Content = "String.Empty;";
                }
                obliczPO(bok);
                lblKomunikat.Content = String.Empty;
            }
            else
            {
                lblKomunikat.Content = "Wpisz liczbę dodatnią";
            }
        }

        private void obliczPO(double bok)
        {
            double pole = 0;
            double obwod = 0;

            if (trapez == 0 || trapez == bok || trapez == bok * 2)
            {
                pole = Math.Pow(bok, 2.0);
                obwod = 4 * bok;
            }
            else if (trapez > 0 && trapez < bok * 2)
            {
                double gora = bok - 2 * Math.Abs(trapez - bok);
                double dol = bok;
                double wysokosc = bok;

                pole = 0.5 * (gora + dol) * wysokosc;

                double ramiono = Math.Sqrt(Math.Pow((dol - gora) / 2, 2) + Math.Pow(wysokosc, 2)); // h^2 + ((dol-gora)/2)^2
                obwod = gora + dol + 2 * ramiono;
            }

            txtPole.Text = pole.ToString();
            txtObwod.Text = obwod.ToString();
        }

        private void paintFunction(Canvas canvas, double bok)
        {
            canvas.Children.Clear();
            SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFromString(cmbKolory.Text);

            Polygon polygon = new Polygon
            {
                Fill = color,
                Opacity = (cbPrzezroczysty.IsChecked.Value) ? 0.5 : 1,
                RenderTransform = new RotateTransform(rotationAngle, bok / 2, bok / 2),
            };

            if (trapez <= bok * 0.5)
            {
                polygon.Points = new PointCollection
                {
                    new System.Windows.Point(0 + trapez, 0), // Lewy róg
                    new System.Windows.Point(0 + bok - trapez, 0), // Prawy róg
                    new System.Windows.Point(0 + bok, 0 + bok), // Prawy dolny róg
                    new System.Windows.Point(0, 0 + bok) // Lewy dolny róg
                };
            }
            if (trapez > bok * 0.5 && trapez < bok)
            {
                polygon.Points = new PointCollection
                {
                    new System.Windows.Point(0 + bok*0.5 - (trapez-bok*0.5), 0), // Lewy róg
                    new System.Windows.Point(0 + bok - bok*0.5 + (trapez-bok*0.5), 0), // Prawy róg
                    new System.Windows.Point(0 + bok, 0 + bok), // Prawy dolny róg
                    new System.Windows.Point(0, 0 + bok) // Lewy dolny róg
                };
            }
            if (trapez >= bok && trapez <= bok*1.5)
            {
                polygon.Points = new PointCollection
                {
                    new System.Windows.Point(0, 0), // Lewy róg
                    new System.Windows.Point(0 + bok, 0), // Prawy róg
                    new System.Windows.Point(0 + bok - (trapez-bok), 0 + bok), // Prawy dolny róg
                    new System.Windows.Point(0 + (trapez-bok), 0 + bok) // Lewy dolny róg
                };
            }
            if (trapez > bok*1.5)
            {
                polygon.Points = new PointCollection
                {
                    new System.Windows.Point(0, 0), // Lewy róg
                    new System.Windows.Point(0 + bok, 0), // Prawy róg
                    new System.Windows.Point(0 + (trapez-bok), 0 + bok), // Prawy dolny róg
                    new System.Windows.Point(0 + bok - (trapez-bok), 0 + bok) // Lewy dolny róg
                };
            }
            if (trapez == 0)
            {
                polygon.Points = new PointCollection
                {
                    new System.Windows.Point(0, 0), // Lewy róg
                    new System.Windows.Point(0 + bok, 0), // Prawy róg
                    new System.Windows.Point(0 + bok, 0 + bok), // Prawy dolny róg
                    new System.Windows.Point(0, 0 + bok) // Lewy dolny róg
                };
            }
            canvas.Children.Add(polygon);
            obliczPO(bok);
        }

        private void btnRysuj_Click(object sender, RoutedEventArgs e)
        {
            double bok;
            if (double.TryParse(txtBok.Text, out bok) && bok <= 380)
            {
                paintFunction(canvas, bok);
            }
            else
            {
                lblKomunikat.Content = "Brak danych lub zbyt duży bok";
            }
        }

        private void radioUkryj_Checked(object sender, RoutedEventArgs e)
        {
            canvas.Opacity = 0;
        }

        private void cbPrzezroczysty_Checked(object sender, RoutedEventArgs e)
        {
            if (cbPrzezroczysty != null && cbPrzezroczysty.IsChecked.HasValue)
            {
                canvas.Opacity = cbPrzezroczysty.IsChecked.Value ? 0.5 : 1;
            }
        }
        private void radioPokaz_Checked(object sender, RoutedEventArgs e)
        {
            if (cbPrzezroczysty != null && cbPrzezroczysty.IsChecked.HasValue)
            {
                canvas.Opacity = cbPrzezroczysty.IsChecked.Value ? 0.5 : 1;
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            double bok;
            double.TryParse(txtBok.Text, out bok);
            if (double.TryParse(txtBok.Text, out bok) && bok <= 379)
            {
                bok = bok + 1;
                txtBok.Text = bok.ToString();
                this.btnRysuj.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else
            {
                lblKomunikat.Content = "Brak danych lub zbyt duży bok";
            }
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            double bok;
            double.TryParse(txtBok.Text, out bok);
            if (double.TryParse(txtBok.Text, out bok) && bok >= 1)
            {
                bok = bok - 1;
                txtBok.Text = bok.ToString();
                this.btnRysuj.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else
            {
                lblKomunikat.Content = "Brak danych lub zbyt duży bok";
            }
        }
        private void btnWyczysc_Click(object sender, RoutedEventArgs e)
        {
            txtBok.Text = String.Empty;
            txtPole.Text = String.Empty;
            txtObwod.Text = String.Empty;
            trapez = 0;
            lblKomunikat.Content = "Wpisz wymiar boku";
        }

        private void TrianglePlus_Click(object sender, RoutedEventArgs e)
        {
            double bok;
            double.TryParse(txtBok.Text, out bok);
            if (trapez < bok*2)
            {
                trapez++;
            } 
            else
            {
                trapez = 1;
            }
            paintFunction(canvas, bok);
        }

        private void btnRotate_Click(object sender, RoutedEventArgs e)
        {
            rotationAngle = (rotationAngle + 5) % 360;
            double bok;
            if (double.TryParse(txtBok.Text, out bok))
            {
                paintFunction(canvas, bok);
            }
        }
    }
}
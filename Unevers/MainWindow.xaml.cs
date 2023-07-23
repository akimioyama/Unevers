using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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

namespace Unevers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int? summa = 0;
        public MainWindow()
        {
            InitializeComponent();

            tb_expenditure.TextChanged += TextBox_TextChanged;
            tb_thermalCond.TextChanged += TextBox_TextChanged;
            tb_density.TextChanged += TextBox_TextChanged;
            tb_compressionDensity.TextChanged += TextBox_TextChanged;
            tb_tensileStrength.TextChanged += TextBox_TextChanged;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double sum = 0;
            bool hasError = false;

            if (double.TryParse(tb_expenditure.Text, out double expenditure))
                sum += expenditure;
            else
                hasError = true;

            if (double.TryParse(tb_thermalCond.Text, out double thermalCond))
                sum += thermalCond;
            else
                hasError = true;

            if (double.TryParse(tb_density.Text, out double density))
                sum += density;
            else
                hasError = true;

            if (double.TryParse(tb_compressionDensity.Text, out double compressionDensity))
                sum += compressionDensity;
            else
                hasError = true;

            if (double.TryParse(tb_tensileStrength.Text, out double tensileStrength))
                sum += tensileStrength;
            else
                hasError = true;

            if (sum > 100)
            {
                tb_summ.Content = "> 100 !";
                hasError = true;
            }
            else if (hasError || sum < 100)
            {
                tb_summ.Content = "Ошибка";
            }
            else
            {
                tb_summ.Content = sum.ToString();

                delta1_Change();
                delta2_Change();

                x1_x2_Change();
                x3_x2_Change();

                var opredelet = Opredeletel();
                double[] q = new double[3] { 0, 0, 1 };
                ModifyMatrixColumn(q, 0, opredelet);
                ModifyMatrixColumn(q, 1, opredelet);
                ModifyMatrixColumn(q, 2, opredelet);

                Procentetd();
            }
        }

        private void delta1_Change()
        {
            var rashod = double.Parse(tb_expenditure.Text.Trim());
            var teploprovod = double.Parse(tb_thermalCond.Text.Trim());
            var plotnosty = double.Parse(tb_density.Text.Trim());
            var prohnosty = double.Parse(tb_compressionDensity.Text.Trim());
            var prochostyRasstoynie = double.Parse(tb_tensileStrength.Text.Trim());

            var result = (prohnosty - prochostyRasstoynie - plotnosty - rashod - teploprovod) / 100;

            delta1.Text = result.ToString();
        }
        private void delta2_Change()
        {
            var rashod = double.Parse(tb_expenditure.Text.Trim());
            var teploprovod = double.Parse(tb_thermalCond.Text.Trim());
            var plotnosty = double.Parse(tb_density.Text.Trim());
            var prohnosty = double.Parse(tb_compressionDensity.Text.Trim());
            var prochostyRasstoynie = double.Parse(tb_tensileStrength.Text.Trim());

            var result = (rashod + teploprovod + plotnosty - prohnosty - prochostyRasstoynie) / 100;

            delta2.Text = result.ToString();
        }
        private void x1_x2_Change()
        {
            var delta = double.Parse(delta1.Text.Trim());

            var result = (delta + 8.09) / 9.09;
            x1_x2.Text = result.ToString();
            x1_x2_2.Content = Math.Round(result, 2).ToString();
        }
        private void x3_x2_Change()
        {
            var delta = double.Parse(delta2.Text.Trim());

            var result = (delta + 3) / 9.09;
            x3_x2.Text = result.ToString();
            x3_x2_2.Content = Math.Round(result, 2).ToString();
        }
        private double Opredeletel()
        {
            double x = double.Parse(x1_x2.Text.Trim());
            double y = double.Parse(x3_x2.Text.Trim()); 
            double[,] matrix = new double[3, 3]
            {
                {-1, x, 0},
                {0, y, -1},
                {1, 1, 1}
            };

            double det = (matrix[0, 0] * matrix[1, 1] * matrix[2, 2]) + (matrix[0, 1] * matrix[1, 2] * matrix[2, 0]) + (matrix[0, 2] * matrix[1, 0] * matrix[2, 1]);
            det -= (matrix[0, 2] * matrix[1, 1] * matrix[2, 0]) + (matrix[0, 1] * matrix[1, 0] * matrix[2, 2]) + (matrix[0, 0] * matrix[1, 2] * matrix[2, 1]);
            opred.Text = Math.Round(det, 5).ToString();
            return det;
        }
        public void ModifyMatrixColumn(double[] freeMembers, int columnIndex, double main_det)
        {
            double x = double.Parse(x1_x2.Text.Trim());
            double y = double.Parse(x3_x2.Text.Trim());

            double[,] matrix = new double[3, 3]
            {
                {-1, x, 0},
                {0, y, -1},
                {1, 1, 1}
            };

            for (int i = 0; i < 3; i++)
            {
                matrix[i, columnIndex] = freeMembers[i];
            }


            double det = (matrix[0, 0] * matrix[1, 1] * matrix[2, 2]) + (matrix[0, 1] * matrix[1, 2] * matrix[2, 0]) + (matrix[0, 2] * matrix[1, 0] * matrix[2, 1]);
            det -= (matrix[0, 2] * matrix[1, 1] * matrix[2, 0]) + (matrix[0, 1] * matrix[1, 0] * matrix[2, 2]) + (matrix[0, 0] * matrix[1, 2] * matrix[2, 1]);
            
            if (columnIndex == 0)
            {
                opred1.Text = Math.Round(det, 5).ToString();
                var result = det / main_det;
                reh1.Text = Math.Round(result, 5).ToString();
            }
            if (columnIndex == 1)
            {
                opred2.Text = Math.Round(det, 5).ToString();
                var result = det / main_det;
                reh2.Text = Math.Round(result, 5).ToString();
            }
            if (columnIndex == 2)
            {
                opred3.Text = Math.Round(det, 5).ToString();
                var result = det / main_det;
                reh3.Text = Math.Round(result, 5).ToString();
            }
        }
        public void Procentetd()
        {
            var temp1 = Math.Round(double.Parse(reh1.Text.Trim()), 2) * 100;
            var temp2 = Math.Round(double.Parse(reh2.Text.Trim()), 2) * 100;
            var temp3 = Math.Round(double.Parse(reh3.Text.Trim()), 2) * 100;

            x1.Content = temp1.ToString();
            x2.Content = temp2.ToString();
            x3.Content = temp3.ToString();

            t1.Text = Math.Round((0.091 + 0.155 * (temp1/temp2) - 0.045 * (temp3/temp2)), 3).ToString();

            t2.Text = Math.Round((2.83 - 0.73 * (temp1 / temp2) - 2.18 * (temp3 / temp2)), 3).ToString();

            t3.Text = Math.Round((1.32 + 0.82 * (temp1 / temp2) - 1.36 * (temp3 / temp2)), 3).ToString();

            t4.Text = Math.Round((984.99 + 133.17 * (temp1 / temp2) - 555.31 * (temp3 / temp2)), 3).ToString();

            t5.Text = Math.Round((514.61 + 5.73 * (temp1 / temp2) + 265.97 * (temp3 / temp2)), 3).ToString();
        }
    }
}

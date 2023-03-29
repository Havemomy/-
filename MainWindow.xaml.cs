using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFCalc.Utils;

namespace WPFCalc
{

    public partial class MainWindow : Window
    {
        string DecimalSeparator => CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
        decimal FirstValue { get; set; }
        decimal? SecondValue { get; set; }

        IOperation CurrentOperation;

        public MainWindow()
        {
            InitializeComponent();
            btnPoint.Content = DecimalSeparator;
            btnSum.Tag = new Sum();
            btnSubtraction.Tag = new Subtraction();
            btnDivision.Tag = new Division();
            btnMultiplication.Tag = new Multiplication();
        }

        private void regularButtonClick(object sender, RoutedEventArgs e)
            => SendToInput(((Button)sender).Content.ToString());

        private void SendToInput(string content)
        {

            if (txtInput.Text == "0")
                txtInput.Text = "";

            txtInput.Text = $"{txtInput.Text}{content}";
        }

        private void btnPoint_Click(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text.Contains(this.DecimalSeparator))
                return;

            regularButtonClick(sender, e);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

            if (txtInput.Text == "0")
                return;

            txtInput.Text = txtInput.Text.Substring(0, txtInput.Text.Length - 1);
            if (txtInput.Text == "")
                txtInput.Text = "0";
        }

        private void operationButton_Click(object sender, RoutedEventArgs e)
        {

            if (CurrentOperation == null)
                FirstValue = Convert.ToDecimal(txtInput.Text);

            CurrentOperation = (IOperation)((Button)sender).Tag;
            SecondValue = null;
            txtInput.Text = "";
        }

        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            switch (e.Text)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    SendToInput(e.Text);
                    break;

                case "*":
                    btnMultiplication.PerformClick();
                    break;

                case "-":
                    btnSubtraction.PerformClick();
                    break;

                case "+":
                    btnSum.PerformClick();
                    break;

                case "/":
                    btnDivision.PerformClick();
                    break;

                case "=":
                    btnEquals.PerformClick();
                    break;

                default:

                    if (e.Text == DecimalSeparator)
                        btnPoint.PerformClick();
                    else if (e.Text[0] == (char)8)
                        btnBack.PerformClick();
                    else if (e.Text[0] == (char)13)
                        btnEquals.PerformClick();

                    break;
            }


            btnEquals.Focus();
        }

        private void btnEquals_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOperation == null)
                return;

            if (txtInput.Text == "")
                return;


            decimal val2 = SecondValue ?? Convert.ToDecimal(txtInput.Text);
            try
            {
                txtInput.Text = (FirstValue = CurrentOperation.DoOperation(FirstValue, (decimal)(SecondValue = val2))).ToString();
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Can't divide by zero", "Divided by zero", MessageBoxButton.OK, MessageBoxImage.Error);
                btnClearAll.PerformClick();
            }
        }

        private void btnClearEntry_Click(object sender, RoutedEventArgs e)
            => txtInput.Text = "0";

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            FirstValue = 0;
            CurrentOperation = null;
            txtInput.Text = "0";

        }

        private void btnSqrt_Click(object sender, RoutedEventArgs e)
        {
            double number;
            var isDouble = double.TryParse(this.txtInput.Text, out number);
            if (isDouble)
            {
                this.txtInput.Text =
                    string.Format(
                        "{0}{1} = {2}",
                        "\u221A",
                        this.txtInput.Text,
                        Math.Round(Math.Sqrt(number), 2));

            }
        }

        private void btnSin_Click(object sender, RoutedEventArgs e)
        {
            double number;
            var isDouble = double.TryParse(this.txtInput.Text, out number);
            if (isDouble)
            {
                this.txtInput.Text =
                    String.Format(
                        "{0}{1} = {2}",
                        "\u223F",
                        this.txtInput.Text,
                        Math.Sin(number));
            }
        }

        private void btnCos_Click(object sender, RoutedEventArgs e)
        {
            double number;
            var isDouble = double.TryParse(this.txtInput.Text, out number);
            if (isDouble)
            {
                this.txtInput.Text =
                    String.Format(
                        "{0}{1} = {2}",
                        "\u223F",
                        this.txtInput.Text,
                        Math.Cos(number));
            }
        }

        private void btnTg_Click(object sender, RoutedEventArgs e)
        {
            double number;
            var isDouble = double.TryParse(this.txtInput.Text, out number);
            if (isDouble)
            {
                this.txtInput.Text =
                    String.Format(
                        "{0}{1} = {2}",
                        "\u223F",
                        this.txtInput.Text,
                        Math.Tan(number));
            }
        }

        private void btnCtg_Click(object sender, RoutedEventArgs e)
        {
            double number;
            var isDouble = double.TryParse(this.txtInput.Text, out number);
            if (isDouble)
            {
                this.txtInput.Text =
                    String.Format(
                        "{0}{1} = {2}",
                        "\u223F",
                        this.txtInput.Text,
                        ((Math.Cos(number) / Math.Sin(number))));
            }
        }

        private void btnFct_Click(object sender, RoutedEventArgs e)
        {
            double number;
            int g = 0;
            var isDouble = double.TryParse(this.txtInput.Text, out number);
            for (int i = 1; number >= i; ++i)
            {
                g = g + i;
            }
            this.txtInput.Text = String.Format("{0}{1} = {2}", this.txtInput.Text, "!", g);
        }

        private void btnDegree_Click(object sender, RoutedEventArgs e)
        {
            double number;
            var isDouble = double.TryParse(this.txtInput.Text, out number);
            if (isDouble)
            {
                this.txtInput.Text =
                    String.Format(
                        this.txtInput.Text,
                        Math.Pow(number, 2));
            }
        }
    }   

}

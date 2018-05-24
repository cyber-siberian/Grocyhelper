using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=391641

namespace Grocyhelper
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool ghosty = false;
        const double CalcHide = 450;
        const double CalcDHide = 550;
        const double CalcOpen = 30;
        const double CalcStepToDHide = 5;

        const double AdderHide = -200;
        const double AdderDHide = -300;
        const double AdderOpen = 0;
        const double AdderStepToDHide = 20;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Подготовьте здесь страницу для отображения.

            // TODO: Если приложение содержит несколько страниц, обеспечьте
            // обработку нажатия аппаратной кнопки "Назад", выполнив регистрацию на
            // событие Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Если вы используете NavigationHelper, предоставляемый некоторыми шаблонами,
            // данное событие обрабатывается для вас.
        }

        private void NumClick(object sender, RoutedEventArgs e)
        {
            string number = (sender as Button).Content.ToString();
            if (ghosty)
            {
                NumText.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                NumText.Text = "";
                ghosty = false;
            }
            if(number != "0" || NumText.Text != "")
                NumText.Text += number;
        }

        private void OutClick(object sender, RoutedEventArgs e)
        {
            if (!ghosty)
            {
                int number = 0;
                int sum = 0;
                if (NumText.Text != "")
                    number = Convert.ToInt32(NumText.Text);
                if (SumText.Text != "")
                    sum = Convert.ToInt32(SumText.Text);
                if (sum < number)
                    number = sum;
                    sum -= number;
                    NumText.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);
                    NumText.Text = "-" + NumText.Text;
                    SumText.Text = sum.ToString();
                    ghosty = true;
            }
        }

        private void InClick(object sender, RoutedEventArgs e)
        {
            if (!ghosty)
            {
                int number = 0;
                int sum = 0;
                if (NumText.Text != "")
                    number = Convert.ToInt32(NumText.Text);
                if (SumText.Text != "")
                    sum = Convert.ToInt32(SumText.Text);
                    sum += number;
                    NumText.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);
                    NumText.Text = "+" + NumText.Text;
                    SumText.Text = sum.ToString();
                    ghosty = true;
            }
        }

        private void EraClick(object sender, RoutedEventArgs e)
        {
            NumText.Text = "";
            NumText.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
        }

        private async void OpenCalc_Click(object sender, RoutedEventArgs e)
        {
            Thickness Cmargin = Calc.Margin;
            Thickness Amargin = Adder.Margin;
            if (Calc.Margin.Left == CalcOpen)
            {
                for(double i = CalcOpen; i <= CalcHide; i += 20)
                {
                    Cmargin.Left = i;
                    if(Adder.Margin.Top != AdderHide)
                    {
                        Amargin.Top += AdderStepToDHide;
                        Adder.Margin = Amargin;
                    }
                    Calc.Margin = Cmargin;
                    await Task.Delay(1);
                }
            }
            else
            { 
                for(double i = CalcHide; i >= CalcOpen; i -= 20)
                {
                    Cmargin.Left = i;
                    if(Adder.Margin.Top != AdderDHide)
                    {
                        Amargin.Top -= AdderStepToDHide;
                        Adder.Margin = Amargin;
                    }
                    Calc.Margin = Cmargin;
                    await Task.Delay(1);
                }
            }
        }


        private async void OpenAdder_Click(object sender, RoutedEventArgs e)
        {
            Thickness Cmargin = Calc.Margin;
            Thickness Amargin = Adder.Margin;
            if (Adder.Margin.Top == AdderOpen)
            {
                for (double i = AdderOpen; i >= AdderHide; i -= 10)
                {
                    Amargin.Top = i;
                    if (Calc.Margin.Left != CalcHide)
                    {
                        Cmargin.Left -= CalcStepToDHide;
                        Calc.Margin = Cmargin;
                    }
                    Adder.Margin = Amargin;
                    await Task.Delay(1);
                }
            }
            else
            {
                for (double i = AdderHide; i <= AdderOpen; i += 10)
                {
                    Amargin.Top = i;
                    if (Calc.Margin.Left != CalcDHide)
                    {
                        Cmargin.Left += CalcStepToDHide;
                        Calc.Margin = Cmargin;
                    }
                    Adder.Margin = Amargin;
                    await Task.Delay(1);
                }
                NewFood.Focus(FocusState.Keyboard);
            }

        }

        private void Delete_fromlist(object sender, RoutedEventArgs e)
        {
            NewFood.IsTabStop = false;
            ShopList.Children.Remove(((Button)sender));
            NewFood.IsTabStop = true;
        }

        private void NewItem(object sender, RoutedEventArgs e)
        {
            string item_name = NewFood.Text;
            if(item_name.Trim(' ') != "")
            {
                Button btn_content = new Button();
                btn_content.Foreground = new SolidColorBrush(Color.FromArgb(0xF8, 0xF8, 0xFF, 0xFF));
                btn_content.Background = new SolidColorBrush(Colors.Transparent);
                btn_content.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x4F, 0x4E, 0x4E));
                btn_content.BorderThickness = new Thickness(0, 0, 0, 1);
                btn_content.Width = 350;
                btn_content.FontSize = 30;
                btn_content.Content = item_name;
                btn_content.HorizontalContentAlignment = HorizontalAlignment.Left;
                btn_content.Click += Delete_fromlist;
                btn_content.Focus(FocusState.Programmatic);
 
                ShopList.Children.Add(btn_content);
            }
            NewFood.Text = "";
            NewFood.Focus(FocusState.Keyboard);
        }
    }
}

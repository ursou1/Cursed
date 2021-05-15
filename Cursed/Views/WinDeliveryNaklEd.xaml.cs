using Cursed.Enttities;
using Cursed.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cursed.Views
{
    /// <summary>
    /// Логика взаимодействия для WinDeliveryNaklEd.xaml
    /// </summary>
    public partial class WinDeliveryNaklEd : Window
    {
        public WinDeliveryNaklEd()
        {
            InitializeComponent();
        }

        public WinDeliveryNaklEd(DeliveryNote selectedDeliveryNote)
        {
            InitializeComponent();
            DataContext = new ViewModelDeliveryNaklEd(selectedDeliveryNote);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

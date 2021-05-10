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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cursed.Views
{
    /// <summary>
    /// Логика взаимодействия для PageProduct.xaml
    /// </summary>
    public partial class PageProduct : Page
    {
        public PageProduct()
        {
            InitializeComponent();
            DataContext = new ViewModelProduct();
        }
        private void OpenProduct(object sender, RoutedEventArgs e) => NavigationService.Navigate(new PageProduct());
        private void OpenProvider(object sender, RoutedEventArgs e) => NavigationService.Navigate(new PageProvider());
        private void OpenClient(object sender, RoutedEventArgs e) => NavigationService.Navigate(new PageClient());
        private void OpenDepatNote(object sender, RoutedEventArgs e) => NavigationService.Navigate(new PageDepartNote());
        private void OpenDeliveryNote(object sender, RoutedEventArgs e) => NavigationService.Navigate(new PageDeliveryNote());
        private void OpenPageSettings(object sender, RoutedEventArgs e) => NavigationService.Navigate(new PageSettings());
        private void OpenPageMoveProduct(object sender, RoutedEventArgs e) => NavigationService.Navigate(new PageMoveProduct());
    }
}

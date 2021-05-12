using Cursed.Commands;
using Cursed.Enttities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Data;

namespace Cursed.ViewModels
{
    public class ViewModelMoveProduct : ViewModelBase, INotifyPropertyChanged
    {
        DB DB;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PartOfWarehouse> PartOfWarehouses { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public MiniCommand MoveProduct { get; set; }

        public ViewModelMoveProduct()
        {
            DB = DB.GetDb();
            PartOfWarehouses = new ObservableCollection<PartOfWarehouse>(DB.PartOfWarehouses);

            #region Фильтр, поиск
            Products = new ObservableCollection<Product>(DB.Products);//
            EmployeesCollectionView = CollectionViewSource.GetDefaultView(Products);
            EmployeesCollectionView.Filter = FilterEmployees;
            //EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Product.Unit))); // эта сортировка группирует по единице товара
            EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Product.Code), ListSortDirection.Ascending)); // это сортировка по коду товара

            #endregion

            MoveProduct = new MiniCommand(() =>
            {
                if (SelectedPartOfWarehouse != null && SelectedPartOfWarehouse2 != null && SelectedPartOfWarehouse2 != SelectedPartOfWarehouse && SelectedProduct != null)
                {
                    SelectedProduct.PartOfWarehouse = SelectedPartOfWarehouse2;

                    DB.SaveChanges();
                }
                else
                {
                    System.Windows.MessageBox.Show("Заполните все поля");
                }
            });
        }

        #region selected properties

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                SignalChanged();
            }
        }

        private PartOfWarehouse selectedPartOfWarehouse;
        public PartOfWarehouse SelectedPartOfWarehouse
        {
            get => selectedPartOfWarehouse;
            set
            {
                selectedPartOfWarehouse = value;
                try
                {
                    if (selectedPartOfWarehouse.Products == null || selectedPartOfWarehouse.Products.Count == 0)
                    {
                        System.Windows.MessageBox.Show("Вы выбрали пустой отсек");
                    }
                    else
                        Products = new ObservableCollection<Product>(selectedPartOfWarehouse.Products);
                        SignalChanged("Products");
                }
                catch(Exception ex)
                {
                }
            }
        }

        private PartOfWarehouse selectedPartOfWarehouse2;
        public PartOfWarehouse SelectedPartOfWarehouse2
        {
            get => selectedPartOfWarehouse2;
            set
            {
                selectedPartOfWarehouse2 = value;
            }
        }
        #endregion

        #region поляна для поиска

        public ICollectionView EmployeesCollectionView { get; set; }

        private string _employeesFilter = string.Empty;
        public string EmployeesFilter
        {
            get
            {
                return _employeesFilter;
            }
            set
            {
                _employeesFilter = value;
                OnPropertyChanged(nameof(EmployeesFilter));
                EmployeesCollectionView.Refresh();
            }
        }
        private bool FilterEmployees(object obj)
        {
            if (obj is Product product)
            {
                return product.Name.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase) || product.Unit.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        // Метод ниже обновляет листвью, чтобы работал поиск после редактирования листвью. (перезаписывает в icollection уже отредаченный список и поиск снова работает)
        private void RefreshListView()
        {
            EmployeesCollectionView = CollectionViewSource.GetDefaultView(Products);
            EmployeesCollectionView.Filter = FilterEmployees;
            //EmployeesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Product.Unit)));
            EmployeesCollectionView.SortDescriptions.Add(new SortDescription(nameof(Product.Code), ListSortDirection.Ascending));
        }

        #endregion

    }

}

using Cursed.Commands;
using Cursed.Enttities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cursed.ViewModels
{
    public class ViewModelProvider: INotifyPropertyChanged
    {
        DB DB;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Provider> Providers { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        #region Список мини-команд
        public MiniCommand AddProvider { get; set; } 
        public MiniCommand SaveProvider { get; set; } 
        public MiniCommand DeleteProvider { get; set; } 
        #endregion

        #region Конструктор
        public ViewModelProvider()
        {
            DB = DB.GetDb();
            Providers = new ObservableCollection<Provider>(DB.Providers);

            #region Команда с добавлением
            AddProvider = new MiniCommand(() =>
            {
                var provider = new Provider { Name="Имя", Email="Email" };
                DB.Providers.Add(provider);
                SelectedProvider = provider;
            });
            #endregion

            #region Команда с сохранением
            SaveProvider = new MiniCommand(() =>
            {
                try
                {
                    DB.SaveChanges();
                    LoadProviders();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            #endregion

            #region Команда с удалением
            DeleteProvider = new MiniCommand(() =>
            {
                DB.Providers.Remove(selectedProvider);
                DB.SaveChanges();
                LoadProviders();
            });
            #endregion
        }

        #endregion

        private Provider selectedProvider;
        public Provider SelectedProvider
        {
            get => selectedProvider;
            set
            {
                selectedProvider = value;
                SignalChanged();
            }
        }
        private void LoadProviders()
        {
            Providers = new ObservableCollection<Provider>(DB.Providers);
            SignalChanged("Providers");
        }

    }
}

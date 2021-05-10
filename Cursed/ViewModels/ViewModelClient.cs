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
    public class ViewModelClient: INotifyPropertyChanged
    {
        DB DB;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Client> Clients { get; set; }
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        #region Список мини-команд
        public MiniCommand AddClient { get; set; } 
        public MiniCommand SaveClient { get; set; } 
        public MiniCommand DeleteClient { get; set; }
        #endregion

        #region Конструктор
        public ViewModelClient()
        {
            DB = DB.GetDb();
            Clients = new ObservableCollection<Client>(DB.Clients);

            #region Команда с добавлением
            AddClient = new MiniCommand(() =>
            {
                var client = new Client { Name = "Имя", Female = "Фамилия", FatherName = "Отчество", Address = "Адрес", Email = "Email" };
                DB.Clients.Add(client);
                SelectedClient = client;
            });
            #endregion

            #region Команда с сохранением
            SaveClient = new MiniCommand(() =>
            {
                try
                {
                    DB.SaveChanges();
                    LoadClients();
                }
                catch(Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            #endregion

            #region Команда с удалением
            DeleteClient = new MiniCommand(() =>
            {
                DB.Clients.Remove(selectedClient);
                DB.SaveChanges();
                LoadClients();
            });
            #endregion

        }
        #endregion

        private Client selectedClient;
        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                SignalChanged();
            }
        }

        private void LoadClients()
        {
            Clients = new ObservableCollection<Client>(DB.Clients);
            SignalChanged("Clients");
        }
    }
}

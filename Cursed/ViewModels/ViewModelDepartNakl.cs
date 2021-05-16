using Cursed.Commands;
using Cursed.Enttities;
using Cursed.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace Cursed.ViewModels
{
    public class ViewModelDepartNakl: INotifyPropertyChanged
    {
        DB DB;
        public Window window { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        void SignalChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public ObservableCollection<DepartNote> DepartNotes { get; set; }

        public MiniCommand AddDepartNakl { get; set; }
        public MiniCommand SaveDepartNakl { get; set; }
        public MiniCommand CheckDepartNakl { get; set; }
        public MiniCommand DeleteDepartNakl { get; set; }

        public ViewModelDepartNakl()
        {
            DB = DB.GetDb();
            DepartNotes = new ObservableCollection<DepartNote>(DB.DepartNotes);
            SignalChanged("DeliveryNotes");

            #region Команда с добавлением

            AddDepartNakl = new MiniCommand(() =>
            {
                var departNote = new DepartNote { };
                DB.DepartNotes.Add(departNote);
                SelectedDepartNote = departNote;
            });
            #endregion

            #region Команда с сохранением
            SaveDepartNakl = new MiniCommand(() =>
            {
                try
                {
                    DB.SaveChanges();
                    LoadDepartNote();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            });
            #endregion

            #region Команда с удалением
            DeleteDepartNakl = new MiniCommand(() =>
            {
                DB.DepartNotes.Remove(selectedDepartNote);
                DB.SaveChanges();
                LoadDepartNote();
            });
            #endregion

            #region команда с открытием листа с продуктами
            CheckDepartNakl = new MiniCommand(() =>
            {
                window = new WinDepartNaklList(); // окно пока не расписано
                window.ShowDialog();
            });
            #endregion

        }

        private DepartNote selectedDepartNote;
        public DepartNote SelectedDepartNote
        {
            get => selectedDepartNote;
            set
            {
                selectedDepartNote = value;
                SignalChanged();
            }
        }

        private void LoadDepartNote()
        {
            DepartNotes = new ObservableCollection<DepartNote>(DB.DepartNotes);
            SignalChanged("DepartNotes");
        }

    }
}

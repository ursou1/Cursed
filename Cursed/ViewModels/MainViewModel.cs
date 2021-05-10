using System;
using System.Collections.Generic;
using System.Text;

namespace Cursed.ViewModels
{
    public class MainViewModel
    {
        DB DB;
        public MainViewModel()
        {
            DB = DB.GetDb();
        }
    }
}

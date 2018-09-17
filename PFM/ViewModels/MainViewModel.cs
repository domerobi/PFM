using LiveCharts;
using LiveCharts.Wpf;
using System.Data.SqlClient;
using System.Windows;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PFM
{
    /// <summary>
    /// ViewModel which handles all ViewModels of the application
    /// </summary>
    class MainViewModel : BaseViewModel
    {
        public IDataBase Database { get; set; }
        public MenuCommand MenuCommand { get; set; }

        public Users ActualUser { get; set; }
        public Pages ActualPage { get; set; }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel(int userid)
        {
            //Database = new CloudDatabase();

            MenuCommand = new MenuCommand(this);

            // Get the actually logged in user
            //Database.Open();
            DataModel dataModel = new DataModel();
            ActualUser = (from user in dataModel.Users
                          where user.UserID == userid
                          select user).First();
            //Database.Close();

            ActualPage = Pages.Reports;
        }

        #endregion
    }
}

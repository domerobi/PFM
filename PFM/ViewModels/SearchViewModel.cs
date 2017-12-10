using System;
using System.Collections.Generic;

namespace PFM
{
    class SearchViewModel : BaseViewModel
    {
        #region Attributes

        public IList<ItemType> SearchTypes { get; set; }
        public IList<String> SearchCategories { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SelectedCategory { get; set; }

        private ItemType selectedType;

        #endregion

        #region Constructor

        public SearchViewModel()
        {
            // Set the possible values for types and categories
            SearchTypes = new List<ItemType>()
            {
                new ItemType()
                {
                    Type = "Összes",
                    Categories = new List<String>(){ "" }
                },
                new ItemType()
                {
                    Type = "Bevétel",
                    Categories = new List<String>()
                    {
                        "Összes",
                        "Fizetés",
                        "Nyugdíj",
                        "Juttatás",
                        "Nyeremény",
                        "Egyéb"
                    }
                },
                new ItemType()
                {
                    Type = "Kiadás",
                    Categories = new List<String>()
                    {
                        "Összes",
                        "Lakhatás",
                        "Hitel",
                        "Utazás",
                        "Háztartás",
                        "Étkezés",
                        "Műszaki cikk",
                        "Bútor",
                        "Ruha",
                        "Személyes",
                        "Szórakozás",
                        "Egyéb"
                    }
                }
            };
            Reset();
        }
        
        #endregion

        #region Getters and setters

        public ItemType SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                if(selectedType != value)
                {
                    selectedType = value;
                    SearchCategories = selectedType.Categories;
                    SelectedCategory = SearchCategories[0];
                }
            }
        }

        #endregion

        #region Methods

        public void Reset()
        {
            DateTime firstDayOfMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            StartDate = firstDayOfMonth.ToString();
            EndDate = DateTime.Today.ToString();
            SelectedType = SearchTypes[0];
        }

        public bool CanReset()
        {
            DateTime firstDayOfMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            if (Convert.ToDateTime(StartDate) != firstDayOfMonth || Convert.ToDateTime(EndDate) != DateTime.Today || SelectedType != SearchTypes[0])
                return true;
            return false;
        }

        public bool CanSearch()
        {
            if (Convert.ToDateTime(StartDate) <=  Convert.ToDateTime(EndDate))
                return true;
            return false;
        }

        #endregion
    }
}

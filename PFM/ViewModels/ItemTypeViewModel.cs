using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PFM
{
    class ItemTypeViewModel : BaseViewModel
    {
        #region Attributes

        
        public IList<ItemType> Types { get; set; }
        public IList<ItemCategory> Cagetogries { get; set; }
        private ItemType selectedItemType;
        private ItemCategory selectedItemCategory;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemTypeViewModel()
        {
            this.Types = new List<ItemType>()
            {
                new ItemType()
                {
                    Type = "Válassz típust...",
                    Categories = new List<ItemCategory>(){new ItemCategory()}
                },
                new ItemType()
                {
                    Type = "Bevétel",
                    Categories = new List<ItemCategory>()
                    {
                        new ItemCategory(){Name = "Válassz kategóriát..."},
                        new ItemCategory(){Name = "Fizetés"},
                        new ItemCategory(){Name = "Nyugdíj"},
                        new ItemCategory(){Name = "Juttatás"},
                        new ItemCategory(){Name = "Nyeremény"},
                        new ItemCategory(){Name = "Egyéb"}
                    }
                },
                new ItemType()
                {
                    Type = "Kiadás",
                    Categories = new List<ItemCategory>()
                    {
                        new ItemCategory(){Name = "Válassz kategóriát..."},
                        new ItemCategory(){Name = "Lakhatás"},
                        new ItemCategory(){Name = "Hitel"},
                        new ItemCategory(){Name = "Utazás"},
                        new ItemCategory(){Name = "Élelmiszer"},
                        new ItemCategory(){Name = "Műszaki cikk"},
                        new ItemCategory(){Name = "Személyes"},
                        new ItemCategory(){Name = "Szórakozás"},
                        new ItemCategory(){Name = "Egyéb"}
                    }
                }
            };

            this.SelectedItemType = this.Types[0];
            this.selectedItemCategory = this.Types[0].Categories[0];
        }

        #endregion

        #region Getters and Setters

        public ItemType SelectedItemType
        {
            get
            {
                return selectedItemType;
            }
            set
            {
                selectedItemType = value;
                this.Cagetogries = selectedItemType.Categories;
                SelectedItemCategory = this.Cagetogries[0];
            }
        }

        public ItemCategory SelectedItemCategory
        {
            get
            {
                return selectedItemCategory;
            }
            set
            {
                selectedItemCategory = value;
            }
        }

        #endregion

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void OnPropertyChanged(string v) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
    }
}

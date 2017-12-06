using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PFM
{
    /// <summary>
    /// ViewModel for representing item types and categories
    /// </summary>
    class ItemTypeViewModel : BaseViewModel
    {
        #region Attributes

        private Inventory Item;
        public IList<ItemType> Types { get; set; }
        public IList<String> Categories { get; set; }
        private ItemType selectedItemType;
        private string selectedItemCategory;
        

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemTypeViewModel()
        {
            Item = new Inventory();
            Types = new List<ItemType>()
            {
                new ItemType()
                {
                    Type = "Válassz típust...",
                    Categories = new List<String>(){ "" }
                },
                new ItemType()
                {
                    Type = "Bevétel",
                    Categories = new List<String>()
                    {
                        "Válassz kategóriát...",
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
                        "Válassz kategóriát...",
                        "Lakhatás",
                        "Hitel",
                        "Utazás",
                        "Élelmiszer",
                        "Műszaki cikk",
                        "Személyes",
                        "Szórakozás",
                        "Egyéb"
                    }
                }
            };

            SelectedItemType = Types[0];
            SelectedItemCategory = Types[0].Categories[0];
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
                Categories = selectedItemType.Categories;
                SelectedItemCategory = selectedItemType.Categories[0];
                Item.Type = selectedItemType.Type;
            }
        }

        public String SelectedItemCategory
        {
            get
            {
                return selectedItemCategory;
            }
            set
            {
                selectedItemCategory = value;
                Item.Category = selectedItemCategory;
            }
        }

        public string Sum
        {
            get
            {
                return Item.Sum.ToString();
            }
            set
            {
                Item.Sum = Convert.ToInt32(value);
            }
        }
        public string Date
        {
            get
            {
                return Item.Date.ToString();
            }
            set
            {
                Item.Date = Convert.ToDateTime(value);
            }
        }
        public string comment
        {
            get
            {
                return Item.Comment;
            }
            set
            {
                Item.Comment = value;
            }
        }

        #endregion

    }
}

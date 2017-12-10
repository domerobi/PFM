using PropertyChanged;
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

        public IList<ItemType> Types { get; set; }
        public IList<String> Categories { get; set; }
        private ItemType selectedItemType;
        private string selectedItemCategory;
        public string ItemSum { get; set; }
        public string ItemComment { get; set; }
        public string ItemDate { get; set; }
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemTypeViewModel()
        {
            // Set the possible values for types and categories
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

            // Set each input field to default state
            ClearFields();

        }

        #endregion
        
        #region Getters and Setters

        /// <summary>
        /// Gets or sets the selected item type
        /// </summary>
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
            }
        }

        /// <summary>
        /// Gets or sets the selected item category
        /// </summary>
        public String SelectedItemCategory
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

        #region Methods

        /// <summary>
        /// Gets the values from input fields and create an inventory item from them
        /// </summary>
        /// <returns></returns>
        public Inventory CreateItem()
        {
            Inventory Item = new Inventory();
            Item.Type = SelectedItemType.ToString();
            Item.Category = SelectedItemCategory;
            Item.Sum = Convert.ToInt32(ItemSum);
            Item.Date = Convert.ToDateTime(ItemDate);
            Item.Comment = ItemComment;
            return Item;
        }

        /// <summary>
        /// Sets default value for each input field
        /// </summary>
        public void ClearFields()
        {
            SelectedItemType = Types[0];
            SelectedItemCategory = Types[0].Categories[0];
            ItemSum = "";
            ItemDate = DateTime.Today.ToString();
            ItemComment = "";
        }

        #endregion

    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace anothermidtermBSdraft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<InventoryItem> _inventoryItems;
        private ICollectionView _inventoryView;
        private string _imagePath;

        public MainWindow()
        {
            InitializeComponent();
            _inventoryItems = new ObservableCollection<InventoryItem>();
            _inventoryView = CollectionViewSource.GetDefaultView(_inventoryItems);
            listView.ItemsSource = _inventoryView;

            string[] lines = File.ReadAllLines("inventory.txt");
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 5)
                {
                    InventoryItem item = new InventoryItem(parts[0], parts[1], int.Parse(parts[2]), decimal.Parse(parts[3]), parts[4]);
                    _inventoryItems.Add(item);
                }
            }

            _imagePath = null;

            if (_inventoryItems.Count > 0)
            {
                listView.SelectedItem = _inventoryItems[0];
            }
        }

        private void IncrementStock_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                InventoryItem selectedItem = (InventoryItem)listView.SelectedItem;
                selectedItem.Stock++;
                stockTextBlock.Text = selectedItem.Stock.ToString();
            }
        }


        private void DecrementStock_Click(object sender, RoutedEventArgs e)
        {
            int stock;
            if (int.TryParse(stockTextBlock.Text, out stock) && stock > 0)
            {
                stock--;
                stockTextBlock.Text = stock.ToString();
            }
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            string description = descriptionTextBox.Text;
            decimal price;

            if (!decimal.TryParse(priceTextBox.Text, out price))
            {
                MessageBox.Show("Invalid price format. Please enter a valid price.");
                return;
            }

            int stock;
            if (!int.TryParse(stockTextBlock.Text, out stock))
            {
                MessageBox.Show("Invalid stock format. Please enter a valid stock.");
                return;
            }

            InventoryItem item = new InventoryItem(description, description, stock, price, _imagePath);
            _inventoryItems.Add(item);

            StringBuilder sb = new StringBuilder();
            foreach (InventoryItem i in _inventoryItems)
            {
                sb.AppendLine($"{i.Name},{i.Description},{i.Stock},{i.Price},{i.ImagePath}");
            }
            File.WriteAllText("inventory.txt", sb.ToString());

            descriptionTextBox.Clear();
            priceTextBox.Clear();
            stockTextBlock.Text = "0"; // Reset stock to 0
            Photo.Source = null;
            _imagePath = null;
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                InventoryItem itemToRemove = (InventoryItem)listView.SelectedItem;
                _inventoryItems.Remove(itemToRemove);

                StringBuilder sb = new StringBuilder();
                foreach (InventoryItem item in _inventoryItems)
                {
                    sb.AppendLine($"{item.Name},{item.Description},{item.Stock},{item.Price},{item.ImagePath}");
                }
                File.WriteAllText("inventory.txt", sb.ToString());

                descriptionTextBox.Clear();
                priceTextBox.Clear();
                stockTextBlock.Text = "0"; // Reset stock to 0
                _imagePath = null;

                listView.Items.Refresh();
            }
        }

        private void UploadImageClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                _imagePath = op.FileName;
                Photo.Source = new BitmapImage(new Uri(_imagePath));
            }
        }
    }
}
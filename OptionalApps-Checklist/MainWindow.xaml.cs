using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using static OptionalApps_Checklist.MainWindow;

namespace OptionalApps_Checklist
{
    public partial class MainWindow : Window
    {
        public List<string> SelectedApps = new List<string>();
        public MainWindow()
        {
             
        InitializeComponent();
        dynamiccheckbox();
        }
        public void dynamiccheckbox()
        {
            string filePath = @"C:\data.json";
            try
            {
                // Read the JSON file
                string jsonText = File.ReadAllText(filePath);

                // Deserialize JSON into objects
                var items = JsonConvert.DeserializeObject<Item[]>(jsonText);

                // Display the deserialized objects
                foreach (var item in items)
                {
                    CheckBox chk = new CheckBox();
                    chk.Content = item.Name.ToString();
                    st1.Children.Add(chk);
                    // Console.WriteLine($"Name: {item.Name}, IsChecked: {item.IsChecked}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
            }
        }
        public class Item
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            
            // Iterate through the child controls of the StockPanel
            foreach (Control control in st1.Children)
            {
                // Check if the control is a CheckBox
                if (control is CheckBox checkBox)
                {
                    // Check if the CheckBox is checked
                   if (checkBox.IsChecked == true)
                   {
                   SelectedApps.Add(checkBox.Content.ToString());
                   }
                }
            }
            InstallApps(SelectedApps);
            
        }

        private void InstallApps(List<string> SelectedApps)
        {
            Hide();
            if (SelectedApps.Count > 0)
            {
                
                foreach (var item in SelectedApps)
                {
                    Console.WriteLine(item.ToString());
                    MessageBox.Show(item);
                }
            }
            Application.Current.Shutdown();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
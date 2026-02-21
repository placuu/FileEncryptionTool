using System;
using System.Collections.Generic;
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
using System.IO;

namespace FileEncryptionTool
{

    public partial class MainWindow : Window
    {
        private bool isEncyptionMode = true;
        string[] files;
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (RadioEncryption.IsChecked == true)
            {
                isEncyptionMode = true;
            }
            else if(RadioDecryption.IsChecked == true)
            {
                isEncyptionMode = false;
            }
        }

        private void StartButton(object sender, RoutedEventArgs e)
        {
            if (!RadioButtonValidation())
            {
                return;
            }
            if (!FileValidation()) 
            {
                return;
            }

            ProcessFile();
        }

        private void DragDrop(object sender, DragEventArgs e)
        {
            InfoText.Text = "";
            
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    string fileName = System.IO.Path.GetFileName(file);
                    InfoText.Text += fileName + "\n";
                }
            }
        }
        
        private bool FileValidation()
        {
            if (files == null || files.Length == 0)
            {
                MessageBox.Show("No files selected. Please drag and drop files first.",
                                "No files",
                                MessageBoxButton.OK, 
                                MessageBoxImage.Warning);
                return false;
            }

               return true;
        }

        private bool RadioButtonValidation()
        {
            if(RadioEncryption.IsChecked != true && RadioDecryption.IsChecked != true)
            {
                MessageBox.Show("Please select encryption or decryption mode.",
                                "No mode selected",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                return false;
            }
            return true;
        }

        private void ProcessFile()
        {
            int successCount = 0;
            int failCount = 0;

            foreach (string file in files) 
            {
                try
                {
                    if (isEncyptionMode)
                    {
                        EncryptFile(file);
                    }
                    else
                    {
                        DecryptFile(file);
                    }
                    successCount++;
                }
                catch (Exception ex) 
                {
                    failCount++;
                }
            }
            string mode = isEncyptionMode ? "Encryption" : "Decryption";
            string message = $"{mode} complete!\n\n Successful: {successCount}\n Failed: {failCount}";
            if(failCount == 0)
            {
                MessageBox.Show($"{message}", "Completed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if(successCount > 0)
            {
                MessageBox.Show($"{message}", "Completed with errors", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show($"{message}", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EncryptFile(string file)
        {
            File.Encrypt(file);
        }

        private void DecryptFile(string file)
        {
            File.Decrypt(file);
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}

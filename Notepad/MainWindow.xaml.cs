using System.IO;
using System.Text;
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

namespace Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isTextChanged = false;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainTextBox.Focus();
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isTextChanged = true;
        }

        // file menu

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            if (isTextChanged)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButton.YesNoCancel);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    // save file
                    SaveFile_Click(sender, e);
                }
                else if (messageBoxResult == MessageBoxResult.Cancel)
                {
                    // cancel creating new file when user click cancel
                    return;
                }
            }

            
            MainTextBox.Clear();
            isTextChanged = false;
        }


        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (isTextChanged)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    // save file
                    SaveFile_Click(sender, e);
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    // cancel opening a new file when the user clicks cancel
                    return;
                }
            }

            // Proceed with opening a new file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);
                MainTextBox.Text = fileContent;
                isTextChanged = false; // reset the flag as the content is now from a file
            }
        }


        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            // save file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, MainTextBox.Text);
            }
        }

        // edit menu

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (MainTextBox.CanUndo)
            {
                MainTextBox.Undo();
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (MainTextBox.CanRedo)
            {
                MainTextBox.Redo();
            }
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Cut();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Copy();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Paste();
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // format menu

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var color = (Color)ColorConverter.ConvertFromString(menuItem.Header.ToString());
            MainTextBox.Foreground = new SolidColorBrush(color);
        }

        private void Font_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                string fontFamily = menuItem.Header.ToString();
                MainTextBox.FontFamily = new FontFamily(fontFamily);
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.FontSize = Math.Min(MainTextBox.FontSize + 2, 72); // increase size of font by 2 up to 72
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.FontSize = Math.Max(MainTextBox.FontSize - 2, 8); // decrease size of font by 2 down to 8
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Notepad\nJakub Radwanski", "About");
        }


    }
}
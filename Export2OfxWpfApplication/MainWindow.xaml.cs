using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Export2OfxWpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            txtEditor1.Text = dir;

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // ... Cast sender object.
            MenuItem item = sender as MenuItem;
            // ... Change Title of this window.
            this.Title = "Info: " + item.Header;
        }

        private void MenuFermer_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// http://www.wpf-tutorial.com/dialogs/the-openfiledialog/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; //todo: a changer en CSV
            openFileDialog.InitialDirectory = @"c:\temp\"; //todo: répertoire de download

            if (openFileDialog.ShowDialog() == true)
                txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void txtEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int row = txtEditor1.GetLineIndexFromCharacterIndex(txtEditor1.CaretIndex);
            int col = txtEditor1.CaretIndex - txtEditor1.GetCharacterIndexFromLineIndex(row);
            lblCursorPosition.Text = "Line " + (row + 1) + ", Char " + (col + 1);
        }

    }
}

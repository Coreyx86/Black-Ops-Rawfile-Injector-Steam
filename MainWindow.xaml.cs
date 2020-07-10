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
using MemoryLib;
using Ookii.Dialogs;
using Ookii.Dialogs.Wpf;

namespace BlackOpsGSCInjector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title += " - " + Manager.obj.process.Replace(".exe", "");
        }

        /// <summary>
        /// Add a new line of text to the output richTextBox
        /// </summary>
        /// <param name="print"></param>
        private void DbgPrint(string print)
        {
            txtOutput.Document.Blocks.Add(new Paragraph(new Run(print)));
        }

        /// <summary>
        /// Opens the project folder to inject
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Document.Blocks.Clear();

            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();

            if ((bool)dialog.ShowDialog())
            {
                Manager.obj.project = new Project(dialog.SelectedPath);

                Manager.obj.project.Load();

                DbgPrint("Opened: " + Manager.obj.project.folder);
            }



        }

        /// <summary>
        /// Injects any rawfiles marked for overwriting 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!Manager.obj.project.ProjectLoaded)
                {
                    MessageBox.Show("Project hasn't been loaded, please load the project folder");
                }
                MessageBox.Show("Injecting project please wait.", "SUCCESS", MessageBoxButton.OK);

                Manager.obj.rawpool.OverwriteRawfiles();

                MessageBox.Show("Injected project successfully! Enjoy", "SUCCESS", MessageBoxButton.OK);
            }
            catch(Exception)
            {
                MessageBox.Show("Failed to inject project", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Reads and dumps the rawfiles from memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDump_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VistaFolderBrowserDialog fb = new VistaFolderBrowserDialog();

                if ((bool)fb.ShowDialog())
                {
                    MessageBox.Show("Starting to dump rawfiles please wait a minute", "Dumping...", MessageBoxButton.OK);

                    //Dump the rawfiles to our selected path and return number of dumped files
                    int numOfDumpedRawfiles = Manager.obj.rawpool.DumpRawfiles(fb.SelectedPath);

                    MessageBox.Show("Dumped : " + numOfDumpedRawfiles + " rawfiles", "SUCCESS", MessageBoxButton.OK);
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

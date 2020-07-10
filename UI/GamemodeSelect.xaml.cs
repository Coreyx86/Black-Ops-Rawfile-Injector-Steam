using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlackOpsGSCInjector
{
    /// <summary>
    /// Interaction logic for GametypeSelect.xaml
    /// </summary>
    public partial class GamemodeSelect : Window
    {
        public GamemodeSelect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set up the program for singleplayer / zm mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSP_Click(object sender, RoutedEventArgs e)
        {
            Manager.obj.process = "BlackOps";
            Manager.obj.rawpool = new RawPool(0x10C3600);

            this.DialogResult = true;
            Window.GetWindow(this).Close();
        }

        /// <summary>
        /// Set up the program for multiplayer mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMP_Click(object sender, RoutedEventArgs e)
        {
            Manager.obj.process = "BlackOpsMP";
            Manager.obj.rawpool = new RawPool(0x28C6B00);

            this.DialogResult = true;
            Window.GetWindow(this).Close();
        }




        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            string DEBUG_PROCESS = "BlackOps";

            //Manager.obj.memory.ProcessName = DEBUG_PROCESS;
            Manager.obj.process = DEBUG_PROCESS;
            Manager.obj.rawpool = new RawPool(0x10C3600);

            string[] XAssetPoolNames = new string[] {
                "xmodelpieces",
                "physpreset",
                "physconstraints",
                "destructibledef",
                "xanim",
                "xmodel",
                "material",
                "techset",
                "image",
                "sound",
                "sound_patch",
                "col_map_sp",
                "col_map_mp",
                "com_map",
                "game_map_sp",
                "game_map_mp",
                "map_ents",
                "gfx_map",
                "lightdef",
                "ui_map",
                "font",
                "menufile",
                "menu",
                "localize",
                "weapon",
                "weapondef",
                "weaponvariant",
                "snddriverglobals",
                "fx",
                "impactfx",
                "aitype",
                "mptype",
                "mpbody",
                "mphead",
                "character",
                "xmodelalias",
                "rawfile",
                "stringtable",
                "packindex",
                "xGlobals",
                "ddl",
                "glasses",
                "emblemset"
            };

            VistaFolderBrowserDialog fb = new VistaFolderBrowserDialog();

            if ((bool)fb.ShowDialog())
            {
                //Dump the rawfiles to our selected path and return number of dumped files
                int numOfDumpedRawfiles = Manager.obj.rawpool.DumpRawfiles(fb.SelectedPath);

                Console.WriteLine("Dumped : " + numOfDumpedRawfiles + " rawfiles");
            }





            //MessageBox.Show("Finished dumping " + rawfileList.Count + " rawfiles");
        }
    }
}

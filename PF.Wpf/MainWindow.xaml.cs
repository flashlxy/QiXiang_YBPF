using System;
using System.Collections.Generic;
using System.Linq;
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
using PF.BLL.SQL;
using PF.Models.SQL;

namespace PF.Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            //BaoWens_BLL bwbll = new BaoWens_BLL();
            //List<BaoWens> list = bwbll.GetList().OrderBy(a => a.YBDateTime).ThenBy(a => a.YBUserName).ToList();
            //ProgressBarTotal.Maximum = list.Count();

            //BaoWen.BaoWenFile bf = new BaoWen.BaoWenFile();
            //int i = 0;
            //label1.Content = list.Count().ToString();
            //foreach (BaoWens baoWens in list)
            //{
            //    bf.JieXiBaoWen(baoWens);
            //    i++;

            //    ProgressBarTotal.Value = i;
            //    label2.Content = i.ToString();
            //}
            BaoWen.BaoWenFile bf = new BaoWen.BaoWenFile();
            bf.BaoWen_Import();
            MessageBox.Show("成功");

        }
    }
}

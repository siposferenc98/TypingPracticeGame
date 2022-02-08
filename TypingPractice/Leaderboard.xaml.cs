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
using System.Windows.Shapes;

namespace TypingPractice
{
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : Window
    {
        public Leaderboard()
        {
            InitializeComponent();
            DataContext = new LeaderBoardVM();
        }


        //Probably breaks MVVM pattern but whatever, binding a simple doubleclick trigger to the viewmodel is a chore, especially if i want to send the selected item as a parameter, and do the same for all 3 of the listboxes..
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            SaveHighScore saveHighScoreWindow = new((Game)listBox.SelectedItem,true);
            saveHighScoreWindow.ShowDialog();
        }
    }
}

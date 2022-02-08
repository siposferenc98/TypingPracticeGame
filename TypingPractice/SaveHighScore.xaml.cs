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
    /// Interaction logic for SaveHighScore.xaml
    /// </summary>
    public partial class SaveHighScore : Window
    {
        public SaveHighScore(Game game, bool isNameReadOnly = false)
        {
            InitializeComponent();
            NameTextBox.IsReadOnly = isNameReadOnly;
            SaveButton.Visibility = isNameReadOnly ? Visibility.Collapsed : Visibility.Visible;
            SaveHighScoreVM viewModel = new(game);
            DataContext = viewModel;
        }
    }
}

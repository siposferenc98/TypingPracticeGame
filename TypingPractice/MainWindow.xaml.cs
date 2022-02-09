global using TypingPractice.Models;
global using TypingPractice.ViewModels;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TypingPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool DifficultyPanelIsEnabled = true;
        public MainWindow()
        {
            InitializeComponent();
            MainWindowVM ViewModel = new();
            DataContext = ViewModel;
            ViewModel.FocusTextBox += ViewModel_FocusTextBox;
            ViewModel.MissedWord += ViewModel_MissedWord;
        }

        private void ViewModel_FocusTextBox(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                DifficultyPanelIsEnabled = !DifficultyPanelIsEnabled;
                DifficultyPanel.IsEnabled = DifficultyPanelIsEnabled;
                TypingTextBox.IsEnabled = !DifficultyPanelIsEnabled;
                TypingTextBox.Focus();
            });
        }
        private void ViewModel_MissedWord(object? sender, EventArgs e)
        {
            Storyboard storyboard = new();
            ColorAnimation animation = new(Colors.Transparent, Colors.Red, TimeSpan.FromSeconds(0.1))
            {
                AutoReverse = true
            };
            PropertyPath color = new("(0).(1)",TextBox.BackgroundProperty,SolidColorBrush.ColorProperty);
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, TypingTextBox);
            Storyboard.SetTargetProperty(animation,color);
            storyboard.Begin();
        }
    }
}

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Windows.Graphics;

namespace SpeechRecognitionApp
{
    public sealed partial class AssistantCommands : Window
    {
        private CommandManager commandManager;

        public AssistantCommands(SpeechHandler speechHandler)
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            AppWindow.Resize(new SizeInt32 { Width = 800, Height = 800 });
            commandManager = new CommandManager(speechHandler, UpdateCommandsListBox);
            UpdateCommandsListBox();
        }

        private void AddCommand_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.ClearMessage(AddingMessage);

            if (string.IsNullOrEmpty(QuestionTextBox.Text))
            {
                UIHelper.SetMessage(AddingMessage, "Please enter a question.");
                return;
            }

            if (string.IsNullOrEmpty(AnswerTextBox.Text))
            {
                UIHelper.SetMessage(AddingMessage, "Please enter an answer.");
                return;
            }

            if (commandManager.CommandExists(QuestionTextBox.Text))
            {
                UIHelper.SetMessage(AddingMessage, "This question already exists. Please enter a new question.");
                return;
            }

            commandManager.AddCommand(QuestionTextBox.Text, AnswerTextBox.Text);
            UIHelper.SetMessage(AddingMessage, "Command added successfully.", true);
            ClearTextBoxes();
        }

        private void DeleteCommand_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.ClearMessage(DeletingMessage);

            if (CommandsListBox.SelectedItem == null)
            {
                UIHelper.SetMessage(DeletingMessage, "Please select a command to delete.");
                return;
            }

            var selectedCommand = CommandsListBox.SelectedItem.ToString();
            if (commandManager.DeleteCommand(selectedCommand))
            {
                UIHelper.SetMessage(DeletingMessage, "Command deleted successfully.", true);
            }
            else
            {
                UIHelper.SetMessage(DeletingMessage, "The selected command does not exist.");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCommandsListBox();
        }

        private void UpdateCommandsListBox()
        {
            CommandsListBox.ItemsSource = commandManager.GetFilteredCommands(SearchTextBox.Text.Trim().ToLower());
        }

        private void ClearTextBoxes()
        {
            QuestionTextBox.Text = string.Empty;
            AnswerTextBox.Text = string.Empty;
        }
    }
}

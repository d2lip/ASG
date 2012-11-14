using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using TouchToolkit.Framework.DataService;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.Framework.Utility;
using TouchToolkit.Framework.Exceptions;
using System.Collections.ObjectModel;
using TouchToolkit.Framework.Components;
using TouchToolkit.Framework.Storage;

namespace TouchToolkit.Framework.UI
{
    public partial class GestureRecordingPanel : UserControl
    {
        #region Constructor & Global variables
        public class ControlCaptions
        {
            public const string StartRecording = "Start Recording";
            public const string StopRecording = "Stop Recording";
            public const string RunGesture = "Run";
            public const string StopGesture = "Stop";
        }

        private TouchInputRecorder _recorder = new TouchInputRecorder();
        private StorageManager _storage = new StorageManager();
        private bool _skipProjectComboboxSelectionChanage = false;

        private List<ProjectDetail> ProjectDetails = new List<ProjectDetail>();

        public GestureRecordingPanel()
        {
            InitializeComponent();
            Init();
        }
        #endregion

        #region Gesture Recording
        private void StartRecording()
        {
            _recorder.StartRecording();
        }

        private void StopRecording()
        {
            string data = _recorder.StopRecording();
            SaveRecordedData(data);
        }

        private void SaveRecordedData(string data)
        {
            string projectName = ProjectNameTextBox.Text.Trim();
            string gestureName = GestureNameTextBox.Text.Trim();

            _storage.SaveGesture(projectName, gestureName, data, (gesName, error) =>
            {
                if (error != null)
                    MessageBox.Show(error.Message);
                else
                {
                    RefreshProjectCombobox(projectName);
                    RefreshGestureListbox(projectName, gesName);
                }
            });
        }

        #endregion

        #region Init & Content Loading
        private void Init()
        {
            if (!_storage.IsLoggedIn())
            {
                ShowLoginScreen();
            }

            // Set control captions
            RunButton.Content = ControlCaptions.RunGesture;
            StartRecordingButton.Content = ControlCaptions.StartRecording;

            // Initialize the recorder
            _recorder.PlaybackCompleted += _recorder_PlaybackCompleted;

            LoadUserData();
        }

        void _recorder_PlaybackCompleted()
        {
            Action act = delegate
            {
                RunButton.IsEnabled = true;
                RunButton.Content = ControlCaptions.RunGesture;
            };

            Dispatcher.BeginInvoke(act);
        }

        private void LoadUserData()
        {
            if (!_storage.IsLoggedIn())
            {
                ShowLoginScreen();
            }
            else
            {
                // Show registered user screen
                UserNameTextBlock.Text = _storage.AccountName;
                LoginScreenGrid.Visibility = System.Windows.Visibility.Collapsed;
                RegisteredUserGrid.Visibility = System.Windows.Visibility.Visible;

                ExistingProjectNameComboBox.Items.Clear();
                ExistingGestureNameListBox.Items.Clear();

                RefreshProjectCombobox();
            }
        }

        private void ShowLoginScreen()
        {
            // Show login screen
            LoginScreenGrid.Visibility = System.Windows.Visibility.Visible;
            RegisteredUserGrid.Visibility = System.Windows.Visibility.Collapsed;
        }


        #endregion

        #region Utility Methods
        private void ShowErrorMessage(string msg)
        {
            MessageBox.Show(msg);
            MessageTextBlock.Text = msg;
        }

        public void RefreshProjectCombobox(string projectName = null)
        {
            _storage.GetAllProjects((projectDetails, error) =>
            {
                if (error == null)
                {
                    // Remove selection change listener so that this update will not trigger a UI update request 
                    ExistingProjectNameComboBox.SelectionChanged -= ExistingProjectNameComboBox_SelectionChanged;

                    ProjectDetail selectedItem = null;

                    // Load projects combobox
                    ExistingProjectNameComboBox.Items.Clear();
                    foreach (var project in projectDetails)
                    {
                        ExistingProjectNameComboBox.Items.Add(project);
                        if (project.ProjectName == projectName)
                            selectedItem = project;
                    }

                    ExistingProjectNameComboBox.SelectedItem = selectedItem;

                    ExistingProjectNameComboBox.SelectionChanged += ExistingProjectNameComboBox_SelectionChanged;
                }
                else
                {
                    ShowErrorMessage(error.Message);
                }
            });
        }

        public void RefreshGestureListbox(string projectName, string gestureName = null)
        {
            //TODO: Extend the webservice so that we can get only the gestures of a particular project
            _storage.GetAllProjects((projectDetails, error) =>
            {
                if (error == null)
                {
                    ProjectDetail selectedProject = projectDetails.SingleOrDefault<ProjectDetail>(p => p.ProjectName == projectName);
                    if (selectedProject != null)
                    {
                        ExistingGestureNameListBox.Items.Clear();
                        foreach (string gesName in selectedProject.GestureNames)
                        {
                            ExistingGestureNameListBox.Items.Add(gesName);
                        }

                        ExistingGestureNameListBox.SelectedItem = gestureName;
                    }
                    else
                    {
                        ShowErrorMessage("Failed to retrieve data!");
                    }
                }
                else
                {
                    ShowErrorMessage(error.Message);
                }
            });
        }

        #endregion

        #region Play Recorded Gestures
        private void RunGesture()
        {
            if (ExistingGestureNameListBox.SelectedItems.Count > 0)
            {
                // Update "start/stop" button text
                RunButton.Content = ControlCaptions.StopGesture;

                List<string> gestureData = new List<string>();
                int count = 0;
                int total = ExistingGestureNameListBox.SelectedItems.Count;
                foreach (var item in ExistingGestureNameListBox.SelectedItems)
                {
                    // Get Gesture info
                    string projectName = (ExistingProjectNameComboBox.SelectedItem as ProjectDetail).ProjectName;
                    string gestureName = (string)item;

                    // Download gesture data
                    _storage.GetGesture(projectName, gestureName, (projName, gesName, data, error) =>
                    {
                        if (error == null)
                            gestureData.Add(data);
                        else
                            ShowErrorMessage(error.Message);

                        // If this is the last download then start the playback
                        if (++count == total)
                        {
                            _recorder.RunGesture(gestureData);
                        }
                    });
                }
            }
            else
            {
                MessageBox.Show("Please select the \"Gesture\" you want to run!");
            }

            /*
            if (ExistingGestureNameListBox.SelectedItem != null && ExistingGestureNameListBox.SelectedItem != null)
            {
                // Get Gesture Data 
                string projectName = (ExistingProjectNameComboBox.SelectedItem as ProjectDetail).ProjectName;
                string gestureName = (string)ExistingGestureNameListBox.SelectedItem;

                _storage.GetGesture(projectName, gestureName, (projName, gesName, data, error) =>
                {
                    if (error == null)
                        _recorder.RunGesture(data);
                    else
                        ShowErrorMessage(error.Message);

                });
                // Run Gesture
                // Show remaining time
                // Provide stop option, on stop change button-state to start mode
                // On end, change button-state to start move
            }
            else
            {
                MessageBox.Show("Please select the \"Gesture\" you want to run!");
            }
             */
        }

        
        #endregion

        #region UI Events
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserNameTextBox.Text.Trim().Length > 0)
            {
                _storage.Login(UserNameTextBox.Text.Trim());
                LoadUserData();
            }
            else
            {
                MessageBox.Show("Please enter your username!");
            }
        }

        private void StartRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)StartRecordingButton.Content == ControlCaptions.StartRecording)
            {
                StartRecording();
                StartRecordingButton.Content = ControlCaptions.StopRecording;
            }
            else if ((string)StartRecordingButton.Content == ControlCaptions.StopRecording)
            {
                StopRecording();
                StartRecordingButton.Content = ControlCaptions.StartRecording;
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)RunButton.Content == ControlCaptions.RunGesture)
            {
                RunGesture();
            }
            else if ((string)RunButton.Content == ControlCaptions.StopGesture)
            {
                _recorder.StopPlayback();
            }
        }

        private void ExistingProjectNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExistingProjectNameComboBox.SelectedValue != null)
            {
                var projName = (ExistingProjectNameComboBox.SelectedValue as ProjectDetail).ProjectName;
                RefreshGestureListbox(projName);
            }
        }

        private void ChangeAccount_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _storage.Logout();
            ShowLoginScreen();
        }

        private void UserNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LoginButton_Click(sender, e);
        }

        #endregion

    }
}

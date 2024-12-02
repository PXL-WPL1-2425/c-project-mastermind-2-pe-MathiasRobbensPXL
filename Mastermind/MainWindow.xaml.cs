using System;
using System.Reflection.Emit;
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
using System.Windows.Threading;

namespace Mastermind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {

        //timer voor tijd bij te houden.
        private DispatcherTimer countdownTimer;
        private int countdownTime = 0;

        private int score = 100; // Startscore

        //lijst aanmaken voor de verschillende kleuren
        List<string> colors = new List<string> { "Red", "Yellow", "Orange", "White", "Green", "Blue" };
        List<string> chosenColors = new List<string>();

        //aanmaken van variabele van attempts
        private int attempts = 1;

        private List<string> feedbackList = new List<string>();

        private Random random = new Random();

        // Variabele voor de kleurcode die we willen tonen in de titel
        private string colorCodeString = "";


        public MainWindow()
        {
            InitializeComponent();


            //hier timer initialiseren
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);//elke seconden ticken
            countdownTimer.Tick += CountdownTimer_Tick;

            ComboBoxes();

            RandomColors();

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }
        private void startgame()
        {
            
        }
        // Methode om de countdown timer te starten (of opnieuw te starten)
        /// <summary>
        /// Starten en herstarten van de countdowntimer
        /// Waarde van de timer tickt met 1 seconden verder omhoog
        /// </summary>
        private void startCountdown()
        {
            countdownTime = 1;  // Zet de tijd op 1 seconde bij elke start

            countdownTimer.Start(); // Start de timer
            timerLabel.Content = $"Tijd: {countdownTime} sec";
        }
        /// <summary>
        /// Stopt de countdowntimer en verhoogt het aantal pogingen 
        /// Timer stopt en wordt gereset naar 0, titel wordt bijgewerkt naar een nieuwe poging
        /// </summary>
        private void StopCountdown()
        {
            countdownTimer.Stop();

            // Controleer of het aantal pogingen al 10 is
            if (attempts >= 10)
            {
                MessageBox.Show("Je hebt het maximale aantal pogingen bereikt! Het spel is voorbij.");
                return;  
            }

            // Verhoog het aantal pogingen (beurten)
            attempts++;
            Title = $"Mastermind - Poging {attempts}";
            
            countdownTime = 0;
            timerLabel.Content = $"Tijd: {countdownTime} sec";
            startCountdown();  
        }

        // Event handler voor de countdown timer
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            countdownTime++;

            if (countdownTime >= 10)
            {
                StopCountdown();
            }

            timerLabel.Content = $"Tijd: {countdownTime} sec";
        }


        // Event handler voor de toetscombinatie CTRL+F12
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Controleer of CTRL+F12 is ingedrukt
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && e.Key == Key.F12)
            {
                ToggleDebug(); 
            }
        }



        /// <summary>
        /// Hier wordt de verborgen label weergegeven die de kleurcode voorziet van het spel
        /// Als de debugTextBox zichtbaar is, wordt deze verborgen.
        /// </summary>
        private void ToggleDebug()
        {
            // Als de debugTextBox zichtbaar is, verberg het en anders toon je de textbox
            if (debugTextBox.Visibility == Visibility.Collapsed)
            {
                debugTextBox.Visibility = Visibility.Visible;
                debugTextBox.Text = colorCodeString;
            }
            else
            {
                debugTextBox.Visibility = Visibility.Collapsed;
            }
        }
        private void RandomColors()
        {
            
            // 4 kleuren leegmaken anders kan dit problemen veroorzaken
            chosenColors.Clear();

            // genereer random kleuren, 4 stuks
            Random random = new Random();
            for (int i = 0; i < 4; i++) 
            { 
             int index = random.Next(colors.Count);
                chosenColors.Add(colors[index]);
            
            }
            string colorstring = string.Join(",", chosenColors);

            //titel updaten met de code
            colorCodeString = string.Join(",", chosenColors);

            Title = $"Mastermind - Poging {attempts}";

            startCountdown();
        }

        private void ComboBoxes()
        {
            comboBox1.ItemsSource = colors;
            comboBox2.ItemsSource = colors;
            comboBox3.ItemsSource = colors;
            comboBox4.ItemsSource = colors;

        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Als er een kleur is geselecteerd in combobox 1, toon deze in label 1
            //if (comboBox1.SelectedItem != null)
            //label1.Content = comboBox1.SelectedItem.ToString();

            // Doe hetzelfde voor de andere comboboxen en labels
            //if (comboBox2.SelectedItem != null)
            //label2.Content = comboBox2.SelectedItem.ToString();
            //if (comboBox3.SelectedItem != null)
            //label3.Content = comboBox3.SelectedItem.ToString();
            //if (comboBox4.SelectedItem != null)
            //label4.Content = comboBox4.SelectedItem.ToString();

            //Deze code gaf enkel de naam weer van de kleur in het bijhorende label, moet worden bijgewerkt.

            if (comboBox1.SelectedItem != null)
                label1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(comboBox1.SelectedItem.ToString()));
            if (comboBox2.SelectedItem != null)
                label2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(comboBox2.SelectedItem.ToString()));
            if (comboBox3.SelectedItem != null)
                label3.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(comboBox3.SelectedItem.ToString()));
            if (comboBox4.SelectedItem != null)
                label4.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(comboBox4.SelectedItem.ToString()));

        }
        private void UpdateFeedbackDisplay()
        {
            // Clear the ListBox before adding new items
            attemptsListBox.Items.Clear();

            // Voeg elke poging en feedback toe aan de ListBox
            foreach (var feedback in feedbackList)
            {
                // Maak een StackPanel voor elke poging
                StackPanel attemptPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                // Voeg de vier kleuren toe als Borders met achtergrondkleur
                for (int i = 0; i < 4; i++)
                {
                    Border colorBorder = new Border
                    {
                        Width = 30,
                        Height = 30,
                        Margin = new Thickness(5),
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(chosenColors[i]))
                    };
                    attemptPanel.Children.Add(colorBorder);
                }

                // Voeg de feedback (rood/wit) toe aan de StackPanel
                TextBlock feedbackTextBlock = new TextBlock
                {
                    Text = feedback,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0),
                    FontSize = 16
                };
                attemptPanel.Children.Add(feedbackTextBlock);

                // Voeg de StackPanel toe aan de ListBox
                attemptsListBox.Items.Add(attemptPanel);
            }
        }

        private int attemptCount = 0; // Aantal pogingen
        private const int maxAttempts = 10; // Maximale aantal pogingen
        private bool gameWon = false; // Of de speler de code heeft gekraakt

        private void validateButton_Click(object sender, RoutedEventArgs e)
        {
            // Verkrijg de geselecteerde kleuren van de comboboxen
            string selectedColor1 = comboBox1.SelectedItem?.ToString();
            string selectedColor2 = comboBox2.SelectedItem?.ToString();
            string selectedColor3 = comboBox3.SelectedItem?.ToString();
            string selectedColor4 = comboBox4.SelectedItem?.ToString();

            // Controleer of alle comboboxen geselecteerd zijn
            if (selectedColor1 == null || selectedColor2 == null || selectedColor3 == null || selectedColor4 == null)
            {
                MessageBox.Show("Kies alstublieft een kleur voor elke combobox.");
                return;
            }

            // Zet de borders van de labels terug naar de standaard (geen rand)
            label1.BorderBrush = null;
            label2.BorderBrush = null;
            label3.BorderBrush = null;
            label4.BorderBrush = null;

            // We maken een tijdelijke lijst van de gekozen kleuren (voor feedback)
            List<string> selectedColors = new List<string> { selectedColor1, selectedColor2, selectedColor3, selectedColor4 };
            List<Border> colorBorders = new List<Border>();  // List om de borders van de ingevoerde kleuren op te slaan

            // Feedback variabelen
            int redCount = 0;
            int whiteCount = 0;
            List<string> tempChosenColors = new List<string>(chosenColors); // Kopie van de geheime code

            // We gaan voor elke geselecteerde kleur controleren of deze correct is (rood/wit)
            for (int i = 0; i < 4; i++)
            {
                // Maak een Border voor elke geselecteerde kleur
                Border colorBorder = new Border
                {
                    Width = 30,
                    Height = 30,
                    Margin = new Thickness(5),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(selectedColors[i])),
                    BorderThickness = new Thickness(2)  // Zorg ervoor dat de rand zichtbaar is
                };
                colorBorders.Add(colorBorder);

                // Feedback bepalen
                if (selectedColors[i] == chosenColors[i]) // Kleur en positie zijn correct
                {
                    colorBorder.BorderBrush = new SolidColorBrush(Colors.DarkRed);  // Rood voor correct
                    redCount++;
                    tempChosenColors[i] = ""; // Verwijder de correct geraden kleur uit de lijst
                }
                else if (chosenColors.Contains(selectedColors[i])) // Kleur is aanwezig, maar op de verkeerde plaats
                {
                    colorBorder.BorderBrush = new SolidColorBrush(Colors.Wheat); // Wit voor incorrecte positie
                    whiteCount++;

                   
                    int tempIndex = tempChosenColors.IndexOf(selectedColors[i]);
                    if (tempIndex != -1) // Zorg ervoor dat de kleur gevonden wordt in tempChosenColors
                    {
                        tempChosenColors[tempIndex] = ""; // Verwijder de kleur uit de lijst
                    }
                }

                else
                {
                    // Kleur die helemaal niet voorkomt krijgt geen border (optioneel)
                    colorBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
            }

            // Voeg de kleuren en de feedback toe aan de lijst
            string feedback = $"{redCount} rood, {whiteCount} wit";

            // Voeg een StackPanel toe voor deze poging met de vier gekozen kleuren en de feedback
            StackPanel attemptPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 5, 0, 5)
            };

            // Voeg de kleuren (Borders) toe aan het StackPanel
            foreach (var border in colorBorders)
            {
                attemptPanel.Children.Add(border);
            }

            // Voeg de feedback als tekst toe
            TextBlock feedbackTextBlock = new TextBlock
            {
                Text = feedback,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0),
                FontSize = 16
            };
            attemptPanel.Children.Add(feedbackTextBlock);

            // Voeg de poging toe aan de ListBox
            attemptsListBox.Items.Add(attemptPanel);

            // Stop de countdown en verhoog de poging
            StopCountdown();

            // **Score berekening**
            int score = 100;
            int incorrectColors = 4 - redCount - whiteCount;
            score -= (whiteCount * 1) + (incorrectColors * 2);
            scoreLabel.Content = $"Score: {score} strafpunt(en)";

            // **Check of het spel afgelopen is**
            attemptCount++; // Verhoog de poging teller

            // Check of de speler de code heeft gekraakt (4 rood)
            if (redCount == 4)
            {
                gameWon = true;
                MessageBox.Show($"Gefeliciteerd! Je hebt de code gekraakt in {attemptCount} pogingen!");
                EndGame();
                return;
            }

            // Als we het aantal pogingen hebben bereikt
            if (attemptCount >= maxAttempts)
            {
                // De speler heeft het maximum aantal pogingen bereikt, toon de geheime code
                MessageBox.Show($"Je hebt het maximale aantal pogingen bereikt. De geheime code was: {string.Join(", ", chosenColors)}");
                EndGame();
            }
        }
        private void GenerateNewCode()
        {
            
            chosenColors.Clear();  

            for (int i = 0; i < 4; i++)
            {
                int index = random.Next(colors.Count);  
                chosenColors.Add(colors[index]);       
            }

           
        }



        // Eindig het spel en vraag of de speler opnieuw wil spelen
        private void EndGame()
        {
            // Vraag de speler of hij opnieuw wil spelen
            var result = MessageBox.Show("Wil je opnieuw spelen?", "Spel beëindigen", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                // Nieuwe code genereren en het spel resetten
                GenerateNewCode();  
                ResetGame();        
            }
            else
            {
                 Close();
            }
        }

       
        private void ResetGame()
        {
            attemptCount = 0;
            attempts = 1;
            gameWon = false;
            scoreLabel.Content = "Score: 100";  
            attemptsListBox.Items.Clear();     
                                             
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            comboBox3.SelectedItem = null;
            comboBox4.SelectedItem = null;

            Title = $"Mastermind - Poging {attempts}";
        }
        //private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
           // if (!gameWon && attemptCount < maxAttempts)  
           // {
              //  var result = MessageBox.Show("Het spel is nog niet beëindigd. Weet je zeker dat je de applicatie wilt afsluiten?",
                                           //  "Beëindigen",
                                             //MessageBoxButton.YesNo,
                                             //MessageBoxImage.Warning);
                //if (result == MessageBoxResult.Yes)
                //{
                   // Application.Current.Shutdown();  
                //}
                //else
                //{
                   // e.Cancel = true; 
                //}
            //}
        //}

        private void MnuNewGame_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewCode();
            ResetGame();
            //Update de debug label met de nieuwe code
            colorCodeString = string.Join(",", chosenColors);
        }

        private void MnuHighscore_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Highscore:","MasterMind Highscore", MessageBoxButton.OK);
        }

        private void MnuClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MnuAttempts_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}

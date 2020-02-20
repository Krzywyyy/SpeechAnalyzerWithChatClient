using Microsoft.Speech.Recognition;
using SpeechAnalyzer.Props;
using System;
using System.Threading;

namespace SpeechAnalyzer.ASR
{
    class ConversationListener
    {
        private SpeechRecognitionEngine pSRE;
        private readonly MainWindow mainWindow;
        private readonly string START = "Start";
        private readonly string STOP = "Stop";
        private bool changeScramblingWord;

        public ConversationListener(MainWindow mainWindow)
        {
            pSRE = new SpeechRecognitionEngine(MyCultureInfo.USCulture);
            pSRE.SetInputToDefaultAudioDevice();
            this.mainWindow = mainWindow;
            LoadGrammarAndStartRecognizing(STOP);
        }

        private void LoadGrammar(string scramblingKeyword)
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Culture = pSRE.RecognizerInfo.Culture;
            Choices choices = new Choices();
            choices.Add(scramblingKeyword);
            choices.Add("Zero");
            choices.Add("Sheila");
            choices.Add("Dog");
            choices.Add("Happy");
            choices.Add("House");
            grammarBuilder.Append(choices);
            Grammar grammar = new Grammar(grammarBuilder);
            pSRE.UnloadAllGrammars();
            pSRE.LoadGrammar(grammar);
        }

        private void StartRecognizing()
        {
            try
            {
                changeScramblingWord = false;
                RecognitionResult recognition;
                do
                {
                    recognition = pSRE.Recognize();
                    if (CorrectlyRecognized(recognition))
                    {
                        Console.WriteLine("Recognized: " + recognition.Text);
                        if (recognition.Text.Equals(START) || recognition.Text.Equals(STOP))
                        {
                            changeScramblingWord = true;
                        }
                        ChangeState(recognition.Text);
                    }
                } while (!changeScramblingWord);
                LoadGrammarAndStartRecognizing(recognition.Text);
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("SpeechListener Thread Interrupted.");
            }
        }

        private void LoadGrammarAndStartRecognizing(string text)
        {
            string keyword = text.Equals(START) ? STOP : START;
            LoadGrammar(keyword);
            StartRecognizing();
        }

        private bool CorrectlyRecognized(RecognitionResult recognition)
        {
            return recognition != null ? recognition.Confidence > 0.7 : false;
        }

        private void ChangeState(string recognizedText)
        {
            switch (recognizedText)
            {
                case "Start":
                    mainWindow.ChangeConversationScramblingState(true);
                    break;
                case "Stop":
                    mainWindow.ChangeConversationScramblingState(false);
                    break;
                default:
                    mainWindow.ChangeConversationClassification(recognizedText);
                    break;
            }
        }
    }
}

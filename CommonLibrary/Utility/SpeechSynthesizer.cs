using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Diagnostics;

namespace CommonLibrary.Utility
{
	public class SpeechSynthesizerUtility
	{
		public bool speakingOn = false;
		private int curLine = 0;

		private string[] speakLines = { 
		    "Failure has detected from KISS Server.",	
		    "Please check out your systems.",
		    "And contact to administrator.",};

		public String SpeakMessage { get; set; }

		public SpeechSynthesizerUtility()
		{
			/*
			 Name:          Microsoft Anna
			 Culture:       en-US
			 Age:           Adult
			 Gender:        Female
			 Description:   Microsoft Anna - English (United States)
			 ID:            MS-Anna-1033-20-DSK
			 Audio formats: Pcm

			 Additional Info -   Age: Adult
			  AudioFormats: 18
			  Gender: Female
			  Language: 409
			  Name: Microsoft Anna
			  Vendor: Microsoft
			  Version: 2.0 
			 */

			SpeakMessage = String.Empty;

			// Initialize a new instance of the SpeechSynthesizer.
			using (SpeechSynthesizer synth = new SpeechSynthesizer())
			{

				// Get information about supported audio formats.
				string AudioFormats = "";
				foreach (SpeechAudioFormatInfo fmt in synth.Voice.SupportedAudioFormats)
				{
					AudioFormats += String.Format("{0}\n",
					fmt.EncodingFormat.ToString());
				}

				// Write information about the voice to the console.
				Debug.WriteLine(" Name:          " + synth.Voice.Name);
				Debug.WriteLine(" Culture:       " + synth.Voice.Culture);
				Debug.WriteLine(" Age:           " + synth.Voice.Age);
				Debug.WriteLine(" Gender:        " + synth.Voice.Gender);
				Debug.WriteLine(" Description:   " + synth.Voice.Description);
				Debug.WriteLine(" ID:            " + synth.Voice.Id);
				if (synth.Voice.SupportedAudioFormats.Count != 0)
				{
					Debug.WriteLine(" Audio formats: " + AudioFormats);
				}
				else
				{
					Debug.WriteLine(" No supported audio formats found");
				}

				// Get additional information about the voice.
				string AdditionalInfo = "";
				foreach (string key in synth.Voice.AdditionalInfo.Keys)
				{
					AdditionalInfo += String.Format("  {0}: {1}\n",
					  key, synth.Voice.AdditionalInfo[key]);
				}

				Debug.WriteLine(" Additional Info - " + AdditionalInfo);
			}

		}

		public SpeechSynthesizerUtility(String speakMessage)
		{
			SpeakMessage = speakMessage;
		}

		public void StartSpeech()
		{
			speakingOn = true;
			SpeakLine();
		}

		public void StopSpeech()
		{
			speakingOn = false;
		}

		private void SpeakLine()
		{
			if (speakingOn)
			{
				// Create Speech object
				SpeechSynthesizer spk = new SpeechSynthesizer();
				spk.Volume = 100;
				spk.Rate = -2;

				spk.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(spk_SpeakCompleted);
				// Speak the line 
				spk.SpeakAsync(speakLines[curLine]);
			}
		}

		private void spk_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
		{
			if (sender is SpeechSynthesizer)
			{

				// get access to our Speech object 
				SpeechSynthesizer spk = (SpeechSynthesizer)sender;
				// Clean up after speaking (thinking the event handler is causing the memory leak) 
				spk.SpeakCompleted -= new EventHandler<SpeakCompletedEventArgs>(spk_SpeakCompleted);
				// Dispose the speech object 
				spk.Dispose();
				// bump it 
				curLine++;
				// check validity 
				if (curLine >= speakLines.Length)
				{
					// back to the beginning 
					curLine = 0;
				}
				// Speak line 
				SpeakLine();
			}
		}
	}
}

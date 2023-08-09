using Microsoft.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMClient
{
	class TTS
	{
		SpeechSynthesizer ts;

		public TTS() {}
		
		public void initTTS(string RFIDTag) {
			ts = new SpeechSynthesizer();
			string path = @"C:\Users\이동현\Desktop\졸작발표\" + RFIDTag + ".wav";
			ts.SetOutputToWaveFile(path);
		}

		public void attachTTS(string ttsText) {
			ts.Speak(ttsText);
		}

		public void disposeTTS() {
			ts.Dispose();
		}

	}
}

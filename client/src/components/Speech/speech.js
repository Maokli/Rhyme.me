import './speech.css';
import { useRef, useState } from "react";
import SpeechRecognition, { useSpeechRecognition } from "react-speech-recognition";
const Speech = () => {
  let microPhoneIcon = "https://img.icons8.com/ios/452/microphone.png";
  const {
    transcript,
    interimTranscript,
    finalTranscript,
    resetTranscript,
    listening,
    browserSupportsSpeechRecognition,
  } = useSpeechRecognition();
  const [isListening, setIsListening] = useState(false);
  const microphoneRef = useRef(null);
  const [words, setWords] = useState([{word: ""}]);
  if (!browserSupportsSpeechRecognition) {
    return (
      <div className="mircophone-container">
        Browser is not Support Speech Recognition.
      </div>
    );
  }
  const handleListing = () => {
    setIsListening(true);
    microphoneRef.current.classList.add("listening");
    SpeechRecognition.startListening({
      continuous: true,
    });
    
  };
  const stopHandle = () => {
    setIsListening(false);
    microphoneRef.current.classList.remove("listening");
    SpeechRecognition.stopListening();
    };
  const handleReset = () => {
    stopHandle();
    resetTranscript();
    };
  
  const fetchWords = (word) => {
      if(word){
        fetch("/api/rhymes/"+word)
        .then(res => res.json())
        .then((data) => {
          setWords(data)
          console.log(data)
        })
        .catch(console.log)
      }
    }
  const getLastWord = () => {
    if(finalTranscript) {
      let wordsSpokenArray = finalTranscript.split(' ');
      console.log(finalTranscript);
      console.log(wordsSpokenArray);
      const lastWord = wordsSpokenArray[wordsSpokenArray.length - 1];
      console.log(lastWord);
      resetTranscript();
      fetchWords(lastWord)
    }
  }
  return (
    <div className="microphone-wrapper">
      <div className="mircophone-container">
        <div className="microphone-icon-container"
        ref={microphoneRef}
        onClick={handleListing}>
          <img src={microPhoneIcon} className="microphone-icon" />
        </div>
        <div className="microphone-status">
          {isListening ? "Listening........." : "Click to start Rapping"}
        </div>
        {isListening && (
          <button className="microphone-stop btn" onClick={stopHandle}>
            Stop
          </button>
        )}
      </div>

        <div className="microphone-result-container">
        {isListening && (
        <div className="microphone-result-container">
          <div className="microphone-result-text">{transcript}</div>
          <>{getLastWord()}</>
          <div className="rhymes-container">
            {words.map((word) => (
                <span key={word.word} className="rhyme">{word.word}, </span>
            ))}
          </div>
          <button className="microphone-reset btn" onClick={handleReset}>
            Reset
          </button>
        </div>
      )}
        </div>
    </div>
  );
}
export default Speech;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

/*

This script controls how GameObjects react to various events in the First Scenario (i.e. beginning of the scenario, 
and the end of videos, knowledge checks), including: how specific text elements (fact panels, question panels) react to 
events in the gameplay.

*/

public class EventScript : MonoBehaviour
{
    
    // Panel GameObjects (i.e. different "screens")
    public GameObject PatientProfile;
    public GameObject MultipleQuestionPanel; // Question with multiple choices
    public GameObject TrueFalseQuestionPanel; // T/F Question
    public GameObject EndPanel;
    public GameObject KnowledgeCheck;
    public GameObject FactPanel;

    public GameObject MenuButton;
    public GameObject MenuPanel;

    // Video player control GameObjects
    public GameObject SkipButton;
    public GameObject PauseButton;
    public GameObject PlayButton;
    public VideoPlayer[] Videos;

    // Texts
    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI ExplanationText;
    public TextMeshProUGUI FactText;

    // Variable Trackers
    private int currentVideoIndex = 0;
    private int currentQuestionIndex = 0;

    // Popups in scenario 
    public GameObject QuestionPanel_Scenario; 
    public GameObject CorrectPanel_Scenario;
    public GameObject IncorrectPanel_Scenario;
    public GameObject CorrectPanel; 
    public GameObject IncorrectPanel;
    public GameObject Continue;
    public GameObject Close;
    public string PlayerAnswer;
    public string[] CorrectAnswers;

    // Video playback speed
    public GameObject SpeedButton;
    public GameObject SpeedButtonPressed;

    public GameObject Canvas;
    public GameObject ComputerScreen;

    // Refers to quizzes within scenarios
    private int currentQuizNum = 0;
    public TextMeshProUGUI QuizQuestionText;
    public TextMeshProUGUI QuizExplanationText;
    public TextMeshProUGUI Choice1Text;
    public TextMeshProUGUI Choice2Text;
    public TextMeshProUGUI Choice3Text;
    public TextMeshProUGUI Choice4Text;
    public string[] QuizCorrectAnswers;

    private int currentFactNum = 0;
    

    void Start()
    {   PatientProfile.SetActive(true);
        FactPanel.SetActive(false);
        MultipleQuestionPanel.SetActive(false);
        TrueFalseQuestionPanel.SetActive(false);
        KnowledgeCheck.SetActive(false);
        EndPanel.SetActive(false);
        ComputerScreen.SetActive(false);

        SkipButton.SetActive(false);
        SpeedButton.SetActive(false);
        SpeedButtonPressed.SetActive(false);
        PauseButton.SetActive(false);
        PlayButton.SetActive(false);

        QuestionPanel_Scenario.SetActive(false);
        CorrectPanel.SetActive(false);
        IncorrectPanel.SetActive(false);      
    }

    public void StartVideo()
    {
        PatientProfile.SetActive(false);

        MenuButton.SetActive(true);
        SkipButton.SetActive(true);
        SpeedButton.SetActive(true);
        PauseButton.SetActive(true);

        Videos[currentVideoIndex].loopPointReached += LoadFactPanel;
    }

    public void LoadFactPanel(VideoPlayer vp){
        SkipButton.SetActive(false);
        SpeedButton.SetActive(false);
        SpeedButtonPressed.SetActive(false);
        PauseButton.SetActive(false);
        PlayButton.SetActive(false);
        if (currentVideoIndex == 0 || currentVideoIndex == 5){
            Show_QuestionInScenario();
            return;     
        }
        else if (currentVideoIndex == 1){
            // In the second video, we are bypassing fact/question panels and just going to 
            // the next act. So "simulate" clicking "Continue" button on a question/fact panel
            // Attach all methods that would normally be attached to a fact/question panel 
            // to proceed to the next act.
            Canvas.GetComponent<SequenceStarterScript>().StartSeq();
            Canvas.GetComponent<VideoController>().ContinueClicked();
            ContinueClicked();
            StartVideo();
            return;
        }
        else if (currentVideoIndex == 2){
            ComputerScreen.SetActive(true);
            return;
        }
        // if currentvideoindex == 1 (second video), after it ends currentvideoindex += 1 and play next
        if (currentVideoIndex >= Videos.Length || currentVideoIndex < 0){
            return;
        }
        else if (currentVideoIndex == Videos.Length - 1){ 
            // start knowledge check 
            KnowledgeCheck.SetActive(true);
        }
        else{
            FactPanel.SetActive(true);
            FactText.text = GetFactText(currentFactNum);
            currentFactNum++;
        }

    }

    public void Choice1Clicked(){
        PlayerAnswer = "1";
        CheckPlayerAnswer();
    }
     public void Choice2Clicked(){
        PlayerAnswer = "2";
        CheckPlayerAnswer();
    }
     public void Choice3Clicked(){
        PlayerAnswer = "3";
        CheckPlayerAnswer();
    }
     public void Choice4Clicked(){
        PlayerAnswer = "4";
        CheckPlayerAnswer();
    }
    public void CheckPlayerAnswer(){
        if (PlayerAnswer != QuizCorrectAnswers[currentQuizNum]){
            IncorrectPanel_Scenario.SetActive(true);
        }else{
            CorrectPanel_Scenario.SetActive(true);
        }

    }
    public void CloseClicked(){
        IncorrectPanel.SetActive(false);
        IncorrectPanel_Scenario.SetActive(false);
        PlayerAnswer = "None";
    }
    public void QuestionContinue(){
        QuestionPanel_Scenario.SetActive(false);
        ComputerScreen.SetActive(false);
        SkipButton.SetActive(true);
        SpeedButton.SetActive(true);
        PauseButton.SetActive(true);
        currentVideoIndex++;
    }

    public void ContinueClicked(){ // Continue in Fact Panel
            FactPanel.SetActive(false);
            ComputerScreen.SetActive(false);

            QuestionPanel_Scenario.SetActive(false);
            CorrectPanel_Scenario.SetActive(false);
            IncorrectPanel_Scenario.SetActive(false);


            SkipButton.SetActive(true);
            SpeedButton.SetActive(true);
            PauseButton.SetActive(true);
            currentVideoIndex++;
            // if (currentVideoIndex != 1 && currentVideoIndex != 2 && currentVideoIndex != 3){
            //     currentQuizNum++;
            // }   
    }

    public void IncreaseQuizNum(){
        currentQuizNum++;
    }

    // public void ContinueinComputer(){
    //     QuestionPanel_Scenario.SetActive(true);
    // }

    public void StartKnowledgeCheck(){
        Videos[currentVideoIndex].Stop();
        KnowledgeCheck.SetActive(false);
        MultipleQuestionPanel.SetActive(true);
        currentQuestionIndex++;
    }

    public void QuestionContinueClicked(){ // Continue in Knowledge Check (question panel)
        if (currentQuestionIndex == 4){ // last question panel
            TrueFalseQuestionPanel.SetActive(false);
            EndPanel.SetActive(true);
        }else{
            TrueFalseQuestionPanel.SetActive(true);
            QuestionText.text = GetQuestionText(currentQuestionIndex);
            ExplanationText.text = GetExplanationText(currentQuestionIndex);
            currentQuestionIndex++;
        }
        
    }

    public void Show_QuestionInScenario(){
        QuestionPanel_Scenario.SetActive(true);
        QuizQuestionText.text = GetQuizQuestionText(currentQuizNum);
        QuizExplanationText.text = GetQuizExplanationText(currentQuizNum);
        Choice1Text.text = GetChoice1Text(currentQuizNum);
        Choice2Text.text = GetChoice2Text(currentQuizNum);
        Choice3Text.text = GetChoice3Text(currentQuizNum);
        Choice4Text.text = GetChoice4Text(currentQuizNum);
        // currentQuizNum++;

    }
    
    // Text events 

    public string GetQuizQuestionText(int index){
        string[] QuizQuestions = {
            "What do you think is Sayed's most appropriate next action to help Bob?",
            "What do you think is Damilola's most appropriate next action to help Bob?",
            "What do you think is Damilola's most appropriate next action to help Bob?",
            "Which of the following authorized acts are within the scope of practice of a registered pharmacy technician? Select all that apply."
        };
        if (index >= 0 && index < QuizQuestions.Length)
            return QuizQuestions[index];
        else
            return "";
    }
    public string GetQuizExplanationText(int index){
        string[] QuizQuestions = {
            "It is within the registered pharmacy technician's (RPhT) scope of practice to collect medical and medication history. This history will inform the pharmacist's assessment and clinical recommendations. While the RPhT can highlight available OTC product options, they cannot recommend one clinically for a specific patient. Once the pharmacist has completed their assessment of the patient, they may recommend an OTC product or refer the patient for further evaluation (e.g., by a physician, nurse practitioner).",
            "The pharmacy technician, Sayed, has already started gathering information on Bob's symptoms and they have reviewed Bob's prescription medication profile together. The pharmacist, Damilola, can now proceed with gathering additional information (as needed) to assess Bob and make a clinical recommendation.",
            "According to the scope of practice, a RPhT cannot make clinical recommendations.",
            "According to the scope of practice, a RPhT can perform tasks related to information gathering and triage, perform a technical (but NOT clinical) check of a prescription, and educate a patient on the proper use of a medical device (such as a blood pressure monitor). It is within a pharmacist's scope of practice to perform both a clinical verification and a technical check for product release to the patient."
        };
        if (index >= 0 && index < QuizQuestions.Length)
            return QuizQuestions[index];
        else
            return "";
    }

    public string GetChoice1Text(int index){
        string[] Choice1Texts = {"He selects an over the counter (OTC) product for Bob to treat his cold symptoms.", 
        "She asks Bob what symptoms he is experiencing.", 
        "She refers Bob back to his primary healthcare provider.", 
        "Perform a clinical check of a refill prescription."};
        if (index >= 0 && index < Choice1Texts.Length)
            return Choice1Texts[index];
        else
            return "";
    }
    public string GetChoice2Text(int index){
        string[] Choice2Texts = {"He asks the pharmacist to assess Bob and make a recommendation.", 
        "She gathers more information, based on what Sayed has already collected, to assess Bob and make a clinical recommendation.", 
        "She asks Sayed to make a clinical recommendation based on the information collected.", 
        "Perform a technical check of a refill prescription."};
        if (index >= 0 && index < Choice2Texts.Length)
            return Choice2Texts[index];
        else
            return "";
    }
    public string GetChoice3Text(int index){
        string[] Choice3Texts = {"He collects more information and then asks the pharmacist to assess Bob and make a recommendation.", 
        "She helps Bob pick an OTC product, counsels him on the product, and 'cashes him out.'", 
        "She makes a clinical recommendation based on the information collected.", 
        "Educate a patient on the use of a medical device."};
        if (index >= 0 && index < Choice3Texts.Length)
            return Choice3Texts[index];
        else
            return "";
    }
    public string GetChoice4Text(int index){
        string[] Choice4Texts = {"He recommends that Sayed sees his doctor or nurse practitioner for a recommendation.", 
        "She helps Bob pick a product, 'cashes him out', and provides him with blood pressure device training.", 
        "She makes a clinical recommendation based on the information gathered and her assessment of Bob's signs and symptoms.", 
        "Choice 4.3",
        "Choice 4.4"};
        if (index >= 0 && index < Choice4Texts.Length)
            return Choice4Texts[index];
        else
            return "";
    }

    public string GetFactText(int index){
        string[] FactTexts = {"It is within a Registered Pharmacy Technician’s scope of practice to provide medical device training.\n\nGiven Sayed's scope of practice, it would be appropriate to maximize his contribution to patient care by having him provide Bob with blood pressure monitor training.",
        "While the Registered Pharmacy Technician (RPhT) can perform triages (assign degree of urgency to determine treatment) and highlight available over-the-counter (OTC) product options, they cannot recommend a product clinically for a specific patient as that is outside their scope of practice.",  
        "It is within a Registered Pharmacy Technician’s (RPhT) scope of practice to perform technical checks on prescriptions. This includes performing technical checks on new prescriptions and refill prescriptions.\n\nHowever, a product cannot be released to the patient until the Registered Pharmacist (RPh) has authorized the prescription for therapeutic appropriateness.", 
        "Both Registered Pharmacists (RPh) and Registered Pharmacy Technicians (RPhT) are bound to uphold the Ontario College of Pharmacists Code of Ethics. \n\nThe Ontario College of Pharmacists Code of Ethics is a set of guidelines that aims to ensure the delivery of safe, effective, and ethical pharmaceutical care."};
        if (index >= 0 && index < FactTexts.Length)
            return FactTexts[index];
        else
            return "";
    }
    public string GetQuestionText(int index)
    {
        string[] questionTexts = { "None", 
        "It is within the Registered Pharmacy Technician’s (RPhT) scope of practice to perform a triage (assign degree of urgency to determine treatment) in order to gather appropriate products and to provide a clinical recommendation to the patient.", 
        "A Registered Pharmacy Technician (RPhT) is allowed to perform the technical check on prescriptions.", 
        "Registered Pharmacists (RPh) and Registered Pharmacy Technicians (RPhT) are bound to uphold the Ontario College of Pharmacists Code of Ethics."};
        if (index >= 0 && index < questionTexts.Length)
            return questionTexts[index];
        else
            return "";
    }

    public string GetExplanationText(int index)
    {
        string[] explanationTexts = { "None", 
        "While the RPhT can perform triages and highlight available over-the-counter (OTC) product options, they cannot recommend a product clinically for a specific patient as that is outside their scope of practice.\n\nOnce the pharmacist has completed their clinical assessment of the patient, they may make a clinical recommendation (e.g., for an OTC product) or refer the patient for further evaluation by another health care professional (e.g., by a physician, nurse practitioner).", 
        "It is within a Registered Pharmacy Technician’s (RPhT) scope of practice to perform the technical checks on prescriptions. This includes performing technical checks on new prescriptions and refill prescriptions.\n\nHowever, a product cannot be released to the patient until the Registered Pharmacist (RPh) has authorized the prescription for therapeutic appropriateness.", 
        "Both Registered Pharmacists (RPh) and Registered Pharmacy Technicians (RPhT) are bound to uphold the Ontario College of Pharmacists Code of Ethics. \n\nThe Ontario College of Pharmacists Code of Ethics is a set of guidelines that aims to ensure the delivery of safe, effective, and ethical pharmaceutical care." };
        if (index >= 0 && index < explanationTexts.Length)
            return explanationTexts[index];
        else
            return "";
    }

    public void SkipVideo(){
        Videos[currentVideoIndex].Stop();
        LoadFactPanel(null);
    }

    public void SpeedClicked(){
        SpeedButton.SetActive(false);
        SpeedButtonPressed.SetActive(true);
    }

    public void SpeedUnclicked(){
        SpeedButton.SetActive(true);
        SpeedButtonPressed.SetActive(false);
    }
}

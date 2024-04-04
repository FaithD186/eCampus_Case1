using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/*

This script determines video player controls for the current video being played, in 
particular pausing/playing video. 


*/

public class VideoController : MonoBehaviour
{

    public VideoPlayer[] Videos;
    private int currentVideoIndex = 0;
    private long lastFrame;
   
    public GameObject SkipButton;
    public GameObject PauseButton;
    public GameObject PlayButton;
    public GameObject SpeedButton;
    public GameObject SpeedButtonPressed;


    public GameObject FactPanel;
    public GameObject MenuPanel;
    public GameObject MultipleQuestionPanel; // Question with multiple choices
    public GameObject TrueFalseQuestionPanel;
    public GameObject QuestionPanel_Scenario; 
    public GameObject PatientProfile;
    public GameObject KnowledgeCheck;

    public GameObject ComputerScreen;

    public void MenuClicked(){
        MenuPanel.SetActive(true);
        if (currentVideoIndex <= Videos.Length - 1){
            Time.timeScale = 0;
            Videos[currentVideoIndex].Pause();
            SkipButton.SetActive(false);
            SpeedButton.SetActive(false);
            SpeedButtonPressed.SetActive(false);
            PauseButton.SetActive(false);
            PlayButton.SetActive(false);
        }
    }

    public void CloseMenu(){
        if (FactPanel.activeSelf || MultipleQuestionPanel.activeSelf || TrueFalseQuestionPanel.activeSelf || PatientProfile.activeSelf || KnowledgeCheck.activeSelf || QuestionPanel_Scenario.activeSelf || ComputerScreen.activeSelf){ 
            MenuPanel.SetActive(false);
        }
        else{
            MenuPanel.SetActive(false);
            Time.timeScale = 1;
            Videos[currentVideoIndex].Play();
            SkipButton.SetActive(true);

            if (Videos[currentVideoIndex].playbackSpeed == 1.0f){
                SpeedButton.SetActive(true);
            }
            else{
                SpeedButtonPressed.SetActive(true);
            }
            
            PauseButton.SetActive(true);
            PlayButton.SetActive(false);
        }
    }
    public void PauseClicked(){
        Time.timeScale = 0;
        Videos[currentVideoIndex].Pause();
        SkipButton.SetActive(false);
        SpeedButton.SetActive(false);
        PlayButton.SetActive(true);
    }
    public void UnPauseClicked(){
        Time.timeScale = 1;
        Videos[currentVideoIndex].Play();
        SkipButton.SetActive(true);
        SpeedButton.SetActive(true); // if speedbutttonpressed is set active, then that should be active
        PlayButton.SetActive(false);
    }
    public void ContinueClicked(){
        currentVideoIndex++;
    }
    public void ReplayScene(){
        SkipButton.SetActive(true);
        SpeedButton.SetActive(true);
        PauseButton.SetActive(true);
        PlayButton.SetActive(false);
        Videos[currentVideoIndex].time = 0;
        Videos[currentVideoIndex].Play();
        FactPanel.SetActive(false);
        KnowledgeCheck.SetActive(false);

        QuestionPanel_Scenario.SetActive(false);
        ComputerScreen.SetActive(false);

    } 
    public void TogglePlaybackSpeed(){
        if (Videos[currentVideoIndex].playbackSpeed == 1.0f){
            Videos[currentVideoIndex].playbackSpeed = 2.0f;
        }
        else{
            Videos[currentVideoIndex].playbackSpeed = 1.0f;
        }
    } 
    // public void SpeedClicked(){
    //     SpeedButton.SetActive(false);
    //     SpeedButtonPressed.SetActive(true);
    // }

    // public void SpeedUnclicked(){
    //     SpeedButton.SetActive(true);
    //     SpeedButtonPressed.SetActive(false);
    // }
}


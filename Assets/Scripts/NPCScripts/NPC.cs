using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePane1;
    public TMP_Text dialogueText, nameText;
    //public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

}

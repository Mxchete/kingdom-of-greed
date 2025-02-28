using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite ncpPortrait;
    public string[] dialogueLines;
    public bool[] autoPorgressLines;
    public float autoPorgressDelay = 1.5f;
    public float typingSpeed = 0.1f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;




}

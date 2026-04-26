using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour , IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText , nameText;
    public Image portraitImage;

    private int dialogueIndex ;
    private bool isTyping , isDialogueActive;

    public bool CanInteract() => !isDialogueActive;

    public void Interact()
    {
        if (dialogueData == null)
        {
            Debug.LogWarning("No dialogue data assigned to NPC.");
            return;
        }
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;
        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else
        {
            dialogueIndex++;
            if (dialogueIndex < dialogueData.dialogueLines.Length)
            {
                StartCoroutine(TypeLine());
            }
            else
            {
                EndDialogue();
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");
        string line = dialogueData.dialogueLines[dialogueIndex];

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            SoundEffectManager.PlayVoice(dialogueData.voiceSound , dialogueData.voicePitch);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);
    }
}

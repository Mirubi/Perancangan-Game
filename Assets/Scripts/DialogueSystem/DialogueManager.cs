using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogData
    {
        public string speakerID; // ID unik karakter
        [TextArea(4, 6)]
        public string dialog;
        [Header("Opsional Ganti Data Default")]
        public string overrideName;
        public Sprite overrideIcon;
    }

    [System.Serializable]
    public class CharacterUI
    {
        public string speakerID; // ID unik karakter (contoh: "bima", "arjuna")
        public GameObject bubbleObject; // Bubble UI dari karakter ini
        public string defaultName;
        public Sprite defaultIcon;
    }

    [Header("Daftar Karakter dan Bubble-nya")]
    public List<CharacterUI> characterUIs;

    [Header("Pengaturan")]
    public float typingSpeed = 0.04f;

    [Header("Events")]
    public UnityEvent onDialogueFinish;

    private Dictionary<string, CharacterUI> characterUIDict;
    private List<DialogData> dialogsToPlay;
    private int currentDialogIndex = 0;
    private bool isDialogueActive = false;

    // Public property untuk dicek dari luar
    public bool IsDialogueActive => isDialogueActive;

    private PlayerMovement playerMove;
    private PlayerCombat playerCombat;
    private Rigidbody2D playerRb;



    void Start()
    {
        playerMove = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerCombat = GameObject.Find("Player").GetComponent <PlayerCombat>();
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();


        // Inisialisasi dictionary karakter
        characterUIDict = new Dictionary<string, CharacterUI>();
        foreach (var charUI in characterUIs)
        {
            if (!characterUIDict.ContainsKey(charUI.speakerID))
            {
                characterUIDict.Add(charUI.speakerID, charUI);
                charUI.bubbleObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Duplicate speakerID ditemukan: {charUI.speakerID}");
            }
        }
    }

    void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
        {
            AdvanceDialogue();
        }
    }

    public void StartDialogue(List<DialogData> newDialogs)
    {
        if (isDialogueActive) return;

        isDialogueActive = true;
        this.dialogsToPlay = newDialogs;
        currentDialogIndex = 0;
        ShowDialogue();

        //frezze movement and combat
        if (playerMove != null) playerMove.enabled = false;
        if (playerCombat != null) playerCombat.enabled = false;

        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero; // stop movement fisik
        }

    }

    private void AdvanceDialogue()
    {
        currentDialogIndex++;
        if (currentDialogIndex < dialogsToPlay.Count)
        {
            ShowDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowDialogue()
    {
        // Nonaktifkan semua bubble sebelum tampilkan yang baru
        foreach (var characterUI in characterUIDict.Values)
        {
            characterUI.bubbleObject.SetActive(false);
        }

        DialogData data = dialogsToPlay[currentDialogIndex];

        if (characterUIDict.TryGetValue(data.speakerID, out CharacterUI charUI))
        {
            string nameToShow = string.IsNullOrEmpty(data.overrideName) ? charUI.defaultName : data.overrideName;
            Sprite iconToShow = data.overrideIcon == null ? charUI.defaultIcon : data.overrideIcon;

            DialogueBubbleUI bubbleUI = charUI.bubbleObject.GetComponent<DialogueBubbleUI>();
            charUI.bubbleObject.SetActive(true);
            bubbleUI.Setup(nameToShow, data.dialog, iconToShow, typingSpeed);
        }
        else
        {
            Debug.LogWarning($"Speaker ID '{data.speakerID}' tidak ditemukan dalam daftar CharacterUI.");
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;

        foreach (var charUI in characterUIDict.Values)
        {
            charUI.bubbleObject.SetActive(false);
        }

        onDialogueFinish?.Invoke();

        playerMove.enabled = true;
        playerCombat.enabled = true;
    }
}

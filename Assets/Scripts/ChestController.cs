using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ChestController : MonoBehaviour
{
    [Header("Interact Settings")]
    public KeyCode interactKey = KeyCode.E;
    public GameObject interactPrompt;

    [Header("Chest Animation")]
    public Animator chestAnimator;
    private static readonly int OpenTrigger = Animator.StringToHash("Open");

    [Header("Quiz Settings")]
    public QuizManager quizManager;

    [Header("Buff Settings")]
    public int healthBuffAmount = 20;
    public int damageBuffAmount = 5;

    private bool playerInRange = false;
    private bool isOpened = false;

    void Start()
    {
        if (interactPrompt) interactPrompt.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isOpened && col.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactPrompt) interactPrompt.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!isOpened && col.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactPrompt) interactPrompt.SetActive(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactPrompt) interactPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && !isOpened && Input.GetKeyDown(interactKey))
        {
            isOpened = true;
            if (interactPrompt) interactPrompt.SetActive(false);
            if (chestAnimator) chestAnimator.SetTrigger(OpenTrigger);

            if (quizManager)
                StartCoroutine(HandleQuizAndBuff());
            else
                ApplyRandomBuff();
        }
    }

    private IEnumerator HandleQuizAndBuff()
    {
        quizManager.ShowQuiz();
        while (quizManager.IsQuizActive())
            yield return null;

        if (quizManager.IsQuizPassed())
            ApplyRandomBuff();
        quizManager.HideQuiz();
    }

    private void ApplyRandomBuff()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;

        // HealthBar component referenced by player
        var hb = player.GetComponent<HealthBar>();
        // CharacterController2D for damage buff
        var cc = player.GetComponent<CharacterController2D>();

        bool giveHealth = Random.value < 0.5f;
        if (giveHealth && hb != null)
        {
            // buff health by sending negative damage
            hb.TakeDamage(-healthBuffAmount);
            Debug.Log($"Chest Buff: +{healthBuffAmount} Health");
        }
        else if (cc != null)
        {
            cc.BoostDamage(damageBuffAmount);
            Debug.Log($"Chest Buff: +{damageBuffAmount} Damage");
        }
    }
}

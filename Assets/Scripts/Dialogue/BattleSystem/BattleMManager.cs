using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DialogueEditor
{
    public class BattleMManager : MonoBehaviour
    {
        private enum eState
        {
            TransitioningDialogueBoxOn,
            ScrollingText,
            Idle,
            TransitioningDialogueOff,
            Off,
            NONE,
        }

        private const float TRANSITION_TIME = 0.2f; // Transition time for fades

        public static BattleMManager Instance { get; private set; }

        public delegate void ConversationStartEvent();
        public delegate void ConversationEndEvent();

        public static ConversationStartEvent OnConversationStarted;
        public static ConversationEndEvent OnConversationEnded;

        // User-Facing options
        public bool ScrollText;
        public float ScrollSpeed = 1;
        public Sprite BackgroundImage;
        public bool BackgroundImageSliced;
        public bool AllowMouseInteraction;

        // Base panels
        public RectTransform DialoguePanel;
        public Image DialogueBackground;
        public Image NpcIcon;
        public TMPro.TextMeshProUGUI DialogueText;
        public AudioSource AudioPlayer;

        private float m_elapsedScrollTime;
        private int m_scrollIndex;
        public int m_targetScrollTextCount;
        private eState m_state;
        private float m_stateTime;
        
        private Conversation m_conversation;
        private SpeechNode m_currentSpeech;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                GameObject.Destroy(this.gameObject);
            }
            Instance = this;

            DialogueText.text = "";
            TurnOffUI();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            switch (m_state)
            {
                case eState.TransitioningDialogueBoxOn:
                    TransitioningDialogueBoxOn_Update();
                    break;

                case eState.ScrollingText:
                    ScrollingText_Update();
                    break;

                case eState.Idle:
                    Idle_Update();
                    break;

                case eState.TransitioningDialogueOff:
                    TransitioningDialogueBoxOff_Update();
                    break;
            }
        }

        public void StartConversation(NPCConversation conversation)
        {
            m_conversation = conversation.Deserialize();
            if (OnConversationStarted != null)
                OnConversationStarted.Invoke();

            TurnOnUI();
            m_currentSpeech = m_conversation.Root;
            SetState(eState.TransitioningDialogueBoxOn);
        }

        public void EndConversation()
        {
            SetState(eState.TransitioningDialogueOff);

            if (OnConversationEnded != null)
                OnConversationEnded.Invoke();
        }

        private void SetState(eState newState)
        {
            m_state = newState;
            m_stateTime = 0f;
        }

        
        public bool IsIdle()
        {
            return (m_state == eState.Idle) ;
        }

        private void TransitioningDialogueBoxOn_Update()
        {
            m_stateTime += Time.deltaTime;
            float t = m_stateTime / TRANSITION_TIME;

            if (t > 1)
            {
                SetupSpeech(m_currentSpeech);
            }
        }

        private void ScrollingText_Update()
        {
            const float charactersPerSecond = 1500;
            float timePerChar = (60.0f / charactersPerSecond);
            timePerChar *= ScrollSpeed;

            m_elapsedScrollTime += Time.deltaTime;

            if (m_elapsedScrollTime > timePerChar)
            {
                m_elapsedScrollTime = 0f;

                DialogueText.maxVisibleCharacters = m_scrollIndex;
                m_scrollIndex++;

                if (AudioPlayer != null)
                {
                    AudioPlayer.Play();
                }

                if (m_scrollIndex >= m_targetScrollTextCount)
                {
                    SpeechNode next = GetValidSpeechOfNode(m_currentSpeech);
                    if (next == null)
                    {
                        SetState(eState.Idle);
                    }
                    else
                    {
                        SetupSpeech(next);
                    }
                }
            }
        }

        private void Idle_Update()
        {
            m_stateTime += Time.deltaTime;
        }

        private void TransitioningDialogueBoxOff_Update()
        {
            m_stateTime += Time.deltaTime;
            float t = m_stateTime / TRANSITION_TIME;

            if (t > 1)
            {
                TurnOffUI();
            }
        }

        private void SetupSpeech(SpeechNode speech)
        {
            if (speech == null)
            {
                EndConversation();
                return;
            }

            m_currentSpeech = speech;

            // Set sprite

            // Set text
            if (string.IsNullOrEmpty(speech.Text))
            {
                DialogueText.text = "";
                m_targetScrollTextCount = 0;
                DialogueText.maxVisibleCharacters = 0;
                m_elapsedScrollTime = 0f;
                m_scrollIndex = 0;
            }
            else
            {
                DialogueText.text = speech.Text;
                m_targetScrollTextCount = speech.Text.Length + 1;
                DialogueText.maxVisibleCharacters = 0;
                m_elapsedScrollTime = 0f;
                m_scrollIndex = 0;
            }

            if (ScrollText)
            {
                SetState(eState.ScrollingText);
            }
            else
            {
                SpeechNode next = GetValidSpeechOfNode(m_currentSpeech);

                if (next == null)
                {
                    SetState(eState.Idle);
                }
                else
                {
                    SetupSpeech(next);
                }
            }            
        }

        private SpeechNode GetValidSpeechOfNode(ConversationNode parentNode)
        {
            if (parentNode.Connections.Count == 0) { return null; }

            for (int i = 0; i < parentNode.Connections.Count; i++)
            {
                SpeechConnection connection = parentNode.Connections[i] as SpeechConnection;
                if (connection != null)
                {
                    return connection.SpeechNode;
                }
            }

            return null;
        }

        private void TurnOnUI()
        {
            DialoguePanel.gameObject.SetActive(true);
        }

        private void TurnOffUI()
        {
            DialoguePanel.gameObject.SetActive(false);
            SetState(eState.Off);
        }
    }
}
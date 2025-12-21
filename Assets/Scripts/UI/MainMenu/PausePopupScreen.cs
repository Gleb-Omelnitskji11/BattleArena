using TowerDefence.Core;
using TowerDefence.Game;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UI
{
    public class PausePopupScreen : BaseScreen
    {
        [SerializeField]
        private Button m_ResumeButton;

        [SerializeField]
        private Button m_QuitButton;

        private IEventBus m_EventBus;

        protected override void Awake()
        {
            base.Awake();
            m_EventBus = Services.Get<IEventBus>();
            m_ResumeButton.onClick.AddListener(OnResumeClicked);
            m_QuitButton.onClick.AddListener(OnQuitClicked);
        }

        private void OnResumeClicked()
        {
            m_EventBus.Publish(new ResumeGameRequestedEvent());
        }
        
        private void OnQuitClicked()
        {
            m_EventBus.Publish(new ReturnToMenuRequestedEvent());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_ResumeButton.onClick.RemoveListener(OnResumeClicked);
            m_QuitButton.onClick.RemoveListener(OnQuitClicked);
        }
    }
}
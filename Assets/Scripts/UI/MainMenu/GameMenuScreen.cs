using TowerDefence.Core;
using TowerDefence.Game;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UI
{
    public class GameMenuScreen : BaseScreen
    {
        [SerializeField] private Button m_PauseButton;
        private IEventBus m_EventBus;

        protected override void Awake()
        {
            base.Awake();
            m_EventBus = Services.Get<IEventBus>();
            m_PauseButton.onClick.AddListener(OnPauseClicked);
        }
        
        private void OnPauseClicked()
        {
            m_EventBus.Publish(new PauseGameRequestedEvent());
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_PauseButton.onClick.RemoveListener(OnPauseClicked);
        }
    }
}

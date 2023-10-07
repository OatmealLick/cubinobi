using System;
using Cubinobi.Project;
using TMPro;
using UnityEngine;
using Zenject;

namespace Cubinobi
{
    public class StateInfo : MonoBehaviour
    {
        private EventManager _eventManager;

        [SerializeField]
        private TextMeshProUGUI invulnerabilityValue;

        [SerializeField]
        private TextMeshProUGUI stateValue;

        [SerializeField]
        private TextMeshProUGUI stanceValue;
        

        [Inject]
        public void Construct(EventManager eventManager)
        {
            _eventManager = eventManager;
        }

        private void Start()
        {
            _eventManager.AddListener<PlayerStateChangedEvent>(HandlePlayerStateChanged);
            _eventManager.AddListener<PlayerStanceChangedEvent>(HandlePlayerStanceChanged);
            _eventManager.AddListener<PlayerInvulnerabilityChangedEvent>(HandlePlayerInvulnerabilityChanged);
        }

        private void OnDestroy()
        {
            _eventManager.RemoveListener<PlayerStateChangedEvent>(HandlePlayerStateChanged);
            _eventManager.RemoveListener<PlayerStanceChangedEvent>(HandlePlayerStanceChanged);
            _eventManager.RemoveListener<PlayerInvulnerabilityChangedEvent>(HandlePlayerInvulnerabilityChanged);
        }

        private void HandlePlayerInvulnerabilityChanged(IEvent e)
        {
            if (e is PlayerInvulnerabilityChangedEvent invulnerabilityChangedEvent)
            {
                 invulnerabilityValue.text = $"{invulnerabilityChangedEvent.invulnerabilityTimer:0.00}";
            }
        }

        private void HandlePlayerStanceChanged(IEvent e)
        {
            if (e is PlayerStanceChangedEvent stanceChangedEvent)
            {
                stanceValue.text = stanceChangedEvent.Stance.ToString();
            }
        }

        private void HandlePlayerStateChanged(IEvent e)
        {
            if (e is PlayerStateChangedEvent stateChangedEvent)
            {
                stateValue.text = stateChangedEvent.State.ToString();
            }
        }
    }
}

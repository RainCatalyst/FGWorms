using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace FGWorms.Gameplay
{
    [RequireComponent(typeof(CharacterStateMachine), typeof(TurnParticipant))]
    public class BaseCharacterController : MonoBehaviour
    {
        public string Id { get; private set; }
        public CharacterStateMachine Character { get; private set; }
        public TurnParticipant Turn { get; private set; }

        public virtual void Setup(string id)
        {
            Id = id;
            _nameLabel.text = id;
        }

        protected virtual void UpdateTurn() { }

        protected virtual void Awake()
        {
            Turn = GetComponent<TurnParticipant>();
            Character = GetComponent<CharacterStateMachine>();
            Character.SetController(this);

            Turn.StartedTurn += OnStartTurn;
            Turn.EndedTurn += OnEndTurn;
        }

        protected virtual void Update()
        {
            if (!Turn.enabled)
                return;
            UpdateTurn();
        }
        
        private void OnStartTurn()
        {
            Character.ResetStamina();
            Character.ChangeState(Character.StateMove);
            Character.Weapon.Refresh();
        }

        private void OnEndTurn()
        {
            Character.ChangeState(Character.StateWait);
        }

        [SerializeField]
        private TMP_Text _nameLabel;
        private bool _activeTurn;
    }
}
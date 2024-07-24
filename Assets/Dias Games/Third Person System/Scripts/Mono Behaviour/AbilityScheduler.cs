using System;
using UnityEngine;
using DiasGames.Abilities;
using DiasGames.Combat;

namespace DiasGames
{
    public class AbilityScheduler : MonoBehaviour
    {
        private AbstractAbility[] CharAbilities = null;
        private AbstractCombat[] CharCombats = null;

        public AbstractAbility CurrentAbility { get; private set; }
        public AbstractAbility LastAbility { get; private set; }

        public AbstractCombat CurrentCombat { get; private set; }

        public CharacterActions characterActions = new CharacterActions();

        // Reference to the CS Camera Controller game object
        public GameObject csCameraController;
        private MonoBehaviour climbIK;

        // Observers
        public event Action OnUpdatedAbilities = null;
        public event Action<AbstractAbility> OnAbilityStopped = null;
        public event Action<AbstractAbility> OnAbilityStarted = null;
        public event Action OnCombatUpdate = null;

        private void Awake()
        {
            CharAbilities = GetComponents<AbstractAbility>();
            CharCombats = GetComponents<AbstractCombat>();

            foreach (AbstractAbility ability in CharAbilities)
                ability.SetActionReference(ref characterActions);

            foreach (AbstractCombat combat in CharCombats)
                combat.SetActionReference(ref characterActions);

            // Get the ClimbIK component from the CS Camera Controller
            if (csCameraController != null)
            {
                climbIK = csCameraController.GetComponent("ClimbIK") as MonoBehaviour;
                if (climbIK == null)
                {
                    Debug.LogError("ClimbIK component not found on CS Camera Controller");
                }
            }
        }

        // stop scheduler
        public void StopScheduler()
        {
            if (CurrentCombat != null)
                CurrentCombat.StopCombat();

            enabled = false;
        }

        private void Update()
        {
            CheckAbilitiesStates();

            if (CurrentAbility != null)
                CurrentAbility.UpdateAbility();

            UpdateCombats();

            // Tells any observer that it has updated.
            OnUpdatedAbilities?.Invoke();
        }

        /// <summary>
        /// Loop through all frames to check whether some ability wants to enter
        /// </summary>
        private void CheckAbilitiesStates()
        {
            AbstractAbility nextAbility = CurrentAbility;

            foreach (AbstractAbility ability in CharAbilities)
            {
                if (ability == CurrentAbility) continue;

                if (ability.ReadyToRun())
                {
                    // If this ability has a greater priority, it should start executing
                    if (nextAbility == null || ability.AbilityPriority > nextAbility.AbilityPriority)
                    {
                        // check combat
                        if (CurrentCombat != null && !CurrentCombat.IsAbilityAllowed(ability))
                            continue;

                        nextAbility = ability;
                    }
                }
            }

            // After loop through all abilities, check if should change the current ability
            if (nextAbility != CurrentAbility)
            {
                // Stops the current ability
                if (CurrentAbility != null)
                    CurrentAbility.StopAbility();

                // Starts the new Ability
                nextAbility.StartAbility();

                // Update current ability and register observer
                CurrentAbility = nextAbility;
                CurrentAbility.abilityStopped += AbilityHasStopped;
                OnAbilityStarted?.Invoke(CurrentAbility);

                if (CurrentCombat != null)
                    CurrentCombat.SetCurrentAbility(CurrentAbility);

                // Enable or disable ClimbIK based on the current ability
                if (CurrentAbility is PushAbility)
                {
                    if (climbIK != null) climbIK.enabled = false;
                }
                else
                {
                    if (climbIK != null) climbIK.enabled = true;
                }
            }
        }

        private void AbilityHasStopped(AbstractAbility ability)
        {
            LastAbility = CurrentAbility;
            CurrentAbility = null;

            // Remove this function from observer
            ability.abilityStopped -= AbilityHasStopped;

            // call observer
            OnAbilityStopped?.Invoke(LastAbility);

            // Enable or disable ClimbIK based on the last ability
            if (LastAbility is PushAbility)
            {
                if (climbIK != null) climbIK.enabled = true;
            }
        }

        // update state to check if any combat wants to play
        private void UpdateCombats()
        {
            if (CurrentCombat != null)
            {
                CurrentCombat.UpdateCombat();
                return;
            }

            foreach (AbstractCombat combat in CharCombats)
            {
                if (combat.CombatReadyToRun())
                {
                    if (combat.IsAbilityAllowed(CurrentAbility))
                    {
                        CurrentCombat = combat;
                        CurrentCombat.SetCurrentAbility(CurrentAbility);
                        CurrentCombat.StartCombat();
                        CurrentCombat.OnCombatStop += CombatStopped;
                        OnCombatUpdate?.Invoke();
                        break;
                    }
                }
            }
        }

        private void CombatStopped()
        {
            CurrentCombat.OnCombatStop -= CombatStopped;
            CurrentCombat = null;

            OnCombatUpdate?.Invoke();
        }
    }
}

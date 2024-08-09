using UnityEngine;
using DiasGames; // Namespace for AbilityScheduler
using DiasGames.Abilities; // Namespace for the abilities

namespace DiasGames
{
    public class ClimbMonitor : MonoBehaviour
    {
        public AbilityScheduler abilityScheduler; 
        public KuroFollow kuroFollow;
        private bool isClimbing = false;

        private void Update()
        {
            bool climbing = (abilityScheduler.CurrentAbility is ClimbAbility || 
                             abilityScheduler.CurrentAbility is ClimbLadderAbility || 
                             abilityScheduler.CurrentAbility is WallRun);

            if (climbing != isClimbing)
            {
                isClimbing = climbing;
                Debug.Log("Climbing state changed: " + isClimbing);
                kuroFollow.SetClimbingState(isClimbing);
            }
        }
    }
}

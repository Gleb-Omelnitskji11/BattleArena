using Gameplay.Controllers;

namespace Gameplay.Models
{
    public class CharacterModel
    {
        public TeamId TeamId { get; private set; }
        public RaceType Race { get; }

        public HealthComponent Health { get; }
        public AttackComponent AttackComponent { get; }
        public MovementStats Movement { get; }

        public CharacterModel(TeamId teamId, RaceType race, HealthComponent health, AttackComponent gun,
            MovementStats movement)
        {
            TeamId = teamId;
            Race = race;
            Health = health;
            AttackComponent = gun;
            Movement = movement;
        }

        public void ChangeTeam(TeamId newTeam)
        {
            TeamId = newTeam;
        }
    }
}
using Gameplay.Controllers;

namespace Gameplay.Models
{
    public class CharacterModel : IComponent
    {
        public TeamId TeamId { get; private set; }
        public RaceType Race { get; }

        public HealthComponent Health { get; }
        public AttackComponent AttackComponent { get; }
        public MovementStats Movement { get; }
        public int Damage { get; }

        public CharacterModel(TeamId teamId, RaceType race, HealthComponent health, AttackComponent gun,
            MovementStats movement, int damage)
        {
            TeamId = teamId;
            Race = race;
            Health = health;
            AttackComponent = gun;
            Movement = movement;
            Damage = damage;
        }

        public void ChangeTeam(TeamId newTeam)
        {
            TeamId = newTeam;
        }

        public void ResetData()
        {
            Health.ResetData();
            AttackComponent.ResetData();
        }
    }
}
    namespace Ecs.Components
{
    public class HealthComponent : IComponent
    {
        public int CurrentHealth { get; set; }

        public HealthComponent(int currentHealth)
        {
            CurrentHealth = currentHealth;
        }
    }
}
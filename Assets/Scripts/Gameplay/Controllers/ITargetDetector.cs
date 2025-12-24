public interface ITargetDetector
{
    bool TryDetectTarget(out CharacterView target);
    void UpdateEnemies();
}
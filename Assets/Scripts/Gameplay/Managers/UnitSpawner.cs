using System;
using System.Collections.Generic;
using Game.Enemy;
using Game.Tanks;
using TowerDefence.Core;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private FieldView m_Field;

    [SerializeField]
    private CharactersConfig m_CharactersConfig;

    [SerializeField]
    private LevelProgressChecker m_LevelProgressChecker;

    public SeparatePlayerController SpawnPlayer(RaceType type)
    {
        var view = CreateNewCharacter(type);

        view.SetTeam(TeamId.Blue, true);
        view.transform.position = GetRandomPositionForNew(true);
        SeparatePlayerController player = new SeparatePlayerController(view);
        view.SetController(player);
        return player;
    }

    public SeparateBotController SpawnBot(RaceType type, bool isEnemy)
    {
        var view = CreateNewCharacter(type);
        view.SetTeam(isEnemy ? TeamId.Red : TeamId.Blue, false);
        ITargetDetector detector = new DirectionalTargetDetector(view.transform, view, m_LevelProgressChecker);
        view.transform.position = GetRandomPositionForNew(false);

        SeparateBotController bot = new SeparateBotController(view, detector);
        view.SetController(bot);
        return bot;
    }

    private CharacterView CreateNewCharacter(RaceType raceType)
    {
        switch (raceType)
        {
            case RaceType.Tank:
                return CreateNewTank();
            default: throw new InvalidOperationException();
        }
    }

    private CharacterView CreateNewTank()
    {
        Game.Enemy.CharacterConfigModel characterConfigModel = m_CharactersConfig.GetCharacterModel(RaceType.Tank);
        HealthComponent healthComponent = new HealthComponent(characterConfigModel.MaxHealth);

        AttackComponent attackComponent =
            new AttackComponent(characterConfigModel.FireRate, characterConfigModel.Damage);

        MovementStats movementStats = new MovementStats(characterConfigModel.Speed);

        CharacterModel characterModel = new CharacterModel(TeamId.Neutral, RaceType.Tank, healthComponent,
            attackComponent, movementStats);

        CharacterView newUnit = Instantiate(characterConfigModel.CharacterPrefab);
        newUnit.Init(characterModel);
        return newUnit;
    }

    private Vector2 GetRandomPositionForNew(bool isPlayer)
    {
        Vector2 spawnPos = m_Field.GetRandomPosition();
        int locker = 30;

        while (!IsSpawnPossible(spawnPos, !isPlayer))
        {
            if (--locker == 0)
            {
                spawnPos = Vector2.zero;
                Debug.LogWarning($"Set default player position");
                break;
            }

            spawnPos = m_Field.GetRandomPosition();
        }

        Debug.Log($"player start position {spawnPos.x}, {spawnPos.y}");
        return spawnPos;
    }

    private bool IsSpawnPossible(Vector2 placeToSpawn, bool playerCheck)
    {
        const int minAllowableDistance = 10;
        float distance;

        foreach (var enemy in m_LevelProgressChecker.AllBots)
        {
            if (enemy != null && !IsDistanceSufficient(enemy.transform.position, placeToSpawn))
                return false;
        }

        if (playerCheck
            && !IsDistanceSufficient(m_LevelProgressChecker.Player.CharacterView.transform.position, placeToSpawn))
        {
            return false;
        }

        return true;

        bool IsDistanceSufficient(Vector2 point1, Vector2 point2)
        {
            distance = Vector2.Distance(point1, point2);

            return distance >= minAllowableDistance;
        }
    }
}
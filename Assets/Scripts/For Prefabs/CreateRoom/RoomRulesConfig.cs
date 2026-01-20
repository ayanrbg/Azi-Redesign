using System;

[Serializable]
public class RoomRules  
{
    public int minPlayers;
    public int mafiaCount;
    public int minActiveCitizens;
    public int maxActiveCitizens;
    public Role[] availableRoles;
}

public static class RoomRulesConfig
{
    public static readonly RoomRules[] Rules =
    {
        // 6 èãðîêîâ (ÌÈÍÈÌÓÌ)
        new RoomRules
        {
            minPlayers = 6,
            mafiaCount = 1,
            minActiveCitizens = 0,
            maxActiveCitizens = 1,
            availableRoles = new[]
            {
                Role.sherif
            }
        },

        // 7
        new RoomRules
        {
            minPlayers = 7,
            mafiaCount = 1,
            minActiveCitizens = 0,
            maxActiveCitizens = 1,
            availableRoles = new[]
            {
                Role.sherif,
                Role.doctor
            }
        },

        // 8
        new RoomRules
        {
            minPlayers = 8,
            mafiaCount = 2,
            minActiveCitizens = 0,
            maxActiveCitizens = 1,
            availableRoles = new[]
            {
                Role.sherif,
                Role.doctor
            }
        },

        // 9
        new RoomRules
        {
            minPlayers = 9,
            mafiaCount = 3,
            minActiveCitizens = 0,
            maxActiveCitizens = 2,
            availableRoles = new[]
            {
                Role.sherif,
                Role.doctor,
                Role.lover
            }
        },

        // 10–11
        new RoomRules
        {
            minPlayers = 10,
            mafiaCount = 3,
            minActiveCitizens = 1,
            maxActiveCitizens = 3,
            availableRoles = new[]
            {
                Role.sherif,
                Role.doctor,
                Role.lover,
                Role.bodyguard
            }
        },

        // 12–13
        new RoomRules
        {
            minPlayers = 12,
            mafiaCount = 4,
            minActiveCitizens = 1,
            maxActiveCitizens = 4,
            availableRoles = new[]
            {
                Role.sherif,
                Role.doctor,
                Role.lover,
                Role.bodyguard,
                Role.priest
            }
        },

        // 14+
        new RoomRules
        {
            minPlayers = 14,
            mafiaCount = 4,
            minActiveCitizens = 1,
            maxActiveCitizens = 5,
            availableRoles = (Role[])Enum.GetValues(typeof(Role))
        }
    };

    public static RoomRules GetRules(int players)
    {
        RoomRules result = Rules[0];

        foreach (var rule in Rules)
        {
            if (players >= rule.minPlayers)
                result = rule;
        }

        return result;
    }
}

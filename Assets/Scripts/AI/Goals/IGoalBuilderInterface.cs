// Copyright (C) Threetee Gang All Rights Reserved

namespace Assets.Scripts.AI.Goals
{
    public interface IGoalBuilderInterface
    {
        Goal CreateGoalForId(EGoalID inId);
    }
}

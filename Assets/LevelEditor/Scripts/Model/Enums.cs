namespace CommonLevelEditor
{
    //只能追加加不能插入，导致配置错乱
    

    //TODO:这个类使用字符串代替，用json来配
    public enum ItemTypeId
    {
        None,
        BackgroundField,
        BlackHole,
        Blocking,
        RoundBomb,
        Boss,
        BossPart,
        LineBomb,
        TriLineBomb,
        StarBomb,
        Cannon,
        CannonGem,
        Caged,
        Climber,
        Creator,
        Dirt,
        Drop1,
        Drop2,
        Drop3,
        DoubleRoundBomb,
        DoubleRainbow,
        Empty,
        EnterField,
        ExitField,
        Layered,
        Magnet,
        PortalEntrance,
        PortalExit,
        RainBomb,
        RainbowBomb,
        RainBullet,
        Question,
        Spawning,
        SpawnClimberField,
        TutorialCover,
        TutorialBlocker,
        Cream,
        CreamFactory,
        Window,
        Wheel,
        Snake,
        Ican,
        Stone,
        EventDrop1,
        Cleaner,
        Tape,
        Net,
        NetFactory,
        Key,
        Lock,
        Switcher,
        StoneBomb_Bullet,
        StoneBomb_Bomb,
        StoneBomb_TriBullet,
        StoneBomb_DoubleBomb,
        StoneBomb_Rainbow,
        StoneBomb_DoubleRainbow,
        StoneBomb_StarBullet,
        BossHit = 1000,
        TreasureBox,
        TreasureBoxDrop1,
        TreasureEnterField,
        TreasureBoxDrop2,
        TreasureBoxDrop3,
        ColorBomb,
        TriRoundBomb,
        Gem_Color0,
        Gem_Color1,
        Gem_Color2,
        Gem_Color3,
        Gem_Color4,
        Gem_Color5,
        Rain,
        Bricks
    }

    //TODO:这个类使用字符串代替，用json来配
    public enum CompanionTypeId
    {
        Empty,

        ColorChanger,
        ColorCollect,
        InstaBomb,//圆形炸弹
        Magneto,
        ColumnBurster,
        ColorLimiter,
        Matcho,
        StarDroid,
        BombClassic,
        RowBurster,


        Snake,
        S_ActivateBomb,            //1.S->Skill 2.R->Random 3.S->Step
        S_ReplaceBomb,
        S_ReplaceBomb_R3_S3,
        S_ActovateDoubleBomb,
        S_ReplaceDoubleBomb,
        S_ActivateLine,
        S_ReplaceLine,
        S_ReplaceLine_S3,
        S_ActivateCrossLine,

        S_ReplaceCrossLine,
        S_ReplaceTriLine,
        S_ReplaceRainBomb,
        S_ReplaceRainBomb_R,
        S_ReplaceCliber,
        S_ActivateForceBomb,
        S_ActivateForceRowBullet,
        S_ActivateRocket,
        S_ActivateCannon_R,
        S_ReplaceLine_RS,

        S_BombBoss,
        S_ReplaceStarDroid,
        Snake_Up_Down,
        S_ActivateDDBomb,
        S_ReplaceQuestion,
        S_ReplaceRoundBlueGem,
        S_ReplaceColumnBlueGem,
        S_ActivateForceRocket,
        S_ActivateSuperBlackHole,
        S_ActivateForceRocket_5,


        S_ReplaceChar,
        S_Disslve_Green_Gem,//企鹅
        S_Dissolve_Blue_Gem,//白熊
        S_Disslve_Red_Gem,//猪排
        S_Disslve_Yellow_Gem,//猫
        S_Disslve_Lightblue_Gem,//蜥蜴
    }

   
}

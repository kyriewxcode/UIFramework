public class EventManager
{
    public static FEvent<bool> OnGuildCreated = new FEvent<bool>();// 创建公会成功

    public static FEvent<EUICareAboutMoneyType[]> OnMoneyTypeChange = new FEvent<EUICareAboutMoneyType[]>();// 货币栏显示变化

}
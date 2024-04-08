namespace Bet.Communication.Request;
public class RequestGetUserBets
{
    public PageQuery PageQuery { get; set; }

    public RequestGetUserBets(PageQuery pageQuery)
    {
        PageQuery = pageQuery;
    }
}

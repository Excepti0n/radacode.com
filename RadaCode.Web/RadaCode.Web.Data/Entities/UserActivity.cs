namespace RadaCode.Web.Data.Entities
{
    public abstract class UserActivity: ActionStampableIdableEntity
    {}

    public class UserLogin: UserActivity
    {}
}

namespace Pleiades.Web.Security.Model
{
    public class AggregateUser
    {
        public int ID { get; set; }
        public IdentityProfile IdentityProfile { get; set; }
        public PfMembershipUser Membership { get; set; }

        public static AggregateUser AnonymousFactory()
        {
            return
                new AggregateUser
                {
                    IdentityProfile = new IdentityProfile
                    {
                        UserRole = UserRole.Anonymous
                    },
                    Membership = new PfMembershipUser
                    {
                        UserName = null,
                    }
                };
        }
    }
}
namespace Entity
{
    
    public class AuthEvent
    {
        private string username;
        private string type;
        private string id;

        public string GetUsername()
        {
            return username;
        }

        public void SetUsername(string pusername)
        {
            username = pusername;
        }

        public string GetType()
        {
            return type;
        }

        public void SetType(string ptype)
        {
            type = ptype;
        }

        public string GetId()
        {
            return id;
        }

        public void SetId(string pid)
        {
            id = pid;
        }
    }
}

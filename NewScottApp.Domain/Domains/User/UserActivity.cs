namespace NewScottApp.Domain.Domains.User
{
    public class UserActivity
    {
        private long _id;
        private string _title;
        private string _description;
        private string _cssClass;
        private string? _extraInfo;

        public UserActivity(string title, string description, string cssClass, string? extraInfo = null)
        {
            _title = title;
            _description = description;
            _cssClass = cssClass;
            _extraInfo = extraInfo;
        }

        public long Id
        {
            get => _id;
            private set => _id = value;
        }
        public string Title
        {
            get => _title;
            private set => _title = value;
        }

        public string Description
        {
            get => _description;
            private set => _description = value;
        }

        public string CssClass
        {
            get => _cssClass;
            private set => _cssClass = value;
        }

        public string? ExtraInfo
        {
            get => _extraInfo;
            private set => _extraInfo = value;
        }

        public dynamic? GetExtraInfo()
        {
            // return _extraInfo.ConvertToJson();
            return null;
        }
    }
}
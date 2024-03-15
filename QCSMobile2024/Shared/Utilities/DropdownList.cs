namespace QCSMobile2024.Shared.Utilities
{
    public static class DropdownList
    {
        public static List<StateInfo> GetStateInfo()
        {
            return new List<StateInfo>
            {
                new StateInfo(1, "AL", "Alabama"),
                new StateInfo(2, "AK", "Alaska"),
                new StateInfo(3, "AZ", "Arizona"),
                new StateInfo(4, "AR", "Arkansas"),
                new StateInfo(5, "CA", "California"),
                new StateInfo(6, "CO", "Colorado"),
                new StateInfo(7, "CT", "Connecticut"),
                new StateInfo(8, "DE", "Delaware"),
                new StateInfo(9, "DC", "District of Columbia"),
                new StateInfo(10, "FL", "Florida"),
                new StateInfo(11, "GA", "Georgia"),
                new StateInfo(12, "HI", "Hawaii"),
                new StateInfo(13, "ID", "Idaho"),
                new StateInfo(14, "IL", "Illinois"),
                new StateInfo(15, "IN", "Indiana"),
                new StateInfo(16, "IA", "Iowa"),
                new StateInfo(17, "KS", "Kansas"),
                new StateInfo(18, "KY", "Kentucky"),
                new StateInfo(19, "LA", "Louisiana"),
                new StateInfo(20, "ME", "Maine"),
                new StateInfo(21, "MD", "Maryland"),
                new StateInfo(22, "MA", "Massachusetts"),
                new StateInfo(23, "MI", "Michigan"),
                new StateInfo(24, "MN", "Minnesota"),
                new StateInfo(25, "MS", "Mississippi"),
                new StateInfo(26, "MO", "Missouri"),
                new StateInfo(27, "MT", "Montana"),
                new StateInfo(28, "NE", "Nebraska"),
                new StateInfo(29, "NV", "Nevada"),
                new StateInfo(30, "NH", "New Hampshire"),
                new StateInfo(31, "NJ", "New Jersey"),
                new StateInfo(32, "NM", "New Mexico"),
                new StateInfo(33, "NY", "New York"),
                new StateInfo(34, "NC", "North Carolina"),
                new StateInfo(35, "ND", "North Dakota"),
                new StateInfo(36, "OH", "Ohio"),
                new StateInfo(37, "OK", "Oklahoma"),
                new StateInfo(38, "OR", "Oregon"),
                new StateInfo(39, "PA", "Pennsylvania"),
                new StateInfo(40, "RI", "Rhode Island"),
                new StateInfo(41, "SC", "South Carolina"),
                new StateInfo(42, "SD", "South Dakota"),
                new StateInfo(43, "TN", "Tennessee"),
                new StateInfo(44, "TX", "Texas"),
                new StateInfo(45, "UT", "Utah"),
                new StateInfo(46, "VT", "Vermont"),
                new StateInfo(47, "VA", "Virginia"),
                new StateInfo(48, "WA", "Washington"),
                new StateInfo(49, "WV", "West Virginia"),
                new StateInfo(50, "WI", "Wisconsin"),
                new StateInfo(51, "WY", "Wyoming")
            };
        }
    }

    public class StateInfo
    {
        public StateInfo(int stateId, string stateAbbreviation, string stateName)
        {
            StateId = stateId;
            StateAbbreviation = stateAbbreviation;
            StateName = stateName;
            IsSelected = false;
        }

        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateAbbreviation { get; set; }
        public bool IsSelected { get; set; }
        public string StateCSS => IsSelected ? "state-enabled-css" : "state-disabled-css";
    }
}

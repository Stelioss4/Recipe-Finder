﻿namespace Recipe_Finder
{
    public class Rating
    {

        private double _value;

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private DateTime _timeStam;

        public DateTime TimeStam
        {
            get { return _timeStam; }
            set { _timeStam = value; }
        }

        private UsersProfile _profile;

        public UsersProfile Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }



    }
}

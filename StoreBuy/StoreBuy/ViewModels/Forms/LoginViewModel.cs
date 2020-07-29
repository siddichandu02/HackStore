using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.Forms
{
    [Preserve(AllMembers = true)]
    public class LoginViewModel : BaseViewModel
    {
        #region Fields

        private string email;

        private string password;

        private string phone;

        private bool isInvalidEmail;

        private bool isInvalidPassword;

        private bool isInvalidPhone;

        

        #endregion

        #region Property

       
        public string Email
        {
            get
            {
                return  email;
            }

            set
            {
                if ( email == value)
                {
                    return;
                }

                 email = value;
                 NotifyPropertyChanged();
            }
        }


        public string Password
        {
            get
            {
                return  password;
            }

            set
            {
                if ( password == value)
                {
                    return;
                }

                 password = value;
                 NotifyPropertyChanged();
            }
        }

        public string Phone
        {
            get
            {
                return  phone;
            }

            set
            {
                if ( phone == value)
                {
                    return;
                }

                 phone = value;
                 NotifyPropertyChanged();
            }
        }


        public bool IsInvalidEmail
        {
            get
            {
                return  isInvalidEmail;
            }

            set
            {
                if ( isInvalidEmail == value)
                {
                    return;
                }

                 isInvalidEmail = value;
                 NotifyPropertyChanged();
            }
        }


        public bool IsInvalidPassword
        {
            get
            {
                return  isInvalidPassword;
            }

            set
            {
                if ( isInvalidPassword == value)
                {
                    return;
                }

                 isInvalidPassword = value;
                 NotifyPropertyChanged();
            }
        }

        public bool IsInvalidPhone
        {
            get
            {
                return  isInvalidPhone;
            }

            set
            {
                if ( isInvalidPhone == value)
                {
                    return;
                }

                 isInvalidPhone = value;
                 NotifyPropertyChanged();
            }
        }
        #endregion
    }
}

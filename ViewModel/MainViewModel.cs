using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPFMvvMTest.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int iNum;
        public int Number
        {
            get { return iNum; }
            set
            {
                int iOldNum = iNum;

                iNum = value;
                OnPropertyChanged("Number");

                if (0 < iNum && iNum < 11)
                {
                    OnPropertyChanged("PlusEnable");
                    OnPropertyChanged("MinusEnable");

                    PageContents = string.Format("{0} 페이지를 보고 있어요.", iNum);
                }
                else
                {
                    MessageBox.Show("1-10 페이지만 입력 가능합니다.");
                    iNum = iOldNum;
                    OnPropertyChanged("Number");
                }
            }
        }

        public MainViewModel()
        {
            Number = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
#if false
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
#else
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
#endif
        }

        private ICommand minusCommand;
        public ICommand MinusCommand
        {
            get { return (this.minusCommand) ?? (this.minusCommand = new DelegateCommand(Minus)); }
        }
        private void Minus()
        {
            Number--;
        }

        public bool MinusEnable
        {
            get { return Number > 1 ? true : false; }
        }

        private ICommand plusCommand;
        public ICommand PlusCommand
        {
            get { return (this.plusCommand) ?? (this.plusCommand = new DelegateCommand(Plus)); }
        }
        private void Plus()
        {
            Number++;
        }

        public bool PlusEnable
        {
            get { return Number < 10 ? true : false; }
        }

        private string pageContent;
        public string PageContents
        {
            get { return pageContent; }
            set
            {
                pageContent = value;
                OnPropertyChanged("PageContents");
            }
        }

#region DelegateCommand Class
        public class DelegateCommand : ICommand
        {

            private readonly Func<bool> canExecute;
            private readonly Action execute;

            /// <summary>
            /// Initializes a new instance of the DelegateCommand class.
            /// </summary>
            /// <param name="execute">indicate an execute function</param>
            public DelegateCommand(Action execute) : this(execute, null)
            {
            }

            /// <summary>
            /// Initializes a new instance of the DelegateCommand class.
            /// </summary>
            /// <param name="execute">execute function </param>
            /// <param name="canExecute">can execute function</param>
            public DelegateCommand(Action execute, Func<bool> canExecute)
            {
                this.execute = execute;
                this.canExecute = canExecute;
            }
            /// <summary>
            /// can executes event handler
            /// </summary>
            public event EventHandler CanExecuteChanged;

            /// <summary>
            /// implement of icommand can execute method
            /// </summary>
            /// <param name="o">parameter by default of icomand interface</param>
            /// <returns>can execute or not</returns>
            public bool CanExecute(object o)
            {
                if (this.canExecute == null)
                {
                    return true;
                }
                return this.canExecute();
            }

            /// <summary>
            /// implement of icommand interface execute method
            /// </summary>
            /// <param name="o">parameter by default of icomand interface</param>
            public void Execute(object o)
            {
                this.execute();
            }

            /// <summary>
            /// raise ca excute changed when property changed
            /// </summary>
            public void RaiseCanExecuteChanged()
            {
                if (this.CanExecuteChanged != null)
                {
                    this.CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }
#endregion
    }
}

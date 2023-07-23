using ATM.Commands;
using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ATM.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public RelayCommand InsertCardCommand { get; set; }
        public RelayCommand LoadDataCommand { get; set; }
        public RelayCommand TransferMoneyCommand { get; set; }


        private bool _insertCardButtonClickedIsEnable;

        public bool InsertCardButtonClickedIsEnable
        {
            get { return _insertCardButtonClickedIsEnable; }
            set { _insertCardButtonClickedIsEnable = value; OnPropertyChanged(); }
        }

        private bool _loadDataButtonClickedIsEnable;

        public bool LoadDataButtonClickedIsEnable
        {
            get { return _loadDataButtonClickedIsEnable; }
            set { _loadDataButtonClickedIsEnable = value; OnPropertyChanged(); }
        }

        private int _insertCardPassword;
        public int InsertCardPassword
        {
            get { return _insertCardPassword; }
            set { _insertCardPassword = value; OnPropertyChanged(); }
        }

        private string _cardHolder;

        public string CardHolder
        {
            get { return _cardHolder; }
            set { _cardHolder = value; OnPropertyChanged(); }
        }

        private decimal _theAmountOnTheCard;

        public decimal TheAmountOnTheCard
        {
            get { return _theAmountOnTheCard; }
            set { _theAmountOnTheCard = value; OnPropertyChanged(); }
        }

        private decimal _amountYouWantToithdrawFromTheCard;

        public decimal AmountYouWantToithdrawFromTheCard
        {
            get { return _amountYouWantToithdrawFromTheCard; }
            set { _amountYouWantToithdrawFromTheCard = value; OnPropertyChanged(); }
        }

        private decimal _outgoingMoney;

        public decimal OutgoingMoney
        {
            get { return _outgoingMoney; }
            set { _outgoingMoney = value; OnPropertyChanged(); }
        }

        public static Kart Kart { get; set; }

        static object obj = new object();
        public MainWindowViewModel()
        {
            List<Kart> cards = new List<Kart>
            {
                new Kart
                {
                    Name = "Nezrin",
                    Surname = "Mehdiyeva",
                    Amount = 2000,
                    Password = 1222,
                },
                new Kart
                {
                    Name = "Vusal",
                    Surname = "Keremli",
                    Amount = 1500,
                    Password = 1224,

                },
                new Kart
                {
                    Name = "Haci",
                    Surname = "Babayev",
                    Amount = 1000,
                    Password = 1225,

                },
                new Kart
                {
                    Name = "Ilkin",
                    Surname = "Baxsiyev",
                    Amount = 1000,
                    Password = 1226,
                },
            };

            InsertCardCommand = new RelayCommand((obj) =>
            {
                InsertCardButtonClickedIsEnable = true;
            });

            bool isWrongPassword = false;

            LoadDataCommand = new RelayCommand((obj) =>
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (cards[i].Password == InsertCardPassword)
                    {
                        CardHolder = cards[i].Name + " " + cards[i].Surname;
                        TheAmountOnTheCard = cards[i].Amount;
                        Kart = cards[i];
                        LoadDataButtonClickedIsEnable = true;
                        isWrongPassword = true;
                        break;
                    }
                }

                if (!isWrongPassword)
                {
                    MessageBox.Show("The code that you wrote is wrong", "İnformation", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });


            TransferMoneyCommand = new RelayCommand((o) =>
            {
                lock (obj)
                {
                    if (Kart.Amount >= AmountYouWantToithdrawFromTheCard)
                    {
                        Thread thread = new Thread(() =>
                        {
                            OutgoingMoney = 0;
                            decimal amount = AmountYouWantToithdrawFromTheCard / 10;
                            Kart.Amount -= AmountYouWantToithdrawFromTheCard;
                            TheAmountOnTheCard = Kart.Amount;
                            for (int i = 0; i < amount; i++)
                            {
                                AmountYouWantToithdrawFromTheCard -= 10;
                                OutgoingMoney += 10;
                                Thread.Sleep(300);
                                if (OutgoingMoney == amount * 10)
                                {
                                    MessageBox.Show($"AZN {amount * 10} has been deleted from your card");
                                    OutgoingMoney = 0;
                                }
                            }
                        });
                        thread.Start();
                    }


                    else
                    {
                        MessageBox.Show("You don't have enough money", "İnformation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                };
            });

        }
    }
}
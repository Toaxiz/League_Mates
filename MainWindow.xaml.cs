using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using League_Mates.Services;
using RiotSharp;
using RiotSharp.Endpoints.MatchEndpoint;

namespace League_Mates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int MatchListIndex; // Hmmmm?!

        public MainWindow()
        {
            InitializeComponent();
            TextBoxAPIKEY.Text = Properties.Settings.Default.APIKEY;
            TextBoxSumonnerName.Text = Properties.Settings.Default.SumonnerName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Properties.Settings.Default.APIKEY = TextBoxAPIKEY.Text;
            Properties.Settings.Default.SumonnerName = TextBoxSumonnerName.Text;
            Properties.Settings.Default.Save();

            // set APIKEY from TextBox APIKEY
            var api = RiotApi.GetDevelopmentInstance(TextBoxAPIKEY.Text);
            var summoner = api.Summoner.GetSummonerByNameAsync(RiotSharp.Misc.Region.Euw, TextBoxSumonnerName.Text).Result;
            List<string> matchlist = api.Match.GetMatchListAsync(RiotSharp.Misc.Region.Europe, summoner.Puuid, MatchListIndex, 10).Result;

            foreach (Match match in from matchId in matchlist
                                    let match = api.Match.GetMatchAsync(RiotSharp.Misc.Region.Europe, matchId).Result
                                    select match)
                {
                ListView1.Items.Add(newItem: match.BuildGameItem(summoner.Puuid));
                }
            MatchListIndex += 10;
            matchlist.Clear();
        }

    }
}

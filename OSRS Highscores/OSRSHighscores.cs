using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace OSRS_Highscores {
    public partial class OSRSHighscores : Form {
        public OSRSHighscores() {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // ignore illegal cross threaded calls for simplicity
        }
        List<Player> players = new List<Player>();

        Stopwatch sw;

        const int STEP_SIZE = 10;
        int threadsCompleted;
        int start;
        int playersCompleted;

        private void button1_Click(object sender, EventArgs e) {
            sw = new Stopwatch();
            sw.Start();
            for(int i = 0; i < 5; i++) {
                BackgroundWorker bg = new BackgroundWorker();
                bg.DoWork += bg_DoWork;
                bg.RunWorkerCompleted += bg_RunWorkerCompleted;
                bg.RunWorkerAsync(start);
                start = start + STEP_SIZE;
            }
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            threadsCompleted++;
            listBox1.Items.Add(threadsCompleted);
            if(threadsCompleted == 5) { // 1 thread per 10 pages is fine, curious to see the performance of maybe 1 thread per page
                sw.Stop();
                listBox1.Items.Add(sw.Elapsed.Seconds);
                listBox1.Items.Add(players.Count);
            }
        }

        void bg_DoWork(object sender, DoWorkEventArgs e) {
            int end = (int)e.Argument + STEP_SIZE;
            int start = (int)e.Argument;
            List<int> tempStats = new List<int>();
            for(int m = start; m < end; m++) {
                string[] dataSplit;
                WebRequest request = WebRequest.Create("http://services.runescape.com/m=hiscore_oldschool/overall.ws?table=0&page=" + m);
                request.Proxy = null; // apparently helps speed the process up a bit... unsure of its effectiveness
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string strResponse = reader.ReadToEnd();
                reader.Close();
                response.Close();
                string[] userData;
                dataSplit = strResponse.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for(int i = 0; i < dataSplit.Length; i++) { // run on a different thread to complete faster?
                    if(dataSplit[i].Contains("hiscorepersonal.ws?user1=")) {
                        int startIndex = dataSplit[i].IndexOf('>') + 1;
                        int endIndex = dataSplit[i].LastIndexOf('<');
                        string name = dataSplit[i].Substring(startIndex, endIndex - startIndex);
                        string replacedName = name.Replace("�", "%20"); // reading pure html so we're going to run into issues like this
                        request = WebRequest.Create("http://services.runescape.com/m=hiscore_oldschool/index_lite.ws?player=" + replacedName);
                        response = request.GetResponse();
                        dataStream = response.GetResponseStream();
                        reader = new StreamReader(dataStream);
                        strResponse = reader.ReadToEnd();
                        userData = strResponse.Split(',');
                        response.Close();
                        reader.Close();
                        for(int l = 0; l < userData.Length; l++) {
                            if(l > 2 && l % 2 != 0 && !userData[l].Contains("-1") && l < 48) // pure html again
                                tempStats.Add(int.Parse(userData[l]));
                        }
                        players.Add(new Player(tempStats[0], tempStats[1], tempStats[2], tempStats[3], tempStats[4], tempStats[5], tempStats[6], tempStats[7], tempStats[8], tempStats[9],
                            tempStats[10], tempStats[11], tempStats[12], tempStats[13], tempStats[14], tempStats[15], tempStats[16], tempStats[17], tempStats[18], tempStats[19], tempStats[20],
                            tempStats[21], tempStats[22], replacedName));
                        tempStats.Clear();
                        playersCompleted++;
                        //listBox1.Items.Add(playersCompleted + " players completed");
                    }
                }
                listBox1.Items.Add("page " + m + " done");
            }
        }
    }
}

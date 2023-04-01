using ExcelConverter.Utils;
using ExcelToCode.Excel;
using ExcelToCodeCore.Utils;

namespace ExcelToCode
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<string> fileList = null;
        private void OnFormLoaded(object sender, EventArgs e)
        {
            this.CenterToScreen();

            LogUtil.Init(new FormLogUtil(this.logTb, this.errLogTb));
            //��ʼ��������Ϣ
            if (!Setting.Init())
            {
                MessageBox.Show("�Ҳ������������ļ�");
                return;
            }
            //��ȡ���ñ�·�����Ͳ���ʾ
            fileList = FileUtil.GetFileList(Setting.ConfigPath, false, ".xlsx");
            this.configListBox.Items.AddRange(fileList.ToArray());

            var orgName = Path.GetFileName(Path.GetFullPath(Setting.ConfigPath));
            var folderList = Directory.GetDirectories(Path.GetDirectoryName(Path.GetFullPath(Setting.ConfigPath)));
            if (string.IsNullOrEmpty(orgName))
            {
                var configPath = Path.GetDirectoryName(Path.GetFullPath(Setting.ConfigPath));
                orgName = Path.GetFileName(configPath);
                folderList = Directory.GetDirectories(Path.GetDirectoryName(configPath));
            }

            foreach (var folder in folderList)
            {
                var folderName = Path.GetFileName(folder);
                if (folderName.Contains(orgName))
                    coverList.Items.Add(folderName);
                if (orgName == folderName)
                {
                    coverList.SetItemChecked(coverList.Items.Count - 1, true);
                    coverList.SelectedIndex = coverList.Items.Count - 1;
                }
            }

            bool codeCheck = false;
            coverList.SelectedIndexChanged += (s, e) =>
            {
                codeCheck = true;
                for (int i = 0; i < coverList.Items.Count; ++i)
                    coverList.SetItemChecked(i, false);
                coverList.SetItemChecked(coverList.SelectedIndex, true);
                codeCheck = false;
            };

            coverList.ItemCheck += (s, e) =>
            {
                if (codeCheck)
                    return;

                var sel = 0;
                for (int i = 0; i < coverList.Items.Count; ++i)
                {
                    if (coverList.GetItemChecked(i))
                        sel++;
                }
                if (sel <= 0)
                {
                    var cache = coverList.SelectedIndex;
                    coverList.SelectedIndex = 0;
                    coverList.SelectedIndex = 1;
                    coverList.SelectedIndex = cache;
                }
            };
        }

        private void ServerAll_Click(object sender, EventArgs e)
        {
            _ = ExportAll(ExportType.Server);
        }

        private void ServerSingle_Click(object sender, EventArgs e)
        {
            _ = ExportSingle(ExportType.Server);
        }

        private void ClientAll_Click(object sender, EventArgs e)
        {
            _ = ExportAll(ExportType.Client);
        }

        private void ClientSingle_Click(object sender, EventArgs e)
        {
            _ = ExportSingle(ExportType.Client);
        }

        private void ClearLogBtn_Click(object sender, EventArgs e)
        {
            this.logTb.Text = "";
        }

        private void ClearErrLogBtn_Click(object sender, EventArgs e)
        {
            this.errLogTb.Text = "";
        }

        private List<string> GetCoverFileList()
        {
            var coverStr = coverList.Items[coverList.SelectedIndex].ToString();
            List<string> coverFileList = null;
            var configPath = Path.GetFullPath(Setting.ConfigPath);
            var inputName = Path.GetFileName(configPath);
            if (string.IsNullOrEmpty(inputName))
            {
                configPath = Path.GetDirectoryName(configPath);
                inputName = Path.GetFileName(configPath);
            }
            if (inputName != coverStr)
                coverFileList = FileUtil.GetFileList(Path.GetDirectoryName(configPath) + Path.DirectorySeparatorChar + coverStr, false, ".xlsx");
            return coverFileList;
        }

        private async Task ExportAll(ExportType etype)
        {
            if (fileList == null || fileList.Count <= 0)
            {
                MessageBox.Show("û�з������ñ�");
                return;
            }
            ToggleAllBtn(false);
            var startTime = TimeUtils.CurrentTimeMillis();
            await ExportHelper.Export(etype, fileList, GetCoverFileList(), true);
            ToggleAllBtn(true);
            LogUtil.Add("�����ʱ��" + (TimeUtils.CurrentTimeMillis() - startTime) + "ms");
        }

        private async Task ExportSingle(ExportType etype)
        {
            var items = this.configListBox.CheckedItems;
            var selectList = new List<string>();
            foreach (var o in items)
            {
                var str = this.configListBox.GetItemText(o);
                if (!selectList.Contains(str))
                    selectList.Add(str);
            }
            if (selectList == null || selectList.Count <= 0)
            {
                MessageBox.Show("����ѡ��һ�����ñ�");
                return;
            }
            if (selectList == null || selectList.Count > 1)
            {
                if (!(selectList.Count == 2 && selectList.Find(f => f.Contains("typedefine.xlsx")) != null))
                {
                    MessageBox.Show("��֧�ֵ�ѡ");
                    return;
                }
            }
            ToggleAllBtn(false);
            await ExportHelper.Export(etype, selectList, GetCoverFileList(), false);
            ToggleAllBtn(true);
        }

        public void ToggleAllBtn(bool flag)
        {
            ServerAll.Enabled = flag;
            ServerSingle.Enabled = flag;
            ClientAll.Enabled = flag;
            ClientSingle.Enabled = flag;
        }
    }
}
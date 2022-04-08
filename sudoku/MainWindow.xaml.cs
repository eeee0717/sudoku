using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sudoku
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // 初始化变量
        public string[,] sudoku = new string[9, 9];
        public string[,] history = new string[9, 9];
        public int x = 0;
        public int y = 0;
        public MainWindow()
        {
            InitializeComponent();
            InitSudoku();
            InitSudokuButton();
            InitInputButton();

            Status.Text = "准备就绪";
        }

        /**
         * 初始化数独
         */
        private void InitSudoku()
        {
            int i, j;
            for (i = 0; i < 9; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    sudoku[i, j] = " ";
                    history[i, j] = " ";
                }
            }
        }
        /**
         * 初始化数独按键
         */
        private void InitSudokuButton()
        {
            int i, j;
            for (i = 0; i < SudokuPanel.Children.Count; i++)
            {
                if (SudokuPanel.Children[i] is Grid g)
                {
                    for (j = 0; j < g.Children.Count; j++)
                    {
                        if (g.Children[j] is Button b)
                        {
                            b.Click += new RoutedEventHandler(OnSudokuButtonClick);
                        }
                    }
                }
            }

        }

        /**
         * 数独按键点击事件
         */
        private void OnSudokuButtonClick(object sender, RoutedEventArgs e)
        {
            RemoveAllSudokuButtonStyle();
            Button b = (Button)sender;
            b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBEE6FD"));
            b.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF424242"));

            string[] name = b.Name.Split('_'); // 将按键的名字划分
            //Console.WriteLine(name[0]);
            x = int.Parse(name[1]);
            y = int.Parse(name[2]);
            Status.Text = $"已选中[{x},{y}]";
        }

        private void RemoveAllSudokuButtonStyle()
        {
            int i, j;
            for (i = 0; i < SudokuPanel.Children.Count; i++)
            {
                if (SudokuPanel.Children[i] is Grid g)
                {
                    for (j = 0; j < g.Children.Count; j++)
                    {
                        if (g.Children[j] is Button b)
                        {
                            b.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
                            b.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE0E0E0"));
                        }
                    }
                }
            }
        }


        /**
         * 初始化输入按键
         */
        private void InitInputButton()
        {
            int i;
            for (i = 0; i < InputPanel.Children.Count; i++)
            {
                if (InputPanel.Children[i] is Button b)
                {
                    b.Click += new RoutedEventHandler(OnInputButtonClick);
                }
            }
        }

        /**
         * 输入按键点击事件
         */
        private void OnInputButtonClick(object sender, RoutedEventArgs e)
        {
            // 已选中区块
            if (x != 0 && y != 0)
            {
                Array.Copy(sudoku, history, sudoku.Length); // 将sudoku复制到history
                Button b = (Button)sender;
                string content = (string)b.Content; // 获取按键内容
                //Console.WriteLine(content);
                sudoku[x - 1, y - 1] = content;
                UpdateTable();
                if (IsFull())
                {
                    if (IsCorrect())
                    {
                        Solve.Content = "重置数独";
                        Status.Text = "Congratulations!";
                        MessageBox.Show("你完成了一个数独", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        Solve.Content = "求解与验证";
                        Status.Text = "数独解得不对哦";
                    }
                }
            }
        }

        /**
         * 更新数独
         */
        private void UpdateTable()
        {
            for (int i = 0; i < SudokuPanel.Children.Count; i++)
            {
                if (SudokuPanel.Children[i] is Grid g)
                {
                    for (int j = 0; j < g.Children.Count; j++)
                    {
                        if (g.Children[j] is Button b)
                        {
                            int x = int.Parse(b.Name.Split('_')[1]) - 1;
                            int y = int.Parse(b.Name.Split('_')[2]) - 1;
                            b.Content = sudoku[x, y].Trim();
                        }
                    }
                }
            }
        }

        /**
         * 判断数独是否填完
         */
        private bool IsFull()
        {
            int count = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // 如果内容为空 count++
                    if (sudoku[i, j].Trim() == "")
                    {
                        count++;
                    }
                }
            }

            return count == 0;
        }

        /**
         * 判断数独是否正确
         * 判断行列单元格内数字是否重复
         */
        private bool IsCorrect()
        {
            // 判断行
            for (int i = 0; i < 0; i++)
            {
                HashSet<int> rowCheck = new HashSet<int>();
                string[] row = GetRow(i);
                for (int j = 0; j < 9; j++)
                {
                    if (row[j].Trim() != "")
                    {
                        rowCheck.Add(int.Parse(row[j]));
                    }
                }
                if (rowCheck.Count != 9)
                {
                    return false;
                }
            }

            // 判断列
            for (int i = 0; i < 0; i++)
            {
                HashSet<int> colCheck = new HashSet<int>();
                string[] col = GetCol(i);
                for (int j = 0; j < 9; j++)
                {
                    if (col[j].Trim() != "")
                    {
                        colCheck.Add(int.Parse(col[j]));
                    }
                }
                if (colCheck.Count != 9)
                {
                    return false;
                }
            }

            // 判断单元格
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    HashSet<int> boxCheck = new HashSet<int>();
                    string[] box = GetBox(i, j);
                }
            }
            return true;

        }

        private string[] GetBox(int x, int y)
        {
            int dx = (int)(x / 3);
            int dy = (int)(y / 3);

            string[] arr = new string[9];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    arr[i * 3 + j] = sudoku[i + dx * 3, j + dy * 3];
                }
            }
            return arr;
        }

        /**
         * 获取该行所有元素
         */
        private string[] GetRow(int x)
        {
            string[] row = new string[9];
            for (int i = 0; i < 9; i++)
            {
                row[i] = sudoku[x, i];
            }
            return row;
        }

        /**
         * 获取该列所有元素
         */
        private string[] GetCol(int x)
        {
            string[] col = new string[9];
            for (int i = 0; i < 9; i++)
            {
                col[i] = sudoku[i, x];
            }
            return col;
        }


        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            Status.Text = "正在生成~~";
            Array.Copy(sudoku, history, sudoku.Length);

            const int blank = 40;

            do
            {
                InitSudoku();

                for (int i = 0; i < 3; i++)
                {
                    string[] temp = GetRandomArray();
                    int p = 0;
                    for (int j = 0 + (i * 3); j < 3 + (i * 3); j++)
                    {
                        for (int k = 0 + (i * 3); k < 3 + (i * 3); k++)
                        {
                            sudoku[j, k] = temp[p];
                            p++;
                        }
                    }
                }
                SolveSudoku();
            } while (sudoku[8, 5] == " ");
            DigHole(blank);
            UpdateTable();
            Status.Text = "已生成数独";
            Solve.Content = "求解与验证";

        }

        private void DigHole(int blank)
        {
            int num = blank;
            while (num > 0)
            {
                Random rx = new Random();
                Random ry = new Random();
                int x = rx.Next(0, 9);
                int y = ry.Next(0, 9);
                if (sudoku[x, y] != " ")
                {
                    string[,] copy = new string[9, 9];
                    Array.Copy(sudoku, copy, sudoku.Length);
                    sudoku[x, y] = " ";
                    SolveSudoku();
                    if (IsFull())
                    {
                        copy[x, y] = " ";
                        num--;
                    }
                    Array.Copy(copy, sudoku, sudoku.Length);
                }
            }
        }

        private void SolveSudoku()
        {
            throw new NotImplementedException();
        }

        /**
         * 生成随机数组
         */
        private string[] GetRandomArray()
        {
            Random r = new Random();
            string[] arr = new string[9] { " ", " ", " ", " ", " ", " ", " ", " ", " ", };
            int count = 1;
            while (count <= 9)
            {
                int num = r.Next(0, 9);
                if (arr[num] == " ")
                {
                    arr[num] = count.ToString();
                    count++;
                }
            }
            return arr;
        }

        private void Revoke_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
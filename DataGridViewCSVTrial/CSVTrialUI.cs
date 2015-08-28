using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DataGridViewCSVTrial
{
    public partial class CSVTrialUI : Form
    {
        char delimiter;
        Boolean DataLoaded = false;
        public CSVTrialUI()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:" ;
            openFileDialog1.Filter = "CSV files (*.csv)|*.CSV";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.ShowDialog();
                       
            textBox1.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fileRow;
            string[] fileDataField;
            int count = 0;
            if (DataLoaded == false)
            {
                try
                {
                    delimiter = Convert.ToChar(textBox2.Text);
                }
                catch (Exception exceptionObject)
                {
                    MessageBox.Show(exceptionObject.ToString());
                    this.Close();
                }

                try
                {
                    if (System.IO.File.Exists(textBox1.Text))
                    {
                        System.IO.StreamReader fileReader = new StreamReader(textBox1.Text);

                        if (fileReader.Peek() != -1)
                        {
                            fileRow = fileReader.ReadLine();
                            fileDataField = fileRow.Split(delimiter);
                            count = fileDataField.Count();
                            count = count - 1;

                            //Reading Header information
                            for (int i = 0; i <= count; i++)
                            {
                                DataGridViewTextBoxColumn columnDataGridTextBox = new DataGridViewTextBoxColumn();
                                columnDataGridTextBox.Name = fileDataField[i];
                                columnDataGridTextBox.HeaderText = fileDataField[i];
                                columnDataGridTextBox.Width = 120;
                                dataGridView1.Columns.Add(columnDataGridTextBox);
                            }
                        }
                        else
                        {
                            MessageBox.Show("File is Empty!!");
                        }
                        //Reading Data
                        while (fileReader.Peek() != -1)
                        {
                            fileRow = fileReader.ReadLine();
                            fileDataField = fileRow.Split(delimiter);
                            dataGridView1.Rows.Add(fileDataField);
                        }

                        fileReader.Close();
                    }
                    else
                    {
                        MessageBox.Show("No File is Selected!!");
                    }

                    DataLoaded = true;

                }
                catch (Exception exceptionObject)
                {
                    MessageBox.Show(exceptionObject.ToString());
                }
            }
            else
            {
                MessageBox.Show("Clear DataGridView First!!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                delimiter = Convert.ToChar(textBox2.Text);
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());
                this.Close();
            }

            try
            {
                System.IO.StreamWriter fileWriter = new StreamWriter(textBox1.Text, false);
                string columnHeaderText = "";
                
                //Writing DataGridView Header in File
                int countColumn = dataGridView1.ColumnCount -1;
                
                if(countColumn >= 0)
                {
                    columnHeaderText = dataGridView1.Columns[0].HeaderText;
                }

                for (int i = 1; i <=countColumn; i++)
                {
                    columnHeaderText = columnHeaderText + delimiter + dataGridView1.Columns[i].HeaderText;
                }

                fileWriter.WriteLine(columnHeaderText);

                //Writing Data in File
                foreach (DataGridViewRow dataRowObject in dataGridView1.Rows)
                {
                    if (!dataRowObject.IsNewRow)
                    {
                        string dataFromGrid = "";

                        dataFromGrid = dataRowObject.Cells[0].Value.ToString();

                        for (int i = 1; i <= countColumn; i++)
                        {
                            dataFromGrid = dataFromGrid + delimiter + dataRowObject.Cells[i].Value.ToString();
                        }
                        
                        fileWriter.WriteLine(dataFromGrid);
                    }
                }

                MessageBox.Show("Data is successfully saved in File");

                fileWriter.Flush();
                fileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new CSVTrialUI().Show();
            this.Dispose(false);
        }
    }
}
